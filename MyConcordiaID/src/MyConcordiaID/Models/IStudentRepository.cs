using System.Collections.Generic;

namespace MyConcordiaID.Models
{
    public interface IStudentRepository
    {
        void Add(Student item);
        IEnumerable<Student> GetAll();
        Student Find(int id);
        Student Remove(int id);
        void Update(Student item);
    }
}
