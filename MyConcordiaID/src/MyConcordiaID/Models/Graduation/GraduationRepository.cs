using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Graduation
{
    public class GraduationRepository : IGraduationRepository
    {
        public GraduationStatus GetMarshallingCard(string netName)
        {
            //call external concordia service using netName
            //get information 

            //if student can graduate then he will have a marshalling card


            MarshallingCard card = new MarshallingCard
            {
                Semester = "Winter",
                Year = 2016,
                MarshallingCode = "01-0001Z",
                Department = "Computer Science & Software Engineering",
                Location = "Place des Arts -- Theatre Maisonneuve",
                DateTime = new DateTime(2016, 11, 07).AddHours(9),
                Degree = "Bachelor of Engineering",
                SID = 29649727
            };

            GraduationStatus status = new GraduationStatus
            {
                Status = true,
                Card = card
            };

            return status;
        }

        public GraduationStatus GetMarshallingCardRequestDenied(string netName)
        {
            GraduationStatus status = new GraduationStatus
            {
                Status = false,
                Card = null
            };

            return status;
        }
    }
}
