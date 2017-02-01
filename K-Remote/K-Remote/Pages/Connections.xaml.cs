using K_Remote.Models;
using K_Remote.Pages;
using K_Remote.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;

namespace K_Remote
{
    /// <summary>
    /// A page that shows all available connections
    /// </summary>
    public sealed partial class Connections : Page
    {
        ObservableCollection<Connection> connections { get; set; }

        public Connections()
        {
            this.InitializeComponent();

            //Fill list view
            connections = SettingsManager.getInstance().getConnectionsList();
            connections_listView.ItemsSource = connections;
        }

        private void connections_listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Connection activeCon = e.AddedItems[0] as Connection;
            Debug.WriteLine("Connection.selectionChanged: Clicked: " + activeCon.description);
            foreach(Connection c in connections)
            {
                c.active = false;
            }
            //Connection dummy = new Connection();
            //connections.Add(dummy);
            SettingsManager.getInstance().setCurrentConnection(activeCon.host);
            ConnectionHandler.getInstance().refreshConnectionData();
            activeCon.active = true;
            Shell.navigate(typeof(Remote));
        }
    }
}
