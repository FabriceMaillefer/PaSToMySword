using Common.Commentaries.Model;
using Common.Commentaries.Tools;

namespace CommonTools.Commentaries.Tools
{
    public interface ICommentaryFormater
    {
        #region Methods

        string ToString(Commentaire commentaire);

        #endregion Methods
    }

    public interface ICommentaryFormater<TReferenceConverter> : ICommentaryFormater
        where TReferenceConverter : IReferenceConverter
    {
    }
}