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
    class InputRPC
    {
        /// <summary>
        /// Sends Input.Back JSON to move cursor up
        /// </summary>
        public static async Task back()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.Back", null);
        }

        /// <summary>
        /// Sends Input.ContextMenu to show context Menu
        /// </summary>
        public static async Task contextMenu()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.ContextMenu");
        }

        public static async Task down()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.Down");
        }

        public static async Task home()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.Home");
        }

        public static async Task info()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.Info");
        }

        public static async Task left()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.Left", null);
        }

        public static async Task right()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.Right", null);
        }

        public static async Task select()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.Select", null);
        }

        public static async Task sendText(string text, bool done)
        {
            JObject jsonObject = new JObject(
                new JProperty("text", text),
                new JProperty("done", done)
            );            
            await ConnectionHandler.getInstance().sendHttpRequest("Input.SendText", jsonObject);
        }

        public static async Task up()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.Up", null);
        }
    }
}
