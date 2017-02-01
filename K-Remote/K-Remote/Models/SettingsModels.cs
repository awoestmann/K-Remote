using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Models
{
    class Connection
    {
        public string description { get; set; }
        public string host {get; set; }
        public int httpPort { get; set; }
        public int tcpPort { get; set; }
        public string username { get; set; }
        public string password;
        public bool active;

        public Connection(string description, string host, int httpPort, int tcpPort, string username, string password, bool active)
        {
            this.host = host;
            this.httpPort = httpPort;
            this.tcpPort = tcpPort;
            this.username = username;
            this.password = password;
            this.description = description;
            this.active = active;
        }

        public Connection(string base64String)
        {
            string completeData = Encoding.UTF8.GetString(Convert.FromBase64String(base64String));
            string[] conProperties = completeData.Split(':');
            if(conProperties.Length == 7)
            {
                description = conProperties[0];
                host = conProperties[1];
                httpPort = int.Parse(conProperties[2]);
                tcpPort = int.Parse(conProperties[3]);
                username = conProperties[4];
                password = conProperties[5];
                active = bool.Parse(conProperties[6]);
            }
        }

        public string toBase64String()
        {
            string completeData = description + ":" + host + ":" + httpPort + ":" + tcpPort + ":" + username + ":" + password + ":" + active;
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(completeData));
        }

        public string toReadableString()
        {
            return description + ":" + host + ":" + httpPort + ":" + tcpPort + ":" + username + ":" + password + ":" + active;
        }
    }
}
