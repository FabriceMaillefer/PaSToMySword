using Microsoft.EntityFrameworkCore;

namespace MySwordTools.Commentaries.Model
{
    public class MySwordCommentaryDbContext : DbContext
    {
        #region Fields

        public static string Suffix = ".cmt.mybible";

        #endregion Fields

        #region Constructors

        public MySwordCommentaryDbContext(string dataSource)
        {
            _dataSource = dataSource;
        }

        public MySwordCommentaryDbContext()
        {
            _dataSource = "test.db";
        }

        #endregion Constructors

        #region Properties

        public DbSet<Commentary> Commentaries { get; set; }
        public DbSet<Details> Details { get; set; }

        #endregion Properties

        #region Methods

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={_dataSource}{Suffix}");

        #endregion Methods

        private readonly string _dataSource;
    }
}