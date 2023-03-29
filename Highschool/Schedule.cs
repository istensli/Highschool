using System;

namespace Highschool
{
    internal class Schedule
    {
        private Subject[][] _schedule;
        public Schedule()
        { 
            _schedule = new Subject[5][4];
        }
        public void Add(Subject subject, int day, int hour)
        {
            _schedule[day][hour] = subject;
        }
    }
}
