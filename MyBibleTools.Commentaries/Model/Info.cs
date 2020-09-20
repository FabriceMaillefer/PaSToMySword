using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBibleTools.Commentaries.Model
{
    [Table("info")]
    public class Info
    {
        public Info()
        {
        }

        public Info(string name, string value)
        {
            Name = name;
            Value = value;
        }

        [Key]
        [Column("name")]
        public string Name { get; set; }

        [Column("value ")]
        public string Value { get; set; }
    }
}