using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CSharp;
using Newtonsoft.Json;

namespace CardFez
{
    public class RestClient : IRestClient
    {

        private string _authToken = "a9fc32e7-84d0-462f-bad9-d578fc33ff72";
        private static readonly RestClient _instance = new RestClient();
        static RestClient()
        {
        }
        public static RestClient Instance
        {
            get
            {
                return _instance;
            }
        }
        private HttpClient GetClient(HttpClientHandler handler = null)
        {
            try
            {
                var client = handler == null ? new HttpClient() : new HttpClient(handler, true);
                client.Timeout = TimeSpan.FromSeconds(360);
                return client;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error while Sending Request : " + ex.Message);
            }
            return null;
        }
        private void Dispose()
        {
        }
        private Task<HttpResponseMessage> RequestAsync(HttpMethod method, string url, object payload = null)
        {

            try
            {

                var request = PrepareRequest(method, url, payload);

                if (_authToken != null)
                {
                    request.Headers.Add("Authorization", _authToken.ToString());
                }
                return GetClient().SendAsync(request, HttpCompletionOption.ResponseContentRead);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error while Sending Request : " + ex.Message);
            }

            //else
            //    Application.Current.MainPage = new Views.HomePages.ErrorPage();
            return null;

        }
        private HttpRequestMessage PrepareRequest(HttpMethod method, string url, object payload)
        {
            try
            {
                var uri = PrepareUri(url);
                var request = new HttpRequestMessage(method, uri);
                if (payload != null)
                {
                    var json = JsonConvert.SerializeObject(payload);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }
                return request;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error while Sending Request : " + ex.Message);
            }
            return null;
        }
        private Uri PrepareUri(string url)
        {
            return new Uri(url);
        }

        private readonly Action<HttpStatusCode, string> _defaultErrorHandler = (statusCode, body) =>
        {
            if (statusCode < HttpStatusCode.OK || statusCode >= HttpStatusCode.BadRequest)
            {
                Debug.WriteLine(string.Format("Request responded with status code={0}, response={1}", statusCode, body));

            }
        };
        private void HandleIfErrorResponse(HttpStatusCode statusCode, string content, Action<HttpStatusCode, string> errorHandler = null)
        {
            if (errorHandler != null)
            {
                errorHandler(statusCode, content);
            }
            else
            {
                _defaultErrorHandler(statusCode, content);
            }
        }
        private T GetValue<T>(String value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        public string GetResponse { get; set; }
        public async Task<T> GetAsync<T>(string url, bool useAuthToken =true)
        {
            try
            {
                
                HttpResponseMessage response = await RequestAsync(HttpMethod.Get, url).ConfigureAwait(false);
                GetResponse = response.ToString();
                

                var jsonString = await response.Content.ReadAsStringAsync();
                HandleIfErrorResponse(response.StatusCode, jsonString);
               
                var shrineCard = JsonConvert.DeserializeObject(jsonString).ToString();

                return JsonConvert.DeserializeObject<T>(shrineCard);
            }
            catch (System.Net.WebException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in GET Request :" + ex.Message);
            }
            return default(T);
        }
      


    }
}
