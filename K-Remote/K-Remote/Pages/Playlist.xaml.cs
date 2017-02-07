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

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace K_Remote.Pages
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class Playlist : Page
    {
        private ObservableCollection<PlayerItem> musicItems;
        private ObservableCollection<PlayerItem> videoItems;

        public Playlist()
        {
            refreshLists();            
            this.InitializeComponent();
        }
        
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

                    playlist_music_listview.ItemsSource = null;
                    musicItems = await PlaylistRPC.getPlaylistItems();

                    Debug.WriteLine("Playlist.refresh: Items:");
                    Debug.WriteLine("Currently played: " + current);
                    Debug.WriteLine("list: ");
                    if(musicItems != null)
                    {
                        foreach(PlayerItem p in musicItems)
                        {
                            Debug.WriteLine(p);
                            p.background = "Transparent";
                            if(current != null && current.Equals(p))
                            {
                                Debug.WriteLine("is played");
                                p.background = Application.Current.Resources["SystemAccentColor"].ToString();
                            }
                        }
                        playlist_music_listview.ItemsSource = musicItems;
                    }                    
                    break;
                //Video playlist
                case 1:
                    playlist_music_listview.Visibility = Visibility.Collapsed;
                    playlist_video_listview.Visibility = Visibility.Visible;

                    videoItems = await PlaylistRPC.getPlaylistItems();
                    playlist_video_listview.ItemsSource = videoItems;

                    Debug.WriteLine("Playlist.refresh: Items:");
                    Debug.WriteLine("Currently played: " + current);
                    Debug.WriteLine("list: ");
                    if (videoItems != null)
                    {
                        foreach (PlayerItem p in videoItems)
                        {
                            Debug.WriteLine(p);
                            p.background = "Transparent";
                            if (current != null && current.Equals(p))
                            {
                                Debug.WriteLine("is played");
                                p.background = Application.Current.Resources["SystemAccentColor"].ToString();
                            }
                        }
                        playlist_music_listview.ItemsSource = musicItems;
                    }
                    break;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SettingsManager.getInstance().setLastPage("playlist");
            base.OnNavigatedTo(e);
        }
    }
}
