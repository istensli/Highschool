using Highschool;

public class BookingRequest
{
    public string Subject { get; }
    public string Room { get; }
    public DaysOfWeek Day { get; }
    public TimeOnly StartTime { get; }
    public TimeOnly EndTime { get; }

    public BookingRequest(string subject, string room, DaysOfWeek day, TimeOnly startTime, TimeOnly endTime)
    {
        Subject = subject;
        Room = room;
        Day = day;
        StartTime = startTime;
        EndTime = endTime;
    }
}