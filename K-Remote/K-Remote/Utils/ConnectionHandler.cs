using K_Remote.Wrapper;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace K_Remote.Utils
{
    /**
     *  Utility class to handle tcp/http connections
     *  Implemented as a singleton
     */
    class ConnectionHandler
    {

        private readonly string RELATIVE_PATH = "/jsonrpc";
        private readonly string MEDIA_TYPE = "application/json";

        private string hostString = "192.168.0.18";
        private string httpPortString = "44556";
        
        private HttpClient httpClient;

        private static ConnectionHandler instance = null;

        public static ConnectionHandler getInstance()
        {
            if(ConnectionHandler.instance != null)
            {
                return ConnectionHandler.instance;
            }
            else
            {
                ConnectionHandler.instance = new ConnectionHandler();
                return ConnectionHandler.instance;
            }
        }        

        public string getConnectionString()
        {
            return hostString+ ":" + httpPortString;
        }

        public ConnectionHandler()
        {
            httpClient = new HttpClient();
        }

        public async Task<String> sendHttpRequest(string jsonString)
        {
            Uri requestUri = new Uri("http://" + hostString + ":" + httpPortString + RELATIVE_PATH);
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

            requestMessage.Headers.Accept.Add(new HttpMediaTypeWithQualityHeaderValue(MEDIA_TYPE));
            requestMessage.Headers.Authorization = new HttpCredentialsHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "xbmc", "xbmc"))));
            requestMessage.Content = new HttpStringContent(jsonString);
            requestMessage.Content.Headers.ContentType = new HttpMediaTypeHeaderValue(MEDIA_TYPE);

            HttpResponseMessage httpResponse = new HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                httpResponse = await httpClient.SendRequestAsync(requestMessage);
                httpResponse.EnsureSuccessStatusCode();                    
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                return httpResponseBody;
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                return null;
            }           

        }

        /// <summary>
        /// Sends a default request json with method and parameters
        /// </summary>
        /// <param name="method">Method</param>
        /// <param name="param">Parameter if needed</param>
        /// <returns>Response JSON</returns>
        public async Task<string> sendHttpRequest(string method, JObject param)
        {
            return await this.sendHttpRequest(getRequestJson(method, param).ToString());
        }

        public async Task<bool> checkHttpConnection()
        {
            if(await PlayerRPC.getActivePlayers() == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public JObject getRequestJson(string method, JObject param)
        {
            if(method == null)
            {
                throw new ArgumentException("Empty method not allowed");
            }
            JObject jObject = new JObject(
                new JProperty("jsonrpc", "2.0"),
                new JProperty("method", method),
                new JProperty("id", 1)
            );
            if (param != null)
            {
                jObject.Add(new JProperty("params", param));
            }
            return jObject;
        }
    }
}
