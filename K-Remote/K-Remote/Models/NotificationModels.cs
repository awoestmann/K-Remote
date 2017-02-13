using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Models
{

    /// <summary>
    /// Models input requested notifications
    /// </summary>
    class InputRequested{
        string jsonrpc;
        string method;
        InputRequestedData data;
    }

    class InputRequestedParams
    {
        InputRequestedData data;
        string sender;
    }

    class InputRequestedData
    {
        string title;
        string type;
        string value;
    }
    /// <summary>
    /// Models Player.OnPlay and Player.OnPause Notifications
    /// </summary>
    class PlayerStateChanged
    {
        public string jsonrpc;
        public string method;
        public PlayerStateChangedParams @params;
    }

    class PlayerStateChangedParams
    {
        public PlayerStateChangedParamsData data;
    }

    class PlayerStateChangedParamsData
    {
        //Player info
        public PlayerItem item;
        public Player player;

        //Volume info:
        public bool muted;
        public float volume;

        //Sender
        public string sender;
    }

    /// <summary>
    /// Models playlist OnAdd, OnClear and OnRemove notifications
    /// </summary>
    class PlaylistChanged
    {
        public string jsonrpc;
        public string method;
        public PlaylistChangedParams @params;
    }

    class PlaylistChangedParams
    {
        public string sender;
        public PlaylistChangedParamsData data;
    }

    class PlaylistChangedParamsData
    {
        public PlayerItem item;
        public int playlistid;
        public int position;
    }

    /// <summary>
    /// Models volume changed notifications
    /// </summary>
    class VolumeChanged
    {
        public string jsonrpc;
        public string method;
        public VolumeChangedParams @params;

    }

    class VolumeChangedParams
    {
        public VolumeChangedParamsData data;
        public string sender;
    }

    class VolumeChangedParamsData
    {
        public bool muted;
        public float volume;
    }
}
