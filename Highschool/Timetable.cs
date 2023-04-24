using System.Linq;

namespace Highschool
{
    public class Timetable
    {
        private List<Booking> _bookings;
        private List<Room> _rooms;

        public Timetable()
        {
            _bookings = new List<Booking>();
        }

        public void AddBooking(Booking booking, List<Room> rooms)
        {
            _rooms = rooms;
            _bookings.Add(booking);
        }

        private bool IsBookable(Booking booking)
        {

            return _bookings.Any(b => b.IsBooked(booking));
        }

        public Booking[] GetTimeTable(ISchoolMember schoolMember, List<Room> rooms)
        {
            _rooms = _rooms;

            return _bookings
                .Where(b =>
                    b.Subject.Students.Contains(schoolMember) || b.Subject.Teacher == schoolMember)
                .ToArray();
        }

        public Booking[] GetSuggestedTimes(Subject subject, List<Room> rooms)
        {
            _rooms = rooms;
            var classMemberBookings = GetClassMemberBookings(subject);
            var suggestedTimes = FindSuggestedTimes(classMemberBookings);

            return suggestedTimes.OrderBy(t => t.Day)
                .ThenBy(t => t.StartTime)
                .ThenBy(t => t.Day)
                .ToArray(); ;
        }

        private List<Booking> GetClassMemberBookings(Subject subject)
        {
            var subjectBookings = _bookings.Where(b => b.Subject == subject);
            var teacherBookings = _bookings.Where(b => b.Subject.Teacher == subject.Teacher);
            var studentBookings = _bookings.Where(b => b.Subject.Students.Intersect(subject.Students).Count() > 0);

            return subjectBookings.Union(teacherBookings).Union(studentBookings).ToList();
        }

        private IEnumerable<Booking> FindSuggestedTimes(List<Booking> classMemberBookings)
        {
            var times = new List<Booking>();

            foreach (var room in _rooms)
            {
                var roomBookings = _bookings.Where(b => b.Room == room).ToList();
                var unavailableTimesForRoomAndClassMembers = MergeBookings(roomBookings, classMemberBookings);
                var suggestedTimesForRoom = GetSuggestedTimesForRoom(unavailableTimesForRoomAndClassMembers);
                times.AddRange(suggestedTimesForRoom);
            }

            return times.OrderBy(t => t.Room.Name)
                .ThenBy(t => t.Day)
                .ThenBy(t => t.StartTime);
        }

        private List<Booking> GetSuggestedTimesForRoom(List<Booking> alreadyBooked)
        {
            var room = alreadyBooked[0].Room;
            var times = new List<Booking>();

            if (alreadyBooked[0].StartTime.Hour == 9 && alreadyBooked[0].StartTime.Minute > 0)
            {
                var startTime = new TimeOnly(9, 0);
                times.Add(new Booking(null, room, alreadyBooked[0].Day, startTime, alreadyBooked[0].StartTime));
            }

            for (int i = 0; i < alreadyBooked.Count(); i++)
            {
                var cb = alreadyBooked[i];

                if ((i + 1 != alreadyBooked.Count()) && cb.Day == alreadyBooked[i+1].Day)
                {
                    var endTime = alreadyBooked[i+1].StartTime;
                    times.Add(new Booking(null, room, cb.Day, cb.EndTime, endTime));
                }
                else if ((i+1 != alreadyBooked.Count()) && cb.Day != alreadyBooked[i+1].Day)
                {
                    var nextBooking = alreadyBooked[i + 1];
                    times.Add(new Booking(null, room, cb.Day, cb.EndTime, new TimeOnly(17, 0)));

                    if(nextBooking.StartTime.Hour >= 9 && nextBooking.StartTime.Minute > 0)
                    {
                        times.Add(new Booking(null, room, nextBooking.Day, new TimeOnly(9, 0), nextBooking.StartTime));
                    }
                }
                else if (i == alreadyBooked.Count() - 1 && cb.EndTime.Hour != 17)
                {
                    times.Add(new Booking(null, room, cb.Day, cb.EndTime, new TimeOnly(17, 0)));
                }
            }

            var daysWithNoBookingClashes = GetFreeDays(times);
            times.AddRange(daysWithNoBookingClashes);

            return times;
        }

        private List<Booking> MergeBookings(List<Booking> roomBookings, List<Booking> classMemberBookings)
        {
            var room = roomBookings[0].Room;
            var mergedBookings = new List<Booking>();

            foreach (var booking in roomBookings)
            {
                var clonedBooking = (Booking)booking.Clone();
                classMemberBookings.ForEach(cm => cm.MatchStartOrEndTime(clonedBooking));
                mergedBookings.Add(clonedBooking);
            }

            foreach (var booking in classMemberBookings)
            {
                if (!mergedBookings.Any(b => b.IsBooked(booking)))
                {
                    var clonedBooking = (Booking)booking.Clone();
                    clonedBooking.Room = room;
                    mergedBookings.Add(clonedBooking);
                }
            }

            return mergedBookings.OrderBy(b => b.Day).ThenBy(b => b.StartTime).ToList();
        }
        private List<Booking> GetFreeDays(List<Booking> suggestedTimes)
        {
            var room = suggestedTimes[0].Room;
            var daysNotToInclude = suggestedTimes.Select(t => t.Day).Distinct();
            var list = new List<Booking>();

            foreach (DaysOfWeek day in Enum.GetValues(typeof(DaysOfWeek)))
            {
                if (!daysNotToInclude.Any(d => d == day))
                {
                    list.Add(new Booking(null, room, day, new TimeOnly(9, 0), new TimeOnly(17, 0)));
                }
            }
            return list;
        }
    }
}