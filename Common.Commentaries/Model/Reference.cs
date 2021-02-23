namespace Common.Commentaries.Model
{
    public class Reference
    {
        #region Properties

        public bool DisplayBook { get; set; }
        public string Book { get; set; }

        public string Suffix { get; set; }

        public bool DisplayChapter { get; set; }
        public int? Chapter { get; set; }

        public int? FromVerse { get; set; }

        public int? ToVerse { get; set; }

        #endregion Properties
    }
}