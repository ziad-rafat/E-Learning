using E_Learning.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Context
{
    public class ELearningContext: IdentityDbContext<User>
    {

        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderCourse> OrderCourses { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Topic> Topics { get; set; }

        public ELearningContext(DbContextOptions<ELearningContext> options) :base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Modify delete behavior for Enrollments -> Users
            builder.Entity<Enrollment>()
                .HasOne(e => e.User)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Prevent cascade delete from User

            // Modify delete behavior for Enrollments -> Courses
            builder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade); // Keep cascade delete for Course

            // Modify delete behavior for OrderCourses -> Orders
            builder.Entity<OrderCourse>()
                .HasOne(oc => oc.Order)
                .WithMany(o => o.OrderCourses)
                .HasForeignKey(oc => oc.OrderId)
                .OnDelete(DeleteBehavior.NoAction); // Prevent cascading delete

            // Modify delete behavior for OrderCourses -> Courses
            builder.Entity<OrderCourse>()
                .HasOne(oc => oc.Course)
                .WithMany(c => c.OrderCourses)
                .HasForeignKey(oc => oc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Modify delete behavior for Reviews -> Users
            builder.Entity<Review>()
                .HasOne(r => r.User)               // Each Review has one User
                .WithMany(u => u.Reviews)          // Each User can have many Reviews
                .HasForeignKey(r => r.UserId)      // Foreign key is UserId
                .OnDelete(DeleteBehavior.NoAction); // Prevent cascading delete

            // Modify delete behavior for Reviews -> Courses
            builder.Entity<Review>()
                .HasOne(r => r.Course)             // Each Review has one Course
                .WithMany(c => c.Reviews)          // Each Course can have many Reviews
                .HasForeignKey(r => r.CourseId)    // Foreign key is CourseId
                .OnDelete(DeleteBehavior.Cascade); // Allow cascading delete


            var adminRole = new IdentityRole
            {
                Id = "1",
                Name = "Admin",
                NormalizedName = "ADMIN"
            };
            var studentRole = new IdentityRole
            {
                Id = "2",
                Name = "Student",
                NormalizedName = "STUDENT"
            };
            var instructorRole = new IdentityRole
            {
                Id = "3",
                Name = "Instructor",
                NormalizedName = "INSTRUCTOR"
            };
            var hasher = new PasswordHasher<User>();

            // Seeding the admin user
            var mohamedUser = new User
            {
                Id = "1", // Assign a unique ID (string type)
                FirstName = "Mohamed",
                LastName = "Misarh",
                UserName = "Mohamed22",
                Email = "mohamedmisarh2@gmail.com",
                EmailConfirmed = true,
                IsDeleted = false,
                Photo = null
            };

            // Set the password for admin
            mohamedUser.PasswordHash = hasher.HashPassword(mohamedUser, "AdminPassword123!");

             var ziadUser = new User
            {
                Id = "2", // Assign a unique ID (string type)
                FirstName = "Ziad",
                LastName = "Rafat",
                UserName = "Ziad22",
                Email = "ziadrafat10@gmail.com",
                EmailConfirmed = true,
                IsDeleted = false,
                Photo = null
            };

            // Set the password for admin
            ziadUser.PasswordHash = hasher.HashPassword(ziadUser, "AdminPassword123!");

             var ayaUser = new User
            {
                Id = "3", // Assign a unique ID (string type)
                FirstName = "Aya",
                LastName = "Ahmed",
                UserName = "Aya22",
                Email = "AyaAhmed18@gmail.com",
                EmailConfirmed = true,
                IsDeleted = false,
                Photo = null
            };

            // Set the password for admin
            ayaUser.PasswordHash = hasher.HashPassword(ayaUser, "AdminPassword123!");

             var hasnaaUser = new User
            {
                Id = "4", // Assign a unique ID (string type)
                FirstName = "Hasnaa",
                LastName = "Mohamed",
                UserName = "Hasnaa22",
                Email = "7asnaa2009@gmail.com",
                EmailConfirmed = true,
                IsDeleted = false,
                Photo = null
            };

            // Set the password for admin
            hasnaaUser.PasswordHash = hasher.HashPassword(hasnaaUser, "AdminPassword123!");

            builder.Entity<IdentityRole>().HasData(adminRole, studentRole, instructorRole);


            // Seeding the users into the database
            builder.Entity<User>().HasData(mohamedUser,ziadUser,ayaUser,hasnaaUser );

            builder.Entity<IdentityUserRole<string>>().HasData(
                    new IdentityUserRole<string>
                    {
                        UserId = mohamedUser.Id,
                        RoleId = adminRole.Id
                    },
                    new IdentityUserRole<string>
                    {
                        UserId = ziadUser.Id,
                        RoleId = adminRole.Id
                    },
                    new IdentityUserRole<string>
                    {
                        UserId = ayaUser.Id,
                        RoleId = adminRole.Id
                    },
                    new IdentityUserRole<string>
                    {
                        UserId = hasnaaUser.Id,
                        RoleId = adminRole.Id
                    }
                );

            base.OnModelCreating(builder);
        }
    }
}
