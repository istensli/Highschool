using Highschool;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        var schoolBuilder = new SchoolBuilder(new School());
        var school = schoolBuilder.Build();

        app.MapGet("/", () =>
        {
            return "Hello World!";
        });

        app.MapGet("/students", () =>
        {
            var students = school.GetStudents();

            var studentObject = students.Select(s =>
            {
                var subjectNames = s.Subjects.Select(sub => sub.Name);
                return new { name = s.Name, subjects = subjectNames };
            });

            return studentObject;
        });

        app.MapGet("/students/{name}", (string name) =>
        {
            var student = school.GetStudents().FirstOrDefault(t => t.Name == name);
            var subjectNames = student.Subjects.Select(sub => sub.Name);

            return new { name = student.Name, subjects = subjectNames };
        });

        app.MapGet("/students/{name}/timetable", (string name) =>
        {
            var students = school.GetStudents().FirstOrDefault(t => t.Name == name);
            var timetableObject = school.GetTimeTable(students)
                .Select(b =>
                    {
                        return new {
                            subject = b.Subject.Name,
                            room = b.Room.Name,
                            day = b.Day,
                            startTime = b.StartTime,
                            endTime = b.EndTime
                        };
                    }
                );

            return timetableObject;
        });

        app.MapPost("/students/", (Student student) =>
        {
            school.AddStudent(student);

            return student;
        });

        app.MapGet("/teachers", () =>
        {
            var teachers = school.GetTeachers();

            var teachersObject = teachers.Select(s =>
            {
                var subjectNames = s.Subjects.Select(sub => sub.Name);
                return new { name = s.Name, subjects = subjectNames };
            });

            return teachersObject;
        });

        app.MapGet("/teachers/{name}", (string name) =>
        {
            var teacher = school.GetTeachers().FirstOrDefault(t => t.Name == name);
            var subjectNames = teacher.Subjects.Select(sub => sub.Name);

            return new { name = teacher.Name, subjects = subjectNames };
        });

        app.MapPost("/teachers/", (Teacher teacher) =>
        {
            school.AddTeacher(teacher);

            return teacher;
        });

        app.MapGet("/teachers/{name}/timetable", (string name) =>
        {
            var teacher = school.GetTeachers().FirstOrDefault(t => t.Name == name);
            var timetableObject = school.GetTimeTable(teacher)
                .Select(b =>
                    {
                        return new {
                            subject = b.Subject.Name,
                            room = b.Room.Name,
                            day = b.Day,
                            startTime = b.StartTime,
                            endTime = b.EndTime
                        };
                    }
                );

            return timetableObject;
        });

        app.MapGet("/subjects", () =>
        {
            var subjects = school.GetSubjects();

            var subjectsObject = subjects.Select(s =>
            {
                var studentNames = s.Students.Select(s => s.Name);

                return new {
                    name = s.Name,
                    teacher = s.Teacher.Name,
                    students = studentNames
                };
            });

            return subjectsObject;
        });

        app.MapGet("/subjects/{name}", (string name) =>
        {
            var subject = school.GetSubjects().FirstOrDefault(t => t.Name == name);
            var studentNames = subject.Students.Select(s => s.Name);

            return new
            {
                name = subject.Name,
                teacher = subject.Teacher.Name,
                students = studentNames
            };
        });

        app.MapGet("/subjects/{name}/suggestedTimes", (string name) =>
        {
            var subject = school.GetSubjects().FirstOrDefault(t => t.Name == name);
            var suggestedTimes = school.GetSuggestedTimes(subject)
                .Select(b =>
                {
                    return new
                    {
                        room = b.Room.Name,
                        day = b.Day,
                        startTime = b.StartTime,
                        endTime = b.EndTime
                    };
                });

            return suggestedTimes;
        });

        app.MapPost("/subjects/", (SubjectRequest req) =>
        {
            var teacher = school.GetTeachers()
                .FirstOrDefault(t => t.Name.ToLower() == req.TeacherName.ToLower());
            var subject = new Subject(teacher, req.SubjectName);
            var studentNames = subject.Students.Select(s => s.Name);

            school.AddSubject(subject);

            return new
            {
                name = subject.Name,
                teacher = subject.Teacher.Name,
                students = studentNames
            };
        });

        app.MapGet("/classes", () =>
        {
            var classes = school.GetClasses();

            var classesObject = classes.Select(c =>
            {
                var studentNames = c.Students.Select(st => st.Name);
                var subjectNames = c.Subjects.Select(sub => sub.Name);

                return new
                {
                    name = c.Name,
                    students = studentNames,
                    subject = subjectNames
                };
            });

            return classesObject;
        });

        app.MapGet("/classes/{name}", (string name) =>
        {
            var selectedClass = school.GetClasses().FirstOrDefault(c => c.Name.Replace(" ", string.Empty) == name);
            var studentNames = selectedClass.Students.Select(s => s.Name);
            var subjectNames = selectedClass.Subjects.Select(sub => sub.Name);

            return new
            {
                name = selectedClass.Name,
                students = studentNames,
                subject = subjectNames
            };
        });

        app.MapPost("/classes/", (ClassRequest req) =>
        {
            var reqSubjectNames = req.SubjectNames;
            var subjects = school.GetSubjects()
                .Where(sub => reqSubjectNames.Any(sn => sn == sub.Name))
                .ToList();

            var newClass = new Class(req.ClassName, subjects);

            var studentNames = newClass.Students.Select(s => s.Name);
            var subjectNames = newClass.Subjects.Select(sub => sub.Name);

            return new
            {
                name = newClass.Name,
                students = studentNames,
                subjects = subjectNames
            };
        });

        app.MapGet("/rooms", () =>
        {
            var rooms = school.GetRooms();

            var roomsObject = rooms.Select(r =>
                {
                    return new
                    {
                        name = r.Name,
                    };
                });

            return roomsObject;
        });

        app.MapGet("/rooms/{name}", (string name) =>
        {
            var room = school.GetRooms().FirstOrDefault(c => c.Name.Replace(" ", string.Empty) == name); ;

            return new
            {
                name = room.Name
            };
        });

        app.MapPost("/rooms/", (string name) =>
        {
            var room = new Room(name);

            school.AddRoom(room);

            return room;
        });

        app.MapPost("/bookings/", (BookingRequest req) =>
        {
            var subject = school.GetSubjects().FirstOrDefault(s => s.Name == req.Subject);
            var room = school.GetRooms().FirstOrDefault(r => r.Name == req.Room);

            var booking = new Booking(subject, room, req.Day, req.StartTime, req.EndTime);

            school.AddBooking(booking);

            return new
            {
                subject = booking.Subject.Name,
                room = booking.Room.Name,
                day = booking.Day,
                startTime = booking.StartTime,
                endTime = booking.EndTime
            };
        });

        app.Run();
    }
}