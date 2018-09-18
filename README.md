# Whispir.Messaging.SDK
SDK for Whispir Messaging

Download this Solution to your local drive and Build.
Make sure you have the latest Newtonsoft.Json DLL.

## To use this SDK
```
 IMessageService service = new MessageService(new WhispirSettings()
            {
                 ApiAuthorization = AppConfig.WhispirAuthorization,
                ApiBaseUrl = AppConfig.WhispirApiUrl,
                ApiKey = AppConfig.WhispirApiKey,
                LoggingFolder = @"{PATH TO YOUR LOGGING FOLDER}",
                DataBaseFolder= @"{PATH TO YOUR DB Folder}",
                LoggingHours = AppConfig.LoggingHours
            });
```
 > ApiAuthorization = "Basic Authentication Given to you By Whispir"
 
 > ApiBaseUrl = "Whispir API URL"
 
 > ApiKey = "Your Whispit API KEY"
 
 > LoggingFolder = "PATH TO YOUR LOGGING FOLDER" - Leave Empty if you dont Need Logging
 
 > DataBaseFolder = "PATH TO YOUR DB Folder" - If Empty will Attempt to use cache(ONLY ON IIS)
 
 > LoggingHours = "Hours To store Messagges, before the messages are deleted", range  24-240 hours
 
 > Make Sure the Folders Have Proper Read/Write Rights

 ## To Send a SMS
 ```
 IMessage sms = new SMS() { To = "000000000", Content = String.Format("Hello Stranger at {0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")), Subject = "Test",CallBackID="YourCallBackName" };
>If there is no Callback setup, leave callback blank
var result = await service.SendMessageAsync(sms);
```

>The Method will return a Message ID if Successful, if not an Error Message.

 ## To Send a SMS using a Template
 ```
  MessageTemplate template = new MessageTemplate();
  template.MessageTemplateId = "Your Template ID";
  template.Attributes = new List<MessageAttribute>();
  template.Attributes.Add(new MessageAttribute() { Name = "attribute1", Value = DateTime.Now.ToString("hh:mm tt") });
  template.Attributes.Add(new MessageAttribute() { Name = "attribute2", Value = DateTime.Now.ToString("dd/MM/yyyy") });
  IMessage sms = new SMS() { To = "Phone Number with Country Prefix",  Subject = "Test", MessageTemplate = template };
```

>The Method will return a Message ID if Successful, if not an Error Message.

## To Get a List Of Templates
 
```
var result = await service.GetTemplates();
```
## To Get a List Of Messages Sent
```
 // get the Messages Sent
 List<MessageSentResponse> MessagesSent = new List<MessageSentResponse>();
 var Messages = await service.GetMessagesAsync();
 foreach (var msg in Messages)
 {
     var msgresponse = (MessageSentResponse)await service.GetMessages(msg.ID);
     // print value of each attribute
     foreach(var attr in msgresponse.messageattributes.attribute)
     {
         Console.WriteLine("Attribute:{0}={1}", attr.name, attr.value);
     }
     MessagesSent.Add(msgresponse);
 }
```
 ## To Get a Message Status
 
```
_service.GetMessageStatus({Message ID})
```

>To Get the Message Status of all Message Sent. Please note Can only view Messages of the last 24 Hours

>If you don't have the Message ID, You have to First Get a List Of Messages and Then send a Status Request for Each Message.

```
 List<MessageStatusResponse> messageStatus = new List<MessageStatusResponse>();
 
 var result = await _service.GetMessagesAsync();
 foreach(var msg in result)
 {
   messageStatus.Add((MessageStatusResponse)await _service.GetMessageStatus(msg.ID));
 }
 ```


 ## To Get a Message Response(s)
 
```
_service.GetMessageResponse({Message ID})
```

>To Get the Message GetMessageResponse of all Message Sent - Please note Can only view Messages of the last 24 Hours

>If you don't have the Message ID, You have to First Get a List Of Messages and Then send a Message Response Request for Each Message.

```
 List<MessageResponseResponse> responses = new List<MessageResponseResponse>();
 var result = await _service.GetMessagesAsync();
 foreach (var msg in result)
 {
   responses.Add((MessageResponseResponse)await _service.GetMessageResponse(msg.ID));
 }

```


  ## To Get a List Of Messages That Failed
 
```
 // get CallBacks
  var CallBacks =await service.GetCallBacks();
  // You can Get the call information by iterating the calls
  foreach(var call in CallBacks)
  {
      var responseMessage = call.responseMessage.content;
  }
  // To Update calls with the status to Success, send a List<Calls> to the updateCallStatus
  // will return OK if the update was successfull or null if it failed.
  var status = await service.updateCallStatus(CallBacks);
```
