public class StudentRequest
{
    public string Name { get; }
    public string ProfilePicture { get; }
    public string ClassName { get; }

    public StudentRequest(string name, string profilePicture, string className)
    {
        Name = name;
        ProfilePicture = profilePicture;
        ClassName = className;
    }
}