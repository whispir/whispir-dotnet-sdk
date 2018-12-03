using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
    public interface IMessageService : IDisposable
    {
        Task<IMessage> SendMessageAsync(IMessage Message);
        Task<List<DBMessage>> GetMessagesAsync();
        Task<string> GetMessageStatus(string MessageID);
        Task<string> GetMessageResponse(string MessageID);
        Task<IResponse> GetMessages(string MessageID);
        Task<IResponse> GetTemplates();
        Task<IResponse> GetworkSpaces();
        //Task<List<Call>> GetCallBacks();
        //Task<string> updateCallStatus(List<Call> calls);
        Task<bool> SetMessageAsProcessed(string MessageID);
    }
    public interface IBaseMessageService : IDisposable
    {
        Task<IMessage> SendMessageAsync(IMessage Message);
    }

}
