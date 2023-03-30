using System;
namespace Highschool
{
    internal class Class
    {
        private string _name;
        private List<Subject> _subjects;
        private List<Student> _students => RegisterStudents();

        public Class(string name, List<Subject> subjects)
        {
            _name = name;
            _subjects = subjects;
        }
        private List<Student> RegisterStudents()
        {
            var students = new List<Student>();
            foreach (var subject in _subjects)
            {
                students.AddRange(subject.Students);
            }
            return students;
        }
        public string[] GetStudents()
        {
            return _students.Select(s => s.Name).ToArray();
        }
    }
}

