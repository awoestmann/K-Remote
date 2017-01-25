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
            string fileJson = await ConnectionHandler.getInstance().sendHttpRequest("Files.PrepareDownload", new JObject(new JProperty("path", filename)));
            
            Debug.WriteLine(fileJson);
            return fileJson;
        }
    }
}
