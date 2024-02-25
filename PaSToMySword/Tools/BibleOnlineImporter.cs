using Common.Commentaries.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PaSToMySword.Tools
{
    public class BibleOnlineImporter
    {
        #region Constructors

        public BibleOnlineImporter(ILogger<BibleOnlineImporter> logger)
        {
            _logger = logger;
            _regex = new Regex(@"\$\$\$(?<referencePrincipale>.*?)$|\\\\\\\$(?<titre>.*?)\\\$\\\\|\\\&\\\&\\\%.*, (?<recueilDate>.*?)\\\%|\\@(?<citation>.*?)\\@|\\\\#(?<reference>.*?)\\\\|\\\\.*?\""(?<linkedReference>.*?)\"".*?\@\@.*?\""(?<linkedTitre>.*?)\""\\\\|(?<texte>[^[\\@*\\@]+|[\\\\#*\\\\]+]|[\$\$\$.*?])"
                , RegexOptions.Compiled);
        }

        #endregion Constructors

        #region Methods

        public RecueilExchange ReadFile(string path)
        {
            using StreamReader streamReader = new StreamReader(path);
            string textContent = streamReader.ReadToEnd();
            streamReader.Close();

            List<string> textContentSplited = textContent.Split("* * *").ToList();

            _logger.LogInformation($"Nombre de commentaire PaS trouvé : {textContentSplited.Count}");

            RecueilExchange recueils = new RecueilExchange();
            Dictionary<int, Recueil> recueilsAnnee = new Dictionary<int, Recueil>();

            foreach (string texte in textContentSplited)
            {
                if (!string.IsNullOrWhiteSpace(texte))
                {
                    Commentaire commentaire = ImportCommentaire(texte);

                    if (commentaire.IsValid)
                    {
                        if (recueilsAnnee.TryGetValue(commentaire.Date.Year, out Recueil recueil))
                        {
                            recueil.CommentairesId.Add(commentaire.Id);
                        }
                        else
                        {
                            recueilsAnnee.Add(commentaire.Date.Year, new Recueil
                            {
                                Id = Guid.NewGuid().ToString(),
                                Annee = commentaire.Date.Year,
                                Titre = $"Plaire au Seigneur {commentaire.Date.Year}"
                            });
                        }

                        recueils.Commentaires.Add(commentaire);
                    }
                    else
                    {
                        _logger.LogError($"commentaire incomplet dans {commentaire.Titre}");
                        _logger.LogError($"Texte courant : {texte}");
                    }
                }
            }

            recueils.Recueils.AddRange(recueilsAnnee.Values);

            return recueils;
        }

        #endregion Methods

        #region Fields

        private readonly ILogger<BibleOnlineImporter> _logger;
        private readonly Regex _regex;

        #endregion Fields

        private Commentaire ImportCommentaire(string texte)
        {
            Commentaire commentaire = new Commentaire()
            {
                Id = Guid.NewGuid().ToString()
            };

            List<string> lignes = texte.Split("\r\n").ToList();

            bool citationEntete = true;
            foreach (string ligne in lignes)
            {
                MatchCollection matches = _regex.Matches(ligne);

                //Citation tempCitation = new Citation();
                Paragraphe tempParagraphe = new Paragraphe();
                CommentaireLink tempCommentaireLink = new CommentaireLink();

                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;

                    foreach (string groupName in _regex.GetGroupNames())
                    {
                        if (!groups[groupName].Success)
                            continue;

                        _logger.LogTrace($"Group:{groupName} value: {groups[groupName].Value}");

                        switch (groupName)
                        {
                            case "referencePrincipale":
                                commentaire.Reference = groups[groupName].Value.Trim();
                                break;

                            case "titre":
                                {
                                    citationEntete = false; // Le titre viens toujours après les citations d'entête
                                    commentaire.Titre = groups[groupName].Value.Trim().Replace("\\@", string.Empty).Replace("“", "<i>“").Replace("”", "”</i>");
                                }
                                break;

                            case "recueilDate":
                                {
                                    if (DateTime.TryParse(groups[groupName].Value, out DateTime dateTime))
                                    {
                                        commentaire.Date = dateTime;
                                    }
                                    else
                                    {
                                        _logger.LogError($"Parse failed pour recueilDate:{groups[groupName].Value}");
                                    }
                                }
                                break;

                            case "citation":
                                {
                                    tempParagraphe.Content.Add(new ParagrapheItem() { Texte = groups[groupName].Value, Type = ParagrapheItemType.Citation });
                                }
                                break;

                            case "reference":
                                {
                                    tempParagraphe.Content.Add(new ParagrapheItem() { Texte = groups[groupName].Value, Type = ParagrapheItemType.Reference });
                                }
                                break;

                            case "texte":
                                {
                                    tempParagraphe.Content.Add(new ParagrapheItem() { Texte = groups[groupName].Value, Type = ParagrapheItemType.Normal });
                                }
                                break;

                            case "linkedTitre":
                                {
                                    tempCommentaireLink.Titre = groups[groupName].Value;
                                }
                                break;

                            case "linkedReference":
                                {
                                    tempCommentaireLink.Reference = groups[groupName].Value;
                                }
                                break;

                            default:
                                break;
                        };
                    }
                }

                if (tempParagraphe.Content.Count > 0)
                {
                    if (citationEntete)
                    {
                        commentaire.Versets.Add(tempParagraphe);
                    }
                    else
                    {
                        commentaire.Paragraphes.Add(tempParagraphe);
                    }
                }

                if (tempCommentaireLink.IsValid)
                {
                    commentaire.CommentaireLinks.Add(tempCommentaireLink);
                    tempCommentaireLink = new CommentaireLink();
                }
                else if (tempCommentaireLink.IsPartial)
                {
                    _logger.LogError($"CommentaireLink incomplet dans {commentaire.Titre} ligne:{ligne}");
                }
            }

            return commentaire;
        }
    }
}