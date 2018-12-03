using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
    public class FileDB : IDisposable, IDataBase
    {
        string _folder;
        Logging _logger;
        int _loggingHours;
        public FileDB(string Folder, Logging _logger, int loggingHours)
        {
            _folder = Folder;
            _loggingHours = loggingHours;
        }
        public async Task InsertRecord(DBMessage entity)
        {
            try
            {
                using (var database = new LiteDatabase(Path.Combine(_folder, "Whispir.db")))
                {
                    // Get customer collection
                    var messages = database.GetCollection<DBMessage>("Messages");
                    messages.Insert(entity);
                }
            }
            catch (Exception exception)
            {
                _logger.LogInfo("Cannot Insert Records Into Database on the File System, Make Sure the Folder has Proper Read/Write rights. Exception:" + exception);
            }
        }
        public async Task<DBMessage> GetRecord(string ID)
        {
            using (var database = new LiteDatabase(Path.Combine(_folder, "Whispir.db")))
            {
                // Get customer collection
                var _messages = database.GetCollection<DBMessage>("Messages");
                // Do the Query
                return _messages.FindById(ID);
            }
            return null;
        }
        public async Task UpdateRecord(DBMessage entity)
        {
            using (var database = new LiteDatabase(Path.Combine(_folder, "Whispir.db")))
            {
                // Get customer collection
                var _messages = database.GetCollection<DBMessage>("Messages");
                // Do the Query
                var DBMessage = _messages.FindById(entity.ID);
                if (DBMessage != null)
                {
                    DBMessage.MessageStatus = entity.MessageStatus;
                    DBMessage.Reply = entity.Reply;
                    _messages.Update(DBMessage);
                }
            }
        }

        public Task deleteRecord(DBMessage entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DBMessage>> GetRecords()
        {
            try
            {
                List<DBMessage> DBMessages = new List<SDK.DBMessage>();
                using (var database = new LiteDatabase(Path.Combine(_folder, "Whispir.db")))
                {
                    // Get customer collection
                    var _messages = database.GetCollection<DBMessage>("Messages");
                    // Remove All Messages Older than n Hours
                    DateTime cuttOffTime = DateTime.Now.AddHours(-(_loggingHours));
                    _messages.Delete(x => x.TimeStamp < cuttOffTime);
                    // Do the Query
                    var messages = _messages.FindAll();
                    foreach (var msg in messages)
                    {
                        DBMessages.Add(msg);
                    }
                }
                return DBMessages;
            }
            catch (Exception exception)
            {
                _logger.LogInfo("Cannot Get Records from Database on the File System, Make Sure the Folder has Proper Read/Write rights. Exception:" + exception);
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
        // ~LiteDB() {
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
