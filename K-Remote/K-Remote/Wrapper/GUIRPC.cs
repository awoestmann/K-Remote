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
    /// <summary>
    /// Static class wrapping gui methods
    /// </summary>
    static class GuiRPC
    {
        /// <summary>
        /// Toggle menu
        /// </summary>
        public static async void toggleGui()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Gui.SetFullscreen", new JObject(new JProperty("fullscreen", "toggle")));
        }

        /// <summary>
        /// Get current windows
        /// </summary>
        /// <returns>Current window object</returns>
        public static async Task<GuiCurrentWindow> getCurrentWindow()
        {
            GuiPropertiesResponse response = await guiGetProperties(new string[] { "currentwindow" });
            if(response != null)
            {
                Debug.WriteLine("GuiRPC: current window ID - label: " + response.result.currentwindow.id + " - " + response.result.currentwindow.label);
                return response.result.currentwindow;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get gui properties
        /// </summary>
        /// <param name="properties">Property names</param>
        /// <returns>Propeties object</returns>
        public static async Task<GuiPropertiesResponse> guiGetProperties(string[] properties)
        {
            if(properties != null && properties.Length > 0)
            {
                string responseString = await ConnectionHandler.getInstance().sendHttpRequest("GUI.GetProperties",
                    new JObject(
                        new JProperty("properties", properties)
                    )
                );
                try
                {
                    GuiPropertiesResponse responseObject = JsonConvert.DeserializeObject<GuiPropertiesResponse>(responseString);
                    return responseObject;
                }
                catch (JsonReaderException)
                {
                    Debug.WriteLine("GuiRPC.guiGetProperties: Error on parsing " + Environment.NewLine + responseString);
                    return null;
                }
            }
            return null;

        }
    }
}
