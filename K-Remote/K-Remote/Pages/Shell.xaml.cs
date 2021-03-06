﻿using K_Remote.Utils;
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
        private static Frame frame;
        private static Shell instance;

        private bool tcpConnected = false;
        private bool httpConnected = false;

        public Shell(Frame frame)
        {
            this.InitializeComponent();

            ConnectionHandler.getInstance().ConnectionStateChanged += handleConnectionStateChanged;

            instance = this;
            main_menu_splitView.Content = frame;
            (main_menu_splitView.Content as Frame).Navigate(typeof(MainPage));
            Shell.frame = frame;

            //Set connection Indicator
            if (SettingsManager.getInstance().getCurrentConnection() == null)
            {
                main_button_connection_icon.Background = new SolidColorBrush(Color.FromArgb(125, 0, 0, 255));
            }
            else
            {
                //Switch to last page;
                string lastPage = SettingsManager.getInstance().getLastPage();
                switch (lastPage)
                {
                    case "connections": Shell.navigateToConnections(); break;
                    case "remote": Shell.navigateToRemote(); break;
                    case "nowPlaying": Shell.navigateToNowPlaying(); break;
                    case "playlist": navigateToPlaylist(); break;
                    default: break;
                }
            }
        }

        public static void navigateToRemote()
        {
            instance.main_button_remote_Click(null, null);
        }

        public static void navigateToConnections()
        {
            instance.main_button_connections_Click(null, null);
        }

        public static void navigateToNowPlaying()
        {
            instance.main_button_nowPlaying_Click(null, null);
        }

        public static void navigateToPlaylist()
        {
            instance.main_button_playlist_Click(null, null);
        }

        public static void navigateToCreateConnection(object parameter)
        {
            frame.Navigate(typeof(CreateConnection), parameter);
        }

        private void main_button_menu_Click(object sender, RoutedEventArgs e)
        {            
            main_menu_splitView.IsPaneOpen = !main_menu_splitView.IsPaneOpen;
        }

        private void main_button_connections_Click(object sender, RoutedEventArgs e)
        {            
            focusButton(main_button_connections);
            frame.Navigate(typeof(Connections));
        }

        private void main_button_remote_Click(object sender, RoutedEventArgs e)
        {
            focusButton(main_button_remote);
            frame.Navigate(typeof(Remote));
        }

        private void main_button_nowPlaying_Click(object sender, RoutedEventArgs e)
        {
            focusButton(main_button_nowPlaying);
            frame.Navigate(typeof(NowPlaying));
        }

        private void main_button_playlist_Click(object sender, RoutedEventArgs e)
        {
            focusButton(main_button_playlist);
            frame.Navigate(typeof(Playlist));
        }

        private void focusButton(Button button)
        {
            unfocusButtons();
            string accentColorHexString = Application.Current.Resources["SystemAccentColor"].ToString();
            button.Background = Tools.GetSolidColorBrushFromHex(accentColorHexString);
        }

        private void unfocusButtons()
        {
            main_button_connections.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            main_button_nowPlaying.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            main_button_playlist.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
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
                main_button_connection_icon.Background = new SolidColorBrush(Color.FromArgb(255, 0, 125, 0));
                main_button_connections_text.Text = args.conName;
            }
            else
            {
                main_button_connection_icon.Background = new SolidColorBrush(Color.FromArgb(255, 125, 0, 0));
            }
            Debug.WriteLine("Shell.handleConnectionStateChanged: Connection-Name: " + args.conName + ", Type: " + args.conType + ", state: " + args.state);
        }

    }
}
