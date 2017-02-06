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
    static class PlayerRPC
    {
        /// <summary>
        /// Sends Player.GetActivePlayers JSON to get active Players
        /// </summary>
        /// <returns>Array of Player instances</returns>
        public static async Task<Player[]> getActivePlayers()
        {
            ConnectionHandler handler = ConnectionHandler.getInstance();
            var responseJson = await handler.sendHttpRequest("Player.GetActivePlayers");
            ActivePlayers players = JsonConvert.DeserializeObject<ActivePlayers>(responseJson);        
            return players.result;
        }

        public static async Task<PlayerItem> getItem()
        {
            ConnectionHandler handler = ConnectionHandler.getInstance();
            Player[] players = await getActivePlayers();
            if (players.Length == 0)
            {
                return null;
            }

            string responseJson;
            JObject param = null;
            string id = null;
                       
            switch (players[0].playerId)
            {
                //Music
                case 0:
                    id = "AudioGetItem";
                    param = new JObject(
                        new JProperty("playerid", 0),
                        new JProperty("properties", new string[]{ "title", "album", "artist",
                            "duration", "thumbnail", "file", "fanart", "streamdetails"})
                    );
                    break;
                //Video
                case 1:
                    id = "VideoGetItem";
                    param = new JObject(
                        new JProperty("playerid", 1),
                        new JProperty("properties", new string[] { "title", "album", "artist", "season",
                            "episode", "duration", "showtitle", "tvshowid", "thumbnail", "file", "fanart", "streamdetails" })
                    );
                    break;
                //Pictures
                case 2: return null;
                default: return null;
            }
            responseJson = await handler.sendHttpRequest("Player.GetItem", param, id);
            Debug.WriteLine("PlayerRPC.getItem: Response: " + responseJson);
            PlayerItemResponse responseItem = JsonConvert.DeserializeObject<PlayerItemResponse>(responseJson);
            return responseItem.result.item;
        }

        public static async void getProperties()
        {
            Player[] players = await getActivePlayers();
            if (players.Length > 0)
            {
            }
            else
            {
                Debug.WriteLine("No active player");
            }

        }

        /// <summary>
        /// Go to previous, next or specific item in playlist
        /// </summary>
        /// <param name="direction">"previous", "next" or item position</param>
        public static async Task goTo(string direction)
        {
            //Get active players
            ConnectionHandler handler = ConnectionHandler.getInstance();
            Player[] players = await getActivePlayers();
            if (players.Length == 0)
            {
                return;
            }

            foreach(Player p in players)
            {
                await handler.sendHttpRequest("Player.GoTo", 
                    new JObject(
                        new JProperty("playerid", p.playerId),
                        new JProperty("to", direction)
                    )
                );
            }
        }

        /// <summary>
        /// Sends Player.PlayPause JSON 
        /// </summary>
        /// <returns>true if Kodi is currently playing, false if not</returns>
        public static async Task playPause()
        {
            ConnectionHandler handler = ConnectionHandler.getInstance();            
            Player[] players = await getActivePlayers();
            foreach (Player i in players)
            {
                var responseJson = await handler.sendHttpRequest("Player.PlayPause", new JObject(new JProperty("playerid", i.playerId)));

            }             
        }

        public static async void stop()
        {
            ConnectionHandler handler = ConnectionHandler.getInstance();
            Player[] players = await getActivePlayers();
            if(players == null)
            {
                return;
            }
            foreach (Player i in players)
            {
                var responseJson = await handler.sendHttpRequest("Player.Stop", new JObject(new JProperty("playerid", i.playerId)));
            }
        }
    }
}
