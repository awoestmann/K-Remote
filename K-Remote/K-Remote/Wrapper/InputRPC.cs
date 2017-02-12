using K_Remote.Models;
using K_Remote.Resources;
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
            await ConnectionHandler.getInstance().sendHttpRequest("Input.Back");
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

        public static async Task inputExecuteAction(string action)
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.ExecuteAction",
                new JObject(
                    new JProperty("action", action)
                )
             );
        }

        public static async Task left()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.Left");
        }

        public static async Task right()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.Right");
        }

        public static async Task select()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.Select");
        }

        public static async Task sendText(string text, bool done)
        {
            JObject jsonObject = new JObject(
                new JProperty("text", text),
                new JProperty("done", done)
            );            
            await ConnectionHandler.getInstance().sendHttpRequest("Input.SendText", jsonObject);
        }

        /// <summary>
        /// Toggle osd
        /// </summary>
        /// <returns>Task</returns>
        public static async Task showOSD()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.ShowOSD");
        }

        /// <summary>
        /// Skips in direction if player is in foreground
        /// </summary>
        /// <param name="direction">Direction string: stepforward, stepback, bigstepforward, bigstepback</param>
        /// <returns>Task</returns>
        public static async Task skipIfinPlayer(string direction)
        {
            if(direction != null && direction != "")
            {
                GuiCurrentWindow window = await GuiRPC.getCurrentWindow();
                if(window.id == Constants.CURRENT_WINDOW_FULLSCREEN_VIDEO
                    || window.id == Constants.CURRENT_WINDOW_FULLSCREEN_VIDEO_INFO)
                {
                    await inputExecuteAction(direction);
                }
            }
        }

        /// <summary>
        /// Move cursor up
        /// </summary>
        /// <returns>Tasl</returns>
        public static async Task up()
        {
            await ConnectionHandler.getInstance().sendHttpRequest("Input.Up");
        }
    }
}
