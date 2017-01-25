using K_Remote.Models;
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
        public event EventHandler<NotificationEventArgs> NotificationEvent;
        public event EventHandler<NotificationEventArgs> PlayerStateChangedEvent;
        public event EventHandler<NotificationEventArgs> VolumeChangedEvent;

        private static NotificationRPC instance;

        private NotificationRPC() { }

        public static NotificationRPC getInstance()
        {
            if(instance == null)
            {
                instance = new NotificationRPC();
            }
            return instance;
        }

        public void processNotification(string notificationString)
        {
            Debug.WriteLine("Websocket response:");
            Debug.WriteLine(notificationString + Environment.NewLine);
            Debug.WriteLine("");
            dynamic notificationObject = JObject.Parse(notificationString);
            string value = notificationObject.method;
            NotificationEventArgs args = new NotificationEventArgs();
            args.playerState = JsonConvert.DeserializeObject<PlayerStateChanged>(notificationString);
            switch (value)
            {
                case "Player.OnPause":                    
                    OnPlayerStateChangedEvent(args);
                    break;
                case "Player.OnPlay":
                    OnPlayerStateChangedEvent(args);
                    break;
                case "Application.OnVolumeChanged":
                    OnVolumeChangedEvent(args);
                    break;
                default: Debug.WriteLine("Unknown Response/Notification: " + value); break;
            }
        }

        protected virtual void OnNotificationEvent(NotificationEventArgs args)
        {
            NotificationEvent?.Invoke(this, args);
        }

        protected virtual void OnPlayerStateChangedEvent(NotificationEventArgs args)
        {
            PlayerStateChangedEvent?.Invoke(this, args);
        }

        protected virtual void OnVolumeChangedEvent(NotificationEventArgs args)
        {
            VolumeChangedEvent?.Invoke(this, args);
        }
    }

    class NotificationEventArgs : EventArgs
    {
        public PlayerStateChanged playerState { get; set; }
    }
}
