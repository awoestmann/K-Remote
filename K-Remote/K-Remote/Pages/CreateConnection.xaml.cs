using K_Remote.Models;
using K_Remote.Utils;
using System;
using System.Collections.Generic;
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
    public sealed partial class CreateConnection : Page
    {
        public CreateConnection()
        {
            this.InitializeComponent();
        }

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            Shell.navigateToConnections();
        }

        private void buttom_ok_Click(object sender, RoutedEventArgs e)
        {
            //Check if neccessary fields are set
            if (description_textbox.Text != "" && host_textbox.Text != "" && port_textbox.Text != "")
            {
                string description = description_textbox.Text;
                string host = host_textbox.Text;
                int port = int.Parse(port_textbox.Text);
                string username = username_textbox.Text;
                string password = password_textbox.Text;
                if (username == null)
                {
                    username = "";
                }
                if (password == null)
                {
                    password = "";
                }
                Connection con = new Connection(description, host, port, 9090, username, password, false);
                SettingsManager.getInstance().addConnection(con);
                Shell.navigateToConnections();
            }
            else
            {
                error_textbox.Visibility = Visibility.Visible;
                description_textbox.BorderBrush = new SolidColorBrush(Color.FromArgb(125, 255, 0, 0));
                host_textbox.BorderBrush = new SolidColorBrush(Color.FromArgb(125, 255, 0, 0));
                port_textbox.BorderBrush = new SolidColorBrush(Color.FromArgb(125, 255, 0, 0));
            }
        }
    }
}
