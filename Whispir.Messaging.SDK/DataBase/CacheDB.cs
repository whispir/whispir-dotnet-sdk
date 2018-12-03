using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
    public class CacheDB:IDisposable,IDataBase
    {
        MemoryCache currentCache;
        CacheItemPolicy policy;
        List<DBMessage> Messages;
        Logging _logger;
        int _loggingHours;
        public CacheDB(Logging logger, int loggingHours)
        {
            _logger = logger;
            _loggingHours = loggingHours;
            try
            {
                currentCache = MemoryCache.Default;
                _logger.LogInfo("Database Created in Cache");
            }
            catch(Exception exception)
            {
                _logger.LogInfo("Cannot Create Database in Cache, Cache should only be used Web Applications. Exception:"+ exception);
            }
        }
        public async Task InsertRecord(DBMessage entity)
        {
            try
            {
                if (currentCache["MessageTable"] == null)
                {
                    Messages = new List<DBMessage>();
                }
                else
                {
                    Messages = (List<DBMessage>)currentCache["MessageTable"];
                }
                Messages.Add(entity);
                policy = new CacheItemPolicy { AbsoluteExpiration = DateTime.Now.AddHours(_loggingHours) };
                currentCache.Set("MessageTable", Messages, policy);
            }
            catch (Exception exception)
            {
                _logger.LogInfo("Cannot Insert Record to Database in cache. Exception:" + exception);
            }

        }
        public async Task<DBMessage> GetRecord(string ID)
        {
            if (currentCache["MessageTable"] == null)
            {
                Messages = new List<DBMessage>();
            }
            else
            {
                Messages = (List<DBMessage>)currentCache["MessageTable"];
                DateTime cuttOffTime = DateTime.Now.AddHours(-(_loggingHours));
                Messages.RemoveAll(m => m.TimeStamp <= cuttOffTime);
                policy = new CacheItemPolicy { SlidingExpiration = TimeSpan.FromHours(_loggingHours) };
                currentCache.Set("MessageTable", Messages, policy);
                return Messages.Where(m => m.ID == ID).FirstOrDefault(); ;
            }

            return null;
        }
        public async Task UpdateRecord(DBMessage entity)
        {
            if (currentCache["MessageTable"] == null)
            {
                Messages = new List<DBMessage>();
            }
            else
            {
                Messages = (List<DBMessage>)currentCache["MessageTable"];
                DateTime cuttOffTime = DateTime.Now.AddHours(-(_loggingHours));
                Messages.RemoveAll(m => m.TimeStamp <= cuttOffTime);
                var message = Messages.Where(m => m.ID == entity.ID).FirstOrDefault(); ;
                if(message != null)
                {
                    message.MessageStatus = entity.MessageStatus;
                    message.Reply = entity.Reply;
                    policy = new CacheItemPolicy { SlidingExpiration = TimeSpan.FromHours(_loggingHours) };
                    currentCache.Set("MessageTable", Messages, policy);
                }
            }

        }

        public async Task deleteRecord(DBMessage entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DBMessage>> GetRecords()
        {
            try
            {
                if (currentCache["MessageTable"] == null)
                {
                    Messages = new List<DBMessage>();
                }
                else
                {
                    Messages = (List<DBMessage>)currentCache["MessageTable"];
                    DateTime cuttOffTime = DateTime.Now.AddHours(-(_loggingHours));
                    Messages.RemoveAll(m => m.TimeStamp <= cuttOffTime);
                    policy = new CacheItemPolicy { SlidingExpiration = TimeSpan.FromHours(_loggingHours) };
                    currentCache.Set("MessageTable", Messages, policy);
                }
            }
            catch (Exception exception)
            {
                _logger.LogInfo("Cannot Get Records From Database in Cache. Exception:" + exception);
            }

            return Messages;
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
        // ~CacheDB() {
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
