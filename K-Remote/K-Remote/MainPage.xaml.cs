using System;
using System.Collections.Generic;
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

//Debug
using System.Data;
using System.Diagnostics;

using K_Remote.Utils;
using K_Remote.Pages;
using K_Remote.Wrapper;
using K_Remote.Models;

// Die Vorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 dokumentiert.

namespace K_Remote
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            test();
            this.InitializeComponent();
        }

        public async void test()
        {
            bool connected = await ConnectionHandler.getInstance().checkHttpConnection();
            Debug.WriteLine("Connected: " + connected);
            Debug.WriteLine("Connected to: " + ConnectionHandler.getInstance().getConnectionString());
            Player[] active = await PlayerRPC.getActivePlayers();
            PlayerItem now = await PlayerRPC.getItem();

            ConnectionHandler.getInstance().connectTcp();
            bool tcpConnected = ConnectionHandler.getInstance().checkTcpConnection();
            Debug.WriteLine("TCP connection: " + tcpConnected);

            if(now != null)
            {
                Debug.WriteLine("Active player: " + active[0].playerId + " - " + active[0].type);
                Debug.WriteLine("Now played: " + now.title);
            }
            else
            {
                Debug.WriteLine("Nothing played");
            }            
        }

        private void main_button_connections_Click(object sender, RoutedEventArgs e)
        {            
            this.Frame.Navigate(typeof(Connections));                  
        }

        private void main_button_remote_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Remote));
        }
    }
}
