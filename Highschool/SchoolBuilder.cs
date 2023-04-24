namespace Highschool
{
    public class SchoolBuilder
    {
        private School _school;

        public SchoolBuilder(School school)
        {
            _school = school;
        }
        public School Build()
        {
            var classroomOne = new Room("Classroom One");
            var classroomTwo = new Room("Classroom Two");
            _school.AddRooms(classroomOne, classroomTwo);

            var teacherTerje = new Teacher("Terje");
            var teacherAmundsen = new Teacher("Amundsen");
            var teacherEskil = new Teacher("Eskil");
            _school.AddTeachers(teacherTerje, teacherAmundsen, teacherEskil);

            var martin = new Student("Martin");
            var stian = new Student("Stian");
            var geir = new Student("Geir");
            var meabh = new Student("Meabh");
            _school.AddStudents(martin, stian, geir, meabh);

            var computing = new Subject(teacherTerje, "Computing");
            var geography = new Subject(teacherAmundsen, "Geography");
            var keyCompetences = new Subject(teacherEskil, "Key Competences");
            _school.AddSubjects(computing, geography, keyCompetences);

            computing.AddStudents(martin, stian);
            geography.AddStudents(geir, meabh);
            keyCompetences.AddStudents(martin, stian, geir, meabh);

            var mediaAndCommunciationSubjects = new List<Subject>() { computing };
            var mediaAndCommunication = new Class("Media And Communication", mediaAndCommunciationSubjects);
            var generalStudiesSubjects = new List<Subject>() { geography };
            var generalStudies = new Class("General Studies", generalStudiesSubjects);
            _school.AddClasses(mediaAndCommunication, generalStudies);

            _school.AddBooking(new Booking(computing, classroomOne, DaysOfWeek.Monday, new TimeOnly(9, 15), new TimeOnly(9, 45)));
            _school.AddBooking(new Booking(keyCompetences, classroomOne, DaysOfWeek.Friday, new TimeOnly(12, 45), new TimeOnly(14, 45)));
            _school.AddBooking(new Booking(keyCompetences, classroomTwo, DaysOfWeek.Wednesday, new TimeOnly(11, 5), new TimeOnly(12, 35)));
            _school.AddBooking(new Booking(computing, classroomTwo, DaysOfWeek.Tuesday, new TimeOnly(14, 20), new TimeOnly(15, 20)));
            _school.AddBooking(new Booking(geography, classroomTwo, DaysOfWeek.Thursday, new TimeOnly(10, 50), new TimeOnly(13, 50)));

            return _school;
        }
    }
}