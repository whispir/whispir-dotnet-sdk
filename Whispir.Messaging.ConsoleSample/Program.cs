using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whispir.Messaging.SDK;

namespace Whispir.Messaging.ConsoleSample
{
    public class Program
    {
        static private async Task MessagingTest()
        {
            IMessageService service = new MessageService(new WhispirSettings()
            {
                ApiAuthorization = AppConfig.WhispirAuthorization,
                APIEndpoint = SDK.Enums.APIEndpoints.IT,
                ApiKey = AppConfig.WhispirApiKey,
                LoggingFolder = @"E:\Logs",
                DataBaseFolder = @"E:\Logs",
                LoggingHours = AppConfig.LoggingHours
            });

            // Samples 
            // Simple SMS 
            // IMessage sms = new SMS() { To = "+61Phone", Subject = "Simple Test", Content = "Simple Test", CallBackID = "callback_name" };
            //var result = await service.SendMessageAsync(sms);

            //sms = new SMS() { To = "+61phone", Subject = "Simple Test-2", Content = "Simple Test-2" };
            //result = await service.SendMessageAsync(sms);

            //// Simple SMS To wrong Number
            //sms = new SMS() { To = "+61phone", Subject = "Simple Test", Content = "Simple Test-Wrong Number" };
            //result = await service.SendMessageAsync(sms);

            // get the Messages Sent
            //List<MessageSentResponse> MessagesSent = new List<MessageSentResponse>();
            //var Messages = await service.GetMessagesAsync();
            //foreach (var msg in Messages)
            //{
            //    var msgresponse = (MessageSentResponse)await service.GetMessages(msg.ID);
            //    // print value of each attribute
            //    if (msgresponse != null && msgresponse.messageattributes != null)
            //    {
            //        foreach (var attr in msgresponse.messageattributes.attribute)
            //        {
            //            Console.WriteLine("Attribute:{0}={1}", attr.name, attr.value);
            //        }
            //        MessagesSent.Add(msgresponse);
            //    }
            //}

            // Now get the status of the messages sent in the last 24 hours
            //List<MessageStatusResponse> messageStatus = new List<MessageStatusResponse>();
            //var Messages = await service.GetMessagesAsync();
            //foreach (var msg in Messages)
            //{
            //    var msgresponse = await service.GetMessageStatus(msg.ID);
            //    // write to Log
            //    Console.WriteLine(msgresponse);
            //    //Console.ReadLine();
            //   // await service.SetMessageAsProcessed(msg.ID);
            //}

            // Get Responses from the messages if any
            //List<MessageResponseResponse> responses = new List<MessageResponseResponse>();
            //var MessageResponses = await service.GetMessagesAsync();
            //foreach (var msg in MessageResponses)
            //{
            //    var msgresponse = await service.GetMessageResponse(msg.ID);
            //    Console.WriteLine($"Reply : {msgresponse}");
                
            //    //await service.SetMessageAsprocessed(msg.ID));
            //}

            // Get Templates
            //List<TemplatesResponse> TemplateResponse = new List<TemplatesResponse>();
            //var Templates = (TemplatesResponse)await service.GetTemplates();
            //foreach (var template in Templates.messagetemplates)
            //{
            //    Console.Write(template.id);
            //    Console.Write(template.messageTemplateName);
            //    Console.Write(template.messageTemplateDescription);

            //    // send message using template
            //    MessageTemplate Mytemplate = new MessageTemplate();
            //    Mytemplate.MessageTemplateId = template.id;
            //    Mytemplate.Attributes = new List<MessageAttribute>();
            //    Mytemplate.Attributes.Add(new MessageAttribute() { Name = "attribute1", Value = DateTime.Now.ToString("hh:mm tt") });
            //    Mytemplate.Attributes.Add(new MessageAttribute() { Name = "attribute2", Value = DateTime.Now.ToString("dd/MM/yyyy") });
            //    IMessage sms = new SMS() { To = "+61phone", Subject = "Test Template Message", MessageTemplate = Mytemplate };
            //    var Templateresult = await service.SendMessageAsync(sms);
            //}

            //Console.ReadLine();
        }

        static void Main(string[] args)
        {
            try
            {
                MessagingTest().Wait();
            }
            catch (Exception ex)
            {

            }
        }
    }
    public static class AppConfig
    {
        public static string WhispirApiKey = ConfigurationManager.AppSettings["WhispirApiKey"];
        public static string WhispirAuthorization = ConfigurationManager.AppSettings["WhispirApiAuthorization"];
        public static int LoggingHours = Convert.ToInt32(ConfigurationManager.AppSettings["LoggingHours"]);
    }
}