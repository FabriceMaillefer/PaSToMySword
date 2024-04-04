using Common.Commentaries.Model;
using Common.Commentaries.Tools;
using System.Collections.Generic;
using System.IO;

namespace BU3Tools.Commentaries.Tools
{
    public class BU3ReferenceConverter : BaseReferenceConverter
    {
        public override string ReferenceToBookLink(Reference reference)
        {
            return $"r.php?v=DBY&r={ToStringReference(reference)}";
        }

        public override string ReferenceToCommentaryLink(Reference reference)
        {
            return $"r.php?v=PASA&r={ToStringReference(reference)}";
        }

        private string ToStringReference(Reference reference)
        {
            int bookIndex = BookNumberFromAbbreviation(reference.Book);

            if (bookIndex >= 0)
            {
                string stringBuilder = $"{BookIndexConvertion[bookIndex]}{reference.Chapter}.{reference.FromVerse.GetValueOrDefault()}";

                if (reference.ToVerse.HasValue)
                    return $"{stringBuilder}-{reference.ToVerse}";
                else
                    return stringBuilder;
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetDirectory(Reference reference)
        {
            int bookIndex = BookNumberFromAbbreviation(reference.Book);

            if (bookIndex >= 0)
            {
                return BookIndexConvertion[bookIndex];
            }

            throw new KeyNotFoundException("Reference cannot be parsed correctly");
        }

        public string GetFilePath(Reference reference)
        {
            return Path.Combine(GetDirectory(reference), $"{reference.Chapter:000}.html");
        }

        /// <summary>
        /// Nomenclature des livres bibliques de la SIL (Summer Institute of Linguistics).
        /// </summary>
        private static readonly Dictionary<int, string> BookIndexConvertion = new Dictionary<int, string>()
        {
            {1, "GEN"},
            {2, "EXO" },
            {3, "LEV" },
            {4, "NUM" },
            {5, "DEU" },
            {6, "JOS" },
            {7, "JDG" },
            {8, "RUT" },
            {9, "1SA" },
            {10, "2SA" },
            {11, "1KI" },
            {12, "2KI" },
            {13, "1CH" },
            {14, "2CH" },
            {15, "EZR" },
            {16, "NEH" }, // Nehemiah
            {17, "EST" }, // Esther
            {18, "JOB" },
            {19, "PSA" },
            {20, "PRO" },
            {21, "ECC" },
            {22, "SNG" }, // Song
            {23, "ISA" },
            {24, "JER" },
            {25, "LAM" }, //Lamentations
            {26, "EZK" }, //Ezekiel
            {27, "DAN" },
            {28, "HOS" },
            {29, "JOL" },
            {30, "AMO" },
            {31, "OBA" },
            {32, "JON" },
            {33, "MIC" },
            {34, "NAM" },
            {35, "HAB" },
            {36, "ZEP" },
            {37, "HAG" },
            {38, "ZEC" },
            {39, "MAL" },
            {40, "MAT" }, //Matthew
            {41, "MRK" },
            {42, "LUK" },
            {43, "JHN" },
            {44, "ACT" }, //Acts
            {45, "ROM" },
            {46, "1CO" },
            {47, "2CO" },
            {48, "GAL" },
            {49, "EPH" },
            {50, "PHP" },
            {51, "COL" },
            {52, "1TH" },
            {53, "2TH" },
            {54, "1TI" },
            {55, "2TI" },
            {56, "TIT" },
            {57, "PHM" },
            {58, "HEB" },
            {59, "JAS" },
            {60, "1PE" },
            {61, "2PE" },
            {62, "1JN" },
            {63, "2JN" },
            {64, "3JN" },
            {65, "JUD" },
            {66, "REV" },
        };
    }
}