using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using LinkedInLibrary.Models;
using Newtonsoft.Json;

namespace LinkedInLibrary
{
    public class BaseRequest
    {
        public static void Get<TSuccess, TError>(string url, string accesstoken, Func<TSuccess, object> onRequestSuccess, Func<TError, object> handleErrorResponse,
            Func<Exception, object> handleException=null,
            bool defaultHeaders = true,
           List<NameValuePair> additionalHeaders = default(List<NameValuePair>))
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
                    //set additional headers
                    if (defaultHeaders)
                    {
                        client.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
                    }

                    if (additionalHeaders != null)
                    {
                        foreach (NameValuePair kvp in additionalHeaders)
                        {
                            client.DefaultRequestHeaders.Add(kvp.name, kvp.value);
                        }
                    }
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = response.Content.ReadAsStringAsync().Result;
                        var data = JsonConvert.DeserializeObject<TSuccess>(responseBody);
                        onRequestSuccess(data);
                    }
                    else
                    {
                        var errorobject = JsonConvert.DeserializeObject<TError>(response.Content.ReadAsStringAsync().Result);
                        handleErrorResponse(errorobject);
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", ex.Message);
                    handleException?.Invoke(ex);
                }
            }
        }

        public static void Post<TSuccess, TError>(string url, string accesstoken,
           Func<HttpContent> setContent,
            Func<TSuccess, object> onRequestSuccess,
            Func<TError, object> handleErrorResponse,
           Func<Exception, object> handleException = null, 
           string httpMethod = "POST",
           bool defaultHeaders = true,
           List<NameValuePair> additionalHeaders = default(List<NameValuePair>)

           )
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
                    //set additional headers
                    if(defaultHeaders)
                    {
                        client.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
                    }
                    
                    if(additionalHeaders != null)
                    {
                        foreach (NameValuePair kvp in additionalHeaders)
                        {
                            client.DefaultRequestHeaders.Add(kvp.name, kvp.value);
                        }
                    }
                    var content = setContent();
                    HttpResponseMessage response = httpMethod == "POST" ? client.PostAsync(url, content).Result : client.PutAsync(url, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = response.Content.ReadAsStringAsync().Result;
                        var share = JsonConvert.DeserializeObject<TSuccess>(responseBody);
                        onRequestSuccess(share);
                    }
                    else
                    {
                        var errorobject = JsonConvert.DeserializeObject<TError>(response.Content.ReadAsStringAsync().Result);
                        handleErrorResponse(errorobject);
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", ex.Message);
                    handleException?.Invoke(ex);
                }
            }
        }
    }
}
