using System;
namespace Highschool
{
    public class Class : ICourse
    {
        public string Name { get; }
        public List<Subject> Subjects { get; private set; }
        public List<Student> Students => GetRegisteredStudents();
        public string[] StudentNames => Students.Select(s => s.Name).ToArray();

        public Class(string name, List<Subject> subjects)
        {
            Name = name;
            Subjects = subjects;
        }
        private List<Student> GetRegisteredStudents()
        {
            var students = new List<Student>();
            foreach (var subject in Subjects)
            {
                students.AddRange(subject.Students);
            }
            return students;
        }
    }
}