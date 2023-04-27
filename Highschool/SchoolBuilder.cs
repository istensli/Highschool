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

            var teacherTerje = new Teacher("Terje", "https://getacademy.no/hubfs/Imported%20sitepage%20images/Terje.jpg");
            var teacherAmundsen = new Teacher("Amundsen", "https://bok365.no/wp-content/uploads/2022/05/Amundsen_in_fur_skins-1536x1053.jpg");
            var teacherEskil = new Teacher("Eskil", "https://getacademy.no/hubfs/Imported%20sitepage%20images/eskil.jpg");
            _school.AddTeachers(teacherTerje, teacherAmundsen, teacherEskil);

            var martin = new Student("Martin", "https://assets.pokemon.com/assets/cms2/img/pokedex/detail/001.png");
            var stian = new Student("Stian", "https://assets.pokemon.com/assets/cms2/img/pokedex/detail/002.png");
            var geir = new Student("Geir", "https://assets.pokemon.com/assets/cms2/img/pokedex/detail/003.png");
            var meabh = new Student("Meabh", "https://assets.pokemon.com/assets/cms2/img/pokedex/detail/004.png");
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