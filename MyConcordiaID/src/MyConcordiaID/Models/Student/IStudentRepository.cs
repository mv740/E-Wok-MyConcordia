using Microsoft.AspNetCore.Http;
using MyConcordiaID.Models.Picture;
using OracleEntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Models.Student
{
    public interface IStudentRepository
    {
        dynamic GetAll();
        STUDENT Find(int id);
        void AddPendingPicture(int id, IFormFile file);
        dynamic FindPendingPicture(int id);
        void ValidatePicture(PictureValidation pictureValidation);
        dynamic GetAllPending();
        dynamic GetAllValid();
        void Add(STUDENT student);
        PicturePeriod GetUpdatePicturePeriod();
        List<STUDENT> Search(SearchOptions searchOptions);
        bool DoesStudentExist(string firstName, string lastName);
    }
}
