using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PaSToMySword.Tools
{
    public class Reference
    {
        #region Properties

        public string Book { get; set; }

        public int Chapter { get; set; }

        public int FromVerse { get; set; }

        public int? ToVerse { get; set; }

        #endregion Properties
    }

    public class ReferenceConverter
    {
        #region Fields

        static protected Dictionary<string, IEnumerable<string>> BookList = new Dictionary<string, IEnumerable<string>>
        {
            {"Invalid", new string []{ } },
            {"Genèse", new string [] {"Ge", "Gen", "Genese" } },
            {"Exode", new string [] { "Ex", "Exo"} },
            {"Lévitique", new string [] { "Levitique", "Le", "Lé", "Lv", "Lev", "Lév" } },
            {"Nombres", new string [] { "Nombre", "No", "Nb", "Nom"} },
            {"Deutéronome", new string [] { "De", "Deut", "Dt" } },
            {"Josué", new string [] { "Josue", "Jos", "Js" } },
            {"Juges", new string [] { "Juge", "Jug", "Jg" } },
            {"Ruth", new string [] { "Ru", "Rt" } },
            {"1Samuel", new string [] { "1Sa","1S" } },
            {"2Samuel", new string [] { "2Sa","2S"} },
            {"1Roi", new string [] { "1R", "1Ro" } },
            {"2Roi", new string [] {"2R", "2Ro" } },
            {"1Chroniques", new string [] { "1Ch" } },
            {"2Chroniques", new string [] { "2Ch" } },
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
            {"1Corinthiens", new string [] { "1Co" } },
            {"2Corinthiens", new string [] { "2Co" } },
            {"Galates", new string [] { "Gal", "Ga" } },
            {"Éphésiens", new string [] { "Ephesiens", "Ep", "Eph" } },
            {"Philippiens", new string [] { "Phi", "Ph", "Php" } },
            {"Colossiens", new string [] { "Col" } },
            {"1Thessaloniciens", new string [] { "1Th","1Thess" } },
            {"2Thessaloniciens", new string [] { "2Th","2Thess" } },
            {"1Timothée", new string [] { "1Timothee", "1Tim", "1Ti" } },
            {"2Timothée", new string [] { "2Timothee", "2Tim", "2Ti" } },
            {"Tite", new string [] { "Ti", "Tt", "Tit" } },
            {"Philémon", new string [] { "Phm", "Philemon" } },
            {"Hébreux", new string [] { "Hé", "Hebreux", "He", "Heb", "Héb" } },
            {"Jacques", new string [] { "Jc", "Jaccque", "Ja", "Jac" } },
            {"1Pirre", new string [] { "1Pi", "1P" } },
            {"2Pierre", new string [] { "2Pi", "2P" } },
            {"1Jean", new string [] { "1Jn", "1J"} },
            {"2Jean", new string [] { "2Jn", "2J"} },
            {"3Jean", new string [] { "3Jn", "3J"} },
            {"Jude", new string [] { "Jd", "Jud"} },
            {"Apocalypse", new string [] {"Ap"} }
        };

        #endregion Fields

        #region Methods

        static public int BookNumberFromAbbreviation(string book)
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

        static public Reference ConvertReference(string referenceString)
        {
            Regex regex = new Regex(@"(?<book>[^\s]+) (?<chapter>[\d]+)[:.]?(?<verse>[\d]+)?-?(?<verse2>[\d]+)?");
            GroupCollection groups = regex.Match(referenceString).Groups;

            Reference reference = new Reference();

            foreach (string groupName in regex.GetGroupNames())
            {
                if (!groups[groupName].Success)
                    continue;

                if (groupName == "book")
                    reference.Book = groups[groupName].Value;

                if (groupName == "chapter")
                    reference.Chapter = int.Parse(groups[groupName].Value);

                if (groupName == "verse")
                    reference.FromVerse = int.Parse(groups[groupName].Value);

                if (groupName == "verse2")
                    reference.ToVerse = int.Parse(groups[groupName].Value);
            }

            if (reference.ToVerse is null)
                reference.ToVerse = reference.FromVerse;

            return reference;
        }

        static public string ToStringReference(Reference reference)
        {
            int bookIndex = BookNumberFromAbbreviation(reference.Book);

            if (bookIndex >= 0)
            {
                if (reference.FromVerse == reference.ToVerse)
                    return $"b{bookIndex}.{reference.Chapter}.{reference.FromVerse}";
                else
                    return $"b{bookIndex}.{reference.Chapter}.{reference.FromVerse}-{reference.ToVerse}";
            }
            else{
                return string.Empty;
            }
        }

        #endregion Methods
    }
}