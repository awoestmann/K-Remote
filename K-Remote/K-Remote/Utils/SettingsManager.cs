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

        /// <summary>
        /// Local settings
        /// </summary>
        private Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        /// <summary>
        /// A collection of all connections
        /// </summary>
        private ObservableCollection<Connection> connections;

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static SettingsManager instance;

        #region settings string constants
        /// <summary>
        /// Settings key, containing all connections as a concatenated string
        /// </summary>
        private const string CONNECTIONS = "Connections";

        /// <summary>
        /// Settings key, containing the current connections as a string
        /// </summary>
        private const string CURRENT_CONECTION = "CurrentConnection";

        /// <summary>
        /// Settings key, containing the name of the last visited page
        /// </summary>
        private const string LAST_PAGE = "LastPage";
        #endregion

        /// <summary>
        /// Returns current instance, creates new one if needed
        /// </summary>
        /// <returns>The SettingsManager instance</returns>
        public static SettingsManager getInstance()
        {
            if(instance == null)
            {
                instance = new SettingsManager();
            }
            return instance;
        }

        /// <summary>
        /// Constructor
        /// </summary>
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
            saveConnections();
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
            if (connectionBase64 != null && connectionBase64.ToString() != ""){
                return new Connection(connectionBase64.ToString());
            }
            else
            {
                return null;
            }            
        }

        public Connection getConnectionFromString(string targetString)
        {
            foreach(Connection c in connections)
            {
                if (c.toSettingsString() == targetString)
                {
                    return c;
                }
            }
            return null;
        }

        public void updateConnection(Connection old, Connection newCon)
        {
            foreach(Connection c in connections)
            {
                if(c.toSettingsString() == old.toSettingsString())
                {
                    connections[connections.IndexOf(c)].copyFromConnection(newCon);
                    saveConnections();
                    return;
                }
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
