using PaSToMySword.Model.ExchangeFormat;
using System.Text;

namespace PaSToMySword.Tools
{
    public class CommentaireFormatter
    {
        #region Methods

        public static string ToHtml(Commentaire commentaire)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"<h1>{commentaire.Titre}</h1>");
            sb.Append($"<hr />");

            foreach (Citation verset in commentaire.Versets)
            {
                Reference reference = ReferenceConverter.ConvertReference(verset.Reference);
                sb.Append($"<div>" +
                    $"<blockquote>{verset.Texte}</blockquote>" +
                    $"<a href=\"{ReferenceConverter.ToStringReference(reference)}\">{verset.Reference}</a>" +
                    $"</div>");
            }

            sb.Append($"<hr />");

            foreach (Paragraphe paragraphe in commentaire.Paragraphes)
            {
                sb.Append($"<p>");
                foreach (ParagrapheItem paragrapheItem in paragraphe.Content)
                {
                    switch (paragrapheItem.Type)
                    {
                        case ParagrapheItemType.Citation:
                            sb.Append($"<i>{paragrapheItem.Texte}</i>");
                            break;

                        case ParagrapheItemType.Reference:
                            {
                                Reference reference = ReferenceConverter.ConvertReference(paragrapheItem.Texte);
                                sb.Append($"<a href=\"{ReferenceConverter.ToStringReference(reference)}\">{paragrapheItem.Texte}</a>");
                            }
                            break;

                        default:
                        case ParagrapheItemType.Normal:
                            sb.Append(paragrapheItem.Texte);
                            break;
                    }
                }

                sb.Append($"</p>");
            }

            return sb.ToString();
        }

        #endregion Methods
    }
}