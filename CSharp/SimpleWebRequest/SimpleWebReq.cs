using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebRequest
{
    public class SimpleWebRequest 
    {
        private string authorizationHeader = "";
        public string HTTP_Method { get; set; } = "";
        public string UserAgent { get; set; } = "YourUserAgent";
        public string AcceptType { get; set; } = "application/json";
        public string ContentType { get; set; } = "application/json";
        public bool AllowAutoRedirect { get; set; } = true;
        public int MaximumAuthomaticRedirections { get; set; } = 1;

        /// <summary>
        /// Adds basic authorization header (base64 encrypted username:password)
        /// For some requests you might have to change encoding to ASCII or Default
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void SetBasicAuthentication(string username, string password)
        {
            authorizationHeader = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
        }

        /// <summary>
        /// Required for most RESTful API's
        /// </summary>
        /// <param name="token"></param>
        public void SetTokenAuthentication(string token)
        {
            authorizationHeader = "Bearer " + token;
        }

        public ResponseObject GetServerResponse(string url, string body)
        {
            ResponseObject responseResult = new ResponseObject();

            HttpWebResponse response = null;
            try
            {if (url.ToLower().StartsWith("https")) ValidateSSL(); // otherwise we will get nasty ssl exceptions

                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(url);

                if (!string.IsNullOrEmpty(authorizationHeader)) //Only add it if it is set
                    webReq.Headers.Add("Authorization", authorizationHeader);

                if (string.IsNullOrEmpty(body)) // if method is not specified explicitly - assume GET or POST
                    webReq.Method = string.IsNullOrEmpty(body) ? "GET" : "POST";

                webReq.Accept = AcceptType;
                webReq.AllowAutoRedirect = AllowAutoRedirect;
                webReq.MaximumAutomaticRedirections = MaximumAuthomaticRedirections;
                webReq.UserAgent = UserAgent;

                if (!string.IsNullOrEmpty(body)) // if we have something to post - handle it here
                {
                    webReq.ContentType = ContentType;
                    byte[] byteBody = Encoding.UTF8.GetBytes(body);
                    webReq.ContentLength = byteBody.Length;

                    using (var requestWriter = webReq.GetRequestStream())
                    {
                        requestWriter.Write(byteBody, 0, byteBody.Length);
                    }
                }

                response = (HttpWebResponse)webReq.GetResponse();
                responseResult.StatusCode = response.StatusCode;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                   /*   Also valid option :
                    
                   // HttpContent responseContent = response.Content; // Would need to cast as different type, not HttpWebResponse
                   // var result = await responseContent.ReadAsStringAsync();

                   */

                    using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        responseResult.Content = reader.ReadToEnd();
                    }
                }
                else responseResult.ErrorMessage = response.StatusDescription;
            }
            catch (WebException ex)
            {
                using (var reader = new System.IO.StreamReader(ex.Response.GetResponseStream()))
                {
                    responseResult.ErrorMessage = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                responseResult.ErrorMessage = ex.Message;
            }
            return responseResult;
        }

        private void ValidateSSL()
        {
            // allows for validation of SSL conversations - only initialise on first call
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
        }
    }
}
