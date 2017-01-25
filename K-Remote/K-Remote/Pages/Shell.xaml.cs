using K_Remote.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class Shell : Page
    {
        private Frame frame;
        private bool tcpConnected = false;
        private bool httpConnected = false;

        public Shell(Frame frame)
        {
            this.InitializeComponent();
            main_menu_splitView.Content = frame; // ShellSplitView is the SplitView we put in Shell.xaml
            (main_menu_splitView.Content as Frame).Navigate(typeof(MainPage));
            this.frame = frame;

            //Set connection Indicator
            if (SettingsManager.getCurrentConnection() == null)
            {
                main_button_connection_icon.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            }

            ConnectionHandler.getInstance().ConnectionStateChanged += handleConnectionStateChanged;
        }

        private void main_button_menu_Click(object sender, RoutedEventArgs e)
        {            
            main_menu_splitView.IsPaneOpen = !main_menu_splitView.IsPaneOpen;
        }

        private void main_button_connections_Click(object sender, RoutedEventArgs e)
        {
            unfocusButtons();
            main_button_connections.Background = new SolidColorBrush(Color.FromArgb(255, 48, 179, 221));
            frame.Navigate(typeof(Connections));
        }

        private void main_button_remote_Click(object sender, RoutedEventArgs e)
        {
            unfocusButtons();
            main_button_remote.Background = new SolidColorBrush(Color.FromArgb(255, 48, 179, 221));
            frame.Navigate(typeof(Remote));
        }

        private void main_button_nowPlaying_Click(object sender, RoutedEventArgs e)
        {
            unfocusButtons();
            main_button_nowPlaying.Background = new SolidColorBrush(Color.FromArgb(255, 48, 179, 221));
            frame.Navigate(typeof(NowPlaying));
        }

        private void unfocusButtons()
        {
            main_button_connections.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            main_button_nowPlaying.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            main_button_remote.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }
        private void handleConnectionStateChanged(object sender, connectionStateChangedEventArgs args)
        {
            switch (args.conType)
            {
                case 0: httpConnected = args.state; break;
                case 1: tcpConnected = args.state; break;
                default: Debug.WriteLine("Invalid connection type: " + args.conType); return;
            }

            if (args.state)
            {
                main_button_connection_icon.Background = new SolidColorBrush(Color.FromArgb(125, 0, 125, 0));
                main_button_connections_text.Text = args.conName;
            }
            else
            {
                main_button_connection_icon.Background = new SolidColorBrush(Color.FromArgb(125, 125, 0, 0));
            }
            Debug.WriteLine("Connection-Name: " + args.conName + ", Type: " + args.conType + ", state: " + args.state);
        }

    }
}
