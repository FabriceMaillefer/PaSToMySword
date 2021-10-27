using Common.Commentaries.Model;
using Common.Commentaries.Tools;
using CommonTools.Commentaries.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSISTools.Commentaries.Tools
{
    public class OSISFormater : ICommentaryFormater<OSISReferenceConverter>
    {
        #region Constructors

        public OSISFormater(OSISReferenceConverter referenceConverter)
        {
            _referenceConverter = referenceConverter;
        }

        #endregion Constructors

        #region Methods

        public string ToString(Commentaire commentaire)
        {
            StringBuilder sb = new();

            sb.Append($"<title>{commentaire.Titre}</title>");

            foreach (Paragraphe paragraphe in commentaire.Versets)
            {
                sb.Append("<p>");
                foreach (ParagrapheItem paragrapheItem in paragraphe.Content)
                {
                    switch (paragrapheItem.Type)
                    {
                        case ParagrapheItemType.Citation:
                            sb.Append($"<q marker=\"\"><hi type=\"italic\">{paragrapheItem.Texte}</hi></q>");
                            break;

                        case ParagrapheItemType.Reference:
                            {
                                try
                                {
                                    List<Reference> references = _referenceConverter.ConvertReference(paragrapheItem.Texte);
                                    foreach (Reference reference in references)
                                    {
                                        if (reference != references.First())
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
                                        sb.Append($"<reference osisRef=\"{_referenceConverter.ReferenceToBookLink(reference)}\">{_referenceConverter.ReferenceToReadableString(reference)}</reference>{reference.Suffix}");
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

            foreach (Paragraphe paragraphe in commentaire.Paragraphes)
            {
                sb.Append("<p>");
                foreach (ParagrapheItem paragrapheItem in paragraphe.Content)
                {
                    switch (paragrapheItem.Type)
                    {
                        case ParagrapheItemType.Citation:
                            sb.Append($"<q marker=\"\"><hi type=\"italic\">{paragrapheItem.Texte}</hi></q>");
                            break;

                        case ParagrapheItemType.Reference:
                            {
                                try
                                {
                                    List<Reference> references = _referenceConverter.ConvertReference(paragrapheItem.Texte);
                                    foreach (Reference reference in references)
                                    {
                                        if (reference != references.First())
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
                                        sb.Append($"<reference osisRef=\"{_referenceConverter.ReferenceToBookLink(reference)}\">{_referenceConverter.ReferenceToReadableString(reference)}</reference>{reference.Suffix}");
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
                        sb.Append($"Voir note sur <reference osisRef=\"{_referenceConverter.ReferenceToCommentaryLink(reference)}\">{_referenceConverter.ReferenceToReadableString(reference)}{reference.Suffix} - {commentaireLink.Titre}</reference>");
                    }
                }
                catch
                {
                    sb.Append($"Voir note sur {commentaireLink.Reference} - {commentaireLink.Titre}");
                }
                sb.Append("</p>");
            }
            sb.Append("<p>");
            sb.Append($"Publié dans Plaire au Seigneur {commentaire.Date.Year} le {commentaire.Date:d}");
            sb.Append("</p>");
            return sb.ToString();
        }

        #endregion Methods

        #region Fields

        private readonly OSISReferenceConverter _referenceConverter;

        #endregion Fields
    }
}