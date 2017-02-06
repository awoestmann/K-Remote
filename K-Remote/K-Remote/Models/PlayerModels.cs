using System;
using System.Collections.Generic;
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
        public int episode;
        public string fanart;
        public string file;
        public string id;
        public string label;
        public int season;
        public string showtitle;
        public string thumbnail;
        public string title { get; set; }
        public int tvshowid;
        public int duration;

        public string type;
        public StreamDetails streamdetails;
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
