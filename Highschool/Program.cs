namespace Highschool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var school = new School();
            var classroomOne = school.AddRoom("Classroom one");
            var classroomTwo = school.AddRoom("Classroom two");

            var teacherTerje = school.AddTeacher("Terje");
            var teacherAmundsen = school.AddTeacher("Amundsen");
            var teacherEskil = school.AddTeacher("Eskil");

            var martin = school.AddStudent("Martin");
            var stian = school.AddStudent("Stian");
            var geir = school.AddStudent("Geir");
            var meabh = school.AddStudent("Meabh");

            var computing = school.AddSubject(teacherTerje, "Computing");
            var geography = school.AddSubject(teacherAmundsen, "Geography");
            var keyCompetences = school.AddSubject(teacherEskil, "Key Competences");

            computing.AddStudentToCourse(martin);
            computing.AddStudentToCourse(stian);
            geography.AddStudentToCourse(geir);
            geography.AddStudentToCourse(meabh);
            keyCompetences.AddStudentToCourse(martin);
            keyCompetences.AddStudentToCourse(stian);
            keyCompetences.AddStudentToCourse(geir);
            keyCompetences.AddStudentToCourse(meabh);

            var mediaAndCommunciationSubjects = new List<Subject>() { computing };
            var mediaAndCommunication = school.AddClass("Media and Communication", mediaAndCommunciationSubjects);
            var generalStudiesSubjects = new List<Subject>() { geography };
            var generalStudies = school.AddClass("General Studies", generalStudiesSubjects);

            classroomOne.AddBooking(computing, 0, 0);
            classroomTwo.AddBooking(keyCompetences, 2, 3);
            classroomOne.AddBooking(computing, 3, 1);
            classroomOne.AddBooking(keyCompetences, 4, 2);
            classroomTwo.AddBooking(geography, 1, 3);
            
            // Show teachers timetable
            var terjeSchedule = school.GetTimetable(teacherTerje);
            ShowIndividualTimetable(teacherTerje.Name, terjeSchedule);

            
            // Show student timetable
            var martinSchedule = school.GetTimetable(martin);
            ShowIndividualTimetable(martin.Name, martinSchedule);
            
            // Show students in each subject
            Console.WriteLine("Computing subject:");
            ShowSubjectOrClassTimetable(computing.GetStudents());
            Console.WriteLine("Geography subject:");
            ShowSubjectOrClassTimetable(geography.GetStudents());
            Console.WriteLine("Key Competences subject:");
            ShowSubjectOrClassTimetable(keyCompetences.GetStudents());

            // Show students in each class
            Console.WriteLine("Media and Communcation class:");
            ShowSubjectOrClassTimetable(mediaAndCommunication.GetStudents());
            Console.WriteLine("General Studies class:");
            ShowSubjectOrClassTimetable(generalStudies.GetStudents());
            
            // Suggest available time for class
            var suggestedTimes = school.GetSuggestedTimes(computing);
            ShowSuggestedTimes(computing.Name, suggestedTimes);
        }
        static void ShowIndividualTimetable(string name, string[,] schedule)
        {
            Console.WriteLine($"{name}'s Schedule:" + "\n");
            var daysOfWeek = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

            for (int day = 0; day < schedule.GetLength(0); day++)
            {
                Console.WriteLine(daysOfWeek[day]);
                for (int j = 0; j < schedule.GetLength(1); j++)
                {
                    var scheduleEntry = schedule[day, j];
                    
                    if (scheduleEntry != null)
                    {
                        Console.WriteLine(scheduleEntry);
                    }
                }
            }
            Console.WriteLine();
        }
        static void ShowSubjectOrClassTimetable(string[] students)
        {
            foreach (var student in students)
            {
                Console.WriteLine(student);
            }
            Console.WriteLine();
        }
        private static void ShowSuggestedTimes(string classname, List<List<string>> suggestedTimes)
        {
            var daysOfWeek = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            Console.WriteLine($"Suggested times for {classname} class:" + "\n");
            for (int i = 0; i < suggestedTimes.Count; i++)
            {

                List<string>? day = suggestedTimes[i];
                Console.WriteLine(daysOfWeek[i]);

                foreach (var availableTime in suggestedTimes[i])
                {
                    Console.WriteLine(availableTime);
                }
                Console.WriteLine();
            }
        }
    }
}