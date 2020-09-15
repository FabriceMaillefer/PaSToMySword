using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PaSToMySword.Model.ExchangeFormat
{
    public class Citation
    {
        public string Texte { get; set; }
        public string Reference { get; set; }
    }

    public enum ParagrapheItemType
    {
        [EnumMember(Value = "normal")]
        Normal,

        [EnumMember(Value = "citation")]
        Citation,

        [EnumMember(Value = "reference")]
        Reference
    }

    public class ParagrapheItem
    {
        public string Texte { get; set; }
        public ParagrapheItemType Type { get; set; }
    }

    public class Paragraphe
    {
        public IEnumerable<ParagrapheItem> Content { get; set; } = new List<ParagrapheItem>();
    }

    public class Commentaire
    {
        public string Id { get; set; }
        public string RecueilId { get; set; }
        public string Titre { get; set; }
        public string Reference { get; set; }
        public IEnumerable<Citation> Versets { get; set; } = new List<Citation>();
        public IEnumerable<Paragraphe> Paragraphes { get; set; } = new List<Paragraphe>();
    }
}