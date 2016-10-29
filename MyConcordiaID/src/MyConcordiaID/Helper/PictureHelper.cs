using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
