﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
    public class MessageService : IMessageService, IDisposable
    {
        Logging _logger;
        Gateway _gateway;
        IDataBase _database;
        public MessageService(WhispirSettings settings)
        {
            _logger = Logging.GetInstance(settings.LoggingFolder);
            _gateway = new Gateway(settings, _logger);

            if (String.IsNullOrEmpty(settings.DataBaseFolder))
            {
                _database = new CacheDB(_logger, settings.LoggingHours);
            }
            else
            {
                _database = new FileDB(settings.DataBaseFolder, _logger, settings.LoggingHours);
            }
        }

        public async Task<IMessage> SendMessageAsync(IMessage message)
        {
            using (var factory = getServiceFactory(message.GetType().Name))
            {
                var Message = await factory.SendMessageAsync(message);
                return Message;
            }
        }
        public async Task<List<DBMessage>> GetMessagesAsync()
        {
            return await _database.GetRecords();
        }

        public async Task<IResponse> GetMessages(string MessageID)
        {
            var Query = String.Format("/messages/{0}?{1}", MessageID, "apikey={0}");
            _logger.LogInfo("In GetMessages");
            return await _gateway.Request<MessageSentResponse>(Query, ContentTypes.MessageContent);
        }
        public async Task<bool> SetMessageAsProcessed(string MessageID)
        {
            try
            {
                var dbMessage = await _database.GetRecord(MessageID);
                if (dbMessage != null)
                {
                    dbMessage.IsProcessed=true;
                    await _database.UpdateRecord(dbMessage);
                    return true;
                }
            }
            catch { }
            return false;
        }
        public async Task<string> GetMessageStatus(string MessageID)
        {
            // First Check the Status in the DB
            try
            {
                string DBStatus = "";
                string CurrentStatus = "";
                bool isProcessed = false;
                DBMessage dbMessage = null;
                try
                {
                    dbMessage = await _database.GetRecord(MessageID);
                    DBStatus = dbMessage.MessageStatus;
                    isProcessed = dbMessage.IsProcessed;
                    if (dbMessage.IsProcessed) return "";
                    if ((DBStatus == "DELIVRD" || DBStatus == "FAILED")) return DBStatus;
                }
                catch { }
                // Get the Status from the API
                var Query = String.Format("/messages/{0}/messagestatus?view=detailed&{1}", MessageID, "apikey={0}");
                _logger.LogInfo("In GetMessageStatus");
                var msgresponse = await _gateway.Request<MessageStatusResponse>(Query, ContentTypes.MessageStatusContent);
                // Find the Status of the SMS
                if (msgresponse != null)
                {
                    foreach (var MessageStatus in msgresponse.messageStatuses)
                    {
                        foreach (var Status in MessageStatus.status)
                        {
                            if (Status.type == "sms")
                            {
                                if (Status.status != DBStatus)
                                {
                                  
                                        // set the status in the DB And return New DB
                                        if (dbMessage != null)
                                        {
                                            dbMessage.MessageStatus = Status.status.Trim().ToUpper();
                                            await _database.UpdateRecord(dbMessage);
                                        }
                                        //
                                        CurrentStatus = Status.status.Trim().ToUpper();
                                        break;
                                    
                                }
                                else
                                {
                                    if (!isProcessed)
                                    {
                                        // set the status in the DB And return New DB
                                        if (dbMessage != null)
                                        {
                                            dbMessage.MessageStatus = Status.status.Trim().ToUpper();
                                            await _database.UpdateRecord(dbMessage);
                                        }
                                        //
                                        CurrentStatus = Status.status.Trim().ToUpper();
                                        break;
                                    }

                                }
                            }
                        }
                    }
                    return CurrentStatus;
                }
            }
            catch { }
            return null;
        }

        public async Task<string> GetMessageResponse(string MessageID)
        {
            var Query = String.Format("/messages/{0}/messageresponses?view=detailed&{1}", MessageID, "apikey={0}");
            _logger.LogInfo("In GetMessageResponse");
            var response = await _gateway.Request<MessageResponseResponse>(Query, ContentTypes.MessageResponseContent);
            try
            {
                var reply =  response.messageresponses[0].responseMessage.content;
                if (reply != "N/A") return reply;
            }
            catch { }
            return "";
        }
        public async Task<IResponse> GetTemplates()
        {
            var Query = String.Format("/templates?{0}", "apikey={0}");
            _logger.LogInfo("In GetTemplates");
            return await _gateway.Request<TemplatesResponse>(Query, ContentTypes.TemplateResponseContent);
        }
        public async Task<IResponse> GetworkSpaces()
        {
            var Query = String.Format("/workspaces?{0}", "apikey={0}");
            _logger.LogInfo("In GetworkSpaces");
            return await _gateway.Request<WorkSpaceResponse>(Query, ContentTypes.WorkSpaceContent);
        }
        public async Task<IResponse> GetCallBacksSetup()
        {
            var Query = String.Format("/callbacks?{0}", "apikey={0}");
            _logger.LogInfo("In GetCallBacksSetup");
            return await _gateway.Request<CallBacksResponse>(Query, ContentTypes.CallBacksResponseContent);
        }
        private async Task<List<Call>> GetCallBacks()
        {
            var CallBacks = (CallBacksResponse)await GetCallBacksSetup();
            List<Call> Calls = null;
            try
            {
                if (CallBacks != null && CallBacks.callbacks != null)
                {
                    foreach (var cb in CallBacks.callbacks)
                    {
                        var Query = String.Format("/callbacks/{0}/calls?{1}&status=FAILED", cb.id, "apikey={0}");
                        _logger.LogInfo("In GetCallBacks");
                        _logger.LogInfo(Query);
                        var failedcallbacks = await _gateway.Request<CallsResponse>(Query, ContentTypes.CallsResponseContent);
                        foreach (var failedCB in failedcallbacks.calls)
                        {
                            if (Calls == null) Calls = new List<SDK.Call>();
                            Calls.Add(failedCB);

                        }
                    }
                }
            }
            catch { }
            return Calls;
        }
        private async Task<string> updateCallStatus(List<Call> calls)
        {
            try
            {
                dynamic whispirCallBack = new ExpandoObject();
                whispirCallBack.status = "SUCCESS";
                StringBuilder CallIDs = new StringBuilder();
                string CallBackID = "";
                foreach (var call in calls)
                {
                    if (CallBackID == "") CallBackID = call.callback.id;
                    CallIDs.Append(String.Format("&id={0}", call.id));
                }

                var result = await _gateway.Put(whispirCallBack, String.Format("/callbacks/{0}/calls?{1}{2}", CallBackID, "apikey={0}", CallIDs.ToString()), ContentTypes.CallsResponseContent);
                return "OK";
            }
            catch (Exception exception)
            {
                _logger.LogInfo(exception.Message);

            }
            return null;
        }
        public IBaseMessageService getServiceFactory(string type)
        {
            switch (type)
            {
                case "SMS":
                    return new SMSService(_gateway, _logger, _database);
                    break;

            }
            return null;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MessageService() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
