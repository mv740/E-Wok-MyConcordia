using MyConcordiaID.Models.Picture;
using OracleEntityFramework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Student
{
    public interface IStudentRepository
    {
        Task<dynamic> GetAll();
        Task<StudentAccount> FindById(int id);
        Task<StudentAccount> FindByNetName(string netname);
        string ValidatePicture(PictureValidation pictureValidation, string netName);
        string RevalidatePicture(PictureValidation pictureValidation, string netname);
        void Add(STUDENT student);
        PicturePeriod GetUpdatePicturePeriod();
        IEnumerable<STUDENT> Search(SearchOptions searchOptions);
        bool DoesStudentExist(string firstName, string lastName);
        
    }
}
