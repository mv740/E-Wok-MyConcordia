using OracleEntityFramework;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Picture
{
    public interface IPictureRepository
    {
        Task<StudentPictures> FindStudentPicturesAsync(int id);
        void AddPendingPicture(string netName, byte[] picture);
        PICTURE FindPendingPicture(int id);
        string AddPictureComment(PictureComment comment);
    }
}
