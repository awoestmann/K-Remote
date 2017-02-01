using K_Remote.Models;
using K_Remote.Utils;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace K_Remote
{
    /// <summary>
    /// A page that shows all available connections
    /// </summary>
    public sealed partial class Connections : Page
    {
        public Connections()
        {
            this.InitializeComponent();

            //Fill list view
            List<Connection> connections = SettingsManager.getInstance().getConnectionsList();
            connections_listView.ItemsSource = connections;
        }        
    }
}
