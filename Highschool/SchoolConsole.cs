namespace Highschool
{
    public static class SchoolConsole
    {
        public static void ShowTimetable(string timetableName, Booking[] bookings)
        {
            ShowTitle(timetableName);
            foreach (var booking in bookings)
            {
                var maindetails = GetTimetableLine(booking).PadRight(53);
                var className = booking.Subject.Name;

                Console.WriteLine($"{maindetails} {className}");
            }
            Console.WriteLine();
        }

        public static void ShowStudentList(ICourse subject)
        {
            ShowTitle(subject.Name);
            foreach (var name in subject.StudentNames)
            {
                Console.WriteLine(name);
            }
            Console.WriteLine();
        }

        public static void ShowSuggestedTimes(Booking[] times)
        {
            ShowTitle("Suggested Times");
            foreach (var time in times)
            {
                var timetableLine = GetTimetableLine(time);
                Console.WriteLine(timetableLine);
            }
            Console.WriteLine();
        }

        private static string GetTimetableLine(Booking time)
        {
            var dayPadded = $"{time.Day}:".PadRight(12);
            var startPadded = $"{time.StartTime}".PadRight(9);
            var endPadded = $"{time.EndTime}".PadRight(8).PadLeft(9);
            var roomPadded = $"{time.Room.Name}".PadLeft(15);

            return $"{dayPadded} {startPadded} - {endPadded} {roomPadded}";
        }

        private static void ShowTitle(string title)
        {
            Console.WriteLine($"*** {title} ***");
        }
    }
}

