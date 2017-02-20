using OracleEntityFramework;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace MyConcordiaID.Helper
{
    /// <summary>
    /// Helpers functions to create a student
    /// </summary>
    public class StudentHelper
    {
        public static int GenerateRandomId()
        {
            var rnd = new Random();
            return rnd.Next(20000000, 99999999);
        }

        /// <summary>
        ///  Generate usrname from first name and last name 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public static string GenerateNetName(string firstName,string lastName)
        {
            //we normally get the netname from concordia 

            var firstNameCleaned = RemoveSpecialCharacters(firstName);
            var lastNameCleaned = RemoveSpecialCharacters(lastName);


            var lastNameTrimmed = CleanInput(lastNameCleaned).Trim();

            var length = lastNameTrimmed.Length;

            if(length > 6)
            {
                length = 6;
            }

            return firstNameCleaned[0].ToString().ToLower() + "_" + lastNameTrimmed.Substring(0, length).ToLower();

       
        }

        /// <summary>
        ///  Remove special characters eg : ô é ... 
        ///  http://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net
        /// </summary>
        /// <param name="specialString"></param>
        /// <returns></returns>
        public static string RemoveSpecialCharacters(string specialString)
        {
            var tempBytes = Encoding.GetEncoding("ISO-8859-8").GetBytes(specialString);

            return Encoding.UTF8.GetString(tempBytes);
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
                return string.Empty;
            }
        }

        public static bool ValidId(int id)
        {
            //21941097 = 8 characters
            return (id.ToString().Length == 8);
        }


        public static STUDENT CreateStudent(string firstName, string lastname)
        {

            var firstNameLowerCase = firstName.ToLower();
            var lastNameLowerCase = lastname.ToLower();

            var newStudent = new STUDENT
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
