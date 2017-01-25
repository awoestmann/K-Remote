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

    class PlayerItemResponse
    {
        public string id;
        public string jsonrpc;
        public PlayerItemResult result;
        
        public PlayerItemResponse(string id, string jsonrpc, PlayerItemResult result)
        {
            this.id = id;
            this.jsonrpc = jsonrpc;
            this.result = result;
        }
    }

    class PlayerItemResult
    {
        public PlayerItem item;
        public PlayerItemResult(PlayerItem item)
        {
            this.item = item;
        }
    }

    class PlayerItem
    {
        string sorttitle;
        string productioncode;
        string[] cast;
        string votes = "";
        int duration = 0;
        string trailer = "";
        int albumid = -1;
        string musicbrainzartistid = "";
        string mpaa = "";
        string albumlabel = "";
        string originaltitle = "";
        string[] write = new string[0];
        int[] albumartistid = new int[0];
        string type = "unknown";
        int episode = 0;
        string firstaired = "";
        string showtitle = "";
        string[] country = new string[0];
        string[] mood = new string[0];
        string set = "";
        string musicbrainztrackid = "";
        string[] tag = new string[0];
        string lyrics = "";
        int top250 = 0;
        string comment = "";
        string premiered = "";
        string[] showlink = new string[0];
        string[] style = new string[0];
        string album = "";
        int tvshowid = -1;
        int season = 0;
        string[] theme = new string[0];
        string description = "";
        int setid = -1;
        int track = 0;
        string tagline = "";
        string plotoutline = "";
        int watchedepisodes = 0;
        int id = -1;
        int disc = 0;
        string[] albumartist = new string[0];
        string[] studio = new string[0];
        object uniqueid = null;
        string episodeguide = "";
        string imdbnumber = "";

        
        string channeltype = "tv";
        string channel = "";
        string starttime = "";
        string endtime = "";
        int channelnumber = 0;
        bool hidden = false;
        bool locked = false;

        //public string album;
        public string[] artist;
        //public int episode;
        public string fanart;
        public string file;
        //public string id;
        public string label;
        //public int season;
        //public string showtitle;
        public string thumbnail;
        public string title;
        //public int tvshowid;
        //public string type;
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
