using todo_app_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace todo_app_backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {

        }

        public DbSet<User> User { get; set; }
        public DbSet<TempUser> TempUser { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<TodoTask> TodoTask { get; set; }
        public DbSet<TodoTaskTag> TodoTaskTag { get; set; }
        public DbSet<TodoSubtask> TodoSubtask { get; set; }
        public DbSet<Otp> Otp { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoTaskTag>().HasKey(e => new {
                e.TagId, e.TodoTaskId
            });

            modelBuilder.Entity<TodoTaskTag>()
                .HasOne(e => e.Tag)
                .WithMany(e => e.TodoTaskTags)
                .HasForeignKey(e => e.TagId);
                

            modelBuilder.Entity<TodoTaskTag>()
                .HasOne(e => e.TodoTask)
                .WithMany(e => e.TodoTaskTags)
                .HasForeignKey(e => e.TodoTaskId);
        }
    }
}