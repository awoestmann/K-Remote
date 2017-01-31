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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace K_Remote.Pages
{
    public sealed partial class Remote : Page
    {
        /// <summary>
        /// Determines if the volume slider value was changed by user interaction(true) or by code (false)
        /// </summary>
        private bool volumeChangedByUserInteraction;

        public Remote()
        {
            this.InitializeComponent();
            NotificationRPC.getInstance().VolumeChangedEvent += volumeChanged;
            setVolumeSlider(ApplicationRPC.getVolume());            
        }

        static void volumeChanged(object sender, NotificationEventArgs args)
        {
            Debug.WriteLine("Remote: New Volume :" + args.volumeChanged.@params.data.volume);
        }

        static void inputRequested(object sender, NotificationEventArgs args)
        {
            Debug.WriteLine("Input Requested @remote");
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

        private async void setVolumeSlider(Task<int> volumeTask)
        {
            volumeChangedByUserInteraction = false;
            int volume = await volumeTask;
            if(remote_volume_slider.Value != volume)
            {
                remote_volume_slider.Value = volume;
            }                      
        }

        private void remote_volume_slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (volumeChangedByUserInteraction)
            {
                ApplicationRPC.setVolume(Convert.ToInt32(e.NewValue));
            }
            else
            {
                volumeChangedByUserInteraction = true;
            }
                       
        }

        private void remote_button_context_menu_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.contextMenu();
        }

        private void remote_button_info_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.info();
        }
    }
}
