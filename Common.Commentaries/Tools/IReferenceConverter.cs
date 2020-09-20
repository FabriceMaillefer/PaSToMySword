using Common.Commentaries.Model;

namespace Common.Commentaries.Tools
{
    public interface IReferenceConverter
    {
        int BookNumberFromAbbreviation(string book);

        Reference ConvertReference(string referenceString);

        string ReferenceToReadableString(Reference reference);

        string ReferenceToBookLink(Reference reference);

        string ReferenceToCommentaryLink(Reference reference);
    }
}