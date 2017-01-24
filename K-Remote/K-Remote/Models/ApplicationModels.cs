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
     */
    class Player
    {
        public int playerId;
        public string type;

        public Player(int playerId, string type)
        {
            this.playerId = playerId;
            this.type = type;
        }
    }
}
