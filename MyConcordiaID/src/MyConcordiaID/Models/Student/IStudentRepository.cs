using MyConcordiaID.Models.Picture;
using OracleEntityFramework;
using System.Collections.Generic;

namespace MyConcordiaID.Models.Student
{
    public interface IStudentRepository
    {
        dynamic GetAll();
        STUDENT FindById(int id);
        dynamic FindByNetName(string netname);
        void AddPendingPicture(string netName, byte[] picture);
        dynamic FindPendingPicture(int id);
        string ValidatePicture(PictureValidation pictureValidation);
        void Add(STUDENT student);
        PicturePeriod GetUpdatePicturePeriod();
        IEnumerable<STUDENT> Search(SearchOptions searchOptions);
        bool DoesStudentExist(string firstName, string lastName);
        StudentPictures FindStudentPictures(int id);
    }
}
