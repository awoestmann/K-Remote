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
            ActivePlayers players = null;
            Debug.WriteLine("Active players:" + responseJson);
            try
            {
               players  = JsonConvert.DeserializeObject<ActivePlayers>(responseJson);
            }
            catch (ArgumentNullException)
            {
                Debug.WriteLine("PlayerRPC.getActivePLayers: No response received");
                return null;
            }      
            return players.result;
        }

        /// <summary>
        /// Get currently played item
        /// </summary>
        /// <returns>Currently played PlayerItem </returns>
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
                            "episode", "duration", "showtitle", "tvshowid", "thumbnail", "file", "fanart", "streamdetails"})
                    );
                    break;
                //Pictures
                case 2: return null;
                default: return null;
            }
            responseJson = await handler.sendHttpRequest("Player.GetItem", param, id);
            PlayerItemResponse responseItem = null;
            try
            {
                responseItem = JsonConvert.DeserializeObject<PlayerItemResponse>(responseJson);
            }
            catch(JsonReaderException)
            {
                Debug.WriteLine("PlayerRPC.getItem: Error on parsing getItem response: " + responseJson);
            }
            return responseItem.result.item;
        }

        public async static Task<int> getPlayerSpeed()
        {
            PlayerProperties props = await getProperties(new String[] { "speed" });
            if(props != null)
            {
                return props.speed;
            }
            else
            {
                return -1;
            }   
        }

        public static async Task<PlayerProperties> getProperties(string[] props)
        {
            ConnectionHandler handler = ConnectionHandler.getInstance();
            Player[] players = await getActivePlayers();
            if (players.Length == 0)
            {
                return null;
            }
            string response = await ConnectionHandler.getInstance().sendHttpRequest("Player.getProperties",
                new JObject(
                    new JProperty("playerid", players[0].playerId),
                    new JProperty("properties", props)
                )
            );
            try
            {
                PlayerPropertiesResponse propertiesResponse = JsonConvert.DeserializeObject<PlayerPropertiesResponse>(response);
                return propertiesResponse.result;
            }
            catch (JsonReaderException)
            {
                Debug.WriteLine("PlayerRPC.getPlayerspeed: Error parsing speed response: " + response);
                return null;
            }

        }

        /// <summary>
        /// Go to previous, next or specific item in playlist. Only one parameter may be passed
        /// </summary>
        /// <param name="direction">"previous", "next"</param>
        /// <param name="position">Item position</param>
        public static async Task goTo(string direction = "", int position=-1)
        {
            //Check if none or both parameters are passed   
            if((direction == "" && position == -1) || (direction != "" && position != -1))
            {
                Debug.WriteLine("PlayerRPC.goto: No direction or both given");
                throw new ArgumentException("PlayerRPC.goTo: No or too many Arguments given");
            }
                   
            //Get active players
            ConnectionHandler handler = ConnectionHandler.getInstance();
            Player[] players = await getActivePlayers();
            if (players.Length == 0)
            {
                return;
            }

            Debug.WriteLine("PlayerRPC.goto: GoTo: " + direction);
            foreach (Player p in players)
            {
                JObject param = new JObject(
                        new JProperty("playerid", p.playerId)
                );
                if(position != -1)
                {
                    param.Add(new JProperty("to", position));
                }
                else
                {
                    param.Add(new JProperty("to", direction));
                }
                        
                string response = await handler.sendHttpRequest("Player.GoTo", param);
                Debug.WriteLine("PlayerRPC.goto: response: " + response);
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
