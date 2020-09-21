using Common.Commentaries.Model;

namespace Common.Commentaries.Tools
{
    public interface IReferenceConverter
    {
        #region Methods

        int BookNumberFromAbbreviation(string book);

        Reference ConvertReference(string referenceString);

        string ReferenceToReadableString(Reference reference);

        string ReferenceToBookLink(Reference reference);

        string ReferenceToCommentaryLink(Reference reference);

        #endregion Methods
    }
}