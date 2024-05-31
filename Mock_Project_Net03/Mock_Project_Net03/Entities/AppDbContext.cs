using Microsoft.EntityFrameworkCore;

namespace Mock_Project_Net03.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        #region Dbset
        public DbSet<User> Users { get; set; }
        public DbSet<AssessmentScheme> AssessmentSchemes { get; set; }
        public DbSet<Syllabus> Syllabuses { get; set; }
        public DbSet<Materials> Materials { get; set; }
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<LearningObj> LearningObjs { get; set; }
        public DbSet<StudentStatus> StudentStatuses { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<OutputStandard> OutputStandards { get; set; }
        public DbSet<TrainingProgramUnit> TrainingProgramUnits { get; set;}
        public DbSet<AssessmentScheme_Syllabus> AssessmentScheme_Syllabus { get; set; }
        public DbSet<TrainingProgram_Syllabus> TrainingProgram_Syllabus { get; set;}
        public DbSet<Class_TrainingUnit> Class_TrainingUnit { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssessmentScheme_Syllabus>(e =>
            {
                e.ToTable("AssessmentScheme_Syllabus");
                e.HasKey(e => new { e.AssessmentSchemeId, e.SyllabusId });

                e.HasOne(e => e.Syllabus)
                .WithMany(e => e.AssessmentScheme_Syllabus)
                .HasForeignKey(e => e.SyllabusId)
                .HasConstraintName("FK_AssessmentScheme_Syllabus_Syllabus");

                e.HasOne(e => e.AssessmentScheme)
                .WithMany(e => e.AssessmentScheme_Syllabus)
                .HasForeignKey(e => e.AssessmentSchemeId)
                .HasConstraintName("FK_AssessmentScheme_Syllabus_AssessmentScheme");

            });
            modelBuilder.Entity<TrainingProgram_Syllabus>(e =>
            {
                e.ToTable("TrainingProgram_Syllabus");
                e.HasKey(e => new { e.TrainingProgramId, e.SyllabusId });

                e.HasOne(e => e.Syllabus)
                .WithMany(e => e.TrainingProgram_Syllabus)
                .HasForeignKey(e => e.SyllabusId)
                .HasConstraintName("FK_TrainingProgram_Syllabus_Syllabus");

                e.HasOne(e => e.TrainingProgram)
                .WithMany(e => e.TrainingProgram_Syllabus)
                .HasForeignKey(e => e.TrainingProgramId)
                .HasConstraintName("FK_TrainingProgram_Syllabus_TrainingProgram");

            });
        }

    }
}
