using System;

namespace Highschool
{
    public class Student : ISchoolMember
    {
        public List<Subject> Subjects { get; }
        public string Role { get; }
        public string ProfilePicture { get; }
        public string Name { get; }

        public Student(string name, string profilePictureUrl)
        {
            Name = name;
            Subjects = new List<Subject>();
            Role = "Student";
            ProfilePicture = profilePictureUrl;
        }
        
        public void AddSubject(Subject subject)
        {
            Subjects.Add(subject);
        }
    }   
}