using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySwordTools.Commentaries.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MySwordTools.Commentaries
{
    public class MySwordCommentariesExport
    {

        #region Constructors

        public MySwordCommentariesExport(ILogger<MySwordCommentariesExport> logger)
        {
            _logger = logger;
        }

        #endregion Constructors

        #region Methods

        public void SaveToDb(IEnumerable<Commentary> commentaries, Details details, string outputDbName)
        {
            using var db = new MySwordCommentaryDbContext(outputDbName);

            _logger.LogDebug($"Migrade database {outputDbName}");

            db.Database.Migrate();

            if (!db.Details.Any())
            {
                db.Add(details);
            }
            db.AddRange(commentaries);
            db.SaveChanges();
        }

        #endregion Methods

        #region Fields

        private readonly ILogger<MySwordCommentariesExport> _logger;

        #endregion Fields
    }
}