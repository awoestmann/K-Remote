using K_Remote.Models;
using K_Remote.Pages;
using K_Remote.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace K_Remote
{
    /// <summary>
    /// A page that shows all available connections
    /// </summary>
    public sealed partial class Connections : Page
    {
        /// <summary>
        /// An observable collection of connections objects
        /// </summary>
        ObservableCollection<Connection> connections { get; set; }

        /// <summary>
        /// Determines if a click on a list item causes a deletion
        /// </summary>
        private bool deleteMode;

        public Connections()
        {
            this.InitializeComponent();
            deleteMode = false;
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
            Shell.navigateToRemote();
        }

        private void onAddClicked(object sender, RoutedEventArgs e)
        {
            Shell.navigateToCreateConnection();
        }

        private void onDeleteCicked(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Delete clicked");
        }
    }
}
