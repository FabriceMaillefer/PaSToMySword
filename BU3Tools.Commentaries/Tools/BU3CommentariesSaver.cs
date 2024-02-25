using Common.Commentaries.Model;
using Common.Commentaries.Tools;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BU3Tools.Commentaries.Tools
{
    public class BU3CommentariesSaver : ICommentariesSaver
    {
        private readonly BU3ReferenceConverter _referenceConverter;
        private readonly ILogger<BU3CommentariesSaver> _logger;
        private readonly BU3Formater _bU3Formater;

        public BU3CommentariesSaver(
            BU3ReferenceConverter referenceConverter,
            ILogger<BU3CommentariesSaver> logger,
            BU3Formater bU3Formater)
        {
            _referenceConverter = referenceConverter;
            _logger = logger;
            _bU3Formater = bU3Formater;
        }

        private sealed class ReferenceCommentaire
        {
            public Reference Reference { get; init; }

            public Commentaire Commentaire { get; init; }
        }

        public void Save(IEnumerable<Commentaire> commentaires, string filename)
        {
            List<ReferenceCommentaire> referenceCommentaires = new();

            foreach (var commentaire in commentaires)
            {
                List<Reference> referencesList = _referenceConverter.ConvertReference(commentaire.Reference);
                Reference mainReference = referencesList[0];

                referenceCommentaires.Add(new ReferenceCommentaire()
                {
                    Commentaire = commentaire,
                    Reference = mainReference
                });
            }

            // get all individual chapters
            var groupedByChapter = referenceCommentaires.GroupBy(x => new { x.Reference.Book, x.Reference.Chapter });

            foreach (var group in groupedByChapter)
            {
                ReferenceCommentaire firstCommentaire = group.First();

                string newPath = Path.Combine(filename, _referenceConverter.GetFilePath(firstCommentaire.Reference));
                Directory.CreateDirectory(Path.GetDirectoryName(newPath));
                using var file = new StreamWriter(newPath);

                int lastVerset = 0;

                IOrderedEnumerable<ReferenceCommentaire> orderedCommentaires = group.OrderBy(x => x.Reference.FromVerse);

                foreach (ReferenceCommentaire? commentaire in orderedCommentaires)
                {
                    if(commentaire.Reference.FromVerse != lastVerset)
                    {
                        file.Write($"<c v=\"{commentaire.Reference.FromVerse}\"/>");
                        lastVerset = commentaire.Reference.FromVerse ?? 0;
                    }

                    file.WriteLine(_bU3Formater.ToString(commentaire.Commentaire));
                }
            }


            // index of all commentararies (without duplicates)
            using var fileIndex = new StreamWriter(Path.Combine(filename, "index.html"));

            fileIndex.WriteLine(@"
                <!doctype html>
                <html>
                <head>
                    <meta http-equiv=""content-type"" content=""text/html; charset=utf-8"">
                    <title>Plaire au Seigneur annuel</title>
                </head>
                <body>
            ");

            fileIndex.WriteLine(@"
                <table>
                <th>
                    <tr>
                        <td>Titre</td>
                        <td>Date</td>
                        <td>Références</td>
                    </tr>
                </th>
                <tr>
            ");

            var groupedByTitleDoublons = referenceCommentaires.GroupBy(x => new { x.Commentaire.Titre, x.Commentaire.Reference });
            foreach (var group in groupedByTitleDoublons)
            {
                if (group.Count() > 1)
                {
                    _logger.LogWarning($"Doublons possible pour : {group.Key.Titre}");
                }
            }

            var groupedByTitle = referenceCommentaires.GroupBy(x => new { x.Commentaire.Titre });
            foreach (var group in groupedByTitle)
            {
                ReferenceCommentaire firstCommentaire = group.First();

                fileIndex.WriteLine(@$"
                    <tr>
                        <td>{firstCommentaire.Commentaire.Titre}</td>
                        <td>{firstCommentaire.Commentaire.Date.ToLongDateString()}</td>
                        <td>{string.Join("<br/>", group.Select(x => $"<a href=\"{_referenceConverter.ReferenceToCommentaryLink(x.Reference)}\">{_referenceConverter.ReferenceToReadableString(x.Reference)}</a>"))}</td>
                    </tr>
                ");
            }

            fileIndex.WriteLine(@"
                </table>
            ");

            fileIndex.WriteLine(@"
                </body>
                </html>
            ");
        }
    }
}