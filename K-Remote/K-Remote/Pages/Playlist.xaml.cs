using K_Remote.Models;
using K_Remote.Resources;
using K_Remote.Utils;
using K_Remote.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace K_Remote.Pages
{
    /// <summary>
    /// A page, displaying the current playlist
    /// </summary>
    public sealed partial class Playlist : Page
    {
        private ObservableCollection<PlayerItem> musicItems;
        private ObservableCollection<PlayerItem> videoItems;
        
        public Playlist()
        {
            musicItems = new ObservableCollection<PlayerItem>();
            videoItems = new ObservableCollection<PlayerItem>();

            //Register for events, fired if a new item is played
            NotificationRPC.getInstance().AudioLibraryOnUpdateEvent += handleAudioLibraryUpdate;
            NotificationRPC.getInstance().PlayerStateChangedEvent += handlePlayerStateChanged;
            NotificationRPC.getInstance().PlaylistChangedEvent += handlePlaylistChanged;
            NotificationRPC.getInstance().VideoLibraryOnUpdateEvent += handleVideoLibraryUpdate;

            createLists();            
            InitializeComponent();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Playlist()
        {
            Debug.WriteLine("Playlist.~Playlist()");
            //Unregister events
            NotificationRPC.getInstance().AudioLibraryOnUpdateEvent -= handleAudioLibraryUpdate;
            NotificationRPC.getInstance().PlayerStateChangedEvent -= handlePlayerStateChanged;
            NotificationRPC.getInstance().VideoLibraryOnUpdateEvent -= handleVideoLibraryUpdate;
        }

        #region Methods

        /// <summary>
        /// Adds item to playlist on speficic position and sets item properties
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <param name="playlistId">Playlist to which the item should be added</param>
        /// <param name="position">Position of the item</param>
        /// <returns></returns>
        private void addItem(PlayerItem item, int playlistId, int position)
        {
            
            Debug.WriteLine("Playlist.Add: Adding item :" + item.id + " - " + item.title + " on position " + position + " to playlist " + playlistId);
            if(playlistId == Constants.KODI_AUDIO_PLAYLIST_ID)
            {
                musicItems.Insert(position, item);
            }
            if(playlistId == Constants.KODI_VIDEO_PLAYLIST_ID)
            {
                videoItems.Insert(position, item);
            }
            //TODO: Fix ugly workaround
            Task.WaitAll(Task.Run(() => PlaylistRPC.getItemProperties(item)));
            Debug.WriteLine("Playlist.additem: New item title: " + videoItems[position].title);

        }

        private async void clearPlaylist(int playlistId)
        {
            if(playlistId == Constants.KODI_AUDIO_PLAYLIST_ID)
            {
                musicItems.Clear();
            }
            if(playlistId == Constants.KODI_VIDEO_PLAYLIST_ID)
            {
                videoItems.Clear();
            }
        }

        /// <summary>
        /// Refreshes list views according to the active playlist. Receives a new item collection instance
        /// </summary>
        /// <returns>Task</returns>
        private async Task createLists()
        {
            Player[] players = await PlayerRPC.getActivePlayers();
            if(players.Length == 0)
            {
                playlist_notification_textblock.Text = "No items in playlist";
                return;
            }
            int playerId = players[0].playerId;
            PlayerItem current = await PlayerRPC.getItem();
            PlayerItem activeItem = null;

            switch (playerId)
            {
                case Constants.KODI_AUDIO_PLAYLIST_ID:
                    playlist_music_listview.Visibility = Visibility.Visible;
                    playlist_video_listview.Visibility = Visibility.Collapsed;
                    playlist_notification_textblock.Text = "Audio";

                    musicItems.Clear();
                    musicItems = await PlaylistRPC.getPlaylistItems();
                    if (musicItems != null && musicItems.Count > 0)
                    {

                        foreach (PlayerItem p in musicItems)
                        {
                            p.currentlyPlayed = false;
                            p.title = Tools.StripTags(p.title);

                            if (current != null && current.Equals(p))
                            {
                                p.currentlyPlayed = true;
                                activeItem = p;
                            }
                        }
                        playlist_music_listview.ItemsSource = musicItems;
                        if(activeItem != null)
                        {
                            playlist_music_listview.ScrollIntoView(activeItem);
                        }
                    }
                    else
                    {
                        playlist_notification_textblock.Text = "No items in playlist";
                        Debug.WriteLine("No items in audio playlist");
                    }
                break;
                case Constants.KODI_VIDEO_PLAYLIST_ID:
                    playlist_music_listview.Visibility = Visibility.Collapsed;
                    playlist_video_listview.Visibility = Visibility.Visible;
                    playlist_notification_textblock.Text = "Video";

                    videoItems.Clear();
                    videoItems = await PlaylistRPC.getPlaylistItems();
                    if (videoItems != null && videoItems.Count >0)
                    {
                        foreach (PlayerItem p in videoItems)
                        {
                            p.currentlyPlayed = false;
                            p.title = Tools.StripTags(p.title);
                            if (current != null && current.Equals(p))
                            {
                                p.currentlyPlayed = true;
                                activeItem = p;
                            }
                        }
                        playlist_video_listview.ItemsSource = videoItems;
                        if(activeItem != null)
                        {
                            playlist_video_listview.ScrollIntoView(activeItem);
                        }
                    }
                    else
                    {
                        playlist_notification_textblock.Text = "No items in playlist";
                        Debug.WriteLine("Playlist.refreshList: No items in video playlist");
                    }
                break;
            }
        }
        #endregion

        #region UI event handler
        /// <summary>
        /// Invoked if a music list item is clicked. Sends goto message to play selected item
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args containing picked item</param>
        private void playlist_music_listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //There should be only one picked item
            if (e.AddedItems.Count == 1)
            {
                PlayerItem picked = e.AddedItems[0] as PlayerItem;
                PlayerRPC.goTo(position: musicItems.IndexOf(picked));
            }
        }

        /// <summary>
        /// Invoked if a video list item is clicked. Sends goto message to play selected item
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args containing picked item</param>
        private void playlist_video_listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //There should be only one picked item
            if(e.AddedItems.Count == 1)
            {
                PlayerItem picked = e.AddedItems[0] as PlayerItem;
                PlayerRPC.goTo(position: videoItems.IndexOf(picked));
            }
        }
        #endregion

        #region event handler
        /// <summary>
        /// Handles playlist changes. RefreshesLists
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="args">Event args</param>
        void handlePlayerStateChanged(object sender, NotificationEventArgs args)
        {
        }

        void handleAudioLibraryUpdate(object sender, NotificationEventArgs args)
        {
        }

        void handleVideoLibraryUpdate(object sender, NotificationEventArgs args)
        {
        }

        void handlePlaylistChanged(object sender, NotificationEventArgs args)
        {
            switch (args.playlistChanged.method)
            {
                case "Playlist.OnAdd":
                    addItem(args.playlistChanged.@params.data.item,
                        args.playlistChanged.@params.data.playlistid,
                        args.playlistChanged.@params.data.position);
                   
                    break;
                case "Playlist.OnClear":
                    clearPlaylist(args.playlistChanged.@params.data.playlistid);
                    break;
                case "Playlist.OnRemove":
                    break;
                default: Debug.WriteLine("Playlist.handlePlaylistChanged: Invalid Method: " + args.playlistChanged.method); break;
            }
        }

        /// <summary>
        /// Handles onNavigatedTo event
        /// </summary>
        /// <param name="e">Event args</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SettingsManager.getInstance().setLastPage("playlist");
            base.OnNavigatedTo(e);
        }
        #endregion
    }
}
