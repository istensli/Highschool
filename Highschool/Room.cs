using System;

namespace Highschool
{
    internal class Room
    {
        public string Name { get; }
        public Subject[,] Schedule;

        public Room(string name) 
        {
            Name = name;
            Schedule = new Subject[5, 4];
        }
        public void AddBooking(Subject subject, int day, int hour) 
        {
            Schedule[day, hour] = subject;
        }
    }
}
