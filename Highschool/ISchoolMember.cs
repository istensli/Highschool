using System;

namespace Highschool
{
    public interface ISchoolMember
    {
        List<Subject> Subjects { get; }
        string Name { get; }

        public abstract void AddSubject(Subject subject);
    }
}