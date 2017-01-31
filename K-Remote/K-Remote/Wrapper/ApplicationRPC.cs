using K_Remote.Models;
using K_Remote.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Wrapper
{
    class ApplicationRPC
    {
        public static async void toggleMute()
        {
            string response = await ConnectionHandler.getInstance().sendHttpRequest("Application.SetMute", new JObject(new JProperty("mute", "toggle")));
        }
        public static async void setVolume(int volume)
        {
            string response = await ConnectionHandler.getInstance().sendHttpRequest("Application.SetVolume", new JObject(new JProperty("volume", volume)));
        }

        public static async Task<int> getVolume()
        {
            string response = await ConnectionHandler.getInstance().sendHttpRequest("Application.GetProperties",
                new JObject(new JProperty("properties", new String[1] { "volume" })));
            Debug.WriteLine(response);
            return JsonConvert.DeserializeObject<ApplicationProperties>(response).result.volume;
        }
    }
}
