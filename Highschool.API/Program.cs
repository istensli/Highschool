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

        app.MapGet("/students", () =>
        {
            var students = school.GetStudents();

            var studentObject = students.Select(s =>
            {
                var subjectNames = s.Subjects.Select(sub => sub.Name);
                var classes = school.GetClasses()
                    .Where(c => c.Students.Any(cs => cs == s))
                    .Select(c => c.Name);

                return new {
                    name = s.Name,
                    subjects = subjectNames,
                    classes,
                    role = s.Role,
                    profilePicture = s.ProfilePicture
                };
            });

            return studentObject;
        });

        app.MapGet("/students/{studentName}", (string studentName) =>
        {
            Student student;
            if (studentName.Any(c => c == ' '))
            {
                student = school.GetStudents().FirstOrDefault(t => t.Name == studentName);
            }
            else
            {
                student = school.GetStudents().FirstOrDefault(t => t.Name.Replace(" ", string.Empty) == studentName);
            }

            var subjectNames = student.Subjects.Select(sub => sub.Name);
            var classes = school.GetClasses()
                    .Where(c => c.Students.Any(cs => cs == student))
                    .Select(c => c.Name);

            return new {
                name = student.Name,
                subjects = subjectNames,
                classes,
                role = student.Role,
                profilePicture = student.ProfilePicture
            };
        });

        app.MapGet("/students/{studentName}/timetable", (string studentName) =>
        {
            Student student;
            if (studentName.Any(c => c == ' '))
            {
                student = school.GetStudents().FirstOrDefault(t => t.Name == studentName);
            }
            else
            {
                student = school.GetStudents().FirstOrDefault(t => t.Name.Replace(" ", string.Empty) == studentName);
            }

            var timetableObject = school.GetTimeTable(student)
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

        app.MapPost("/students/", (StudentRequest studentRequest) =>
        {
            var student = new Student(studentRequest.Name, studentRequest.ProfilePicture);
            var subjects = school.GetClasses().FirstOrDefault(c => c.Name == studentRequest.ClassName).Subjects;
            var keyCompetences = school.GetSubjects().FirstOrDefault(sub => sub.Name == "Key Competences");

            school.AddStudent(student);
            keyCompetences.AddStudent(student);

            foreach (var subject in subjects)
            {
                subject.AddStudent(student);
            }

            var subjectNames = student.Subjects.Select(sub => sub.Name);
            var classes = school.GetClasses()
                    .Where(c => c.Students.Any(cs => cs == student))
                    .Select(c => c.Name);

            return new {
                name = student.Name,
                subjects = subjectNames,
                classes,
                role = student.Role,
                profilePicture = student.ProfilePicture
            };
        });

        app.MapGet("/teachers", () =>
        {
            var teachers = school.GetTeachers();

            var teachersObject = teachers.Select(t =>
            {
                var subjectNames = t.Subjects.Select(sub => sub.Name);
                return new {
                    name = t.Name,
                    subjects = subjectNames,
                    profilePicture = t.ProfilePicture
                };
            });

            return teachersObject;
        });

        app.MapGet("/teachers/{teacherName}", (string teacherName) =>
        {
            Teacher teacher;
            if (teacherName.Any(c => c == ' '))
            {
                teacher = school.GetTeachers().FirstOrDefault(t => t.Name == teacherName);

            }
            else
            {
                teacher = school.GetTeachers().FirstOrDefault(t => t.Name.Replace(" ", string.Empty) == teacherName);
            }

            var subjectNames = teacher.Subjects.Select(sub => sub.Name);
                
            return new {    
                name = teacher.Name,
                subjects = subjectNames,
                profilePicture = teacher.ProfilePicture
            };
        });

        app.MapGet("/teachers/{teacherName}/timetable", (string teacherName) =>
        {
            Teacher teacher;
            if (teacherName.Any(c => c == ' '))
            {
                teacher = school.GetTeachers().FirstOrDefault(t => t.Name == teacherName);

            }
            else
            {
                teacher = school.GetTeachers().FirstOrDefault(t => t.Name.Replace(" ", string.Empty) == teacherName);
            }

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

        app.MapPost("/teachers/", (TeacherRequest teacherRequest) =>
        {
            var teacher = new Teacher(teacherRequest.Name, teacherRequest.ProfilePicture);
            school.AddTeacher(teacher);

            var subjectNames = teacher.Subjects.Select(sub => sub.Name);

            return new
            {
                name = teacher.Name,
                subjects = subjectNames,
                profilePicture = teacher.ProfilePicture
            };
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

        app.MapGet("/subjects/{subjectName}", (string subjectName) =>
        {
            Subject subject;
            if (subjectName.Any(c => c == ' '))
            {
                subject = school.GetSubjects().FirstOrDefault(t => t.Name == subjectName);
            }
            else
            {
                subject = school.GetSubjects().FirstOrDefault(t => t.Name.Replace(" ", string.Empty) == subjectName);
            }

            return new
            {
                name = subject.Name,
                teacher = subject.Teacher.Name,
                students = subject.StudentNames
            };
        });

        app.MapGet("/subjects/{subjectName}/suggestedTimes", (string subjectName) =>
        {
            Subject subject;
            if (subjectName.Any(c => c == ' '))
            {
                subject = school.GetSubjects().FirstOrDefault(t => t.Name == subjectName);
            }
            else
            {
                subject = school.GetSubjects().FirstOrDefault(t => t.Name.Replace(" ", string.Empty) == subjectName);
            }

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

        app.MapPost("/subjects/", (SubjectRequest subjectRequest) =>
        {
            var teacher = school.GetTeachers()
                .FirstOrDefault(t => t.Name.ToLower() == subjectRequest.Teacher.ToLower());
            var students = school.GetStudents()
                .Where(s => subjectRequest.Students.Any(srs => s.Name.ToLower() == srs.ToLower()))
                .ToArray();
            var subject = new Subject(teacher, subjectRequest.SubjectName);

            subject.AddStudents(students);
            school.AddSubject(subject);

            var studentNames = subject.Students.Select(s => s.Name);

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

        app.MapGet("/classes/{className}", (string className) =>
        {
            Class selectedClass;
            if (className.Any(c => c == ' '))
            {
                selectedClass = school.GetClasses().FirstOrDefault(c => c.Name == className);
            }
            else
            {
                selectedClass = school.GetClasses().FirstOrDefault(c => c.Name.Replace(" ", string.Empty) == className);
            }

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

        app.MapGet("/rooms/{roomName}", (string roomName) =>
        {
            Room room;
            if (roomName.Any(c => c == ' '))
            {
                room = school.GetRooms().FirstOrDefault(c => c.Name == roomName);
            }
            else
            {
                room = school.GetRooms().FirstOrDefault(c => c.Name == roomName);
            }

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