using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Models
{
    /**
     * Response of getActivePlayers
     */
    class ActivePlayers
    {
        public int id;
        public string jsonrpc;
        public Player[] result;

        public ActivePlayers(int id, string jsonrpc, Player[] result)
        {
            this.id = id;
            this.jsonrpc = jsonrpc;
            this.result = result;
        }
    }

    /**
     * Model of a player
     * Playerid:  0=music, 1=video, 2=pictures
     */
    class Player
    {
        public int playerId;
        public string type;
        public int speed;
    }

    class PlayerProperties
    {

    }

    class PlayerItemResponse
    {
        public string id;
        public string jsonrpc;
        public PlayerItemResult result;
    }

    class PlayerItemResult
    {
        public PlayerItem item;

    }

    class PlayerItem
    {
        //propeties
        public string album { get; set; }
        public string[] artist { get; set; }

        /// <summary>
        /// Read-only property containing all artists in a comma separated string
        /// </summary>
        public string artistProperty
        {   get
            {
                if(artist == null)
                {
                    return null;
                }
                else
                {
                    return String.Join(", ", artist);
                }
            }
        }

        /// <summary>
        /// A read-only property containing showtitle, season and episode
        /// </summary>
        public string seriesProperty
        {
            get
            {
                if(type == "episode")
                {
                    return showtitle + " S" +  season + "E" + episode;
                }
                else
                {
                    return "";
                }
            }
        }

        public string background { get; set; }

        public string title { get; set; }
        
        //fields
        public int episode;
        public string fanart;
        public string file;
        public string id;
        public string label;
        public int season;
        public string showtitle;
        public string thumbnail;
        public int tvshowid;
        public int duration;

        public string uniqueid;
        public string type;
        public StreamDetails streamdetails;

        public override bool Equals(object obj)
        {
            Debug.WriteLine(this + " == " + obj);
            
            if(obj == null || obj.GetType() != typeof(PlayerItem))
            {
                Debug.WriteLine("PlayerItem.equals: invalid object");
                return false;
            }
            PlayerItem item = obj as PlayerItem;
            Debug.WriteLine(this.uniqueid + " == " + item.uniqueid);
            switch (type)
            {
                case "episode":
                    return title == item.title && episode == item.episode && season == item.season && showtitle == item.showtitle;
                case "movie":
                    return title == item.title;
                case "song":
                    return title == item.title && album == item.album && artistProperty == item.artistProperty;
                default: return title == item.title;
            }
        }
        public override string ToString()
        {
            return type + ": " + title;
        }
    }

    class StreamDetails
    {
        public StreamDetailItem[] audio;
        public StreamDetailItem[] subtitles;
        public StreamDetailItem[] video;
    }

    class StreamDetailItem
    {
        public float aspect;
        public string codec;
        public int duration;
        public int height;
        public int width;
        public string language;
        public string stereomode;
    }
    

    class PlayerState
    {
        public int playerId;
        public int playerSpeed;

        public PlayerState(int playerId, int playerSpeed)
        {
            this.playerId = playerId;
            this.playerSpeed = playerSpeed;
        }
    }

}
