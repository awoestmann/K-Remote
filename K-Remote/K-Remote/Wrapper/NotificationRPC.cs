using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Wrapper
{
    class NotificationRPC
    {
        public static void processNotification(string notificationString)
        {
            Debug.WriteLine(notificationString + Environment.NewLine);
            Debug.WriteLine("");
            dynamic notificationObject = JObject.Parse(notificationString);
            string value = notificationObject.method;
            switch (value)
            {
                case "Player.OnPause": break;
                case "Player.OnPlay": break;
                default: Debug.WriteLine("Unknown Response/Notification: " + value); break;
            }
        }


    }
}
