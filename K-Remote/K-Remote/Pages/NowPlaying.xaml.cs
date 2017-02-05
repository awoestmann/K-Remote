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
        public NowPlaying()
        {
            updateItem(PlayerRPC.getItem());
            this.InitializeComponent();
        }

        private async void updateItem(Task<PlayerItem> itemTask)
        {
            PlayerItem item = await itemTask;
            if(item != null)
            {
                nowPlaying_title.Text = item.title;

                //Get thumbnail
                string imageString = item.thumbnail;
                Debug.WriteLine("NowPlaying.updateItem: imageString of currentItem: " + Environment.NewLine +  imageString);
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
                else
                {
                    Debug.WriteLine("NowPLaying.updateItem: Downloaded buffer is null");
                }
                
            }
            else
            {
                nowPlaying_title.Text = "Nothing is played";
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SettingsManager.getInstance().setLastPage("nowPlaying");
        }
    }
}
