using Common.Commentaries.Model;
using Common.Commentaries.Tools;

namespace CommonTools.Commentaries.Tools
{
    public interface ICommentaryFormater
    {
        string ToString(Commentaire commentaire);
    }

    public interface ICommentaryFormater<TReferenceConverter> : ICommentaryFormater
        where TReferenceConverter : IReferenceConverter
    {
    }

}