using K_Remote.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Wrapper
{
    static class ApplicationRPC
    {
        public static bool checkHttpConnection()
        {
            getActivePlayers();
            return false;
        }

        public static async Task<int[]> getActivePlayers()
        {
            ConnectionHandler handler = ConnectionHandler.getInstance();
            var playersJson = await handler.sendHttpRequest("{\"jsonrpc\": \"2.0\", \"method\": \"Player.GetActivePlayers\", \"id\": 1}");
            Debug.WriteLine(playersJson);

            return new int[1];
        }
    }
}
