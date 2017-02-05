using K_Remote.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private ObservableCollection<Connection> connections;

        private static SettingsManager instance;

        private const string CONNECTIONS = "Connections";
        private const string CURRENT_CONECTION = "CurrentConnection";
        private const string LAST_PAGE = "LastPage";

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
            //localSettings.Values["Connections"] = "";

            connections = new ObservableCollection<Connection>();
            //try to get connections from settings
            
            if(localSettings.Values[CONNECTIONS] != null)
            {
                //Get all connections as string and decode
                string settingsString = Encoding.UTF8.GetString(Convert.FromBase64String(localSettings.Values[CONNECTIONS].ToString()));
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

        private void setSetting(string key, string value)
        {
            localSettings.Values[key] = value;
        }

        private object getSetting(string key)
        {
            return localSettings.Values[key];
        }

        #region connection settings
        public ObservableCollection<Connection> getConnectionsList()
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
                connectionsListString += c.toSettingsString();
                connectionsListString += ";";
            }
            localSettings.Values[CONNECTIONS] = Convert.ToBase64String(Encoding.UTF8.GetBytes(connectionsListString));
        }

        public void setCurrentConnection(string host)
        {
            if(host == null)
            {
                localSettings.Values[CURRENT_CONECTION] = "";
            }

            foreach(Connection con in connections)
            {
                if (con.host.Equals(host))
                {
                    string base64Connection = con.toBase64String();
                    localSettings.Values[CURRENT_CONECTION] = base64Connection;
                }
            }           
        }

        public void removeConnection(Connection toDelete)
        {
            connections.Remove(toDelete);
        }

        public void removeConnection(string host)
        {
            foreach (Connection con in connections)
            {
                if (con.host.Equals(host))
                {
                    connections.Remove(con);
                    saveConnections();
                }
            }
        }

        public Connection getCurrentConnection()
        {
            object connectionBase64 = localSettings.Values["CurrentConnection"];
            if (connectionBase64 != null){
                return new Connection(connectionBase64.ToString());
            }
            else
            {
                return null;
            }
            
        }

        #endregion

        public void setLastPage(string lastPageName)
        {
            setSetting(LAST_PAGE, lastPageName);
        }

        public string getLastPage()
        {
            return (string) getSetting(LAST_PAGE);
        }
    }
}
