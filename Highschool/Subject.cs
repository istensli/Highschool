using System;
namespace Highschool
{
    public class Subject : ICourse
    {
        public string Name { get; }
        public Teacher Teacher { get; private set; }
        public List<Student> Students { get; private set; }
        public string[] StudentNames => Students.Select(s => s.Name).ToArray();

        public Subject(Teacher teacher, string name) 
        {
            Name = name;
            Teacher = teacher;
            Students = new List<Student>();
            teacher.AddSubject(this);
        }
        public void AddStudents(params Student[] students)
        {
            foreach (var student in students)
            {
                Students.Add(student);
                student.AddSubject(this);
            }
        }
    }
}