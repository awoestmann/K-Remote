using K_Remote.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace K_Remote.Wrapper
{
    class FileRPC
    {
        public static async Task<string> prepareDownloadFile(string filename)
        {
            string pathJson = await ConnectionHandler.getInstance().sendHttpRequest("Files.PrepareDownload", 
                new JObject(
                    new JProperty("path", filename)
                )
            );
            
            Debug.WriteLine("FileRPC.prepareDownload: response" + pathJson);
            dynamic jsonObject = JObject.Parse(pathJson);
            return jsonObject.result.details.path;
        }

        public static async Task<IInputStream> downloadFile(string path)
        {
            IInputStream buffer = await ConnectionHandler.getInstance().downloadFile(path);
            if(buffer == null)
            {
                Debug.WriteLine("FileRPC.downloadFile: No Buffer received");
            }
            Debug.WriteLine("FileRPC.downloadFile: return Buffer Object");
            return buffer;
        }
    }
}
