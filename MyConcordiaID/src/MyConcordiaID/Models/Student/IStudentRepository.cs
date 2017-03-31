using MyConcordiaID.Models.Picture;
using OracleEntityFramework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Student
{
    public interface IStudentRepository
    {
        Task<dynamic> GetAllAsync();
        Task<StudentAccount> FindByIdAsync(int id);
        Task<StudentAccount> FindByNetNameAsync(string netname);
        string ValidatePicture(PictureValidation pictureValidation, string netName);
        string RevalidatePicture(PictureValidation pictureValidation, string netname);
        void Add(STUDENT student);
        Task<PicturePeriod> GetUpdatePicturePeriodAsync();
        IEnumerable<STUDENT> Search(SearchOptions searchOptions);
        bool DoesStudentExist(string firstName, string lastName);
        
    }
}
