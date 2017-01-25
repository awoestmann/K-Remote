using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Models
{

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

}
