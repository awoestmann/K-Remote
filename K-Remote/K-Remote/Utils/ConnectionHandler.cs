﻿using K_Remote.Models;
using K_Remote.Utils;
using K_Remote.Wrapper;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace K_Remote.Utils
{
    class ConnectionHandler
    {
        private bool tcpConnected = false;

        private readonly string RELATIVE_PATH = "/jsonrpc";
        private readonly string MEDIA_TYPE = "application/json";

        private string hostString;
        private string httpPortString;
        private string tcpPortString = "9090";
        private string loginBase64;
        private string conName;
        
        private HttpClient httpClient;
        private StreamWebSocket webSocket;

        public event EventHandler<connectionStateChangedEventArgs> ConnectionStateChanged;

        private static ConnectionHandler instance = null;

        public static ConnectionHandler getInstance()
        {
            if(instance != null)
            {
                return instance;
            }
            else
            {
                instance = new ConnectionHandler();
                return instance;
            }
        }        

        public string getConnectionString()
        {
            return conName + "@" + hostString+ ":" + httpPortString;
        }

        public ConnectionHandler()
        {
            App.Current.Resuming += new EventHandler<Object>(resume);
        }

        public async Task refreshConnection()
        {
            //http
            httpClient = new HttpClient();

            //websocket           
            webSocket = new StreamWebSocket();
            //webSocket.Closed += webSocketClosed;

            Connection current = SettingsManager.getInstance().getCurrentConnection();
            if (current == null)
            {
                tcpConnected = false;                

                hostString = ""; 
                httpPortString = ""; ;
                loginBase64 = "";
                conName = "";

                OnConnectionStateChanged(new connectionStateChangedEventArgs("", 1, false));
                //TODO handle no connection
            }
            else
            {
                hostString = current.host;
                httpPortString = current.httpPort.ToString();
                loginBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(current.username + ":" + current.password));
                conName = current.description;
                try
                {
                    if (await checkHttpConnection())
                    {
                        await connectTcp();
                    }
                    else
                    {
                        OnConnectionStateChanged(new connectionStateChangedEventArgs("", 0, false));
                        tcpConnected = false;
                        showErrorDialog();
                        return;
                    }

                }
                catch (Exception ex)
                {
                    OnConnectionStateChanged(new connectionStateChangedEventArgs("", 1, false));
                    tcpConnected = false;
                    Debug.WriteLine("ConnectionHandler.refreshConnectionData: Error on connect: " + ex);
                    showErrorDialog();
                }
            }
        }

        /// <summary>
        /// Downloads file at path via GET request
        /// </summary>
        /// <param name="path">relative URI to file</param>
        /// <returns></returns>
        public async Task<IInputStream> downloadFile(string path)
        {
            Uri requestUri = new Uri("http://" + hostString + ":" + httpPortString + "/" + path);
            Debug.WriteLine("ConnectionHandler.DownloadFile: Downloading file: " + requestUri);
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            
            requestMessage.Headers.Authorization = new HttpCredentialsHeaderValue("Basic", loginBase64);

            HttpResponseMessage httpResponse = new HttpResponseMessage();
            IInputStream httpResponseBody;

            try
            {
                httpResponse = await httpClient.SendRequestAsync(requestMessage);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsInputStreamAsync();
                return httpResponseBody;
            }
            catch (Exception ex)
            {
                Debug.Write("ConnectionHandler.DownloadFile: " + ex);
                return null;
            }
        }

        /// <summary>
        /// Sends a json object via http
        /// </summary>
        /// <param name="jsonString">JSON to send as a string</param>
        /// <returns></returns>
        public async Task<string> sendHttpJson(string jsonString)
        {
            Uri requestUri;
            try
            {
                requestUri = new Uri("http://" + hostString + ":" + httpPortString + RELATIVE_PATH);
            }
            catch(UriFormatException uriEx)
            {
                Debug.WriteLine("ConnectionHandler.sendHttpJson: Bad URI :" + uriEx);
                return null;
            }
            
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

            requestMessage.Headers.Accept.Add(new HttpMediaTypeWithQualityHeaderValue(MEDIA_TYPE));
            requestMessage.Headers.Authorization = new HttpCredentialsHeaderValue("Basic", loginBase64);
            requestMessage.Content = new HttpStringContent(jsonString);
            requestMessage.Content.Headers.ContentType = new HttpMediaTypeHeaderValue(MEDIA_TYPE);

            HttpResponseMessage httpResponse = new HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                httpResponse = await httpClient.SendRequestAsync(requestMessage);
                httpResponse.EnsureSuccessStatusCode();
                
                //Read body as Buffer and read as string to prevent wrong encoding
                IBuffer data = await httpResponse.Content.ReadAsBufferAsync();
                DataReader dataReader = DataReader.FromBuffer(data);
                httpResponseBody = dataReader.ReadString(data.Length);
                return httpResponseBody;
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                return null;
            }           

        }

        private async void showErrorDialog()
        {
            await new MessageDialog("Can't establish connection to: " + hostString + ":" + httpPortString, "Information").ShowAsync();
        }

        /// <summary>
        /// Sends a default request json with method and parameters to jsonrpc uri
        /// </summary>
        /// <param name="method">Method</param>
        /// <param name="param">Parameter if needed</param>
        /// <returns>Response JSON</returns>
        public async Task<string> sendHttpRequest(string method, JObject param, string id = "1")
        {
            return await this.sendHttpJson(getRequestJson(method, param, id).ToString());
        }

        /// <summary>
        /// Sends a default request json with method and no parameters to jsonrpc uri
        /// </summary>
        /// <param name="method">Method</param>
        /// <returns></returns>
        public async Task<string> sendHttpRequest(string method, string id = "1")
        {
            return await this.sendHttpRequest(method, null, id);
        }

        public async Task<bool> checkHttpConnection()
        {
            if(await PlayerRPC.getActivePlayers() == null)
            {
                OnConnectionStateChanged(new connectionStateChangedEventArgs(conName, 0, false));
                return false;
            }
            else
            {
                OnConnectionStateChanged(new connectionStateChangedEventArgs(conName, 0, true));
                return true;
            }
        }

        public JObject getRequestJson(string method, JObject param, string id = "1")
        {
            if(method == null)
            {
                throw new ArgumentException("Empty method not allowed");
            }
            JObject requestObject = new JObject(
                new JProperty("jsonrpc", "2.0"),
                new JProperty("method", method),
                new JProperty("id", id)
            );            

            if (param != null)
            {
                requestObject.Add(new JProperty("params", param));
            }
            return requestObject;
        }

        private async Task connectTcp()
        {
            Debug.WriteLine("ConnectionHandler.connectTpc: Connecting TCP to " + hostString + ":" + tcpPortString);
            if (tcpConnected)
            {
                try
                {
                    webSocket.Close(1000, "");
                    //webSocket.Dispose();
                    
                }
                catch (Exception e)
                {
                    Debug.WriteLine("ConnectionHandler.connectTcp: Error on disposing socket");
                }
                finally
                {
                    webSocket = new StreamWebSocket();
                }
            }
            Uri uri = new Uri("ws://" + hostString + ":" + tcpPortString + "/jsonrpc");
            try
            {
                await webSocket.ConnectAsync(uri);
                tcpConnected = true;
                Task receiving = receiveWebSocketMessage(webSocket);
                OnConnectionStateChanged(new connectionStateChangedEventArgs(conName, 1, true));
                
            }
            catch(Exception e)
            {
                OnConnectionStateChanged(new connectionStateChangedEventArgs(conName, 1, false));
                tcpConnected = false;
                Debug.WriteLine("ConnectionHandler.connectTcp: Connection to " + hostString + " failed: ");
                showErrorDialog();
            }
        }

        public bool checkTcpConnection()
        {
            return tcpConnected;
        }

        private async Task receiveWebSocketMessage(StreamWebSocket sender)
        {
            Debug.WriteLine("Message received");
            Stream readStream = webSocket.InputStream.AsStreamForRead();
            try
            {
                byte[] readBuffer = new byte[1000];
                while (true)
                {
                    readBuffer = new byte[1000];
                    if (webSocket != sender)
                    {
                        Debug.WriteLine("Socket no longer active");
                        return;
                    }
                    int read = await readStream.ReadAsync(readBuffer, 0, readBuffer.Length);
                    if(read > 0)
                    {
                        byte[] message = new byte[read];
                        Array.Copy(readBuffer, message, read);
                        NotificationRPC.getInstance().processNotification(System.Text.Encoding.UTF8.GetString(message));
                    }
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("Error in receive: " + e);
                //try reconnect
                connectTcp();
            }
        }
        
        private void webSocketClosed(IWebSocket sender, WebSocketClosedEventArgs args)
        {
            OnConnectionStateChanged(new connectionStateChangedEventArgs(conName, 1, false));
            this.tcpConnected = false;
        }
        
        protected virtual void OnConnectionStateChanged(connectionStateChangedEventArgs args)
        {
            ConnectionStateChanged?.Invoke(this, args);
        }

        private void resume(object sender, object args)
        {
            Debug.WriteLine("Connection Handler resuming");
            if (tcpConnected)
            {
                OnConnectionStateChanged(new connectionStateChangedEventArgs(conName, 1, true));
            }
        }
    }
}
