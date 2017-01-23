using K_Remote.Wrapper;
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
        private string hostString = "192.168.0.18";
        private string httpPortString = "44556";

        //http
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

        public ConnectionHandler()
        {
            //Init http
            httpClient = new HttpClient();
        }

        public async Task<String> sendHttpRequest(string jsonString)
        {
            Uri requestUri = new Uri("http://" + hostString + ":" + httpPortString + "/jsonrpc");
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

            requestMessage.Headers.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Headers.Authorization = new HttpCredentialsHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "xbmc", "xbmc"))));
            requestMessage.Content = new HttpStringContent(jsonString);
            requestMessage.Content.Headers.ContentType = new HttpMediaTypeHeaderValue("application/json");

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

        public async Task<bool> checkHttpConnection()
        {
            if(await ApplicationRPC.getActivePlayers() == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
