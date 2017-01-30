using K_Remote.Models;
using K_Remote.Utils;
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
        public event EventHandler<NotificationEventArgs> InputRequiredEvent;
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
            Debug.WriteLine("NotificationRPC: Websocket response:");
            Debug.WriteLine(notificationString + Environment.NewLine);
            Debug.WriteLine("");
            dynamic notificationObject = JObject.Parse(notificationString);
            string value = notificationObject.method;
            NotificationEventArgs args = new NotificationEventArgs();
            
            switch (value)
            {
                case "Application.OnVolumeChanged":
                    args.volumeChanged = JsonConvert.DeserializeObject<VolumeChanged>(notificationString);
                    OnVolumeChangedEvent(args);
                    break;
                case "Input.OnInputRequested":
                    args.inputRequested = JsonConvert.DeserializeObject<InputRequested>(notificationString);
                    OnInputRequestetEvent(args);
                    break;
                case "Player.OnPause":
                    args.playerState = JsonConvert.DeserializeObject<PlayerStateChanged>(notificationString);
                    OnPlayerStateChangedEvent(args);
                    break;
                case "Player.OnPlay":
                    args.playerState = JsonConvert.DeserializeObject<PlayerStateChanged>(notificationString);
                    OnPlayerStateChangedEvent(args);
                    break;
                
                default: Debug.WriteLine("Unknown Response/Notification: " + value); return;
            }
        }

        protected virtual void OnInputRequestetEvent(NotificationEventArgs args)
        {
            InputRequiredEvent?.Invoke(this, args);
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
}
