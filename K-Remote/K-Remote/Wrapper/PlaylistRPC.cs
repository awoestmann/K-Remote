using K_Remote.Models;
using K_Remote.Resources;
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
        public static async Task<PropertyObservingCollection<PlayerItem>> getPlaylistItems(int playListid = -1)
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
                        new JProperty("properties", new string[] { "title", "album", "artist", "duration"})
                    ));
                    Debug.WriteLine("PlaylistRPC. getPlaylistItems: playlist audio: " + response);
                    break;
                case 1:
                    response = await ConnectionHandler.getInstance().sendHttpRequest("Playlist.GetItems", new JObject(
                       new JProperty("playlistid", 1),
                       new JProperty("properties", new string[] { "title", "episode", "season", "showtitle"})
                    ));
                    break;
                default: Debug.WriteLine("PlaylistRPC.getPlaylistItems: Unknown Player/Playlist type: " + id);
                    return null;
            }
            try
            {
                PlaylistGetItemsResponse responseObject = JsonConvert.DeserializeObject<PlaylistGetItemsResponse>(response);
                return new PropertyObservingCollection<PlayerItem>(responseObject.result.items);
            }
            catch(Exception)
            {
                Debug.WriteLine("PlaylistRPC.getPlaylistItems: Error on parsing response: " + response);
                return null;
            }
        }

        public static async Task getItemProperties(PlayerItem item)
        {
            ObservableCollection<PlayerItem> playlistItems;
            switch (item.type)
            {
                case "movie":
                case "episode":
                     playlistItems = await getPlaylistItems(Constants.KODI_VIDEO_PLAYLIST_ID);
                    break;
                case "song":
                    playlistItems = await getPlaylistItems(Constants.KODI_AUDIO_PLAYLIST_ID);
                    break;
                default: return;
            }
            if(playlistItems == null)
            {
                return;
            }
            foreach(PlayerItem i in playlistItems)
            {
                if(i.id == item.id)
                {
                    item.copyProperties(i);
                }
            }
        }
    }
}
