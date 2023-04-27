using System;

namespace Highschool
{
    public class Teacher : ISchoolMember
    {
        public List<Subject> Subjects { get; }
        public string Name { get; }
        public string Role { get; }
        public string ProfilePicture { get; }

        public Teacher(string name, string profilePictureUrl)
        {
            Name = name;
            Subjects = new List<Subject>();
            Role = "Teacher";
            ProfilePicture = profilePictureUrl;

        }

        public void AddSubject(Subject subject)
        {
            Subjects.Add(subject);
        }
    }
}