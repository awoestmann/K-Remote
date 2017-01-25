using K_Remote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Utils
{
    static class SettingsManager
    {
        private static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public static void setCurrentConnection(Connection con)
        {
            Windows.Storage.ApplicationDataCompositeValue composite = new Windows.Storage.ApplicationDataCompositeValue();
            composite["host"] = con.host;
            composite["port"] = con.port;
            composite["name"] = con.name;
            composite["loginBase64"] = con.loginBase64;
            localSettings.Values["currentConnection"] = composite;
        }

        public static Connection getCurrentConnection()
        {
            Windows.Storage.ApplicationDataCompositeValue composite = (Windows.Storage.ApplicationDataCompositeValue)localSettings.Values["currentConnection"];
            if(composite != null)
            {
                return new Connection((string)composite["host"], (int)composite["port"], (string)composite["name"], (string)composite["loginBase64"]);
            }
            else
            {
                return null;
            }
        }
    }
}
