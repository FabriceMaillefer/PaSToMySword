using Common.Commentaries.Model;
using Common.Commentaries.Tools;
using CommonTools.Commentaries.Tools;
using System.Text;

namespace PaSToMySword.Tools
{
    public class CommentaireHtmlFormater<TReferenceConverter> : ICommentaryFormater<TReferenceConverter>
        where TReferenceConverter : IReferenceConverter
    {
        #region Constructors

        public CommentaireHtmlFormater(TReferenceConverter referenceConverter)
        {
            _referenceConverter = referenceConverter;
        }

        #endregion Constructors

        #region Methods

        public string ToString(Commentaire commentaire)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"<h1>{commentaire.Titre}</h1>");
            sb.Append("<hr />");

            foreach (Paragraphe paragraphe in commentaire.Versets)
            {
                sb.Append("<div><blockquote>");
                foreach (ParagrapheItem paragrapheItem in paragraphe.Content)
                {
                    switch (paragrapheItem.Type)
                    {
                        case ParagrapheItemType.Citation:
                            sb.Append($"<i>{paragrapheItem.Texte}</i>");
                            break;

                        case ParagrapheItemType.Reference:
                            {
                                try
                                {
                                    Reference reference = _referenceConverter.ConvertReference(paragrapheItem.Texte);
                                    sb.Append($"<a href=\"{_referenceConverter.ReferenceToBookLink(reference)}\">{_referenceConverter.ReferenceToReadableString(reference)}</a>{reference.Suffix}");
                                }
                                catch
                                {
                                    sb.Append(paragrapheItem.Texte);
                                }
                            }
                            break;

                        default:
                        case ParagrapheItemType.Normal:
                            sb.Append(paragrapheItem.Texte);
                            break;
                    }
                }

                sb.Append("</blockquote></div>");
            }

            sb.Append($"<hr />");

            foreach (Paragraphe paragraphe in commentaire.Paragraphes)
            {
                sb.Append("<p>");
                foreach (ParagrapheItem paragrapheItem in paragraphe.Content)
                {
                    switch (paragrapheItem.Type)
                    {
                        case ParagrapheItemType.Citation:
                            sb.Append($"<i>{paragrapheItem.Texte}</i>");
                            break;

                        case ParagrapheItemType.Reference:
                            {
                                try
                                {
                                    Reference reference = _referenceConverter.ConvertReference(paragrapheItem.Texte);
                                    sb.Append($"<a href=\"{_referenceConverter.ReferenceToBookLink(reference)}\">{_referenceConverter.ReferenceToReadableString(reference)}</a>{reference.Suffix}");
                                }
                                catch
                                {
                                    sb.Append(paragrapheItem.Texte);
                                }
                            }
                            break;

                        default:
                        case ParagrapheItemType.Normal:
                            sb.Append(paragrapheItem.Texte);
                            break;
                    }
                }

                sb.Append("</p>");
            }

            foreach (CommentaireLink commentaireLink in commentaire.CommentaireLinks)
            {
                sb.Append("<p>");
                try
                {
                    Reference reference = _referenceConverter.ConvertReference(commentaireLink.Reference);
                    sb.Append($"Voir note sur <a href=\"{_referenceConverter.ReferenceToCommentaryLink(reference)}\">{_referenceConverter.ReferenceToReadableString(reference)}{reference.Suffix} - {commentaireLink.Titre}</a>");
                }
                catch
                {
                    sb.Append($"Voir note sur {commentaireLink.Reference} - {commentaireLink.Titre}");
                }
                sb.Append("</p>");
            }

            sb.Append("<hr />");

            return sb.ToString();
        }

        #endregion Methods

        #region Fields

        private readonly TReferenceConverter _referenceConverter;

        #endregion Fields
    }
}