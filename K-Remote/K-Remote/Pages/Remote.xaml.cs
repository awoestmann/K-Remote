using K_Remote.Models;
using K_Remote.Utils;
using K_Remote.Wrapper;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace K_Remote.Pages
{
    public sealed partial class Remote : Page
    {
        /// <summary>
        /// Determines if the volume slider value was changed by user interaction(true) or by code (false)
        /// </summary>
        private bool volumeChangedByUserInteraction;

        /// <summary>
        /// Static remote instance for use in static event handlers
        /// </summary>
        private static Remote staticInstance;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Remote()
        {
            this.InitializeComponent();
            staticInstance = this;
            if (ConnectionHandler.getInstance().checkTcpConnection())
            {
                NotificationRPC.getInstance().VolumeChangedEvent += volumeChanged;
                NotificationRPC.getInstance().InputRequestedEvent += inputRequested;
                NotificationRPC.getInstance().PlayerStateChangedEvent += playerStateChanged;
                setVolumeSlider(ApplicationRPC.getVolume());
                setPlayPauseIcon();
            }                     
        }

        #region methods

        /// <summary>
        /// Shows a ContentDialog containing a textbox
        /// </summary>
        private async void showInputPrompt()
        {
            Debug.WriteLine("Remote: Input prompt");
            ContentDialog dialog = new ContentDialog();
            TextBox inputTextBox = new TextBox();

            inputTextBox.AcceptsReturn = false;
            //Close dialog on enter key press
            inputTextBox.KeyDown += async (sender, args) =>
            {
                if (args.Key == Windows.System.VirtualKey.Enter)
                {
                    await InputRPC.sendText(inputTextBox.Text, true);
                    dialog.Hide();
                }
            };
            inputTextBox.Height = 32;
            inputTextBox.Focus(FocusState.Keyboard);

            dialog.Content = inputTextBox;
            dialog.Title = "Input required";
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Ok";
            dialog.SecondaryButtonText = "Cancel";

            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                Debug.WriteLine("Remote->InputDialog: Input: " + inputTextBox.Text);
                await InputRPC.sendText(inputTextBox.Text, true);
            }
            else
            {
                Debug.WriteLine("Remote->InputDialog: Canceled or None");
            }
        }

        /// <summary>
        /// Switch Play/Pause button icon according to current player state
        /// </summary>
        private async void setPlayPauseIcon()
        {
            Player[] players = await PlayerRPC.getActivePlayers();
            if (players != null && players.Length > 0)
            {
                Player player = players[0];
                if (player.speed == 0)
                {
                    //switch to play icon
                    remote_button_playPause.Content = "\uE768";
                }
                else
                {
                    //switch to pause icon
                    remote_button_playPause.Content = "\uE769";
                }
            }

        }
        /// <summary>
        /// Waits for volume task and calls setVolumeSlider(float volume)
        /// </summary>
        /// <param name="volumeTask">Task return volume int value</param>
        private async void setVolumeSlider(Task<int> volumeTask)
        {
            int volume = await volumeTask;
            setVolumeSlider(volume);
        }

        /// <summary>
        /// Sets volume slider to a new value and sets volume slider indicator bool to true to prevent infinite event-notification loops
        /// </summary>
        /// <param name="volume">New volume value</param>
        private void setVolumeSlider(float volume)
        {            
            if (remote_volume_slider.Value != volume)
            {
                volumeChangedByUserInteraction = false;
                remote_volume_slider.Value = volume;
            }
        }

        #endregion

        #region event handler

        /// <summary>
        /// Event handler for volume changed events
        /// </summary>
        /// <param name="sender">Sending instance</param>
        /// <param name="args">Event args</param>
        static void volumeChanged(object sender, NotificationEventArgs args)
        {
            Debug.WriteLine("Remote: New Volume :" + args.volumeChanged.@params.data.volume);
            if (staticInstance != null)
            {
                Remote.staticInstance.setVolumeSlider(args.volumeChanged.@params.data.volume);
            }
        }

        /// <summary>
        /// Event handler for input required events
        /// </summary>
        /// <param name="sender">Sender instance</param>
        /// <param name="args">Event args</param>
        static void inputRequested(object sender, NotificationEventArgs args)
        {
            Debug.WriteLine("Input Requested @remote");
            if (staticInstance != null)
            {
                Remote.staticInstance.showInputPrompt();
            }
        }

        /// <summary>
        /// Event handler for playerStateChanged events
        /// If player state changes, change Play/Pause button icon according to new state
        /// </summary>
        /// <param name="sender"> Sending instance</param>
        /// <param name="args">Event arguments</param>
        static void playerStateChanged(Object sender, NotificationEventArgs args)
        {
            if (args.playerState.@params.data.player.speed == 0)
            {
                //Switch to play icon
                staticInstance.remote_button_playPause.Content = "\uE768";
            }
            else
            {
                //Switch to pause icon
                staticInstance.remote_button_playPause.Content = "\uE769";
            }
        }
        #endregion

        #region UI controls handlers

        /// <summary>
        /// Event handler for volume slider changed events
        /// </summary>
        /// <param name="sender">Sending slider instance</param>
        /// <param name="e">Event args</param>
        private void remote_volume_slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            //Check if value changed was initiated by user input
            if (volumeChangedByUserInteraction)
            {
                //If true, send new volume value to kodi
                ApplicationRPC.setVolume(Convert.ToInt32(e.NewValue));
            }
            else
            {
                //else reset indicator bool
                volumeChangedByUserInteraction = true;
            }
        }

        /// <summary>
        /// Click handler for mute toggle button
        /// </summary>
        /// <param name="sender">Sending button instance</param>
        /// <param name="e">Event args</param>
        private void remote_mute_button_Click(object sender, RoutedEventArgs e)
        {
            ApplicationRPC.toggleMute();
        }
        /// <summary>
        /// Click handler for send text button. Opens input prompt
        /// </summary>
        /// <param name="sender">Sending button instance</param>
        /// <param name="e">Event args</param>
        private void remote_button_send_text_Click(object sender, RoutedEventArgs e)
        {
            showInputPrompt();
        }

        /// <summary>
        /// Click handler for home button
        /// </summary>
        /// <param name="sender">Sending button instance</param>
        /// <param name="e">Event args</param>
        private void remote_button_home_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.home();
        }

        private void remote_button_context_menu_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.contextMenu();
        }

        private void remote_button_info_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.info();
        }


        private void remote_button_playPause_Click(object sender, RoutedEventArgs e)
        {
            PlayerRPC.playPause();
        }

        private void remote_button_stop_Click(object sender, RoutedEventArgs e)
        {
            PlayerRPC.stop();
        }

        private void remote_button_left_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.left();
        }

        private void remote_button_right_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.right();
        }

        private void remote_button_enter_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.select();
        }

        private void remote_button_up_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.up();
        }

        private void remote_button_down_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.down();
        }

        private void remote_button_back_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.back();
        }

        private void remote_button_toggle_gui_Click(object sender, RoutedEventArgs e)
        {
            GuiRPC.toggleGui();
        }

        #endregion
    }
}
