using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_Remote.Resources
{
    static class Constants
    {
        #region iconStrings
        /// <summary>
        /// String representation of play icon in segoe font
        /// </summary>
        public const string ICON_PLAY = "\uE768";

        /// <summary>
        /// String representation of pause icon in segoe font
        /// </summary>
        public const string ICON_PAUSE = "\uE769";
        #endregion
        #region current window ids
        /// <summary>
        /// ID of current window if video playback is in foreground
        /// </summary>
        public const int CURRENT_WINDOW_FULLSCREEN_VIDEO = 12005;

        /// <summary>
        /// Id of current window if video playback is in foreground with info overlay opened
        /// </summary>
        public const int CURRENT_WINDOW_FULLSCREEN_VIDEO_INFO = 10142;
        #endregion
    }
}
