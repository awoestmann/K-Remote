using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Models
{

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

    class VolumeChanged
    {
        string jsonrpc;
        string method;

    }

    class VolumeChangedParams
    {
        VolumeChangedParamsData data;
        string sender;
    }

    class VolumeChangedParamsData
    {
        bool muted;
        float volume;
    }
}
