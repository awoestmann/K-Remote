using K_Remote.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Wrapper
{
    class GuiRPC
    {
        public static async void toggleGui()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Gui.SetFullscreen", new JObject(new JProperty("fullscreen", "toggle")));
        }
    }
}
