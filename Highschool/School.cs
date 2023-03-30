using System.Linq;
using System.Xml.Linq;
using Highschool;

namespace Highschool
{
    internal class School
    {
        private List<Room> _rooms;
        private List<Subject> _subjects;
        private List<Class> _classes;
        public List<Student> Students { get; private set; }
        public List<Teacher> Teachers { get; private set; }

        public School()
        {
            _rooms = new List<Room>();
            _subjects = new List<Subject>();
            _classes = new List<Class>();
            Students = new List<Student>();
            Teachers = new List<Teacher>();
        }
        internal Teacher AddTeacher(string name)
        {
            var teacher = new Teacher(name);
            Teachers.Add(teacher);
            return teacher;
        }
        internal Student AddStudent(string name)
        {
            var student = new Student(name);
            Students.Add(student);
            return student;
        }
        internal Room AddRoom(string roomName)
        {
            var room = new Room(roomName);
            _rooms.Add(room);
            return room;
        }
        internal Subject AddSubject(Teacher teacher, string subjectName)
        {
            var subject = new Subject(teacher, subjectName);
            _subjects.Add(subject);
            return subject;
        }
        internal Class AddClass(string className, List<Subject> generalStudiesSubjects)
        {
            var newClass = new Class(className, generalStudiesSubjects);
            _classes.Add(newClass);
            return newClass;
        }
        internal string[,] GetTimetable(ISchoolMember schoolMember)
        {
            var times = new string[] { "0900", "1130", "1330", "1530" };
            var subjects = schoolMember.Subjects;
            var schedule = new string[5, 4];

            foreach (var room in _rooms)
            {
                var roomSchedule = room.Schedule;

                for (int i = 0; i < roomSchedule.GetLength(0); i++)
                {
                    for (int j = 0; j < roomSchedule.GetLength(1); j++)
                    {
                        var hourlySlot = roomSchedule[i, j];

                        if (subjects.Contains(hourlySlot))
                        {
                            schedule[i, j] = $"{times[j]}: {hourlySlot.Name} class in {room.Name}";
                        }
                    }
                }
            }
            return schedule;
        }
        public List<List<string>> GetSuggestedTimes(Subject subject)
        {
            var times = new string[] { "0900", "1130", "1330", "1530" };
            var suggestedTimes = new List<List<string>>();
            var suggestedTimesSchedule = GetSuggestedTimesSchedule(subject);

            for (int day = 0; day < suggestedTimesSchedule.GetLength(0); day++)
            {
                var availableTimesThisDay = new List<string>();
                for (int time = 0; time < suggestedTimesSchedule.GetLength(1); time++)
                {
                    var timetableSlot = suggestedTimesSchedule[day, time];
                    if (timetableSlot != null)
                    {
                        var availableRooms = timetableSlot.Split(",");
                        foreach (var room in availableRooms)
                        {
                            availableTimesThisDay.Add($"Room {room} is available at {times[time]}");
                        }
                    }
                }
                suggestedTimes.Add(availableTimesThisDay);
            }
            return suggestedTimes;
        }
        private string[,] GetSuggestedTimesSchedule(Subject subject)
        {
            var suggestedTimesSchedule = new string[5, 4];
            // loop of rooms
            for (int i = 0; i < _rooms.Count; i++)
            { 
                var roomSchedule = _rooms[i].Schedule;
                // loop for each day of schedule (room) 
                for (int day = 0; day < roomSchedule.GetLength(0); day++)
                {
                    // loop for classtime (room)
                    for (int time = 0; time < roomSchedule.GetLength(1); time++)
                    {
                        var currentRoomBooking = roomSchedule[day, time];
                        var suggestTimeScheduleSlot = suggestedTimesSchedule[day, time];

                        // code to check that room can be booked
                        if (currentRoomBooking != null)
                        {
                            suggestedTimesSchedule[day, time] = null;
                        }
                        else if (currentRoomBooking?.Teacher.Name == subject.Teacher.Name)
                        {
                            suggestedTimesSchedule[day, time] = null;
                        }

                        else if (CurrentRoomBookingContainsAStudentsInRequestedBooking(currentRoomBooking, subject))
                        {
                            suggestedTimesSchedule[day, time] = null;
                        }
                        else
                        {
                            var roomNo = i + 1;

                            if (suggestedTimesSchedule[day, time] == null)
                            {
                                suggestedTimesSchedule[day, time] = roomNo.ToString();
                            }
                            else
                            {
                                suggestedTimesSchedule[day, time] += "," + roomNo;
                            }
                        }
                    }
                }
            }
            return suggestedTimesSchedule;
        }
        private bool CurrentRoomBookingContainsAStudentsInRequestedBooking(Subject currentRoomBooking, Subject requestedRoomBooking)
        {
            if (currentRoomBooking == null) return false;

            foreach (var student in requestedRoomBooking.Students)
            {
                if (currentRoomBooking.Students.Contains(student))
                {
                    return true;
                }
            }
            return false;
        }
    }
}