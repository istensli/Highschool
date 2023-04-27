public class SubjectRequest
{
    public string SubjectName { get; }
    public string Teacher { get; }
    public string[] Students { get; }

    public SubjectRequest(string subjectName, string teacher, string[] students)
    {
        SubjectName = subjectName;
        Teacher = teacher;
        Students = students;
    }
}