using System;

namespace Highschool
{
    internal class Student : ISchoolMember
    {
        public List<Subject> Subjects { get; }
        public string Name { get; }

        public Student(string name)
        {
            Name = name;
            Subjects = new List<Subject>();
        }
        public void AddSubject(Subject subject)
        {
            Subjects.Add(subject);
        }
    }   
}
