using Microsoft.EntityFrameworkCore;
using TrainingManagementApi.Models;

namespace TrainingManagementApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<TrainingAssignment> TrainingAssignments => Set<TrainingAssignment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");
                entity.HasKey(e => e.EmployeeId);
                entity.Property(e => e.EmployeeName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Department).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();           // Email must be unique
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");
                entity.HasKey(c => c.CourseId);
                entity.Property(c => c.CourseName).IsRequired().HasMaxLength(150);
                entity.Property(c => c.TrainerName).IsRequired().HasMaxLength(100);

                // Optional sample data so Swagger has something to show on day one
                entity.HasData(
                    new Course { CourseId = 1, CourseName = "C# Fundamentals", Duration = 16, TrainerName = "Anita Sharma" },
                    new Course { CourseId = 2, CourseName = "ASP.NET Core Web API", Duration = 24, TrainerName = "Rahul Mehta" },
                    new Course { CourseId = 3, CourseName = "Entity Framework Core", Duration = 12, TrainerName = "Priya Nair" });
            });

            modelBuilder.Entity<TrainingAssignment>(entity =>
            {
                entity.ToTable("TrainingAssignment");
                entity.HasKey(a => a.AssignmentId);
                entity.Property(a => a.Status).IsRequired().HasMaxLength(20);

                entity.HasOne(a => a.Employee)
                      .WithMany(e => e.TrainingAssignments)
                      .HasForeignKey(a => a.EmployeeId)
                      .OnDelete(DeleteBehavior.Cascade);      // deleting an employee removes their assignments

                entity.HasOne(a => a.Course)
                      .WithMany(c => c.TrainingAssignments)
                      .HasForeignKey(a => a.CourseId)
                      .OnDelete(DeleteBehavior.Restrict);     // a course in use cannot be deleted

                // Same course cannot be assigned twice to the same employee
                entity.HasIndex(a => new { a.EmployeeId, a.CourseId }).IsUnique();
            });
        }
    }


}

