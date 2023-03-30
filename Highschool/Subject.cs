using System;
namespace Highschool
{
    internal class Subject
    {
        public readonly string Name;
        public Teacher Teacher { get; private set; }
        public List<Student> Students { get; private set; }

        public Subject(Teacher teacher, string name) 
        {
            Name = name;
            Teacher = teacher;
            Students = new List<Student>();
            teacher.AddSubject(this);
        }
        internal void AddStudentToCourse(Student student)
        {
            Students.Add(student);
            student.AddSubject(this);
        }
        public string[] GetStudents()
        {
            return Students.Select(s => s.Name).ToArray();
        }
    }
}

/* 
 * One class could be media and communication
 * Another class be Art
 * A common subject could be math and Norwegian
 */
