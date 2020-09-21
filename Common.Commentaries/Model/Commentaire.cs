using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.Commentaries.Model
{
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
        #region Properties

        public string Texte { get; set; }
        public ParagrapheItemType Type { get; set; } = ParagrapheItemType.Normal;

        #endregion Properties
    }

    public class Paragraphe
    {
        #region Properties

        public List<ParagrapheItem> Content { get; set; } = new List<ParagrapheItem>();

        #endregion Properties
    }

    public class CommentaireLink
    {
        #region Properties

        public string Titre { get; set; }
        public string Reference { get; set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(Titre) && !string.IsNullOrWhiteSpace(Reference);
        public bool IsPartial => !string.IsNullOrWhiteSpace(Titre) ^ !string.IsNullOrWhiteSpace(Reference);

        #endregion Properties
    }

    public class Commentaire
    {
        #region Properties

        public string Id { get; set; }
        public string RecueilId { get; set; }
        public string Titre { get; set; }
        public string Reference { get; set; }
        public DateTime Date { get; set; }

        public List<Paragraphe> Versets { get; set; } = new List<Paragraphe>();
        public List<Paragraphe> Paragraphes { get; set; } = new List<Paragraphe>();
        public List<CommentaireLink> CommentaireLinks { get; set; } = new List<CommentaireLink>();

        public bool IsValid => !string.IsNullOrWhiteSpace(Titre) && !string.IsNullOrWhiteSpace(Reference);

        #endregion Properties
    }
}