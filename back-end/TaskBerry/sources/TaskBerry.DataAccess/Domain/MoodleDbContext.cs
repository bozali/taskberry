namespace TaskBerry.DataAccess.Domain
{
    using Microsoft.EntityFrameworkCore;

    using TaskBerry.Data.Entities;


    public class MoodleDbContext : DbContext
    {
        public MoodleDbContext(DbContextOptions<MoodleDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// </summary>
        public DbSet<UserEntity> Users { get; set; }

        /// <summary>
        /// </summary>
        public DbSet<MoodleUserInfoData> UserInfoData { get; set; }
    }
}