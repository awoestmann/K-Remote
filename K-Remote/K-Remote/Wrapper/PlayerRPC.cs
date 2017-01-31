﻿using K_Remote.Models;
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
            var responseJson = await handler.sendHttpRequest("Player.GetActivePlayers", null);
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
            switch (players[0].playerId)
            {
                //Music
                case 0: return null;
                //Video
                case 1:
                        responseJson = await handler.sendHttpJson("{ \"jsonrpc\": \"2.0\", \"method\": \"Player.GetItem\", \"params\": { \"properties\": [\"title\", \"album\", \"artist\", \"season\", \"episode\", \"duration\", \"showtitle\", \"tvshowid\", \"thumbnail\", \"file\", \"fanart\", \"streamdetails\"], \"playerid\": 1 }, \"id\": \"VideoGetItem\"}");
                        PlayerItemResponse responseItem = JsonConvert.DeserializeObject<PlayerItemResponse>(responseJson);
                        return responseItem.result.item;
                //Pictures
                case 2: return null;
                default: return null;
            }                      
        }

        /// <summary>
        /// Sends Player.PlayPause JSON 
        /// </summary>
        /// <returns>true if Kodi is currently playing, false if not</returns>
        public static async void playPause()
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

        public static async void getProperties()
        {
            Player[] players = await getActivePlayers();
            if(players.Length > 0)
            {
            }
            else
            {
                Debug.WriteLine("No active player");
            }
            
        }
    }
}
