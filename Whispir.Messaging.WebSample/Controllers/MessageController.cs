using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using Whispir.Messaging.SDK;
using Autofac;

namespace Whispir.Messaging.WebSample.Controllers
{
    
    public class MessageController : ApiController
    {
        IMessageService _service;
        public MessageController(IMessageService service)
        {
            _service = service;
        }
        [Route("TestAPI")]
        [HttpGet]
        public async Task<IHttpActionResult> TestAPI()
        {
            return Ok("All Good To Go.");
        }
        [Route("SendMessage")]
        [HttpGet]
        public async Task<IHttpActionResult> SendMessage()
        {
            IMessage sms = new SMS() { To = "Phone Number with Country Prefix", Content = String.Format("Hello Fred at {0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")), Subject = "Test" };
            var result = await _service.SendMessageAsync(sms);
            return Ok(result);
        }
        [Route("SendTemplateMessage")]
        [HttpGet]
        public async Task<IHttpActionResult> SendTemplateMessage()
        {
            MessageTemplate template = new MessageTemplate();
            template.MessageTemplateId = "Your Template ID";
            template.Attributes = new List<MessageAttribute>();
            template.Attributes.Add(new MessageAttribute() { Name = "attribute1", Value = DateTime.Now.ToString("hh:mm tt") });
            template.Attributes.Add(new MessageAttribute() { Name = "attribute2", Value = DateTime.Now.ToString("dd/MM/yyyy") });
            IMessage sms = new SMS() { To = "Phone Number with Country Prefix", Subject = "Test", MessageTemplate = template };
            var result = await _service.SendMessageAsync(sms);
            return Ok(result);
        }
        [Route("GetMessages")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMessages()
        {
            var result = await _service.GetMessagesAsync();
            return Ok(result);
        }
        [Route("GetMessageStatus")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMessageStatus()
        {
            List<MessageStatusResponse> messageStatus = new List<MessageStatusResponse>();
            var result = await _service.GetMessagesAsync();
            foreach(var msg in result)
            {
                messageStatus.Add((MessageStatusResponse)await _service.GetMessageStatus(msg.ID));
            }
            return Ok(messageStatus);
        }
        [Route("GetMessageResponse")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMessageResponse()
        {
            List<MessageResponseResponse> responses = new List<MessageResponseResponse>();
            var result = await _service.GetMessagesAsync();
            foreach (var msg in result)
            {
                responses.Add((MessageResponseResponse)await _service.GetMessageResponse(msg.ID));
            }
            return Ok(responses);
        }
    }
   
}
