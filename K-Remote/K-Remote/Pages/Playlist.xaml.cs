using K_Remote.Models;
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
            refreshLists();            
            this.InitializeComponent();
        }        

        /// <summary>
        /// Refreshes list views according to the active playlist
        /// </summary>
        /// <returns></returns>
        private async Task refreshLists()
        {
            Player[] players = await PlayerRPC.getActivePlayers();
            int playerId = players[0].playerId;
            PlayerItem current = await PlayerRPC.getItem();


            switch (playerId)
            {
                //Audio playlist
                case 0:
                    playlist_music_listview.Visibility = Visibility.Visible;
                    playlist_video_listview.Visibility = Visibility.Collapsed;

                    musicItems.Clear();
                    ObservableCollection<PlayerItem> newMusicItems = await PlaylistRPC.getPlaylistItems();
                    if (newMusicItems != null && newMusicItems.Count > 0)
                    {
                        playlist_notification_textblock.Visibility = Visibility.Collapsed;
                        musicItems = newMusicItems;
                        Debug.WriteLine("Playlist.refresh: Items:");
                        Debug.WriteLine("Currently played: " + current);
                        Debug.WriteLine("list: ");

                        foreach (PlayerItem p in musicItems)
                        {
                            Debug.WriteLine(p);
                            p.currentlyPlayed = false;
                            p.title = Tools.StripTags(p.title);

                            if (current != null && current.Equals(p))
                            {
                                Debug.WriteLine("is played");
                                p.currentlyPlayed = true;
                            }
                        }
                        playlist_music_listview.ItemsSource = musicItems;
                    }
                    else
                    {
                        playlist_notification_textblock.Visibility = Visibility.Visible;
                        Debug.WriteLine("No items in audio playlist");
                    }
                    break;
                //Video playlist
                case 1:
                    playlist_music_listview.Visibility = Visibility.Collapsed;
                    playlist_video_listview.Visibility = Visibility.Visible;

                    videoItems.Clear();
                    ObservableCollection<PlayerItem> newVideoItems = await PlaylistRPC.getPlaylistItems();
                    if (newVideoItems != null && newVideoItems.Count >0)
                    {
                        playlist_notification_textblock.Visibility = Visibility.Collapsed;
                        videoItems = newVideoItems;
                        playlist_video_listview.ItemsSource = videoItems;

                        Debug.WriteLine("Playlist.refresh: Items:");
                        Debug.WriteLine("Currently played: " + current);
                        Debug.WriteLine("list: ");
                    
                        foreach (PlayerItem p in videoItems)
                        {
                            Debug.WriteLine(p);
                            p.currentlyPlayed = false;
                            p.title = Tools.StripTags(p.title);
                            if (current != null && current.Equals(p))
                            {
                                Debug.WriteLine("is played");
                                p.currentlyPlayed = true;
                            }
                        }
                        playlist_music_listview.ItemsSource = musicItems;
                    }
                    else
                    {
                        playlist_notification_textblock.Visibility = Visibility.Visible;
                        Debug.WriteLine("Playlist.refreshList: No items in video playlist");
                    }
                    break;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SettingsManager.getInstance().setLastPage("playlist");
            base.OnNavigatedTo(e);
        }

        private void playlist_music_listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PlayerItem pickedItem = e.AddedItems[0] as PlayerItem;
            if(pickedItem != null)
            {

            }
        }

        private void playlist_video_listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
