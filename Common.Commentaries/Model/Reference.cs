namespace Common.Commentaries.Model
{
    public class Reference
    {
        #region Properties

        public string Book { get; set; }

        public string Suffix { get; set; }

        public int Chapter { get; set; }

        public int FromVerse { get; set; }

        public int? ToVerse { get; set; }

        public bool ReferenceWithoutVerse { get; set; } = false;

        #endregion Properties
    }
}