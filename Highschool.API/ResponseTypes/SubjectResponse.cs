public class SubjectResponse
{
    public string SubjectName { get; }
    public string TeacherName { get; }
    public string[] StudentNames { get; }

    public SubjectResponse(string subjectName, string teacherName)
    {
        SubjectName = subjectName;
        TeacherName = teacherName;
    }
}