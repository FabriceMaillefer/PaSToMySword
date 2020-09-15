using Microsoft.EntityFrameworkCore;

namespace MySwordTools.Commentaries.Model
{
    public class MySwordCommentaryDbContext : DbContext
    {
        private readonly string _dataSource;

        public MySwordCommentaryDbContext(string dataSource)
        {
            _dataSource = dataSource;
        }

        public MySwordCommentaryDbContext()
        {
            _dataSource = "test.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={_dataSource}");

        public DbSet<Commentary> Commentaries { get; set; }
        public DbSet<Details> Details { get; set; }

    }
}