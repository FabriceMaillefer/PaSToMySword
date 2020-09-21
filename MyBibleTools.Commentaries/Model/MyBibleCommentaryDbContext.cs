using Microsoft.EntityFrameworkCore;

namespace MyBibleTools.Commentaries.Model
{
    public class MyBibleCommentaryDbContext : DbContext
    {
        #region Fields

        public static string Suffix = ".commentaries.SQLite3";

        #endregion Fields

        #region Constructors

        public MyBibleCommentaryDbContext(string dataSource)
        {
            _dataSource = dataSource;
        }

        public MyBibleCommentaryDbContext()
        {
            _dataSource = "test.db";
        }

        #endregion Constructors

        #region Properties

        public DbSet<Commentary> Commentaries { get; set; }
        public DbSet<Info> Infos { get; set; }

        #endregion Properties

        #region Methods

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={_dataSource}{Suffix}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Commentary>()
            .HasIndex(p => new { p.Book, p.FromChapter, p.ToChapter });
        }

        #endregion Methods

        private readonly string _dataSource;
    }
}