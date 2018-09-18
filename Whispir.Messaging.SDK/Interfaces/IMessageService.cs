using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
    public interface IMessageService : IDisposable
    {
        Task<IMessage> SendMessageAsync(IMessage Message);
        Task<List<DBMessage>> GetMessagesAsync();
        Task<IResponse> GetMessageStatus(string MessageID);
        Task<IResponse> GetMessageResponse(string MessageID);
        Task<IResponse> GetMessages(string MessageID);
        Task<IResponse> GetTemplates();
        Task<IResponse> GetworkSpaces();
        Task<List<Call>> GetCallBacks();
        Task<string> updateCallStatus(List<Call> calls);
    }
    public interface IBaseMessageService : IDisposable
    {
        Task<IMessage> SendMessageAsync(IMessage Message);
    }

}
