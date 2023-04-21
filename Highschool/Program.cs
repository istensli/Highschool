namespace Highschool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var school = new School();

            var classroomOne = new Room("Classroom one");
            var classroomTwo = new Room("Classroom two");
            school.AddRooms(classroomOne, classroomTwo);

            var teacherTerje = new Teacher("Terje");
            var teacherAmundsen = new Teacher("Amundsen");
            var teacherEskil =  new Teacher("Eskil");
            school.AddTeachers(teacherTerje, teacherAmundsen, teacherEskil);

            var martin = new Student("Martin");
            var stian = new Student("Stian");
            var geir = new Student("Geir");
            var meabh = new Student("Meabh");
            school.AddStudents(martin, stian, geir, meabh);

            var computing = new Subject(teacherTerje, "Computing");
            var geography = new Subject(teacherAmundsen, "Geography");
            var keyCompetences = new Subject(teacherEskil, "Key Competences");
            school.AddSubjects(computing, geography, keyCompetences);

            computing.AddStudents(martin, stian);
            geography.AddStudents(geir, meabh);
            keyCompetences.AddStudents(martin, stian, geir, meabh);

            var mediaAndCommunciationSubjects = new List<Subject>() { computing };
            var mediaAndCommunication = new Class("Media and Communication", mediaAndCommunciationSubjects);
            var generalStudiesSubjects = new List<Subject>() { geography };
            var generalStudies = new Class("General Studies", generalStudiesSubjects);
            school.AddClasses(mediaAndCommunication, generalStudies);

            school.AddBooking(new Booking(computing, classroomOne, DaysOfWeek.Monday, new TimeOnly(9,15), new TimeOnly(9,45)));
            school.AddBooking(new Booking(keyCompetences, classroomOne, DaysOfWeek.Friday, new TimeOnly(12, 45), new TimeOnly(14,45)));
            school.AddBooking(new Booking(keyCompetences, classroomTwo, DaysOfWeek.Wednesday, new TimeOnly(11, 5), new TimeOnly(12,35)));
            school.AddBooking(new Booking(computing, classroomTwo, DaysOfWeek.Tuesday, new TimeOnly(14, 20), new TimeOnly(15,20)));
            school.AddBooking(new Booking(geography, classroomTwo, DaysOfWeek.Thursday, new TimeOnly(10, 50), new TimeOnly(13, 50)));
            
            // Show teachers timetable
            var terjesTimeTable = school.GetTimeTable(teacherTerje);
            SchoolConsole.ShowTimetable("Terje's Timetable", terjesTimeTable);

            // Show student timetable
            var martinsTimeTable = school.GetTimeTable(martin);
            SchoolConsole.ShowTimetable("Martin's Timetable", martinsTimeTable);

            // Show students in each subject
            SchoolConsole.ShowStudentList(computing);
            SchoolConsole.ShowStudentList(geography);
            SchoolConsole.ShowStudentList(keyCompetences);

            //Show students in each class
            SchoolConsole.ShowStudentList(mediaAndCommunication);
            SchoolConsole.ShowStudentList(generalStudies);
            
            // Suggest available time for class
            var suggestedTimes = school.GetSuggestedTimes(computing);
            SchoolConsole.ShowSuggestedTimes(suggestedTimes);
        }
    }
}