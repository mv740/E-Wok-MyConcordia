using System;

namespace MyConcordiaID.Helper
{
    public class PictureHelper
    {
        public static string getPictureBytesAsString(byte[] picture)
        {
            if (picture == null)
                return string.Empty;
            else
                return Convert.ToBase64String(picture);
        }
    }
}
