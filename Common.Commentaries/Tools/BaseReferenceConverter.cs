using Common.Commentaries.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Common.Commentaries.Tools
{
    public abstract class BaseReferenceConverter : IReferenceConverter
    {
        #region Fields

        static protected Dictionary<string, IEnumerable<string>> BookList = new Dictionary<string, IEnumerable<string>>
        {
            {"###", new string []{ } },
            {"Genèse", new string [] {"Ge", "Gen", "Genese" } },
            {"Exode", new string [] { "Ex", "Exo"} },
            {"Lévitique", new string [] { "Levitique", "Le", "Lé", "Lv", "Lev", "Lév" } },
            {"Nombres", new string [] { "Nombre", "No", "Nb", "Nom"} },
            {"Deutéronome", new string [] { "De", "Deut", "Dt" } },
            {"Josué", new string [] { "Josue", "Jos", "Js" } },
            {"Juges", new string [] { "Juge", "Jug", "Jg" } },
            {"Ruth", new string [] { "Ru", "Rt" } },
            {"1 Samuel", new string [] { "1Samuel", "1Sa","1S" } },
            {"2 Samuel", new string [] { "2Samuel", "2Sa","2S"} },
            {"1 Rois", new string [] { "1Rois", "1R", "1Ro" } },
            {"2 Rois", new string [] { "2Rois", "2R", "2Ro" } },
            {"1 Chroniques", new string [] { "1 Chroniques", "1Ch" } },
            {"2 Chroniques", new string [] { "2 Chroniques", "2Ch" } },
            {"Esdras", new string [] { "Esd" } },
            {"Néhémie", new string [] { "Nehemie", "Ne", "Né", "Néh", "Neh"} },
            {"Esther", new string [] { "Est"} },
            {"Job", new string [] { "Jb" } },
            {"Psaumes", new string [] { "Psaume", "Psa", "Ps" } },
            {"Proverbes", new string [] { "Pro","Pr"} },
            {"Ecclésiaste", new string [] {"Ec", "Ecc" } },
            {"Cantique", new string [] { "Ct", "Ca", "Cant" } },
            {"Ésaïe", new string [] { "Esaie", "Es", "És", "Esa", "Ésa" } },
            {"Jérémie", new string [] { "Jér", "Jer", "Jeremie" } },
            {"Lamentations", new string [] { "La", "Lam" } },
            {"Ézéchiel", new string [] { "Ez", "Éz" } },
            {"Daniel", new string [] { "Da", "Dn" } },
            {"Osée", new string [] { "Osee", "Os" } },
            {"Joël", new string [] { "Joel", "Jl", "Joe" } },
            {"Amos", new string [] { "Am" } },
            {"Abdias", new string [] { "Ab", "Abd"} },
            {"Jonas", new string [] { "Jon"} },
            {"Michée", new string [] { "Mi","Michee","Mic"} },
            {"Nahum", new string [] { "Na", "Nah"} },
            {"Habakuk", new string [] { "Ha", "Hab" } },
            {"Sophonie", new string [] { "So", "Sop" } },
            {"Aggée", new string [] { "Ag", "Aggee", "Agg"} },
            {"Zacharie", new string [] { "Za", "Zac"} },
            {"Malachie", new string [] { "Ma", "Mal"} },
            {"Matthieu", new string [] { "Mt", "Mat"} },
            {"Marc", new string [] { "Mr","Mc"} },
            {"Luc", new string [] { "Lu","Lc" } },
            {"Jean", new string [] { "Jn" } },
            {"Actes", new string [] { "Ac", "Act" } },
            {"Romains", new string [] {"Romain", "Ro", "Rom" } },
            {"1 Corinthiens", new string [] { "1Corinthiens", "1Co" } },
            {"2 Corinthiens", new string [] { "2Corinthiens", "2Co" } },
            {"Galates", new string [] { "Gal", "Ga" } },
            {"Éphésiens", new string [] { "Ephesiens", "Ep", "Eph" } },
            {"Philippiens", new string [] { "Phi", "Ph", "Php" } },
            {"Colossiens", new string [] { "Col", "Co" } },
            {"1 Thessaloniciens", new string [] {"1Thessaloniciens", "1Th","1Thess" } },
            {"2 Thessaloniciens", new string [] { "2Thessaloniciens", "2Th","2Thess" } },
            {"1 Timothée", new string [] { "1Timothée", "1Timothee", "1Tim", "1Ti" } },
            {"2 Timothée", new string [] { "2Timothée", "2Timothee", "2Tim", "2Ti" } },
            {"Tite", new string [] { "Ti", "Tt", "Tit" } },
            {"Philémon", new string [] { "Phm", "Philemon" } },
            {"Hébreux", new string [] { "Hé", "Hebreux", "He", "Heb", "Héb" } },
            {"Jacques", new string [] { "Jc", "Jaccque", "Ja", "Jac" } },
            {"1 Pierre", new string [] { "1Pierre", "1Pi", "1P" } },
            {"2 Pierre", new string [] { "2Pierre", "2Pi", "2P" } },
            {"1 Jean", new string [] { "1Jean", "1Jn", "1J"} },
            {"2 Jean", new string [] { "2Jean", "2Jn", "2J"} },
            {"3 Jean", new string [] { "3Jean", "3Jn", "3J"} },
            {"Jude", new string [] { "Jd", "Jud"} },
            {"Apocalypse", new string [] {"Ap"} }
        };

        #endregion Fields

        #region Methods

        public int BookNumberFromAbbreviation(string book)
        {
            book = Regex.Replace(book, @"\s+", ""); // remove all space
            if (BookList.ContainsKey(book))
            {
                return BookList.Keys.ToList().IndexOf(book);
            }
            else
            {
                string bookKey = BookList.Where(t => t.Value.Contains(book)).Select(t => t.Key).SingleOrDefault();
                return BookList.Keys.ToList().IndexOf(bookKey);
            }
        }

        public Reference ConvertReference(string referenceString)
        {
            Regex regex = new Regex(@"(?<book>[^\s]+) (?<chapter>[\d]+)[:.]?(?<verse>[\d]+)?[-]?(?<verse2>[\d]+)?(?<suffix>.*)");
            GroupCollection groups = regex.Match(referenceString).Groups;

            Reference reference = new Reference();

            foreach (string groupName in regex.GetGroupNames())
            {
                if (!groups[groupName].Success || string.IsNullOrEmpty(groups[groupName].Value))
                    continue;

                if (groupName == "book")
                    reference.Book = groups[groupName].Value;

                if (groupName == "chapter")
                {
                    if (!groups["verse"].Success) // Reference without chapter in small books
                    {
                        reference.Chapter = 1;
                        reference.BookWithoutChapter = true;
                        reference.FromVerse = int.Parse(groups[groupName].Value);
                    }
                    else
                    {
                        reference.Chapter = int.Parse(groups[groupName].Value);
                    }
                }

                if (groupName == "verse")
                {
                    reference.FromVerse = int.Parse(groups[groupName].Value);
                }

                if (groupName == "verse2")
                {
                    reference.ToVerse = int.Parse(groups[groupName].Value);
                }

                if (groupName == "suffix")
                    reference.Suffix = groups[groupName].Value;
            }

            if (reference.ToVerse is null)
                reference.ToVerse = reference.FromVerse;

            return reference;
        }

        public abstract string ReferenceToBookLink(Reference reference);

        public abstract string ReferenceToCommentaryLink(Reference reference);

        public string ReferenceToReadableString(Reference reference)
        {
            int bookIndex = BookNumberFromAbbreviation(reference.Book);

            if (bookIndex >= 0)
            {
                string bookName = BookList.Keys.ToList()[bookIndex];

                if (reference.BookWithoutChapter)
                {
                    return $"{bookName} {reference.FromVerse}";
                }
                else
                {
                    if (reference.FromVerse == reference.ToVerse)
                        return $"{bookName} {reference.Chapter}:{reference.FromVerse}";
                    else
                        return $"{bookName} {reference.Chapter}:{reference.FromVerse}-{reference.ToVerse}";
                }
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Methods
    }
}