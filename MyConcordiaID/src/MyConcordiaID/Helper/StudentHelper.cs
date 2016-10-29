using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Helper
{
    public class StudentHelper
    {
        public static int getRandomId()
        {
            Random rnd = new Random();
            return rnd.Next(20000000, 99999999);
        }

        public static string getNetName(string firstName,string lastName)
        {
            return firstName[0] + "_" + lastName.Substring(0, 5);
        }
    }
}
