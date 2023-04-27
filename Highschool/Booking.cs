using System;
using System.Runtime;
using System.Xml.Linq;

namespace Highschool
{
    public class Booking : ICloneable
    {
        public Subject Subject { get; }
        public Room Room { get; set; }
        private string BookingName => Subject == null ? "Free" : Subject.Name;
        public DaysOfWeek Day { get; }
        public TimeOnly StartTime { get; private set; }
        public TimeOnly EndTime { get; private set; }

        public Booking(Subject subject, Room room, DaysOfWeek day, TimeOnly startTime, TimeOnly endTime)
        {
            if (startTime.Hour < 9 || StartTime.Hour > 17)
            {
                throw new ArgumentOutOfRangeException();
            }

            Subject = subject;
            Room = room;
            Day = day;
            StartTime = startTime;
            EndTime = endTime;
        }
        public bool IsBooked(Booking otherTime)
        {
            if (otherTime.StartTime.IsBetween(StartTime, EndTime))
            {
                return true;
            }
            else if (otherTime.EndTime.IsBetween(StartTime, EndTime))
            {
                return true;
            }
            else return false;
        }
        public void MatchStartOrEndTime(Booking otherTime)
        {
            if (otherTime.StartTime.IsBetween(StartTime, EndTime))
            {
                otherTime.StartTime = new TimeOnly(StartTime.Hour, StartTime.Minute);
            }
            else if (otherTime.EndTime.IsBetween(StartTime, EndTime))
            {
                otherTime.EndTime = new TimeOnly(EndTime.Hour, EndTime.Minute);
            }
        }

        public object Clone()
        {
            var time = (Booking)this.MemberwiseClone();
            time.StartTime = new TimeOnly(StartTime.Hour, StartTime.Minute);
            time.EndTime = new TimeOnly(EndTime.Hour, EndTime.Minute);

            return time;
        }
    }
}