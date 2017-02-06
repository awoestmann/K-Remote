using K_Remote.Models;
using K_Remote.Utils;
using K_Remote.Wrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace K_Remote.Pages
{
    /// <summary>
    /// A Page containing details of the item that is currently played
    /// </summary>
    public sealed partial class NowPlaying : Page
    {
        private static NowPlaying instance;

        public NowPlaying()
        {
            instance = this;
            NotificationRPC.getInstance().PlayerStateChangedEvent += playerStateChanged;
            this.InitializeComponent();
            if(SettingsManager.getInstance().getCurrentConnection() != null)
            {
                updateItem(PlayerRPC.getItem());
            }
            else
            {
                nowPlaying_title.Text = "No connection data set";
            }

        }

        private async Task updateItem(Task<PlayerItem> itemTask)
        {
            if(! await ConnectionHandler.getInstance().checkHttpConnection())
            {
                nowPlaying_title.Text = "Not connected, check connection data";
            }
            PlayerItem item = await itemTask;
            if(item != null)
            {
                nowPlaying_title.Text = item.title;
                Debug.WriteLine("NowPlaying.updateItem: Item type: " + item.type);
                switch (item.type)
                {
                    case "episode": nowPlaying_subTitle.Text = item.seriesProperty; break;
                    case "movie": break;
                    case "song": nowPlaying_subTitle.Text = item.album + Environment.NewLine + item.artistProperty; break;
                }
                //Try to get thumbnail
                try
                {
                    string imageString = item.thumbnail;
                    Debug.WriteLine("NowPlaying.updateItem: imageString of currentItem: " + Environment.NewLine + imageString);
                    string response = await FileRPC.prepareDownloadFile(imageString);
                    IInputStream imageStream = await FileRPC.downloadFile(response);
                    if (imageStream != null)
                    {
                        Debug.WriteLine("NowPlaying.updateItem: downloaded file");
                        BitmapImage bmp = new BitmapImage();
                        using (var memStream = new MemoryStream())
                        {
                            await imageStream.AsStreamForRead().CopyToAsync(memStream);
                            memStream.Position = 0;
                            bmp.SetSource(memStream.AsRandomAccessStream());
                            nowPlaying_image.Source = bmp;
                        }
                    }
                }
                catch(Exception e)
                {
                    Debug.WriteLine("NowPLaying.updateItemL Error on getting thumbnail: " + e);
                }                
            }
            else
            {
                nowPlaying_title.Text = "Nothing is played";
            }
        }

        static void playerStateChanged(object sender, NotificationEventArgs args)
        {
            instance.updateItem(PlayerRPC.getItem());
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SettingsManager.getInstance().setLastPage("nowPlaying");
        }
    }
}
