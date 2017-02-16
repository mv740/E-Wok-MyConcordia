using OracleEntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Picture
{
    public interface IPictureRepository
    {
        Task<StudentPictures> FindStudentPictures(int id);
        void AddPendingPicture(string netName, byte[] picture);
        PICTURE FindPendingPicture(int id);
        string AddPictureComment(PictureComment comment);
    }
}
