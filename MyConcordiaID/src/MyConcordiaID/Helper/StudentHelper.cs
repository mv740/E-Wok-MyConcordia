using MyConcordiaID.Models.Student;
using OracleEntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyConcordiaID.Helper
{
    public class StudentHelper
    {
        public static int GenerateRandomId()
        {
            Random rnd = new Random();
            return rnd.Next(20000000, 99999999);
        }

        public static string GenerateNetName(string firstName,string lastName)
        {
            //we normally get the netname from concordia 

            var cleanedLastName = CleanInput(lastName).Trim();


            var length = cleanedLastName.Length;

            if(length > 6)
            {
                length = 6;
            }

            return firstName[0].ToString().ToLower() + "_" + cleanedLastName.Substring(0, length).ToLower();

       
        }

        /// <summary>
        /// 
        /// Strip invalid characters from a String
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static string CleanInput(string strIn)
        {
            // Replace invalid characters with empty strings.
            try
            {
                return Regex.Replace(strIn, @"[!@_#$%-]", "",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            // If we timeout when replacing invalid characters, 
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        public static bool ValidId(int id)
        {
            //21941097 = 8 characters
            return (id.ToString().Length == 8) ?  true : false;
        }


        public static STUDENT createStudent(string firstName, string lastname)
        {

            var firstNameLowerCase = firstName.ToLower();
            var lastNameLowerCase = lastname.ToLower();

            STUDENT newStudent = new STUDENT
            {
                NETNAME = GenerateNetName(firstNameLowerCase, lastNameLowerCase),
                ID = GenerateRandomId(),
                FIRSTNAME = firstNameLowerCase,
                LASTNAME = lastNameLowerCase,
                DOB = DateTime.Today,
                UGRADSTATUS = "U"
            };

            return newStudent;


        }
    }
}
