using System;
namespace Highschool
{
    internal class Subject
    {
        string SubjectName { get; }
        private Teacher _teacher;
        private List<Student> _students;

        public Subject(string subjectName, Teacher teacher) 
        {
            SubjectName = subjectName;
            _teacher = teacher;
            _students = new List<Students>();
        
        }

        public void AddStudent(Student student)
        {
            _students.Add(student);

        }
    }
}
