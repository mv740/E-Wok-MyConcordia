using System;

namespace MyConcordiaID.Models.Graduation
{
    public class GraduationRepository : IGraduationRepository
    {
        public GraduationStatus GetMarshallingCard(string netName)
        {
            //call external concordia service using netName
            //get information 

            //if student can graduate then he will have a marshalling card


            var card = new MarshallingCard
            {
                Semester = "Fall",
                Year = 2016,
                MarshallingCode = "01-0001Z",
                Department = "Computer Science & Software Engineering",
                Location = "Place des Arts -- Theatre Maisonneuve",
                DateTime = new DateTime(2016, 11, 07).AddHours(9),
                Degree = "Bachelor of Engineering",
                SID = 29649727
            };

            var status = new GraduationStatus
            {
                Status = true,
                Card = card
            };

            return status;
        }

        public GraduationStatus GetMarshallingCardRequestDenied(string netName)
        {
            var status = new GraduationStatus
            {
                Status = false,
                Card = null
            };

            return status;
        }
    }
}
