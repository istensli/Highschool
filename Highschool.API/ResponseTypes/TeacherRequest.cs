public class TeacherRequest
{
    public string Name { get; }
    public string ProfilePicture { get; }
    public string ClassName { get; }

    public TeacherRequest(string name, string profilePicture)
    {
        Name = name;
        ProfilePicture = profilePicture;
    }
}