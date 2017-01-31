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
        public static async void back()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.Back", null);
        }

        /// <summary>
        /// Sends Input.ContextMenu to show context Menu
        /// </summary>
        public static async void contextMenu()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.ContextMenu");
        }

        public static async void down()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.Down");
        }

        public static async void info()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.Info");
        }

        public static async void left()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.Left", null);
        }

        public static async void right()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.Right", null);
        }

        public static async void select()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.Select", null);
        }

        public static async void sendText(string text, bool done)
        {
            JObject jsonObject = new JObject(
                new JProperty("text", text),
                new JProperty("done", done.ToString())
            );            
            await ConnectionHandler.getInstance().sendHttpRequest("Input.SendText", jsonObject);
        }

        public static async void up()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.Up", null);
        }
    }
}
