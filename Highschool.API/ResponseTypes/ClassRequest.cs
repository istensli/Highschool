public class ClassRequest
{
    public string ClassName { get; }
    public string[] SubjectNames { get; }

    public ClassRequest(string className, string[] subjectNames)
    {
        ClassName = className;
        SubjectNames = subjectNames;
    }
}