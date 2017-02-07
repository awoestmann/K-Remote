using K_Remote.Models;
using K_Remote.Resources;
using K_Remote.Utils;
using K_Remote.Wrapper;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

namespace K_Remote.Pages
{
    public sealed partial class Remote : Page
    {
        /// <summary>
        /// Determines if the volume slider value was changed by user interaction(true) or by code (false)
        /// </summary>
        private bool volumeChangedByUserInteraction;

        /// <summary>
        /// A Dialog for text input
        /// </summary>
        private ContentDialog inputDialog;

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
            init();             
        }

        #region methods

        private async void init()
        {
            ConnectionHandler.getInstance().ConnectionStateChanged += onConnectionStateChanged;
            NotificationRPC.getInstance().VolumeChangedEvent += volumeChanged;
            NotificationRPC.getInstance().InputRequestedEvent += inputRequested;
            NotificationRPC.getInstance().PlayerStateChangedEvent += playerStateChanged;
            refreshGui();
        }

        private async void refreshGui()
        {
             Debug.WriteLine("Remote.refreshGui: refreshing");
             setVolumeSlider(await ApplicationRPC.getVolume());
            setPlayPauseIcon();
        }

        /// <summary>
        /// Shows a ContentDialog containing a textbox
        /// </summary>
        private async void showInputPrompt()
        {
            Debug.WriteLine("Remote: Input prompt");
            if(inputDialog != null)
            {
                inputDialog.Hide();
                inputDialog = null;
            }
            try
            {
                inputDialog = new ContentDialog();
            }
            catch(Exception e)
            {
                Debug.WriteLine("Remote.showInputPrompt: Error on creating input Dialog: " + e.Message);
                return;
            }
            
            TextBox inputTextBox = new TextBox();

            inputTextBox.AcceptsReturn = false;
            //Close dialog on enter key press
            inputTextBox.KeyDown += async (sender, args) =>
            {
                if (args.Key == Windows.System.VirtualKey.Enter)
                {
                    await InputRPC.sendText(inputTextBox.Text, true);
                    inputDialog.Hide();
                }
            };
            inputTextBox.Height = 32;
            inputTextBox.Focus(FocusState.Keyboard);

            inputDialog.Content = inputTextBox;
            inputDialog.Title = "Input required";
            inputDialog.IsSecondaryButtonEnabled = true;
            inputDialog.PrimaryButtonText = "Ok";
            inputDialog.SecondaryButtonText = "Cancel";

            if (await inputDialog.ShowAsync() == ContentDialogResult.Primary)
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
        private async Task setPlayPauseIcon()
        {            
            try
            {
                int playerspeed = await PlayerRPC.getPlayerSpeed();
                switch (playerspeed)
                {
                    case -1: break;
                    case 0: break;
                    case 1: break;
                    default: break;
                }
                if (playerspeed == 0)
                {
                    //switch to play icon
                    remote_button_playPause.Content = Constants.ICON_PLAY;
                }
                else
                {
                    //switch to pause icon
                    remote_button_playPause.Content = Constants.ICON_PAUSE;
                }
            }            
            catch(Exception e)
            {
                Debug.WriteLine("Remote.setPlayPauseIcon: Error on getting players: " + e);
            }
        }
        /// <summary>
        /// Waits for volume task and calls setVolumeSlider(float volume)
        /// </summary>
        /// <param name="volumeTask">Task return volume int value</param>
        private async Task setVolumeSlider(Task<int> volumeTask)
        {
            try
            {
                int volume = await volumeTask;
                setVolumeSlider(volume);
            }
            catch(Exception e)
            {
                Debug.WriteLine("Remote.setVolumeSlider: Error on getting volume: " + e);
            }
            
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

        static void onConnectionStateChanged(object sender, connectionStateChangedEventArgs args)
        {
            if (args.state)
            {
                staticInstance.refreshGui();
            }
        }

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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SettingsManager.getInstance().setLastPage("remote");
        }

        private void onPageLoaded(object sender, RoutedEventArgs args)
        {
            refreshGui();
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
        private void remote_button_mute_Click(object sender, RoutedEventArgs e)
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
            GuiRPC.getCurrentWindow();
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
            InputRPC.skipIfinPlayer("stepback");
        }

        private void remote_button_right_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.right();
            InputRPC.skipIfinPlayer("stepforward");
        }

        private void remote_button_enter_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.select();
        }

        private void remote_button_up_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.up();
            InputRPC.skipIfinPlayer("bigstepforward");
        }

        private void remote_button_down_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.down();
            InputRPC.skipIfinPlayer("bigstepback");
        }

        private void remote_button_back_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.back();
        }

        private void remote_button_toggle_gui_Click(object sender, RoutedEventArgs e)
        {
            GuiRPC.toggleGui();
        }

        private void remote_button_next_Click(object sender, RoutedEventArgs e)
        {
            PlayerRPC.goTo("next");
        }

        private void remote_button_previous_Click(object sender, RoutedEventArgs e)
        {
            PlayerRPC.goTo("previous");
        }

        private void remote_button_showOSD_Click(object sender, RoutedEventArgs e)
        {
            InputRPC.showOSD();
        }
        #endregion
    }
}
