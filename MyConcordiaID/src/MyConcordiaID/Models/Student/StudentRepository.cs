using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MyConcordiaID.Models
{
    public class StudentRepository : IStudentRepository
    {
        private static ConcurrentDictionary<int, Student> students =
              new ConcurrentDictionary<int, Student>();

        public StudentRepository()
        {
            Add(new Student { FirstName = "John", LastName = "Smith", Id = 212345 });
        }

        public IEnumerable<Student> GetAll()
        {
            return students.Values;
        }

        public void Add(Student student)
        {

            students[student.Id] = student;
        }

        public Student Find(int Id)
        {
            Student item;
            students.TryGetValue(Id, out item);
            return item;
        }

        public Student Remove(int Id)
        {
            Student student;
            students.TryRemove(Id, out student);
            return student;
        }

        public void Update(Student student)
        {
            students[student.Id] = student;
        }
    }
}
