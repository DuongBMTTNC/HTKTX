using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HTKTX.Models
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tránh multiple cascade paths
            modelBuilder.Entity<DormRegistration>()
                .HasOne(d => d.Room)
                .WithMany(r => r.DormRegistrations)
                .HasForeignKey(d => d.RoomNumber)
                .OnDelete(DeleteBehavior.Restrict);  // hoặc .NoAction

            modelBuilder.Entity<DormRegistration>()
                .HasOne(d => d.RoomType)
                .WithMany()
                .HasForeignKey(d => d.RoomTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RoomChangeRequest>()
        .HasOne(r => r.CurrentRoom)
        .WithMany()
        .HasForeignKey(r => r.CurrentRoomNumber)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RoomChangeRequest>()
                .HasOne(r => r.DesiredRoom)
                .WithMany()
                .HasForeignKey(r => r.DesiredRoomNumber)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình Comment - Post
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình Comment - User
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình PostLike - Post
            modelBuilder.Entity<PostLike>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình PostLike - User
            modelBuilder.Entity<PostLike>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<DormRegistration> DormRegistrations { get; set; }
        public DbSet<StudentProfile> StudentProfiles { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<RentalBill> RentalBills { get; set; }
        public DbSet<UtilityBill> UtilityBills { get; set; }
        public DbSet<RepairRequest> RepairRequests { get; set; }
        public DbSet<WaterElectricBill> WaterElectricBills { get; set; }
        public DbSet<StudentRequest> StudentRequests { get; set; }
        public DbSet<RoomChangeRequest> RoomChangeRequests { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
    }
}
