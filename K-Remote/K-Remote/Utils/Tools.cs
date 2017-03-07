using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace K_Remote.Utils
{
    /// <summary>
    /// Static class containting utility Methods
    /// </summary>
    static class Tools
    {
        /// <summary>
        /// Returns a string representation of a channel number. E.g. "5.1" is 6 is passed.
        /// </summary>
        /// <param name="channelCount">Audio channel count.</param>
        /// <returns>String representation of channel count.</returns>
        public static string getChannelStringByChannelCount(string channelCount)
        {
            if(channelCount == null || channelCount == "")
            {
                return "";
            }
            switch (channelCount)
            {
                case "2": return "2.0";
                case "6": return "5.1";
                case "8": return "7.1";
                default: return channelCount.ToString();
            }
        }

        /// <summary>
        /// Returns a language string representation of an abbreviation. E.g. "English" if "en" is passed.
        /// </summary>
        /// <param name="abb">Abbreviated language string</param>
        /// <returns>Language string</returns>
        public static string getLanguageStringByabbreviation(string abb)
        {
            if(abb == null || abb == "")
            {
                return "Unknown";
            }
            switch (abb)
            {
                case "en":
                case "EN":
                case "eng":
                case "ENG":
                    return "English";

                case "de":
                case "DE":
                case "deu":
                case "DEU":
                case "ger":
                case "GER":
                    return "German";

                default: return abb;
            }
        }

        /// <summary>
        /// Converts a hex format color string to a color
        /// </summary>
        /// <param name="hex">Color string, hex formated</param>
        /// <returns>A new SolidColorBrush for input color</returns>
        public static SolidColorBrush GetSolidColorBrushFromHex(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));
            SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
            return myBrush;
        }

        /// <summary>
        /// Removes tags and parantheses of a string
        /// Removed tags are: <>, </>, [], [/]
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StripTagsAndParantheses(string input)
        {
            string returnString =  Regex.Replace(input, "<.*?>", string.Empty);
            returnString = Regex.Replace(returnString, "\\[.*?\\]", string.Empty);
            returnString = Regex.Replace(returnString, "\\(.*?\\)", string.Empty);
            returnString = returnString.Trim();
            return returnString;
        }

        /// <summary>
        /// Removes tags of a string
        /// Removed tags are: <>, </>, [], [/]
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StripTags(string input)
        {
            string returnString = Regex.Replace(input, "<.*?>", string.Empty);
            returnString = Regex.Replace(input, "\\[.*?\\]", String.Empty);
            returnString = returnString.Trim();
            return returnString;
        }
    }
}
