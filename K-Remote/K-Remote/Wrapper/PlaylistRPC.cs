using K_Remote.Models;
using K_Remote.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Wrapper
{
    static class PlaylistRPC
    {
        public static async Task<ObservableCollection<PlayerItem>> getPlaylistItems(int playListid = -1)
        {
            int id = playListid;
            Player[] players = await PlayerRPC.getActivePlayers();
            if (players != null && players.Length > 0)
            {
                id = players[0].playerId;
            }
            string response = "";
            switch (id)
            {
                case 0:
                    response = await ConnectionHandler.getInstance().sendHttpRequest("Playlist.GetItems", new JObject(
                        new JProperty("playlistid", 0),
                        new JProperty("properties", new string[] { "title", "album", "artist", "duration" })
                    ));
                    Debug.WriteLine("PlaylistRPC. getPlaylistItems: playlist audio: " + response);
                    break;
                case 1:
                    response = await ConnectionHandler.getInstance().sendHttpRequest("Playlist.GetItems", new JObject(
                       new JProperty("playlistid", 1),
                       new JProperty("properties", new string[] { "title", "episode", "season", "showtitle" })
                   ));
                    break;
                default: Debug.WriteLine("PlaylistRPC.getPlaylistItems: Unknown Player/Playlist type: " + id);
                    return null;
            }
            try
            {
                PlayerItem[] items = JsonConvert.DeserializeObject<PlaylistGetItemsResponse>(response).result.items;
                return new ObservableCollection<PlayerItem>(items);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("PlaylistRPC.getPlaylistItems: Error on parsing response: " + ex);
                return null;
            }
        }
    }
}
