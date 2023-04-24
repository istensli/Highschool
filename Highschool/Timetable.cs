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

        private List<Booking> GetSuggestedTimesForRoom(List<Booking> bookings)
        {
            var room = bookings[0].Room;
            var times = new List<Booking>();

            if (IsFirstBookingInListAfter9AM(bookings))
            {
                var startOfDayBooking = GetStartOfDayBooking(room, bookings[0]);
                times.Add(startOfDayBooking);
            }

            for (int i = 0; i < bookings.Count(); i++)
            {
                var currentBooking = bookings[i];

                if (IsCurrentBookingTheSameDayAsNextBooking(bookings, i))
                {
                    var bookingBetweenCurrentAndNextBooking = GetBookingBetweenCurrentAndNextBooking(bookings, room, i);
                    times.Add(bookingBetweenCurrentAndNextBooking);
                }
                else if (IsCurrentBookingADifferentDayThanNextBooking(bookings, currentBooking.Day, i))
                {
                    var nextBooking = bookings[i + 1];
                    var endOfDayBooking = GetEndOfDayBooking(room, currentBooking);
                    times.Add(endOfDayBooking);

                    if(IsBookingAfterStartOfDay(nextBooking))
                    {
                        var startOfDayBooking = GetStartOfDayBooking(room, nextBooking);
                        times.Add(startOfDayBooking);
                    }
                }
                else if (IsLastBookingBeforeTheEndOfTheDay(bookings, i))
                {
                    var endOfDayBooking = GetEndOfDayBooking(room, currentBooking);
                    times.Add(endOfDayBooking);
                }
            }

            var daysWithNoBookingClashes = GetFreeDays(times);
            times.AddRange(daysWithNoBookingClashes);

            return times;
        }

        private Booking GetBookingBetweenCurrentAndNextBooking(List<Booking> bookings, Room room, int index)
        {
            var endTime = bookings[index+1].StartTime;
            return new Booking(null, room, bookings[index].Day, bookings[index].EndTime, endTime);
        }

        private bool IsCurrentBookingTheSameDayAsNextBooking(List<Booking> bookings, int index)
        {
            return ((index + 1 != bookings.Count()) && bookings[index].Day == bookings[index + 1].Day);
        }

        private bool IsLastBookingBeforeTheEndOfTheDay(List<Booking> bookings, int index)
        {
            return (index == bookings.Count() - 1 && bookings[index].EndTime.Hour != 17);
        }

        private bool IsBookingAfterStartOfDay(Booking booking)
        {
            return (booking.StartTime.Hour >= 9 && booking.StartTime.Minute > 0);
        }

        private bool IsCurrentBookingADifferentDayThanNextBooking(List<Booking> bookings, DaysOfWeek day, int i)
        {
             return (i + 1 != bookings.Count() && day != bookings[i + 1].Day) ;
        }

        private bool IsFirstBookingInListAfter9AM(List<Booking> bookings)
        {
            return (bookings[0].StartTime.Hour == 9 && bookings[0].StartTime.Minute > 0);
        }

        private Booking GetStartOfDayBooking(Room room, Booking nextBooking)
        {
            return new Booking(null, room, nextBooking.Day, new TimeOnly(9, 0), nextBooking.StartTime);
        }

        private Booking GetEndOfDayBooking(Room room, Booking previousBooking)
        {
            return new Booking(null, room, previousBooking.Day, previousBooking.EndTime, new TimeOnly(17, 0));
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