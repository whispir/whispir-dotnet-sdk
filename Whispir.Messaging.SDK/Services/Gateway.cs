using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
    public class Gateway : IDisposable
    {
        /// <summary>
        /// Taken snippets of code from https://github.com/ghuntley/send-sms-via-whispir
        /// </summary>
        WhispirSettings _settings;
        Logging _logger;
        //public readonly string ContentType = "application/vnd.whispir.message-v1+json";
        public Gateway(WhispirSettings settings, Logging logger)
        {
            _settings = settings;
            _logger = logger;
        }
        string insertQueryAPIKey(string query)
        {
            return String.Format(query, _settings.ApiKey);
        }

        public HttpClient GetHttpClient(string ContentType)
        {
            var client = new HttpClient();
            if(!String.IsNullOrEmpty(ContentType))
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _settings.ApiAuthorization);
            client.DefaultRequestHeaders.Add("x-api-key", _settings.ApiKey);

            return client;
        }
        public async Task<string> getEndpoint()
        {
            string url =  String.Format("https://api.{0}.whispir.com", _settings.APIEndpoint.ToString());
           // string url = "https://api.whispir.com/";
            return url;
        }
        public async Task<MessageResponse> Post<T>(T entity, string query, string ContentType)
        {
            MessageResponse messageResponse = new MessageResponse();
            try
            {
                var client = GetHttpClient(ContentType);
                string address = (await getEndpoint()) + insertQueryAPIKey(query);
                client.Timeout = TimeSpan.FromSeconds(30);
                var jsonRequest = JsonConvert.SerializeObject(entity);
                var content = new StringContent(jsonRequest, Encoding.UTF8, ContentType);

                var response = await client.PostAsync(address, content);
                var responsecontent = await response.Content.ReadAsStringAsync();
                _logger.LogInfo("Post Raw Response:" + responsecontent);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Accepted:
                        _logger.LogInfo("Message OK");
                        messageResponse.MessageID = response.Headers.Location.Segments[2];
                        break;
                    case HttpStatusCode.Unauthorized:
                        messageResponse.ErrorMessage = "The base64 representation of the Whispir Username and Password specified in the Authorization HTTP header was incorrect.";
                        _logger.LogInfo(messageResponse.ErrorMessage);
                        break;
                    default:
                        messageResponse.ErrorMessage = "The request has been rejected by the Whispir API.";
                        _logger.LogInfo(messageResponse.ErrorMessage);
                        break;
                }
            }
            catch (Exception ex) {
                messageResponse.ErrorMessage = String.Format("Run Time Error:{0}", ex.Message);
            }
            return messageResponse;
        }
        public async Task<MessageResponse> Put<T>(T entity, string query, string ContentType)
        {
            MessageResponse messageResponse = new MessageResponse();
            try
            {
                var client = GetHttpClient(ContentType);
                string address = (await getEndpoint()) + insertQueryAPIKey(query);
                client.Timeout = TimeSpan.FromSeconds(30);
                var jsonRequest = JsonConvert.SerializeObject(entity);
                var content = new StringContent(jsonRequest, Encoding.UTF8, ContentType);

                var response = await client.PutAsync(address, content);
                var responsecontent = await response.Content.ReadAsStringAsync();
                _logger.LogInfo("Post Raw Response:" + responsecontent);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Accepted:
                        _logger.LogInfo("Message OK");
                        messageResponse.MessageID = response.Headers.Location.Segments[2];
                        break;
                    case HttpStatusCode.Unauthorized:
                        messageResponse.ErrorMessage = "The base64 representation of the Whispir Username and Password specified in the Authorization HTTP header was incorrect.";
                        _logger.LogInfo(messageResponse.ErrorMessage);
                        break;
                    default:
                        messageResponse.ErrorMessage = "The request has been rejected by the Whispir API.";
                        _logger.LogInfo(messageResponse.ErrorMessage);
                        break;
                }
            }
            catch (Exception ex)
            {
                messageResponse.ErrorMessage = String.Format("Run Time Error:{0}", ex.Message);
            }
            return messageResponse;
        }
        public async Task<T> Request<T>(string query, string ContentType)
        {
            try
            {
                var client = GetHttpClient(ContentType);
                string address = (await getEndpoint()) + insertQueryAPIKey(query);
                _logger.LogInfo("Request:" + address);
                client.Timeout = TimeSpan.FromSeconds(30);
                var response = await client.GetAsync(address);
                var responsecontent = await response.Content.ReadAsStringAsync();
                _logger.LogInfo("Get Raw Response:" + responsecontent);
                return JsonConvert.DeserializeObject<T>(responsecontent);
            }
            catch (Exception ex)
            {
                _logger.LogInfo(String.Format("Run Time Error:{0}", ex.Message));
            }
            return default(T);
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
        // ~Gateway() {
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
