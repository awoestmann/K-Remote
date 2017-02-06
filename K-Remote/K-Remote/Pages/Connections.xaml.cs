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
using Windows.UI.Xaml.Navigation;

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

        private bool editMode;

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

            if (!deleteMode && !editMode)
            {
                if (!chosenCon.active)
                {
                    foreach (Connection c in connections)
                    {
                        c.active = false;
                    }
                    //Connection dummy = new Connection();
                    //connections.Add(dummy);
                    SettingsManager.getInstance().setCurrentConnection(chosenCon.host);
                    chosenCon.active = true;
                    SettingsManager.getInstance().saveConnections();
                }
                else
                {
                    chosenCon.active = false;
                    SettingsManager.getInstance().setCurrentConnection(null);
                    SettingsManager.getInstance().saveConnections();                   
                }

                ConnectionHandler.getInstance().refreshConnection();
                Shell.navigateToConnections();
                return;
            }
            if(deleteMode && !editMode)
            {
                openDeleteDialog(chosenCon);
            }

            if(!deleteMode && editMode)
            {
                Shell.navigateToCreateConnection(chosenCon.toSettingsString());
            }
            
        }

        private void onAddClicked(object sender, RoutedEventArgs e)
        {
            Shell.navigateToCreateConnection(null);
        }

        private void onDeleteCicked(object sender, RoutedEventArgs e)
        {
            deleteMode = !deleteMode;
            editMode = false;
            toggle_button_edit.IsChecked = false;
        }

        private void onEditClicked(object sender, RoutedEventArgs e)
        {
            deleteMode = false;
            editMode = !editMode;
            toggle_button_delete.IsChecked = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SettingsManager.getInstance().setLastPage("connections");
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
