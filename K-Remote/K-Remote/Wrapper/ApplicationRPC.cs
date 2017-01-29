using K_Remote.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Wrapper
{
    class ApplicationRPC
    {
        public static async void setMute()
        {
            string response = await ConnectionHandler.getInstance().sendHttpRequest("Application.SetMute", new JObject(new JProperty("mute", "toggle")));
        }
        public static async void setVolume(int volume)
        {
            string response = await ConnectionHandler.getInstance().sendHttpRequest("Application.SetVolume", new JObject(new JProperty("volume", volume)));
        }
    }
}
