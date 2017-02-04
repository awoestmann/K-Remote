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
            Connection chosenCon;
            try
            {
                chosenCon = e.AddedItems[0] as Connection;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Connection: Error getting clicked item: " + ex);
                return;
            }
            Debug.WriteLine("Connection.selectionChanged: Clicked: " + chosenCon.description);
            if (!deleteMode)
            {
                foreach (Connection c in connections)
                {
                    c.active = false;
                }
                //Connection dummy = new Connection();
                //connections.Add(dummy);
                SettingsManager.getInstance().setCurrentConnection(chosenCon.host);

                ConnectionHandler.getInstance().refreshConnectionData();

                chosenCon.active = true;
                Shell.navigateToRemote();
            }
            else
            {
                openDeleteDialog(chosenCon);
            }
            
        }

        private void onAddClicked(object sender, RoutedEventArgs e)
        {
            Shell.navigateToCreateConnection();
        }

        private void onDeleteCicked(object sender, RoutedEventArgs e)
        {
            deleteMode = !deleteMode;
            Debug.WriteLine("Connections: Delete clicked: " + deleteMode);
        }

        private async void openDeleteDialog(Connection toDelete)
        {
            ContentDialog dialog = new ContentDialog();
            TextBlock questionText = new TextBlock();

            questionText.Text = "Are you sure you want to delete Connection to: " + toDelete.description
                + "(" + toDelete.host + ")";
            dialog.Content = questionText;
            dialog.Title = "Confirm action";
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Ok";
            dialog.SecondaryButtonText = "Cancel";

            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                //connections.Remove(toDelete);
                SettingsManager.getInstance().removeConnection(toDelete);
                Debug.WriteLine("Connections: Removed connection: " + toDelete.description);
            }
            else
            {
                return;
            }
        }
    }
}
