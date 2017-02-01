using K_Remote.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Utils
{
    /// <summary>
    /// Singleton, that manages settings read/write operations
    /// </summary>
    class SettingsManager
    {
        private Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        private List<Connection> connections;

        private static SettingsManager instance;

        public static SettingsManager getInstance()
        {
            if(instance == null)
            {
                instance = new SettingsManager();
            }
            return instance;
        }

        private SettingsManager()
        {
            //Get available connections

            //@DEBUG: clear settings
            localSettings.Values["Connections"] = "";

            connections = new List<Connection>();
            //try to get connections from settings
            
            if(localSettings.Values["Connections"] != null)
            {
                //Get all connections as string and decode
                string settingsString = Encoding.UTF8.GetString(Convert.FromBase64String(localSettings.Values["Connections"].ToString()));

                //Split string into single connections
                string[] connectionsString = settingsString.Split(';');
                
                foreach(string s in connectionsString)
                {
                    if(s.Length > 0)
                    {
                        connections.Add(new Connection(Convert.ToBase64String(Encoding.UTF8.GetBytes(s))));
                    }                   
                }
            }
        }

        public List<Connection> getConnectionsList()
        {
            return connections;
        }

        public void addConnection(Connection con)
        {
            if(con != null)
            {
                connections.Add(con);
                saveConnections();
            }
            
        }

        public void saveConnections()
        {
            string connectionsListString = "";
            foreach(Connection c in connections)
            {
                connectionsListString += c;
                connectionsListString += ";";
            }
            localSettings.Values["Connections"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(connectionsListString));
        }

        public void setCurrentConnection(string host)
        {
            foreach(Connection con in connections)
            {
                if (con.host.Equals(host))
                {
                    string base64Connection = con.toBase64String();
                    localSettings.Values["CurrentConnection"] = base64Connection;
                }
            }
           
        }

        public Connection getCurrentConnection()
        {
            string connectionBase64 = localSettings.Values["CurrentConnection"].ToString();
            return new Connection(connectionBase64);
        }
    }
}
