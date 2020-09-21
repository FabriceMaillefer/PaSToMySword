using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBibleTools.Commentaries.Model
{
    [Table("commentaries")]
    public class Commentary
    {
        #region Properties

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("book_number")]
        public int Book { get; set; }

        [Column("chapter_number_from")]
        public int FromChapter { get; set; }

        [Column("verse_number_from")]
        public int FromVerse { get; set; }

        [Column("chapter_number_to")]
        public int ToChapter { get; set; }

        [Column("verse_number_to")]
        public int ToVerse { get; set; }

        [Column("text")]
        public string Content { get; set; }

        #endregion Properties
    }
}