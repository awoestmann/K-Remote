using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
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
        private string tcpPortString = "9090";

        private HostName host;
        private StreamSocket tcpSocket;
        private bool tcpConnected;

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

            //Init tcp
            host = new HostName(hostString);
            tcpSocket = new StreamSocket();
            tcpConnected = false;
        }

        public async void sendHttpRequest(string jsonString)
        {
            try
            {
                //httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
                httpClient.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new HttpCredentialsHeaderValue("Basic", Convert.ToBase64String( System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "xbmc", "xbmc"))));
               
                Uri requestUri = new Uri("http://" + host + ":" + httpPortString + "/jsonrpc");
                //Send the GET request asynchronously and retrieve the response as a string.
                HttpResponseMessage httpResponse = new HttpResponseMessage();
                string httpResponseBody = "";

                try
                {
                    
                    httpResponse = await httpClient.PostAsync(requestUri, new HttpStringContent(jsonString));
                    
                    Debug.WriteLine(httpResponse);
                    httpResponse.EnsureSuccessStatusCode();
                    
                    httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                }

            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
            }
            

        }
    }
}
