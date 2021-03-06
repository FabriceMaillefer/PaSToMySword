﻿using Common.Commentaries.Model;
using Common.Commentaries.Tools;
using CommonTools.Commentaries.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyBibleTools.Commentaries.Model;
using MyBibleTools.Commentaries.Tools;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyBibleTools.Commentaries
{
    public class MyBibleCommentariesSaver : ICommentariesSaver
    {
        #region Constructors

        public MyBibleCommentariesSaver(
            ILogger<MyBibleCommentariesSaver> logger,
            ICommentaryFormater<MyBibleReferenceConverter> commentaryFormater
            )
        {
            _logger = logger;
            _commentaryFormater = commentaryFormater;
            _referenceConverter = new MyBibleReferenceConverter();
        }

        #endregion Constructors

        #region Methods

        public void Save(IEnumerable<Commentaire> commentaires, string filename)
        {
            SaveToDb(CommentairesToMyBibleCommentaries(
                commentaires),
                new Details
                {
                    Origin = "Plaire au Seigneur",
                    Description = "Recueil de commentaire biblique",
                    Language = "fr",
                    Region = "CH",
                    DetailedInfo = "Recueil de commentaire biblique, Informations détaillées",
                    HyperlinkLanguages = "fr",
                    HistoryOfChanges = "première version",
                    HtmlStyle = ""
                }, filename);
        }

        #endregion Methods

        #region Fields

        private readonly IReferenceConverter _referenceConverter;

        private readonly ILogger<MyBibleCommentariesSaver> _logger;

        private readonly ICommentaryFormater _commentaryFormater;

        #endregion Fields

        private IEnumerable<Commentary> CommentairesToMyBibleCommentaries(IEnumerable<Commentaire> commentaires)
        {
            List<Commentary> commentaries = new List<Commentary>();
            foreach (Commentaire commentaire in commentaires)
            {
                Commentary commentary = CommentaireToMyBibleCommentary(commentaire);
                if (commentary is object)
                    commentaries.Add(commentary);
            }

            return commentaries;
        }

        private Commentary CommentaireToMyBibleCommentary(Commentaire commentaire)
        {
            List<Reference> references = _referenceConverter.ConvertReference(commentaire.Reference);
            int bookIndex = _referenceConverter.BookNumberFromAbbreviation(references.First().Book);

            if (bookIndex >= 0)
            {
                return new Commentary
                {
                    Book = MyBibleReferenceConverter.ConvertBookIndex(bookIndex),
                    FromChapter = references.First().Chapter.GetValueOrDefault(),
                    ToChapter = references.First().Chapter.GetValueOrDefault(),
                    FromVerse = references.First().FromVerse.GetValueOrDefault(),
                    ToVerse = references.First().ToVerse.GetValueOrDefault(references.First().FromVerse.GetValueOrDefault()),
                    Content = _commentaryFormater.ToString(commentaire)
                };
            }
            else
            {
                _logger.LogError($"book {references.First().Book} not found in BookList");
            }
            return null;
        }

        private void SaveToDb(IEnumerable<Commentary> commentaries, Details details, string outputDbName, bool deleteFirst = true)
        {
            if (deleteFirst)
            {
                if (File.Exists(outputDbName + MyBibleCommentaryDbContext.Suffix))
                {
                    _logger.LogInformation($"Delete existing database {outputDbName + MyBibleCommentaryDbContext.Suffix}");
                    File.Delete(outputDbName + MyBibleCommentaryDbContext.Suffix);
                }
            }

            using var db = new MyBibleCommentaryDbContext(outputDbName);

            _logger.LogDebug($"Migrade database {outputDbName + MyBibleCommentaryDbContext.Suffix}");

            db.Database.Migrate();

            if (!db.Infos.Any())
            {
                db.AddRange(details.ToInfos());
            }

            db.AddRange(commentaries);
            db.SaveChanges();
        }
    }
}