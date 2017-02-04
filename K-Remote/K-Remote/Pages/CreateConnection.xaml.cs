using K_Remote.Models;
using K_Remote.Utils;
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
            Connection con = new Connection(description_textbox.Text, host_textbox.Text, int.Parse(port_textbox.Text), 9090, username_textbox.Text, password_textbox.Text, false);
            SettingsManager.getInstance().addConnection(con);
            Shell.navigateToConnections();
        }
    }
}
