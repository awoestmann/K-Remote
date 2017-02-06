using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Models
{
    class PlaylistGetItemsResponse
    {
        public string id;
        public string jsonrpc;
        public PlayListGetItemsResult result;
    }

    class PlayListGetItemsResult
    {
        public PlayerItem[] items;
        public LimitsReturned limits;

    }

    class LimitsReturned
    {
        public int start;
        public int end;
        public int total;
    }
}
