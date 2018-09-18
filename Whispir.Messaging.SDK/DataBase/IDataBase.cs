using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
    public interface IDataBase
    {
        Task InsertRecord(DBMessage entity);
        Task UpdateRecord(DBMessage entity);
        Task deleteRecord(DBMessage entity);
        Task<List<DBMessage>> GetRecords();
    }
}
