using K_Remote.Models;
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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace K_Remote.Pages
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
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
                string imageString = item.fanart;
                Debug.WriteLine(imageString);
                string response = await FileRPC.prepareDownloadFile(imageString);
                //TODO
                /*var bmp = new WriteableBitmap(320, 240);
                using (var stream = bmp.PixelBuffer.AsStream())
                {
                    stream.Write(imageStream, 0, imageStream.Length);
                    nowPlaying_image.Source = bmp;
                }*/
            }
            else
            {
                nowPlaying_title.Text = "Nothing is played";
            }

        }
    }
}
