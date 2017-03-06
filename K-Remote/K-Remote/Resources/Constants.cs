using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Resources
{
    static class Constants
    {
        #region current window ids
        /// <summary>
        /// ID of current window if video playback is in foreground
        /// </summary>
        public const int CURRENT_WINDOW_FULLSCREEN_VIDEO = 12005;

        /// <summary>
        /// Id of current window if video playback is in foreground with info overlay opened
        /// </summary>
        public const int CURRENT_WINDOW_FULLSCREEN_VIDEO_INFO = 10142;

        /// <summary>
        /// Current window id of main menu
        /// </summary>
        public const int CURRENT_WINDOW_MAIN_MENU = 10000;
        #endregion

        #region kodi constants
        /// <summary>
        /// Audio player id
        /// </summary>
        public const int KODI_AUDIO_PLAYER_ID = 0;
        /// <summary>
        /// Video player id
        /// </summary>
        public const int KODI_VIDEO_PLAYER_ID = 1;

        /// <summary>
        /// Audio playlist id
        /// </summary>
        public const int KODI_AUDIO_PLAYLIST_ID = 0;
        /// <summary>
        /// Video playlist id
        /// </summary>
        public const int KODI_VIDEO_PLAYLIST_ID = 1;
        #endregion

        #region iconStrings
        public const string ICON_NOW_PLAYED = "\uEA37";

        public const string ICON_ITEM_DETAILS = "\uE712";

        /// <summary>
        /// String representation of play icon in segoe font
        /// </summary>
        public const string ICON_PLAY = "\uE768";

        /// <summary>
        /// String representation of pause icon in segoe font
        /// </summary>
        public const string ICON_PAUSE = "\uE769";

        /// <summary>
        /// String representation of stop icon in segoe font
        /// </summary>
        public const string ICON_STOP = "\uE71A";
        #endregion
    }
}
