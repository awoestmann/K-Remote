using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Utils
{
    static class JsonFactory
    {
        public static string getPlaylists()
        {
            return "{ \"jsonrpc\": \"2.0\", \"id\": 1, \"method\": \"Playlist.GetPlaylists\"}";
        }
        public static string playPause()
        {
            return "{ \"jsonrpc\": \"2.0\", \"method\": \"Player.PlayPause\", \"params\": { \"playerid\": 0 }, \"id\": 1}";
        }
    }
}
