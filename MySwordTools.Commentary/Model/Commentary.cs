using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySwordTools.Commentaries.Model
{
    [Table("commentary")]
    public class Commentary
    {
        #region Properties

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("id")]
        public int? Id { get; set; }

        [Column("book")]
        public int? Book { get; set; }

        [Column("chapter")]
        public int? Chapter { get; set; }

        [Column("fromverse")]
        public int? FromVerse { get; set; }

        [Column("toverse")]
        public int? ToVerse { get; set; }

        [Column("data")]
        public string Content { get; set; }

        #endregion Properties
    }
}