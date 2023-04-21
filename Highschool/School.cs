using System.Linq;

namespace Highschool
{
    public class School
    {
        private List<Room> _rooms;
        private List<Subject> _subjects;
        private List<Class> _classes;
        private List<Student> _students;
        private List<Teacher> _teachers;
        private Schedule _schedule;

        public School()
        {
            _rooms = new List<Room>();
            _subjects = new List<Subject>();
            _classes = new List<Class>();
            _students = new List<Student>();
            _teachers = new List <Teacher>();
            _schedule = new Schedule();
        }

        public void AddTeachers(params Teacher[] teachers)
        {
            foreach (var teacher in teachers)
            {
                _teachers.Add(teacher);
            }
        }

        public void AddStudents(params Student[] students)
        {
            foreach (var student in students)
            {
                _students.Add(student);
            }
        }

        public void AddRooms(params Room[] rooms)
        {
            foreach (var room in rooms)
            {
                _rooms.Add(room);
            }
        }

        public void AddSubjects(params Subject[] subjects)
        {
            foreach (var subject in subjects)
            {
                _subjects.Add(subject);
            }
        }

        public void AddClasses(params Class[] classes)
        {
            foreach (var clas in classes)
            {
                _classes.Add(clas);
            }
        }

        public Booking[] GetSuggestedTimes(Subject subject)
        {
            return _schedule.GetSuggestedTimes(subject, _rooms);
        }

        public void AddBooking(Booking booking)
        {
            _schedule.AddBooking(booking, _rooms);
        }

        public Booking[] GetTimeTable(ISchoolMember schoolMembers)
        {
            return _schedule.GetTimeTable(schoolMembers, _rooms);
        }
    }
}