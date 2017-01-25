using K_Remote.Wrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class Remote : Page
    {
        public Remote()
        {
            NotificationRPC.getInstance().VolumeChangedEvent += volumeChanged;

            this.InitializeComponent();
        }

        static void volumeChanged(object sender, NotificationEventArgs args)
        {
            Debug.WriteLine("New Volume " + args.playerState.@params.data.volume);
        }

        private void remote_button_playPause_Click(object sender, RoutedEventArgs e)
        {
            PlayerRPC.playPause();
        }

        private void remote_button_stop_Click(object sender, RoutedEventArgs e)
        {
            PlayerRPC.stop();
        }

        private void remote_button_left_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.left();
        }

        private void remote_button_right_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.right();
        }

        private void remote_button_enter_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.select();
        }

        private void remote_button_up_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.up();
        }

        private void remote_button_down_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.down();
        }

        private void remote_button_back_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.back();
        }

        private void remote_button_toggle_gui_Click(object sender, RoutedEventArgs e)
        {
            GuiRPC.toggleGui();
        }
    }
}
