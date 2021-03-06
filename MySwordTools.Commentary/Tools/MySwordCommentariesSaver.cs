﻿using Common.Commentaries.Model;
using Common.Commentaries.Tools;
using CommonTools.Commentaries.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySwordTools.Commentaries.Model;
using MySwordTools.Commentaries.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MySwordTools.Commentaries
{
    public class MySwordCommentariesSaver : ICommentariesSaver
    {
        #region Constructors

        public MySwordCommentariesSaver(
            ILogger<MySwordCommentariesSaver> logger,
            ICommentaryFormater<MySwordReferenceConverter> commentaryFormater)
        {
            _logger = logger;
            _commentaryFormater = commentaryFormater;
            _referenceConverter = new MySwordReferenceConverter();
        }

        #endregion Constructors

        #region Methods

        public void Save(IEnumerable<Commentaire> commentaires, string filename)
        {
            SaveToDb(CommentairesToMySwordCommentaries(
                commentaires),
                new Details
                {
                    Abbreviation = "PaS",
                    Autor = "Plaire au Seigneur",
                    Comments = "Commentaire",
                    Description = "Recueil de commentaire biblique",
                    PublishDate = "2020",
                    Title = "Plaire au Seigneur",
                    Version = "0.1.1",
                    VersionDate = DateTime.UtcNow
                },
                filename);
        }

        #endregion Methods

        #region Fields

        private readonly ILogger<MySwordCommentariesSaver> _logger;

        private readonly ICommentaryFormater _commentaryFormater;

        private readonly MySwordReferenceConverter _referenceConverter;

        #endregion Fields

        private IEnumerable<Commentary> CommentairesToMySwordCommentaries(IEnumerable<Commentaire> commentaires)
        {
            List<Commentary> commentaries = new List<Commentary>();
            foreach (Commentaire commentaire in commentaires)
            {
                Commentary commentary = CommentaireToMySwordCommentary(commentaire);
                if (commentary is object)
                    commentaries.Add(commentary);
            }

            return commentaries;
        }

        private Commentary CommentaireToMySwordCommentary(Commentaire commentaire)
        {
            List<Reference> references = _referenceConverter.ConvertReference(commentaire.Reference);
            int bookIndex = _referenceConverter.BookNumberFromAbbreviation(references.First().Book);

            if (bookIndex >= 0)
            {
                return new Commentary
                {
                    Book = bookIndex,
                    Chapter = references.First().Chapter,
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
                if (File.Exists(outputDbName + MySwordCommentaryDbContext.Suffix))
                {
                    _logger.LogInformation($"Delete existing database {outputDbName}{MySwordCommentaryDbContext.Suffix}");
                    File.Delete(outputDbName + MySwordCommentaryDbContext.Suffix);
                }
            }

            using var db = new MySwordCommentaryDbContext(outputDbName);

            _logger.LogDebug($"Migrade database {outputDbName}{MySwordCommentaryDbContext.Suffix}");

            db.Database.Migrate();

            if (!db.Details.Any())
            {
                db.Add(details);
            }
            db.AddRange(commentaries);
            db.SaveChanges();
        }
    }
}