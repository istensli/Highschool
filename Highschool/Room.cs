using System;

namespace Highschool
{
    internal class Room
    {
        private string RoomName { get;}
        private Schedule _schedule;

        public Room(string roomName) 
        {
            RoomName = roomName;
            Schedule = new Schedule();

        }

        public void AddBooking(Subject subject, int day, int hour) 
        {
            _schedule.Add(subject, day, hour);
                

        }


    }
}
