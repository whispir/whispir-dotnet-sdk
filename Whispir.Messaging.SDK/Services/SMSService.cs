using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
    public class SMSService : BaseService, IBaseMessageService
    {

        public SMSService(Gateway gateway, Logging logger, IDataBase database) : base(gateway, logger, database)
        {

        }

        public async Task<IMessage> SendMessageAsync(IMessage message)
        {
            try
            {
                var sms = (SMS)message;
                dynamic whispirsms = new ExpandoObject();
                if (sms.MessageTemplate != null)
                {
                    messageattributes msgattr = null;
                    msgattr = new messageattributes();
                    msgattr.attribute = new List<attribute>();
                    foreach (var attr in sms.MessageTemplate.Attributes)
                    {
                        msgattr.attribute.Add(new attribute() { name = attr.Name, value = attr.Value });
                    }
                    whispirsms.messageTemplateId = sms.MessageTemplate.MessageTemplateId;
                    whispirsms.messageattributes = msgattr;

                }
                if(!String.IsNullOrEmpty(sms.CallBackID))
                {
                    whispirsms.callbackId = sms.CallBackID;
                }
                whispirsms.body = sms.Content;
                whispirsms.subject = sms.Subject;
                whispirsms.to = sms.To;

                var result = await _gateway.Post(whispirsms , "/messages?apikey={0}", ContentTypes.MessageContent);
                if (result.MessageID != null)
                {
                    _logger.LogInfo(String.Format("Message - Body = {0}, Subject={1}, To={2}, ID={3}",sms.Content,sms.Subject,sms.To,result.MessageID));
                    sms.MessageID = result.MessageID;
                    sms.MessageStatus = CurrentMessageStatus.Pending;
                    await _database.InsertRecord(new DBMessage() { ErrorMessage="", ID = result.MessageID, TimeStamp = DateTime.Now, MessageType = MessageType.SMS.ToString(), MessageStatus = CurrentMessageStatus.Pending.ToString()  });
                }
                else
                {
                    if (result.ErrorMessage != null)
                    {
                        _logger.LogInfo(String.Format("Error sending Message - Body = {0}, Subject={1}, To={2}, ErrorMessage={3}", sms.Content, sms.Subject, sms.To, result.ErrorMessage));
                        sms.ErrorMessage = result.ErrorMessage;
                        await _database.InsertRecord(new DBMessage() { ErrorMessage = result.ErrorMessage, ID = "", TimeStamp = DateTime.Now, MessageType = MessageType.SMS.ToString(), MessageStatus = CurrentMessageStatus.Undeliverable.ToString() });
                    }
                }
                return sms;
            }
            catch(Exception exception)
            {
                _logger.LogInfo(exception.Message);

            }
            return null;
        }
    }
}
