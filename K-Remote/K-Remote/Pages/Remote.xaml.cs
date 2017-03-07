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
        /// A RadioButton subclass, holding the index of an audiostream or subtitle
        /// </summary>
        private class IndexedRadioButton : RadioButton
        {
            /// <summary>
            /// Audiostream/subtitle index
            /// </summary>
            public int Index { get; set; }
        }

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
            NotificationRPC.getInstance().VolumeChangedEvent += onVolumeChanged;
            NotificationRPC.getInstance().InputRequestedEvent += onInputRequested;
            NotificationRPC.getInstance().PlayerStateChangedEvent += onPlayerStateChanged;
            //refreshGui();
        }

        private async void refreshGui()
        {
            try
            {
                setVolumeSlider(await ApplicationRPC.getVolume());
                setPlayPauseIcon();
                setStreamDetails();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Remote.refreshGui: failed refreshing GUI: " + e);
            }
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
        /// Gets audio streams and subtitles of currently played item and sets items in appbar menus accordingly
        /// </summary>
        /// <returns>Task</returns>
        private async Task setStreamDetails()
        {
            StreamDetails details = await PlayerRPC.getStreamDetails();

            //Handle audio streams
            remote_language_stackpanel.Children.Clear();
            bool first = true;
            if(details.audio != null) {
                for(int i = 0; i<details.audio.Length; i++)
                {
                    StreamDetailItem audioItem = details.audio[i];
                    IndexedRadioButton newButton = new IndexedRadioButton();
                    if (first)
                    {
                        newButton.IsChecked = true;
                        first = false;
                    }
                    else
                    {
                        newButton.IsChecked = false;
                    }
                    newButton.Content = audioItem.codec + "@" + audioItem.channels + " Channels(" + audioItem.language + ")";
                    newButton.Index = i;
                    newButton.Checked += remote_language_radio_button_checked;
                    remote_language_stackpanel.Children.Add(newButton);
                }
            }
            else
            {
                IndexedRadioButton invalid = new IndexedRadioButton();
                invalid.Content = "No valid Audio stream received";
                invalid.Index = -1;
                invalid.IsChecked = true;
                remote_language_stackpanel.Children.Add(invalid);
            }

            //Handle subtitles
            remote_subtitle_stackpanel.Children.Clear();

            //Add button for deactivated subtitles
            IndexedRadioButton subtitleButton = new IndexedRadioButton();
            subtitleButton.Content = "None";
            subtitleButton.Index = -1;
            
            //Check it if no other subtitles are given
            if(details.subtitle == null || details.subtitle.Length == 0)
            {
                subtitleButton.IsChecked = true;
                subtitleButton.Checked += remote_subtitle_radio_button_checked;
                remote_subtitle_stackpanel.Children.Add(subtitleButton);
            }
            else
            {
                remote_subtitle_stackpanel.Children.Add(subtitleButton);
                subtitleButton.Checked += remote_subtitle_radio_button_checked;
                first = true;
                for (int j = 0; j<details.subtitle.Length; j++)
                {
                    Debug.WriteLine("Subtitle: ");
                    StreamDetailItem subtitleItem = details.subtitle[j];                    
                    subtitleButton = new IndexedRadioButton();
                    subtitleButton.Content = subtitleItem.language;
                    if (first)
                    {
                        subtitleButton.IsChecked = true;
                        first = false;
                    }
                    subtitleButton.Checked += remote_subtitle_radio_button_checked;
                    remote_subtitle_stackpanel.Children.Add(subtitleButton);
                }
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

        /// <summary>
        /// Event handler for connection state changes, refreshes gui if connection is active
        /// </summary>
        /// <param name="sender">Sending instance</param>
        /// <param name="args">Event args</param>
        private void onConnectionStateChanged(object sender, connectionStateChangedEventArgs args)
        {
            if (args.state)
            {
                refreshGui();
            }
        }

        /// <summary>
        /// Event handler for input required events
        /// </summary>
        /// <param name="sender">Sender instance</param>
        /// <param name="args">Event args</param>
        static void onInputRequested(object sender, NotificationEventArgs args)
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
        void onPlayerStateChanged(Object sender, NotificationEventArgs args)
        {
            if (args.playerState.@params.data.player.speed == 0)
            {
                //Switch to play icon
                remote_button_playPause.Content = "\uE768";
            }
            else
            {
                //Switch to pause icon
                remote_button_playPause.Content = "\uE769";
            }
            if (args.playerState.method == "Player.OnPlay")
            {
                setStreamDetails();
            }
        }

        /// <summary>
        /// Event handler for volume changed events
        /// </summary>
        /// <param name="sender">Sending instance</param>
        /// <param name="args">Event args</param>
        private void onVolumeChanged(object sender, NotificationEventArgs args)
        {
            Debug.WriteLine("Remote: New Volume :" + args.volumeChanged.@params.data.volume);
            if (staticInstance != null)
            {
                Remote.staticInstance.setVolumeSlider(args.volumeChanged.@params.data.volume);
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
        /// Fired if a audio stream radio button is checked
        /// </summary>
        /// <param name="sender">Sending button</param>
        /// <param name="args">Event args</param>
        private void remote_language_radio_button_checked(object sender, RoutedEventArgs args)
        {
            IndexedRadioButton sendingButton = sender as IndexedRadioButton;
            PlayerRPC.setAudioStream(sendingButton.Index);
        }

        /// <summary>
        /// Fired if a subtitle radio button is checked
        /// </summary>
        /// <param name="sender">Sending button</param>
        /// <param name="args">Event args</param>
        private void remote_subtitle_radio_button_checked(object sender, RoutedEventArgs args)
        {
            IndexedRadioButton sendingButton = sender as IndexedRadioButton;
            PlayerRPC.setSubtitle(sendingButton.Index);
        }

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
