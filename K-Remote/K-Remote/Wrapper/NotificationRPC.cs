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
        //Event handler
        public event EventHandler<NotificationEventArgs> AudioLibraryOnUpdateEvent;
        public event EventHandler<NotificationEventArgs> InputRequestedEvent;
        public event EventHandler<NotificationEventArgs> NotificationEvent;
        public event EventHandler<NotificationEventArgs> PlayerStateChangedEvent;
        public event EventHandler<NotificationEventArgs> PlaylistChangedEvent;
        public event EventHandler<NotificationEventArgs> VideoLibraryOnUpdateEvent;
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

        ~NotificationRPC()
        {
            Debug.WriteLine("NotificationRPC.~NotificationRPC()");
        }

        public void processNotification(string notificationString)
        {
            Debug.WriteLine("NotificationRPC: Websocket response:");
            Debug.WriteLine(notificationString);
            try
            {
                dynamic notificationObject = JObject.Parse(notificationString);
                string method = notificationObject.method;
                NotificationEventArgs args = new NotificationEventArgs();

                switch (method)
                {
                    case "Application.OnVolumeChanged":
                        args.volumeChanged = JsonConvert.DeserializeObject<VolumeChanged>(notificationString);
                        OnVolumeChangedEvent(args);
                        break;
                    case "AudioLibrary.OnUpdate":
                        OnAudioLibraryOnUpdate(args);
                        break;
                    case "Input.OnInputRequested":
                        args.inputRequested = JsonConvert.DeserializeObject<InputRequested>(notificationString);
                        OnInputRequestetEvent(args);
                        break;
                    case "Player.OnPause":
                    case "Player.OnPlay":
                        args.playerState = JsonConvert.DeserializeObject<PlayerStateChanged>(notificationString);
                        OnPlayerStateChangedEvent(args);
                        break;
                    case "Playlist.OnAdd":
                    case "Playlist.OnClear":
                    case "Playlist.OnRemove":
                        args.playlistChanged = JsonConvert.DeserializeObject<PlaylistChanged>(notificationString);
                        OnPlaylistChanged(args);
                        break;
                    case "VideoLibrary.OnUpdate":
                        OnVideoLibraryOnUpdateEvent(args);
                        break;
                    default: Debug.WriteLine("Unknown Response/Notification: " + method); return;
                }
            }
            catch(JsonReaderException jre)
            {
                Debug.WriteLine("NotificationRPC: Error on parsing notification json: " + jre);
                return;
            }
        }
        #region events
        protected virtual void OnAudioLibraryOnUpdate(NotificationEventArgs args)
        {
            AudioLibraryOnUpdateEvent?.Invoke(this, args);
        }

        protected virtual void OnInputRequestetEvent(NotificationEventArgs args)
        {
            InputRequestedEvent?.Invoke(this, args);
        }

        protected virtual void OnNotificationEvent(NotificationEventArgs args)
        {
            NotificationEvent?.Invoke(this, args);
        }

        protected virtual void OnPlayerStateChangedEvent(NotificationEventArgs args)
        {
            PlayerStateChangedEvent?.Invoke(this, args);
        }

        protected virtual void OnPlaylistChanged(NotificationEventArgs args)
        {
            PlaylistChangedEvent?.Invoke(this, args);
        }

        protected virtual void OnVideoLibraryOnUpdateEvent(NotificationEventArgs args)
        {
            VideoLibraryOnUpdateEvent?.Invoke(this, args);
        }

        protected virtual void OnVolumeChangedEvent(NotificationEventArgs args)
        {
            VolumeChangedEvent?.Invoke(this, args);
        }
        #endregion
    }
}
