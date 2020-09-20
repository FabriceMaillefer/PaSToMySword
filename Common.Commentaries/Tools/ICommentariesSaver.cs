using Common.Commentaries.Model;
using System.Collections.Generic;

namespace Common.Commentaries.Tools
{
    public interface ICommentariesSaver
    {
        void Save(IEnumerable<Commentaire> commentaires, string filename);
    }
}