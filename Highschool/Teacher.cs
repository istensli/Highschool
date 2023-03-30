using System;

namespace Highschool
{
    internal class Teacher : ISchoolMember
    {
        public List<Subject> Subjects { get; }
        public string Name { get; }

        public Teacher(string name)
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