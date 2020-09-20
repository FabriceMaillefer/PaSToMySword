using Common.Commentaries.Model;
using Common.Commentaries.Tools;

namespace MySwordTools.Commentaries.Tools
{
    public class MySwordReferenceConverter : BaseReferenceConverter
    {
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
                if (reference.FromVerse == reference.ToVerse)
                    return $"{bookIndex}.{reference.Chapter}.{reference.FromVerse}";
                else
                    return $"{bookIndex}.{reference.Chapter}.{reference.FromVerse}-{reference.ToVerse}";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}