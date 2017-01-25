using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Models
{
    class Connection
    {
        public string host;
        public int port;
        public string name;
        public string loginBase64;

        public Connection(string host, int port, string name, string loginBase64)
        {
            this.host = host;
            this.port = port;
            this.name = name;
            this.loginBase64 = loginBase64;
        }

        public override String ToString()
        {
            return "name@" + host + ":" + port;
        }
    }
}
