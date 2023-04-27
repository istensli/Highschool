namespace Highschool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var schoolBuilder = new SchoolBuilder(new School());
            var school = schoolBuilder.Build();
            /*
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
            */
        }
    }
}