using Microsoft.EntityFrameworkCore;
using Mock_Project_Net03.Entities;
using Mock_Project_Net03.Helpers;

namespace Mock_Project_Net03.Data
{
    public interface IDataaseInitialiser
    {
        Task InitialiseAsync();
        Task SeedAsync();
        Task TrySeedAsync();
    }

    public class DatabaseInitialiser : IDataaseInitialiser
    {
        public readonly AppDbContext _context;

        public DatabaseInitialiser(AppDbContext context)
        {
            _context = context;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                // Migration Database - Create database if it does not exist
                await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            if (_context.UserRoles.Any() && _context.Users.Any()
                                         && _context.Syllabuses.Any() && _context.Permissions.Any())
            {
                return;
            }

            var superAdminRole = new UserRole { RoleName = "Super Admin" };
            var adminRole = new UserRole { RoleName = "Admin" };
            var instructorRole = new UserRole { RoleName = "Instructor" };
            var traineeRole = new UserRole { RoleName = "Trainee" };
            List<UserRole> userRoles = new()
            {
                superAdminRole,
                adminRole,
                instructorRole,
                traineeRole
            };
            var instructor = new User
            {
                UserName = "Instructor",
                Password = SecurityUtil.Hash("123456"),
                FullName = "Instructor",
                Email = "instructor@gmail.com",
                Gender = "Male",
                Level = "Senior",
                Address = "HCM",
                PhoneNumber = "12345",
                Status = "Active",
                UserRole = instructorRole,
            };
            var instructor1 = new User
            {
                UserName = "Instructor1",
                Password = SecurityUtil.Hash("123456"),
                FullName = "Instructor1",
                Email = "instructor1@gmail.com",
                Gender = "Male",
                Level = "Senior",
                Address = "HCM",
                PhoneNumber = "12345",
                Status = "Active",
                UserRole = instructorRole,
            };
            var instructor2 = new User
            {
                UserName = "Instructor2",
                Password = SecurityUtil.Hash("123456"),
                FullName = "Instructor2",
                Email = "instructor2@gmail.com",
                Gender = "Male",
                Level = "Senior",
                Address = "HCM",
                PhoneNumber = "12345",
                Status = "Active",
                UserRole = instructorRole,
            };
            var instructor3 = new User
            {
                UserName = "Instructor3",
                Password = SecurityUtil.Hash("123456"),
                FullName = "Instructor3",
                Email = "instructor3@gmail.com",
                Gender = "Male",
                Level = "Senior",
                Address = "HCM",
                PhoneNumber = "12345",
                Status = "Active",
                UserRole = instructorRole,
            };
            var superAdmin = new User
            {
                UserName = "SuperAdmin",
                Password = SecurityUtil.Hash("123456"),
                FullName = "SuperAdmin",
                Email = "superadmin@gmail.com",
                Gender = "Male",
                Level = "boss",
                Address = "HCM",
                PhoneNumber = "12345",
                Status = "Active",
                UserRole = superAdminRole,
            };
            var admin = new User
            {
                UserName = "Admin",
                Password = SecurityUtil.Hash("123456"),
                FullName = "Admin",
                Email = "admin@gmail.com",
                Gender = "Male",
                Level = "boss",
                Address = "HCM",
                PhoneNumber = "12345",
                Status = "Active",
                UserRole = adminRole,
            };
            var trainee = new User
            {
                UserName = "Trainee",
                Password = SecurityUtil.Hash("123456"),
                FullName = "Trainee",
                Email = "trainee@gmail.com",
                Gender = "Male",
                Level = "boss",
                Address = "HCM",
                PhoneNumber = "12345",
                Status = "Active",
                TrainingProgramId = 1,
                UserRole = traineeRole,
            };
            var trainee1 = new User
            {
                UserName = "Trainee1",
                Password = SecurityUtil.Hash("123456"),
                FullName = "Trainee1",
                Email = "trainee1@gmail.com",
                Gender = "Male",
                Level = "boss",
                Address = "HCM",
                PhoneNumber = "12345",
                Status = "Active",
                TrainingProgramId = 1,
                UserRole = traineeRole,
            };
            var trainee2 = new User
            {
                UserName = "Trainee2",
                Password = SecurityUtil.Hash("123456"),
                FullName = "Trainee2",
                Email = "trainee2@gmail.com",
                Gender = "Male",
                Level = "boss",
                Address = "HCM",
                PhoneNumber = "12345",
                Status = "Active",
                TrainingProgramId = 1,
                UserRole = traineeRole,
            };
            var trainee3 = new User
            {
                UserName = "Trainee3",
                Password = SecurityUtil.Hash("123456"),
                FullName = "Trainee3",
                Email = "trainee3@gmail.com",
                Gender = "Male",
                Level = "boss",
                Address = "HCM",
                PhoneNumber = "12345",
                Status = "Active",
                TrainingProgramId = 1,
                UserRole = traineeRole,
            };
            List<User> users = new()
            {
                instructor,
                instructor1,
                instructor2,
                instructor3,
                superAdmin,
                admin,
                trainee,
                trainee1,
                trainee2,
                trainee3
            };
            List<Syllabus> syllabuses = new()
            {
                new Syllabus
                {
                    Code = "SWP",
                    Name = ".NET",
                    Description = "mini capstone",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Outline = "general",
                    Level = "Senior",
                    Version = "1.0",
                    TechnicalRequirement = "Biet Code",
                    CourseObjectives = "Biet dieu, lam viec nhom",
                    Status = "true",
                    TrainingDelivery = "sach giao khoa",
                    AttendeeNumber = 1,
                    Instructor = instructor,
                },
                new()
                {
                    Code = "PRJ",
                    Name = "JAVA",
                    Description = "Final Project",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Outline = "specific",
                    Level = "Senior",
                    Version = "1.0",
                    TechnicalRequirement = "Biết Code",
                    CourseObjectives = "Xây dựng dự án cuối khóa",
                    Status = "true",
                    TrainingDelivery = "Sách giáo khoa",
                    AttendeeNumber = 1,
                    Instructor = instructor,
                },
                new()
                {
                    Code = "DBM",
                    Description = "Database Management",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Outline = "specific",
                    Level = "Junior",
                    Version = "1.0",
                    TechnicalRequirement = "Cơ bản về SQL",
                    CourseObjectives = "Quản lý cơ sở dữ liệu",
                    Status = "true",
                    TrainingDelivery = "Sách giáo khoa",
                    AttendeeNumber = 1,
                    Instructor = instructor,
                },
                new()
                {
                    Code = "SEC",
                    Description = "Security Fundamentals",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Outline = "specific",
                    Level = "Junior",
                    Version = "1.0",
                    TechnicalRequirement = "Kiến thức cơ bản về an ninh mạng",
                    CourseObjectives = "Hiểu về cơ bản an toàn thông tin",
                    Status = "true",
                    TrainingDelivery = "Sách giáo khoa",
                    AttendeeNumber = 1,
                    Instructor = instructor,
                }
            };
            List<TrainingProgram> trainingProgram = new()
            {
                new()
                {
                    ProgramName = "Python Programming Fundamentals",
                    Description = "Basic training for Python programming",
                    CreateBy = "Admin",
                    StartDate = DateTime.Now,
                    CreateDate = DateTime.Now.Date,
                    Version = "1.0",
                    EndDate = DateTime.Now.AddMonths(2),
                    Status = "Active"
                },
                new()
                {
                    ProgramName = "Data Analytics Bootcamp",
                    Description = "Intensive training on data analytics techniques and tools",
                    CreateBy = "Admin",
                    StartDate = DateTime.Now,
                    CreateDate = DateTime.Now.Date,
                    Version = "1.0",
                    EndDate = DateTime.Now.AddMonths(4),
                    Status = "Active"
                },
                new()
                {
                    ProgramName = "Software Engineering Bootcamp",
                    Description = "Intensive software engineering training program",
                    CreateBy = "Admin",
                    StartDate = DateTime.Now,
                    CreateDate = DateTime.Now.Date,
                    Version = "1.0",
                    EndDate = DateTime.Now.AddMonths(6),
                    Status = "Active"
                },
                new()
                {
                    ProgramName = "Data Science Certification",
                    Description = "Comprehensive data science certification program",
                    CreateBy = "Admin",
                    StartDate = DateTime.Now,
                    CreateDate = DateTime.Now.Date,
                    Version = "1.0",
                    EndDate = DateTime.Now.AddMonths(9),
                    Status = "Active"
                },
                new()
                {
                    ProgramName = "Cybersecurity Training Course",
                    Description = "Training program focused on cybersecurity fundamentals",
                    CreateBy = "Admin",
                    StartDate = DateTime.Now,
                    CreateDate = DateTime.Now.Date,
                    Version = "1.0",
                    EndDate = DateTime.Now.AddMonths(3),
                    Status = "Active"
                },
                new()
                {
                    ProgramName = "Web Development Bootcamp",
                    Description = "Intensive web development training bootcamp",
                    CreateBy = "Admin",
                    StartDate = DateTime.Now,
                    CreateDate = DateTime.Now.Date,
                    Version = "1.0",
                    EndDate = DateTime.Now.AddMonths(4),
                    Status = "Active"
                },
                new()
                {
                    ProgramName = "Mobile Development Bootcamp",
                    Description = "Intensive mobile development training bootcamp",
                    CreateBy = "Admin",
                    StartDate = DateTime.Now,
                    CreateDate = DateTime.Now.Date,
                    Version = "1.0",
                    EndDate = DateTime.Now.AddMonths(4),
                    Status = "Active"
                },
                new()
                {
                    ProgramName = "AI Development Bootcamp",
                    Description = "Intensive AI development training bootcamp",
                    CreateBy = "Admin",
                    StartDate = DateTime.Now,
                    CreateDate = DateTime.Now.Date,
                    Version = "1.0",
                    EndDate = DateTime.Now.AddMonths(4),
                    Status = "Active"
                }
            };
            List<TrainingProgram_Syllabus> trainingProgram_Syllabuses = new()
            {
                new()
                {
                    TrainingProgram = trainingProgram[0],
                    Syllabus = syllabuses[0],
                    Status = "Active"
                },
                new()
                {
                    TrainingProgram = trainingProgram[1],
                    Syllabus = syllabuses[1],
                    Status = "Active"
                },
                new()
                {
                    TrainingProgram = trainingProgram[2],
                    Syllabus = syllabuses[2],
                    Status = "Active"
                },
                new()
                {
                    TrainingProgram = trainingProgram[3],
                    Syllabus = syllabuses[3],
                    Status = "Active"
                },
                new()
                {
                    TrainingProgram = trainingProgram[4],
                    Syllabus = syllabuses[1],
                    Status = "Active"
                },
                new()
                {
                    TrainingProgram = trainingProgram[0],
                    Syllabus = syllabuses[3],
                    Status = "Active"
                },
                new()
                {
                    TrainingProgram = trainingProgram[1],
                    Syllabus = syllabuses[2],
                    Status = "Active"
                },

            };
            List<Semester> semesters = new List<Semester>()
            {
                new Semester
                {
                    SemesterName = "Fall 2024",
                    SemesterStartDate = new DateTime(2024, 9, 1),
                    SemesterEndDate = new DateTime(2024, 12, 31)
                },
                new Semester
                {
                    SemesterName = "Spring 2025",
                    SemesterStartDate = new DateTime(2025, 1, 1),
                    SemesterEndDate = new DateTime(2025, 4, 30)
                },
                new Semester
                {
                    SemesterName = "Summer 2025",
                    SemesterStartDate = new DateTime(2025, 5, 1),
                    SemesterEndDate = new DateTime(2025, 8, 31)
                },
                new Semester
                {
                    SemesterName = "Fall 2025",
                    SemesterStartDate = new DateTime(2025, 9, 1),
                    SemesterEndDate = new DateTime(2025, 12, 31)
                },
                new Semester
                {
                    SemesterName = "Spring 2026",
                    SemesterStartDate = new DateTime(2026, 1, 1),
                    SemesterEndDate = new DateTime(2026, 4, 30)
                },
                new Semester
                {
                    SemesterName = "Summer 2026",
                    SemesterStartDate = new DateTime(2026, 5, 1),
                    SemesterEndDate = new DateTime(2026, 8, 31)
                },
                new Semester
                {
                    SemesterName = "Fall 2026",
                    SemesterStartDate = new DateTime(2026, 9, 1),
                    SemesterEndDate = new DateTime(2026, 12, 31)
                },
                new Semester
                {
                    SemesterName = "Spring 2027",
                    SemesterStartDate = new DateTime(2027, 1, 1),
                    SemesterEndDate = new DateTime(2027, 4, 30)
                }
            };
            List<Class> classes = new()
            {
                new Class
                {
                    ClassName = "Software Engineering 101",
                    Program = trainingProgram[0],
                    Instructor = instructor3,
                    Semester = semesters[0],
                    Status = "Active"
                },
                new Class
                {
                    ClassName = "Data Science Fundamentals",
                    Program = trainingProgram[1],
                    Instructor = instructor,
                    Semester = semesters[1],
                    Status = "Active"
                },
                new Class
                {
                    ClassName = "Cybersecurity Essentials",
                    Program = trainingProgram[2],
                    Instructor = instructor2,
                    Semester = semesters[2],
                    Status = "Active"
                },
                new Class
                {
                    ClassName = "Web Development Bootcamp",
                    Program = trainingProgram[3],
                    Instructor = instructor1,
                    Semester = semesters[3],
                    Status = "Active"
                },
                new Class
                {
                    ClassName = "Machine Learning Foundations",
                    Program = trainingProgram[4],
                    Instructor = instructor,
                    Semester = semesters[4],
                    Status = "Active"
                },
                new Class
                {
                    ClassName = "Mobile App Development",
                    Program = trainingProgram[5],
                    Instructor = instructor3,
                    Semester = semesters[5],
                    Status = "Active"
                },
                new Class
                {
                    ClassName = "Database Management",
                    Program = trainingProgram[6],
                    Instructor = instructor2,
                    Semester = semesters[6],
                    Status = "Active"
                },
                new Class
                {
                    ClassName = "UI/UX Design",
                    Program = trainingProgram[7],
                    Instructor = instructor1,
                    Semester = semesters[7],
                    Status = "Active"
                },
                
            };

            List<TrainingProgramUnit> trainingProgramUnits = new()
            {
                new()
                {
                    UnitName = "Introduction to Programming",
                    Description = "Basic concepts of programming",
                    Time = 2,
                    Syllabus = syllabuses[0]
                },
                new()
                {
                    UnitName = "Data Structures and Algorithms",
                    Description = "Fundamental data structures and algorithms",
                    Time = 3,
                    Syllabus = syllabuses[1]
                },
                new()
                {
                    UnitName = "Database Management Systems",
                    Description = "Introduction to database systems",
                    Time = 2,
                    Syllabus = syllabuses[2]
                },
                new()
                {
                    UnitName = "Web Development Basics",
                    Description = "Fundamentals of web development",
                    Time = 2,
                    Syllabus = syllabuses[3]
                }
            };
            List<Room> rooms = new()
            {
                new()
                {
                    Name = 105,
                    Description = "Small classroom for group sessions"
                },
                new()
                {
                    Name = 104,
                    Description = "Medium-sized classroom for workshops"
                },
                new()
                {
                    Name = 103,
                    Description = "Large auditorium for lectures"
                },
                new()
                {
                    Name = 102,
                    Description = "Computer lab equipped with workstations"
                }
            };
            List<Class_TrainingUnit> class_TrainingUnits = new()
            {
                new()
                {
                    TrainingProgramUnit = trainingProgramUnits[0],
                    Class = classes[0],
                    Trainer = instructor,
                    Room = rooms[0],
                    Slot = 1,
                    Day = DateTime.Now.AddDays(7)
                },
                new()
                {
                    TrainingProgramUnit = trainingProgramUnits[1],
                    Class = classes[1],
                    Trainer = instructor,
                    Room = rooms[1],
                    Slot = 2,
                    Day = DateTime.Now.AddDays(14)
                },
                new()
                {
                    TrainingProgramUnit = trainingProgramUnits[2],
                    Class = classes[2],
                    Trainer = instructor,
                    Room = rooms[2],
                    Slot = 1,
                    Day = DateTime.Now.AddDays(21)
                },
                new()
                {
                    TrainingProgramUnit = trainingProgramUnits[3],
                    Class = classes[3],
                    Trainer = instructor,
                    Room = rooms[3],
                    Slot = 2,
                    Day = DateTime.Now.AddDays(30)
                },
                new()
                {
                    TrainingProgramUnit = trainingProgramUnits[1],
                    Class = classes[3],
                    Trainer = trainee,
                    Room = rooms[3],
                    Slot = 2,
                    Day = DateTime.Now.AddDays(30)
                },
                new()
                {
                    TrainingProgramUnit = trainingProgramUnits[2],
                    Class = classes[3],
                    Trainer = trainee,
                    Room = rooms[3],
                    Slot = 2,
                    Day = DateTime.Now.AddDays(30)
                },
                new()
                {
                    TrainingProgramUnit = trainingProgramUnits[3],
                    Class = classes[3],
                    Trainer = trainee,
                    Room = rooms[3],
                    Slot = 2,
                    Day = DateTime.Now.AddDays(30)
                },
                new()
                {
                    TrainingProgramUnit = trainingProgramUnits[2],
                    Class = classes[3],
                    Trainer = instructor,
                    Room = rooms[3],
                    Slot = 2,
                    Day = DateTime.Now.AddDays(30)
                }
            };
            List<OutputStandard> outputStandards = new()
            {
                new()
                {
                    Tags = "LV1",
                    Description = "Understand fundamental programming concepts"
                },
                new()
                {
                    Tags = "LV2",
                    Description = "Apply statistical analysis techniques to data"
                },
                new()
                {
                    Tags = "LV3",
                    Description = "Implement network security measures"
                },
                new()
                {
                    Tags = "LV4",
                    Description = "Develop responsive user interfaces for web applications"
                }
            };
            List<LearningObj> learningObjs = new()
            {
                new()
                {
                    Name = "Introduction to Programming Basics",
                    TrainningTime = DateTime.Now.AddHours(2),
                    Method = true,
                    Index = 1,
                    DeliveryType = "lab",
                    Status = "Active",
                    Duration = "30mins",
                    Unit = trainingProgramUnits[0],
                    OutputStandard = outputStandards[0]
                },
                new()
                {
                    Name = "Data Analysis Techniques",
                    TrainningTime = DateTime.Now.AddHours(3),
                    Method = true,
                    DeliveryType = "lab",
                    Status = "Active",
                    Duration = "30mins",
                    Index = 2,
                    Unit = trainingProgramUnits[0],
                    OutputStandard = outputStandards[1]
                },
                new()
                {
                    Name = "Implementing Security Measures",
                    TrainningTime = DateTime.Now.AddHours(2),
                    Method = true,
                    DeliveryType = "lab",
                    Status = "Active",
                    Index = 3,
                    Duration = "30mins",
                    Unit = trainingProgramUnits[0],
                    OutputStandard = outputStandards[2]
                },
                new()
                {
                    Name = "Developing Responsive UIs",
                    TrainningTime = DateTime.Now.AddHours(2),
                    Method = true,
                    Index = 4,
                    DeliveryType = "lab",
                    Status = "Active",
                    Duration = "30mins",
                    Unit = trainingProgramUnits[1],
                    OutputStandard = outputStandards[3]
                },
                new()
                {
                    Name = "Implementing Test",
                    TrainningTime = DateTime.Now.AddHours(2),
                    Method = true,
                    Index = 3,
                    DeliveryType = "lab",
                    Status = "Active",
                    Duration = "30mins",
                    Unit = trainingProgramUnits[2],
                    OutputStandard = outputStandards[2]
                },
                new()
                {
                    Name = " Security Measures",
                    TrainningTime = DateTime.Now.AddHours(2),
                    Method = true,
                    Index = 3,
                    DeliveryType = "lab",
                    Status = "Active",
                    Duration = "30mins",
                    Unit = trainingProgramUnits[3],
                    OutputStandard = outputStandards[2]
                }
            };
            List<Materials> materials = new List<Materials>()
            {
                new Materials
                {
                    Name = "Material 1",
                    CreateBy = "Admin",
                    CreateDate = DateTime.Now,
                    Url = ".net introduction.pdf",
                    LearningObj = learningObjs[0]
                },
                new Materials
                {
                    Name = "Material 2",
                    CreateBy = "Admin",
                    CreateDate = DateTime.Now,
                    Url = ".net introduction.pdf",

                    LearningObj = learningObjs[0]
                },
                new Materials
                {
                    Name = "Material 3",
                    CreateBy = "Admin",
                    CreateDate = DateTime.Now,
                    Url = ".net introduction.pdf",

                    LearningObj = learningObjs[0]
                },
                new Materials
                {
                    Name = "Material 4",
                    CreateBy = "Admin",
                    CreateDate = DateTime.Now,
                    Url = ".net introduction.pdf",

                    LearningObj = learningObjs[0]
                },
                new Materials
                {
                    Name = "Material 5",
                    CreateBy = "Admin",
                    CreateDate = DateTime.Now,
                    Url = ".net introduction.pdf",

                    LearningObj = learningObjs[1]
                },
                new Materials
                {
                    Name = "Material 6",
                    CreateBy = "Admin",
                    CreateDate = DateTime.Now,
                    Url = ".net introduction.pdf",

                    LearningObj = learningObjs[1]
                },
                new Materials
                {
                    Name = "Material 7",
                    CreateBy = "Admin",
                    CreateDate = DateTime.Now,
                    Url = ".net introduction.pdf",

                    LearningObj = learningObjs[2]
                },
                new Materials
                {
                    Name = "Material 8",
                    CreateBy = "Admin",
                    CreateDate = DateTime.Now,
                    Url = ".net introduction.pdf",

                    LearningObj = learningObjs[3]
                }
            };
            List<Enrollment> enrollments = new List<Enrollment>()
            {
                new Enrollment()
                {
                    Class = classes[0],
                    Status = "Active",
                    Trainee = trainee,
                    EnrollmentDate = semesters[0].SemesterStartDate.AddDays(1)
                },
                new Enrollment()
                {
                    Class = classes[0],
                    Status = "Active",
                    Trainee = trainee1,
                    EnrollmentDate = semesters[0].SemesterStartDate.AddDays(1)
                },
                new Enrollment()
                {
                    Class = classes[1],
                    Status = "Active",
                    Trainee = trainee2,
                    EnrollmentDate = semesters[1].SemesterStartDate.AddDays(1)
                },
                new Enrollment()
                {
                    Class = classes[1],
                    Status = "Active",
                    Trainee = trainee3,
                    EnrollmentDate = semesters[1].SemesterStartDate.AddDays(1)
                },
            };

            List<Permission> permissions = new()
            {
                new Permission
                {
                    Role = superAdminRole,
                    ClassAccess = "Full access",
                    MaterialAccess = "Full access",
                    ProgramAccess = "Full access",
                    SyllabusAccess = "Full access",
                    UserAccess = "Full access"
                },
                new Permission
                {
                    Role = adminRole,
                    ClassAccess = "Full access",
                    MaterialAccess = "Full access",
                    ProgramAccess = "Full access",
                    SyllabusAccess = "Full access",
                    UserAccess = "Full access"
                },
                new Permission
                {
                    Role = instructorRole,
                    ClassAccess = "Full access",
                    MaterialAccess = "Full access",
                    ProgramAccess = "Full access",
                    SyllabusAccess = "Full access",
                    UserAccess = "Full access"
                },
                new Permission
                {
                    Role = traineeRole,
                    ClassAccess = "Full access",
                    MaterialAccess = "Full access",
                    ProgramAccess = "Full access",
                    SyllabusAccess = "Full access",
                    UserAccess = "Full access"
                }
            };
            var assessmentSchemes = new List<AssessmentScheme>
            {
                new AssessmentScheme
                {
                    AssessmentSchemeName = "Quiz",
                    PercentMark = 10,
                    AssessmentScheme_Syllabus = new List<AssessmentScheme_Syllabus>
                    {
                        new AssessmentScheme_Syllabus { Syllabus = syllabuses[0], PercentMark = 10 },
                    }
                },
                new AssessmentScheme
                {
                    AssessmentSchemeName = "Assignment",
                    PercentMark = 40,
                    AssessmentScheme_Syllabus = new List<AssessmentScheme_Syllabus>
                    {
                        new AssessmentScheme_Syllabus { Syllabus = syllabuses[0], PercentMark = 40 },
                    }
                },
                new AssessmentScheme
                {
                    AssessmentSchemeName = "FE",
                    PercentMark = 50,
                    AssessmentScheme_Syllabus = new List<AssessmentScheme_Syllabus>
                    {
                        new AssessmentScheme_Syllabus { Syllabus = syllabuses[0], PercentMark = 50 },
                    }
                }
            };


            // Add Range training program
            await _context.UserRoles.AddRangeAsync(userRoles);
            await _context.Users.AddRangeAsync(users);
            await _context.Syllabuses.AddRangeAsync(syllabuses);
            await _context.TrainingPrograms.AddRangeAsync(trainingProgram);
            await _context.Permissions.AddRangeAsync(permissions);
            await _context.TrainingProgram_Syllabus.AddRangeAsync(trainingProgram_Syllabuses);
            await _context.Semesters.AddRangeAsync(semesters);
            await _context.Classes.AddRangeAsync(classes);
            await _context.TrainingProgramUnits.AddRangeAsync(trainingProgramUnits);
            await _context.Rooms.AddRangeAsync(rooms);
            await _context.Class_TrainingUnit.AddRangeAsync(class_TrainingUnits);
            await _context.OutputStandards.AddRangeAsync(outputStandards);
            await _context.LearningObjs.AddRangeAsync(learningObjs);
            await _context.Materials.AddRangeAsync(materials);
            await _context.Enrollments.AddRangeAsync(enrollments);
            await _context.AssessmentSchemes.AddRangeAsync(assessmentSchemes);

            // Save to DB
            await _context.SaveChangesAsync();
        }
    }

    public static class DatabaseInitialiserExtension
    {
        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            // Create IServiceScope to resolve service scope
            using var scope = app.Services.CreateScope();
            var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitialiser>();

            await initializer.InitialiseAsync();

            // Try to seeding data
            await initializer.SeedAsync();
        }
    }
}