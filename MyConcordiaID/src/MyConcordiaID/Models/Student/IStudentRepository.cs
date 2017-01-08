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
        string ValidatePicture(PictureValidation pictureValidation, string netName);
        string RevalidatePicture(PictureValidation pictureValidation, string netname);
        void Add(STUDENT student);
        PicturePeriod GetUpdatePicturePeriod();
        IEnumerable<STUDENT> Search(SearchOptions searchOptions);
        bool DoesStudentExist(string firstName, string lastName);
        
    }
}
