using K_Remote.Models;
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

namespace K_Remote.Pages
{
    /// <summary>
    /// A page to create or edit a connection
    /// </summary>
    public sealed partial class CreateConnection : Page
    {
        /// <summary>
        /// Connection to edit
        /// </summary>
        private static Connection toEdit;

        /// <summary>
        /// Constructor
        /// </summary>
        public CreateConnection()
        {
            this.InitializeComponent();
            Debug.WriteLine("CreateConnection.init: Creating page");            
        }

        /// <summary>
        /// Cancels connection creation/editing and navigates back to connection page
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event args</param>
        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            Shell.navigateToConnections();
        }

        /// <summary>
        /// Checks if neccessary fields are set and saves connection
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event args</param>
        private void buttom_ok_Click(object sender, RoutedEventArgs e)
        {
            //Check if neccessary fields are set
            if (description_textbox.Text != "" && host_textbox.Text != "" && port_textbox.Text != "")
            {
                string description = description_textbox.Text;
                string host = host_textbox.Text;
                int port = 0;
                try
                {
                    port = int.Parse(port_textbox.Text);
                }
                catch(FormatException formatEx)
                {
                    Debug.WriteLine("CreateConnection.button_ok_Click: Parse error on port");
                    port_textbox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                    error_textbox.Visibility = Visibility.Visible;
                    error_textbox.Text = "Unable to parse port number";
                    return;
                }
                
                string username = username_textbox.Text;
                string password = password_textbox.Password;
                if (username == null)
                {
                    username = "";
                }
                if (password == null)
                {
                    password = "";
                }

                Connection con = new Connection(description, host, port, 9090, username, password, false);
                if (toEdit == null)
                {                    
                    SettingsManager.getInstance().addConnection(con);
                }
                else
                {
                    SettingsManager.getInstance().updateConnection(toEdit, con);
                }

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

        /// <summary>
        /// Handle onNavigation event
        /// Checks if a connection is passed and fills UI elements with connection data to edit
        /// </summary>
        /// <param name="e">Navigation events</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string conString = e.Parameter as string;
            if(conString != null)
            {
                toEdit = SettingsManager.getInstance().getConnectionFromString(conString);
                Debug.WriteLine("CreateConnection.OnNavigatedTo: Edit connection: " + toEdit.description);

                if (toEdit != null)
                {
                    create_connection_page_title.Text = "Edit Connection";
                    description_textbox.Text = toEdit.description;
                    host_textbox.Text = toEdit.host;
                    port_textbox.Text = toEdit.httpPort.ToString();
                    username_textbox.Text = toEdit.username;
                    password_textbox.Password = toEdit.password;
                }
            }
            base.OnNavigatedTo(e);
        }
    }
}
