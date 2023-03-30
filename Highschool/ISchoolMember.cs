using System;

namespace Highschool
{
    interface ISchoolMember
    {
        List<Subject> Subjects { get; }
        string Name { get; }

        public abstract void AddSubject(Subject subject);
    }
}