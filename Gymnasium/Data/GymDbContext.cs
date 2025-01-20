using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Gymnasium.Models;

namespace Gymnasium.Data
{
    public partial class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<GradeScale> GradeScales { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.ClassId).HasName("PK__Class__CB1927A0BF9205F6");
                entity.ToTable("Class");
                entity.Property(e => e.ClassId).HasColumnName("ClassID");
                entity.Property(e => e.ClassName).HasMaxLength(50);
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => e.SubjectId).HasName("PK__Subject__C92D7187D5FADC2F");
                entity.ToTable("Subject");
                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");
                entity.Property(e => e.SubjectName).HasMaxLength(100);
                entity.Property(e => e.TeacherId).HasColumnName("TeacherID");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("FK__Subject__TeacherI__3B75D760");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.HasKey(e => e.GradeId).HasName("PK__Grade__54F87A371D29C5B7");
                entity.ToTable("Grade");
                entity.Property(e => e.GradeId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("GradeID");
                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");
                entity.Property(e => e.GradeValue)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK__Grade__SubjectID__403A8C7D");

                entity.HasOne(d => d.GradeValueNavigation)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => d.GradeValue)
                    .HasConstraintName("FK__Grade__GradeValu__4316F928");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK__Grade__StudentID__412EB0B6");
            });

            modelBuilder.Entity<GradeScale>(entity =>
            {
                entity.HasKey(e => e.GradeValue).HasName("PK__GradeSca__5E16159511AA8ED5");
                entity.ToTable("GradeScale");
                entity.Property(e => e.GradeValue)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasKey(e => e.StaffId).HasName("PK__Personal__2834371383E095A6");
                entity.Property(e => e.StaffId).HasColumnName("StaffID");
                entity.Property(e => e.FirstName).HasMaxLength(35);
                entity.Property(e => e.LastName).HasMaxLength(35);
                entity.Property(e => e.Role).HasMaxLength(50);  // Updated to Role instead of Position
                entity.Property(e => e.StartDate).HasColumnType("date");
                entity.Property(e => e.DateOfBirth).HasColumnType("date");  // Updated
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52A79F7C8E7F9");
                entity.Property(e => e.StudentId).HasColumnName("StudentID");
                entity.Property(e => e.ClassId).HasColumnName("ClassID");
                entity.Property(e => e.FirstName).HasMaxLength(35);
                entity.Property(e => e.LastName).HasMaxLength(35);
                entity.Property(e => e.DateOfBirth).HasColumnType("date");  // Updated

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_Students_ClassID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

