using K_Remote.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Wrapper
{
    class FileRPC
    {
        public static async Task<string> prepareDownloadFile(string filename)
        {
            string pathJson = await ConnectionHandler.getInstance().sendHttpRequest("Files.PrepareDownload", new JObject(new JProperty("path", filename)));
            
            Debug.WriteLine("FileRPC.prepareDownload: " + pathJson);
            return pathJson;
        }

        public static async Task<byte[]> downloadFile(string path)
        {
            string file = await ConnectionHandler.getInstance().sendHttpRequest("Files.Download", new JObject(new JProperty("path", path)));
            Debug.WriteLine("FileRPC.downloadFile: " + file);
            return null;
        }
    }
}
