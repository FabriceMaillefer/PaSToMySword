using Common.Commentaries.Model;
using Common.Commentaries.Tools;

namespace MySwordTools.Commentaries.Tools
{
    public class MySwordReferenceConverter : BaseReferenceConverter
    {
        #region Methods

        public override string ReferenceToBookLink(Reference reference)
        {
            return $"b{ToStringReference(reference)}";
        }

        public override string ReferenceToCommentaryLink(Reference reference)
        {
            return $"b{ToStringReference(reference)}";
        }

        private string ToStringReference(Reference reference)
        {
            int bookIndex = BookNumberFromAbbreviation(reference.Book);

            if (bookIndex >= 0)
            {
                string stringBuilder = $"{bookIndex}.{reference.Chapter}.{reference.FromVerse.GetValueOrDefault()}";

                if (reference.ToVerse.HasValue)
                    return $"{stringBuilder}-{reference.ToVerse}";
                else
                    return stringBuilder;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Methods
    }
}