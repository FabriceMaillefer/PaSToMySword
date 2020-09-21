using Microsoft.EntityFrameworkCore;

namespace MyBibleTools.Commentaries.Model
{
    public class MyBibleCommentaryDbContext : DbContext
    {
        private readonly string _dataSource;

        public static string Suffix = ".commentaries.SQLite3";

        public MyBibleCommentaryDbContext(string dataSource)
        {
            _dataSource = dataSource;
        }

        public MyBibleCommentaryDbContext()
        {
            _dataSource = "test.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={_dataSource}{Suffix}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Commentary>()
            .HasIndex(p => new { p.Book, p.FromChapter, p.ToChapter });
        }

        public DbSet<Commentary> Commentaries { get; set; }
        public DbSet<Info> Infos { get; set; }
    }
}