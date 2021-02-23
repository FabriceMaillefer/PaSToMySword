using Common.Commentaries.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Common.Commentaries.Tools
{
    public abstract class BaseReferenceConverter : IReferenceConverter
    {
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
                string bookKey = BookList.Where(t => t.Value.Item1.Contains(book)).Select(t => t.Key).SingleOrDefault();
                return BookList.Keys.ToList().IndexOf(bookKey);
            }
        }

        public List<Reference> ConvertReference(string referenceString)
        {
            // (?<book>[^\s]+)? (?:(?<chapter>[\d]+):)(?:(?<verseFrom>[\d]+)[-](?<verseTo>[\d]+)|(?<verse0>[\d]+)(?:[,](?<verse1>[\d]+))+)

            Regex regex = new Regex(@"(?:(?<book>\d?[\w]+)[ ]+)?(?:(?:(?<chapter>\d+)[: ]+)?(?:(?:(?<verseFrom>\d+)-(?<verseTo>\d+))|(?<verse>\d+)))");

            List<Reference> references = new List<Reference>();
            Reference reference = new Reference();

            MatchCollection matches = regex.Matches(referenceString);

            foreach (object item in matches)
            {
                if (item is Match match)
                {
                    if (match.Success)
                    {
                        GroupCollection groups = match.Groups;

                        foreach (string groupName in regex.GetGroupNames())
                        {
                            if (!groups[groupName].Success || string.IsNullOrEmpty(groups[groupName].Value))
                                continue;

                            if (groupName == "book")
                            {
                                reference = new Reference();
                                references.Add(reference);

                                reference.DisplayBook = true;
                                reference.Book = groups[groupName].Value;
                            }

                            if (groupName == "chapter")
                            {
                                if(reference.Chapter.HasValue)
                                {
                                    reference = new Reference
                                    {
                                        Book = reference.Book,
                                    };
                                    references.Add(reference);
                                }

                                reference.DisplayChapter = true;
                                reference.Chapter = int.Parse(groups[groupName].Value);
                            }

                            if (groupName == "verse" || groupName == "verseFrom")
                            {
                                int verseValue = int.Parse(groups[groupName].Value);
                                if (reference.FromVerse.HasValue)
                                {

                                    if(verseValue == reference.FromVerse + 1 || verseValue == reference.ToVerse + 1)
                                    {
                                        reference.ToVerse = verseValue;
                                        continue;
                                    }
                                    else
                                    {
                                        reference = new Reference
                                        {
                                            Book = reference.Book,
                                            Chapter = reference.Chapter,
                                        };
                                        references.Add(reference);
                                    }
                                }

                                if(reference.DisplayBook && !groups["chapter"].Success) // reference with Book and Chapter only are Book + Verse
                                {
                                    int bookIndex = BookNumberFromAbbreviation(reference.Book);
                                    if(bookIndex >= 0)
                                    {
                                        (IEnumerable<string>, bool) book = BookList.Values.ElementAt(bookIndex);

                                        if (book.Item2) // Small book with only one chapter
                                        {
                                            reference.Chapter = 1;
                                            reference.FromVerse = int.Parse(groups[groupName].Value);
                                        }
                                        else
                                        {
                                            reference.DisplayChapter = true;
                                            reference.Chapter = int.Parse(groups[groupName].Value);
                                        }
                                    }
                                }
                                else
                                {
                                    int bookIndex = BookNumberFromAbbreviation(reference.Book);
                                    if (bookIndex >= 0)
                                    {
                                        (IEnumerable<string>, bool) book = BookList.Values.ElementAt(bookIndex);

                                        if (book.Item2) // Small book with only one chapter
                                        {
                                            reference.DisplayChapter = false;
                                        }
                                    }
                                    reference.FromVerse = int.Parse(groups[groupName].Value);
                                }
                            }

                            if (groupName == "verseTo")
                            {
                                reference.ToVerse = int.Parse(groups[groupName].Value);
                            }
                        }
                    }
                }
            }

            return references;
        }

        public abstract string ReferenceToBookLink(Reference reference);

        public abstract string ReferenceToCommentaryLink(Reference reference);

        public string ReferenceToReadableString(Reference reference)
        {
            int bookIndex = BookNumberFromAbbreviation(reference.Book);

            if (bookIndex >= 0)
            {
                string bookName = BookList.Keys.ElementAt(bookIndex);

                string referenceString = string.Empty;
                if(reference.DisplayBook)
                {
                    referenceString = $"{bookName} ";
                }

                if(reference.DisplayChapter)
                {
                    referenceString = $"{referenceString}{reference.Chapter}";

                    if (reference.FromVerse.HasValue)
                    {
                        referenceString = $"{referenceString}:";
                    }
                }

                if (reference.FromVerse.HasValue)
                {
                    referenceString = $"{referenceString}{reference.FromVerse}";
                }

                if (reference.ToVerse.HasValue)
                {
                    if(reference.ToVerse == reference.FromVerse + 1)
                    {
                        referenceString = $"{referenceString},{reference.ToVerse}";
                    }
                    else
                    {
                        referenceString = $"{referenceString}-{reference.ToVerse}";
                    }
                }

                return referenceString;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Methods

        #region Fields

        // Key = full book name, (item1 = list of abbreviations, item2 = true if book has only one chapter)
        static protected Dictionary<string, (IEnumerable<string>, bool)> BookList = new Dictionary<string, (IEnumerable<string>, bool)>
        {
            {"###"          , (new string []{ }, false) },
            {"Genèse"       , (new string [] {"Ge", "Gen", "Genese" }, false) },
            {"Exode"        , (new string [] { "Ex", "Exo"}, false) },
            {"Lévitique"    , (new string [] { "Levitique", "Le", "Lé", "Lv", "Lev", "Lév" }, false) },
            {"Nombres"      , (new string [] { "Nombre", "No", "Nb", "Nom"}, false) },
            {"Deutéronome"  , (new string [] { "De", "Deut", "Dt" }, false) },
            {"Josué"        , (new string [] { "Josue", "Jos", "Js" }, false) },
            {"Juges"        , (new string [] { "Juge", "Jug", "Jg" }, false) },
            {"Ruth"         , (new string [] { "Ru", "Rt" }, false) },
            {"1 Samuel"     , (new string [] { "1Samuel", "1Sa","1S" }, false) },
            {"2 Samuel"     , (new string [] { "2Samuel", "2Sa","2S"}, false) },
            {"1 Rois"       , (new string [] { "1Rois", "1R", "1Ro" }, false) },
            {"2 Rois"       , (new string [] { "2Rois", "2R", "2Ro" }, false) },
            {"1 Chroniques" , (new string [] { "1 Chroniques", "1Ch" }, false) },
            {"2 Chroniques" , (new string [] { "2 Chroniques", "2Ch" }, false) },
            {"Esdras"       , (new string [] { "Esd" }, false) },
            {"Néhémie"      , (new string [] { "Nehemie", "Ne", "Né", "Néh", "Neh"}, false) },
            {"Esther"       , (new string [] { "Est"}, false) },
            {"Job"          , (new string [] { "Jb" }, false) },
            {"Psaumes"      , (new string [] { "Psaume", "Psa", "Ps" }, false) },
            {"Proverbes"    , (new string [] { "Pro","Pr"}, false) },
            {"Ecclésiaste"  , (new string [] {"Ec", "Ecc" }, false) },
            {"Cantique"     , (new string [] { "Ct", "Ca", "Cant" }, false) },
            {"Ésaïe"        , (new string [] { "Esaie", "Es", "És", "Esa", "Ésa" }, false) },
            {"Jérémie"      , (new string [] { "Jér", "Jer", "Jeremie" }, false) },
            {"Lamentations" , (new string [] { "La", "Lam" }, false) },
            {"Ézéchiel"     , (new string [] { "Ez", "Éz" }, false) },
            {"Daniel"       , (new string [] { "Da", "Dn" }, false) },
            {"Osée"         , (new string [] { "Osee", "Os" }, false) },
            {"Joël"         , (new string [] { "Joel", "Jl", "Joe" }, false) },
            {"Amos"         , (new string [] { "Am" }, false) },
            {"Abdias"       , (new string [] { "Ab", "Abd"}, true) },
            {"Jonas"        , (new string [] { "Jon"}, false) },
            {"Michée"       , (new string [] { "Mi","Michee","Mic"}, false) },
            {"Nahum"        , (new string [] { "Na", "Nah"}, false) },
            {"Habakuk"      , (new string [] { "Ha", "Hab" }, false) },
            {"Sophonie"     , (new string [] { "So", "Sop" }, false) },
            {"Aggée"        , (new string [] { "Ag", "Aggee", "Agg"}, false) },
            {"Zacharie"     , (new string [] { "Za", "Zac"}, false) },
            {"Malachie"     , (new string [] { "Ma", "Mal"}, false) },
            {"Matthieu"     , (new string [] { "Mt", "Mat"}, false) },
            {"Marc"         , (new string [] { "Mr","Mc"}, false) },
            {"Luc"          , (new string [] { "Lu","Lc" }, false) },
            {"Jean"         , (new string [] { "Jn" }, false) },
            {"Actes"        , (new string [] { "Ac", "Act" }, false) },
            {"Romains"      , (new string [] {"Romain", "Ro", "Rom" }, false) },
            {"1 Corinthiens", (new string [] { "1Corinthiens", "1Co" }, false) },
            {"2 Corinthiens", (new string [] { "2Corinthiens", "2Co" }, false) },
            {"Galates"      , (new string [] { "Gal", "Ga" }, false) },
            {"Éphésiens"    , (new string [] { "Ephesiens", "Ep", "Eph" }, false) },
            {"Philippiens"  , (new string [] { "Phi", "Ph", "Php" }, false) },
            {"Colossiens"   , (new string [] { "Col", "Co" }, false) },
            {"1 Thessaloniciens", (new string [] {"1Thessaloniciens", "1Th","1Thess" }, false) },
            {"2 Thessaloniciens", (new string [] { "2Thessaloniciens", "2Th","2Thess" }, false) },
            {"1 Timothée"   , (new string [] { "1Timothée", "1Timothee", "1Tim", "1Ti" }, false) },
            {"2 Timothée"   , (new string [] { "2Timothée", "2Timothee", "2Tim", "2Ti" }, false) },
            {"Tite"         , (new string [] { "Ti", "Tt", "Tit" }, false) },
            {"Philémon"     , (new string [] { "Phm", "Philemon" }, true) },
            {"Hébreux"      , (new string [] { "Hé", "Hebreux", "He", "Heb", "Héb" }, false) },
            {"Jacques"      , (new string [] { "Jc", "Jaccque", "Ja", "Jac" }, false) },
            {"1 Pierre"     , (new string [] { "1Pierre", "1Pi", "1P" }, false) },
            {"2 Pierre"     , (new string [] { "2Pierre", "2Pi", "2P" }, false) },
            {"1 Jean"       , (new string [] { "1Jean", "1Jn", "1J"}, false) },
            {"2 Jean"       , (new string [] { "2Jean", "2Jn", "2J"}, false) },
            {"3 Jean"       , (new string [] { "3Jean", "3Jn", "3J"}, false) },
            {"Jude"         , (new string [] { "Jd", "Jud"}, true) },
            {"Apocalypse"   , (new string [] {"Ap"}, false) }
        };

        #endregion Fields
    }
}