using Moq;
using System;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Tests
{
    /// <summary>
    /// UnitTestHelper methods 
    /// </summary>
    public static class UnitTestHelper
    {
        /// <summary>
        /// http://stackoverflow.com/questions/13766198/c-sharp-accessing-property-values-dynamically-by-property-name
        ///  Access proprerty dynamically by property name
        /// </summary>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static object ReflectPropertyValue(object source, string property)
        {
            return source.GetType().GetProperty(property).GetValue(source, null);
        }
        /// <summary>
        ///  For Viewing all available properties of a dynamic object
        /// </summary>
        /// <param name="source"></param>
        public static void PrintPropertiesOfDynamicObject(object source)
        {
            var properties = source.GetType().GetProperties();
            foreach (var property in properties)
            {
                var propertyName = property.Name;

                var propetyValue = source.GetType().GetProperty(property.Name).GetValue(source, null);

                Console.Write(propertyName + " : " + propetyValue);
                Console.WriteLine();
            }
        }

        public static byte[] GetImageByte(IMock<IFormFile> image)
        {
            var stream = image.Object.OpenReadStream();
            byte[] byteImage;

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                byteImage = memoryStream.ToArray();
            }

            return byteImage;
        }

        /// <summary>
        /// http://stackoverflow.com/questions/38557942/mocking-iprincipal-in-asp-net-core
        /// </summary>
        /// <param name="givenName"></param>
        /// <param name="surname"></param>
        /// <param name="controller"></param>
        public static void SetUser(string givenName, string surname, Controller controller)
        {
            //mock  user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.GivenName, givenName),
                new Claim(ClaimTypes.Surname, surname)
            }));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

        }
    }
}
