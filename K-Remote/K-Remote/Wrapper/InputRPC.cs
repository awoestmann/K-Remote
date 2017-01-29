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
            ConnectionHandler handler = ConnectionHandler.getInstance();            
            await handler.sendHttpRequest("Input.Back", null);
        }

        public static async void down()
        {
            ConnectionHandler handler = ConnectionHandler.getInstance();
            await handler.sendHttpRequest("Input.Down", null);
        }

        public static async void left()
        {
            ConnectionHandler handler = ConnectionHandler.getInstance();
            await handler.sendHttpRequest("Input.Left", null);
        }

        public static async void right()
        {
            ConnectionHandler handler = ConnectionHandler.getInstance();
            await handler.sendHttpRequest("Input.Right", null);
        }

        public static async void select()
        {
            ConnectionHandler handler = ConnectionHandler.getInstance();
            await handler.sendHttpRequest("Input.Select", null);
        }

        public static async void up()
        {
            ConnectionHandler handler = ConnectionHandler.getInstance();
            await handler.sendHttpRequest("Input.Up", null);
        }
    }
}
