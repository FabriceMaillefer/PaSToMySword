using Common.Commentaries.Model;
using CommonTools.Commentaries.Tools;
using System.Collections.Generic;
using System.Text;

namespace BU3Tools.Commentaries.Tools
{
    public class BU3Formater : ICommentaryFormater<BU3ReferenceConverter>
    {
        #region Constructors

        public BU3Formater(BU3ReferenceConverter referenceConverter)
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
                                    List<Reference> references = _referenceConverter.ConvertReference(paragrapheItem.Texte);
                                    foreach (Reference reference in references)
                                    {
                                        if (reference != references[0])
                                        {
                                            if (reference.DisplayBook)
                                            {
                                                sb.Append("; ");
                                            }
                                            else
                                            {
                                                sb.Append(", ");
                                            }
                                        }
                                        sb.Append($"<a href=\"{_referenceConverter.ReferenceToBookLink(reference)}\">{_referenceConverter.ReferenceToReadableString(reference)}</a>{reference.Suffix}");
                                    }
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
                                    List<Reference> references = _referenceConverter.ConvertReference(paragrapheItem.Texte);
                                    foreach (Reference reference in references)
                                    {
                                        if (reference != references[0])
                                        {
                                            if (reference.DisplayBook)
                                            {
                                                sb.Append("; ");
                                            }
                                            else
                                            {
                                                sb.Append(", ");
                                            }
                                        }
                                        sb.Append($"<a href=\"{_referenceConverter.ReferenceToBookLink(reference)}\">{_referenceConverter.ReferenceToReadableString(reference)}</a>{reference.Suffix}");
                                    }
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
                    List<Reference> references = _referenceConverter.ConvertReference(commentaireLink.Reference);
                    foreach (Reference reference in references)
                    {
                        sb.Append($"Voir note sur <a href=\"{_referenceConverter.ReferenceToCommentaryLink(reference)}\">{_referenceConverter.ReferenceToReadableString(reference)}{reference.Suffix} - {commentaireLink.Titre}</a>");
                    }
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

        private readonly BU3ReferenceConverter _referenceConverter;

        #endregion Fields
    }
}