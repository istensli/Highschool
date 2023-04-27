using System;

namespace Highschool
{
    public interface ISchoolMember
    {
        List<Subject> Subjects { get; }
        string Name { get; }
        string Role { get; }

        public abstract void AddSubject(Subject subject);
    }
}