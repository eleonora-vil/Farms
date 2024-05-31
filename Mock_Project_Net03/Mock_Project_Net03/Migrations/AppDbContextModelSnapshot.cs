﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mock_Project_Net03.Entities;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mock_Project_Net03.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.26")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Mock_Project_Net03.Entities.Announcement", b =>
                {
                    b.Property<int>("AnnouncementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AnnouncementId"));

                    b.Property<string>("AnnouncementText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ClassId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("SenderId")
                        .HasColumnType("integer");

                    b.HasKey("AnnouncementId");

                    b.HasIndex("ClassId");

                    b.HasIndex("SenderId");

                    b.ToTable("Announcement");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.AssessmentScheme", b =>
                {
                    b.Property<int>("AssessmentSchemeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AssessmentSchemeId"));

                    b.Property<string>("AssessmentSchemeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int?>("PercentMark")
                        .HasColumnType("integer");

                    b.HasKey("AssessmentSchemeId");

                    b.ToTable("AssessmentScheme");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.AssessmentScheme_Syllabus", b =>
                {
                    b.Property<int?>("AssessmentSchemeId")
                        .HasColumnType("integer");

                    b.Property<int?>("SyllabusId")
                        .HasColumnType("integer");

                    b.Property<int?>("PercentMark")
                        .HasColumnType("integer");

                    b.HasKey("AssessmentSchemeId", "SyllabusId");

                    b.HasIndex("SyllabusId");

                    b.ToTable("AssessmentScheme_Syllabus", (string)null);
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.Attendance", b =>
                {
                    b.Property<int>("AttendanceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AttendanceId"));

                    b.Property<int>("ClassId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("TraineeId")
                        .HasColumnType("integer");

                    b.HasKey("AttendanceId");

                    b.HasIndex("ClassId");

                    b.HasIndex("TraineeId");

                    b.ToTable("Attendance");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.Class", b =>
                {
                    b.Property<int>("ClassId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ClassId"));

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("InstructorId")
                        .HasColumnType("integer");

                    b.Property<int>("ProgramId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("Time")
                        .HasColumnType("integer");

                    b.HasKey("ClassId");

                    b.HasIndex("InstructorId");

                    b.HasIndex("ProgramId");

                    b.ToTable("Class");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.Class_TrainingUnit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ClassId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Day")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("RoomId")
                        .HasColumnType("integer");

                    b.Property<int>("Slot")
                        .HasColumnType("integer");

                    b.Property<int>("TrainerId")
                        .HasColumnType("integer");

                    b.Property<int>("TrainingProgramUnitId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("RoomId");

                    b.HasIndex("TrainerId");

                    b.HasIndex("TrainingProgramUnitId");

                    b.ToTable("Class_TrainingUnit");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.Enrollment", b =>
                {
                    b.Property<int>("EnrollmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("EnrollmentId"));

                    b.Property<int>("ClassId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("EnrollmentDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("Grade")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("TraineeId")
                        .HasColumnType("integer");

                    b.HasKey("EnrollmentId");

                    b.HasIndex("ClassId");

                    b.HasIndex("TraineeId");

                    b.ToTable("Enrollment");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.Grade", b =>
                {
                    b.Property<int>("GradeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("GradeId"));

                    b.Property<int>("EnrollmentId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("GradeDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ModuleName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.HasKey("GradeId");

                    b.HasIndex("EnrollmentId");

                    b.ToTable("Grade");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.LearningObj", b =>
                {
                    b.Property<int>("LearningObjId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LearningObjId"));

                    b.Property<string>("DeliveryType")
                        .HasColumnType("text");

                    b.Property<string>("Duration")
                        .HasColumnType("text");

                    b.Property<int?>("Index")
                        .HasColumnType("integer");

                    b.Property<bool?>("Method")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("OutputStandardId")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<DateTime?>("TrainningTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("UnitId")
                        .HasColumnType("integer");

                    b.HasKey("LearningObjId");

                    b.HasIndex("OutputStandardId");

                    b.HasIndex("UnitId");

                    b.ToTable("LearningObj");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.Materials", b =>
                {
                    b.Property<int>("MaterialsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("MaterialsId"));

                    b.Property<string>("CreateBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("LearningObjId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("MaterialsId");

                    b.HasIndex("LearningObjId");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("NotificationId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("integer");

                    b.Property<int>("SenderId")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("NotificationId");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.OutputStandard", b =>
                {
                    b.Property<int>("OutputStandardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("OutputStandardId"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Tags")
                        .HasColumnType("text");

                    b.HasKey("OutputStandardId");

                    b.ToTable("OutputStandard");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.Permission", b =>
                {
                    b.Property<int>("PermissionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PermissionId"));

                    b.Property<string>("ClassAccess")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("MaterialAccess")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("ProgramAccess")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<int>("RoleID")
                        .HasColumnType("integer");

                    b.Property<string>("SyllabusAccess")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("UserAccess")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("PermissionId");

                    b.HasIndex("RoleID");

                    b.ToTable("Permission");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.Room", b =>
                {
                    b.Property<int>("RoomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RoomId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Name")
                        .HasColumnType("integer");

                    b.HasKey("RoomId");

                    b.ToTable("Room");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.StudentStatus", b =>
                {
                    b.Property<int>("StatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("StatusId"));

                    b.Property<int>("EnrollmentId")
                        .HasColumnType("integer");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<DateTime>("StatusDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("StatusType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("StatusId");

                    b.HasIndex("EnrollmentId");

                    b.ToTable("StudentStatus");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.Syllabus", b =>
                {
                    b.Property<int>("SyllabusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SyllabusId"));

                    b.Property<int?>("AttendeeNumber")
                        .HasColumnType("integer");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("CourseObjectives")
                        .HasMaxLength(65535)
                        .HasColumnType("character varying(65535)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("InstructorId")
                        .HasColumnType("integer");

                    b.Property<string>("Level")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Outline")
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<string>("TechnicalRequirement")
                        .HasMaxLength(65535)
                        .HasColumnType("character varying(65535)");

                    b.Property<string>("TrainingDelivery")
                        .HasMaxLength(65535)
                        .HasColumnType("character varying(65535)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Version")
                        .HasColumnType("text");

                    b.HasKey("SyllabusId");

                    b.HasIndex("InstructorId");

                    b.ToTable("Syllabus");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.TrainingProgram", b =>
                {
                    b.Property<int>("ProgramId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProgramId"));

                    b.Property<string>("CreateBy")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ProgramName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Version")
                        .HasColumnType("text");

                    b.HasKey("ProgramId");

                    b.ToTable("TrainingProgram");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.TrainingProgram_Syllabus", b =>
                {
                    b.Property<int>("TrainingProgramId")
                        .HasColumnType("integer");

                    b.Property<int>("SyllabusId")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.HasKey("TrainingProgramId", "SyllabusId");

                    b.HasIndex("SyllabusId");

                    b.ToTable("TrainingProgram_Syllabus", (string)null);
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.TrainingProgramUnit", b =>
                {
                    b.Property<int>("UnitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UnitId"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int?>("Index")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<int?>("SyllabusId")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<int?>("Time")
                        .HasColumnType("integer");

                    b.Property<string>("UnitName")
                        .HasColumnType("text");

                    b.HasKey("UnitId");

                    b.HasIndex("SyllabusId");

                    b.ToTable("TrainingProgramUnit");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<string>("Address")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Avatar")
                        .HasColumnType("text");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreateBy")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("FSU")
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Gender")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Level")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("ModifyBy")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("ModifyDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("OTPCode")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<int>("RoleID")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<int?>("TrainingProgramId")
                        .HasColumnType("integer");

                    b.Property<string>("UserName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("UserId");

                    b.HasIndex("RoleID");

                    b.HasIndex("TrainingProgramId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.UserRole", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("RoleId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.Announcement", b =>
                {
                    b.HasOne("Mock_Project_Net03.Entities.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mock_Project_Net03.Entities.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.AssessmentScheme_Syllabus", b =>
                {
                    b.HasOne("Mock_Project_Net03.Entities.AssessmentScheme", "AssessmentScheme")
                        .WithMany("AssessmentScheme_Syllabus")
                        .HasForeignKey("AssessmentSchemeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_AssessmentScheme_Syllabus_AssessmentScheme");

                    b.HasOne("Mock_Project_Net03.Entities.Syllabus", "Syllabus")
                        .WithMany("AssessmentScheme_Syllabus")
                        .HasForeignKey("SyllabusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_AssessmentScheme_Syllabus_Syllabus");

                    b.Navigation("AssessmentScheme");

                    b.Navigation("Syllabus");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.Attendance", b =>
                {
                    b.HasOne("Mock_Project_Net03.Entities.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mock_Project_Net03.Entities.User", "Trainee")
                        .WithMany()
                        .HasForeignKey("TraineeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Trainee");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.Class", b =>
                {
                    b.HasOne("Mock_Project_Net03.Entities.User", "Instructor")
                        .WithMany()
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mock_Project_Net03.Entities.TrainingProgram", "Program")
                        .WithMany()
                        .HasForeignKey("ProgramId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Instructor");

                    b.Navigation("Program");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.Class_TrainingUnit", b =>
                {
                    b.HasOne("Mock_Project_Net03.Entities.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mock_Project_Net03.Entities.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mock_Project_Net03.Entities.User", "Trainer")
                        .WithMany()
                        .HasForeignKey("TrainerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mock_Project_Net03.Entities.TrainingProgramUnit", "TrainingProgramUnit")
                        .WithMany()
                        .HasForeignKey("TrainingProgramUnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Room");

                    b.Navigation("Trainer");

                    b.Navigation("TrainingProgramUnit");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.Enrollment", b =>
                {
                    b.HasOne("Mock_Project_Net03.Entities.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mock_Project_Net03.Entities.User", "Trainee")
                        .WithMany()
                        .HasForeignKey("TraineeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Trainee");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.Grade", b =>
                {
                    b.HasOne("Mock_Project_Net03.Entities.Enrollment", "Enrollment")
                        .WithMany()
                        .HasForeignKey("EnrollmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Enrollment");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.LearningObj", b =>
                {
                    b.HasOne("Mock_Project_Net03.Entities.OutputStandard", "OutputStandard")
                        .WithMany()
                        .HasForeignKey("OutputStandardId");

                    b.HasOne("Mock_Project_Net03.Entities.TrainingProgramUnit", "Unit")
                        .WithMany()
                        .HasForeignKey("UnitId");

                    b.Navigation("OutputStandard");

                    b.Navigation("Unit");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.Materials", b =>
                {
                    b.HasOne("Mock_Project_Net03.Entities.LearningObj", "LearningObj")
                        .WithMany()
                        .HasForeignKey("LearningObjId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LearningObj");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.Notification", b =>
                {
                    b.HasOne("Mock_Project_Net03.Entities.User", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mock_Project_Net03.Entities.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.Permission", b =>
                {
                    b.HasOne("Mock_Project_Net03.Entities.UserRole", "Role")
                        .WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.StudentStatus", b =>
                {
                    b.HasOne("Mock_Project_Net03.Entities.Enrollment", "Enrollment")
                        .WithMany()
                        .HasForeignKey("EnrollmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Enrollment");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.Syllabus", b =>
                {
                    b.HasOne("Mock_Project_Net03.Entities.User", "Instructor")
                        .WithMany()
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Instructor");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.TrainingProgram_Syllabus", b =>
                {
                    b.HasOne("Mock_Project_Net03.Entities.Syllabus", "Syllabus")
                        .WithMany("TrainingProgram_Syllabus")
                        .HasForeignKey("SyllabusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_TrainingProgram_Syllabus_Syllabus");

                    b.HasOne("Mock_Project_Net03.Entities.TrainingProgram", "TrainingProgram")
                        .WithMany("TrainingProgram_Syllabus")
                        .HasForeignKey("TrainingProgramId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_TrainingProgram_Syllabus_TrainingProgram");

                    b.Navigation("Syllabus");

                    b.Navigation("TrainingProgram");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.TrainingProgramUnit", b =>
                {
                    b.HasOne("Mock_Project_Net03.Entities.Syllabus", "Syllabus")
                        .WithMany()
                        .HasForeignKey("SyllabusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Syllabus");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.User", b =>
                {
                    b.HasOne("Mock_Project_Net03.Entities.UserRole", "UserRole")
                        .WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mock_Project_Net03.Entities.TrainingProgram", "TrainingProgram")
                        .WithMany()
                        .HasForeignKey("TrainingProgramId");

                    b.Navigation("TrainingProgram");

                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.AssessmentScheme", b =>
                {
                    b.Navigation("AssessmentScheme_Syllabus");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.Syllabus", b =>
                {
                    b.Navigation("AssessmentScheme_Syllabus");

                    b.Navigation("TrainingProgram_Syllabus");
                });

            modelBuilder.Entity("Mock_Project_Net03.Entities.TrainingProgram", b =>
                {
                    b.Navigation("TrainingProgram_Syllabus");
                });
#pragma warning restore 612, 618
        }
    }
}
