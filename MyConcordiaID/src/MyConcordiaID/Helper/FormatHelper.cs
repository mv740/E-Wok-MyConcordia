using System;

namespace MyConcordiaID.Helper
{
    public class FormatHelper
    {
        /// <summary>
        ///  Convert string timestamps to C# DateTime object
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(string timestamp)
        {
            return DateTime.ParseExact(timestamp, "MM-dd-yyyy'T'HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
