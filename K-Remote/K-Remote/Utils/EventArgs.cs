using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Utils
{

    /// <summary>
    /// Eventargs to handle connection state changes
    /// </summary>
    public class connectionStateChangedEventArgs
    {
        /// <summary>
        /// Connection name/description
        /// </summary>
        public string conName;

        /// <summary>
        /// Connection state
        /// </summary>
        public bool state;

        /// <summary>
        /// Connection type: 0 - http, 1 - Websocket
        /// </summary>
        public int conType;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="conName">Connection Name/Description</param>
        /// <param name="con">Connection type: 0 - http, 1 - Websocket</param>
        /// <param name="state">true if connected, else false</param>
        public connectionStateChangedEventArgs(string conName, int con, bool state)
        {
            this.conName = conName;
            this.conType = con;
            this.state = state;
        }
    }
}
