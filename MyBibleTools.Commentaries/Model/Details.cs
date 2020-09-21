using System.Collections.Generic;

namespace MyBibleTools.Commentaries.Model
{
    public class Details
    {
        #region Properties

        public string Origin { get; set; }
        public string HistoryOfChanges { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public string DetailedInfo { get; set; }
        public string Region { get; set; }
        public string HyperlinkLanguages { get; set; }
        public string HtmlStyle { get; set; }

        #endregion Properties

        #region Methods

        public IEnumerable<Info> ToInfos()
        {
            yield return new Info("origin", Origin);
            yield return new Info("history_of_changes", HistoryOfChanges);
            yield return new Info("language", Language);
            yield return new Info("description", Description);
            yield return new Info("detailed_info", DetailedInfo);
            yield return new Info("region", Region);
            yield return new Info("hyperlink_languages", HyperlinkLanguages);
            yield return new Info("html_style", HtmlStyle);
        }

        #endregion Methods
    }
}