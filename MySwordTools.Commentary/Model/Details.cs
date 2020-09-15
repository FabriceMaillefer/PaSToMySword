using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySwordTools.Commentaries.Model
{
    [Table("details")]
    public class Details
    {
        [Key]
        [Column("title")]
        public string Title { get; set; }

        [Column("abbreviation")]
        public string Abbreviation { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("comments")]
        public string Comments { get; set; }

        [Column("author")]
        public string Autor { get; set; }

        [Column("version")]
        public string Version { get; set; }

        [Column("versiondate")]
        public DateTime? VersionDate { get; set; }

        [Column("publishdate")]
        public string PublishDate { get; set; }
    }
}