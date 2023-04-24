public class SubjectRequest
{
    public string SubjectName { get; }
    public string TeacherName { get; }
    public string[] StudentNames { get; }

    public SubjectRequest(string subjectName, string teacherName)
    {
        SubjectName = subjectName;
        TeacherName = teacherName;
    }
}