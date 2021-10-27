﻿using Common.Commentaries.Model;
using Common.Commentaries.Tools;
using System;
using System.Collections.Generic;

namespace OSISTools.Commentaries.Tools
{
    public class OSISReferenceConverter : BaseReferenceConverter
    {
        private static readonly Dictionary<int, string> BookIndexConvertion = new Dictionary<int, string>()
        {
            {1, "Gen"},
            {2, "Exod" },
            {3, "Lev" },
            {4, "Num" },
            {5, "Deut" },
            {6, "Josh" },
            {7, "Judg" },
            {8, "Ruth" },
            {9, "1Sam" },
            {10, "2Sam" },
            {11, "1Kgs" },
            {12, "2Kgs" },
            {13, "1Chr" },
            {14, "2Chr" },
            {15, "Ezra" },
            {16, "Neh" }, // Nehemiah
            {17, "Esth" }, // Esther
            {18, "Job" },
            {19, "Ps" },
            {20, "Prov" },
            {21, "Eccl" },
            {22, "Song" }, // Song
            {23, "Isa" },
            {24, "Jer" },
            {25, "Lam" }, //Lamentations
            {26, "Ezek" }, //Ezekiel
            {27, "Dan" },
            {28, "Hos" },
            {29, "Joel" },
            {30, "Amos" },
            {31, "Obad" },
            {32, "Jonah" },
            {33, "Mic" },
            {34, "Nah" },
            {35, "Hab" },
            {36, "Zeph" },
            {37, "Hag" },
            {38, "Zech" },
            {39, "Mal" },
            {40, "Matt" }, //Matthew
            {41, "Mark" },
            {42, "Luke" },
            {43, "John" },
            {44, "Acts" }, //Acts
            {45, "Rom" },
            {46, "1Cor" },
            {47, "2Cor" },
            {48, "Gal" },
            {49, "Eph" },
            {50, "Phil" },
            {51, "Col" },
            {52, "1Thess" },
            {53, "2Thess" },
            {54, "1Tim" },
            {55, "2Tim" },
            {56, "Titus" },
            {57, "Phlm" },
            {58, "Heb" },
            {59, "Jas" },
            {60, "1Pet" },
            {61, "2Pet" },
            {62, "1John" },
            {63, "2John" },
            {64, "3John" },
            {65, "Jude" },
            {66, "Rev" },
        };

        public override string ReferenceToBookLink(Reference reference)
        {
            int bookIndex = BookNumberFromAbbreviation(reference.Book);

            string referenceString = $"{BookIndexConvertion[bookIndex]}.{reference.Chapter}.{reference.FromVerse}";

            if (reference.ToVerse.HasValue)
            {
                referenceString = $"{referenceString}-{BookIndexConvertion[bookIndex]}.{reference.Chapter}.{reference.ToVerse}";
            }

            return referenceString;
        }

        public override string ReferenceToCommentaryLink(Reference reference)
        {
            int bookIndex = BookNumberFromAbbreviation(reference.Book);

            string referenceString = $"FrePAS:{BookIndexConvertion[bookIndex]}.{reference.Chapter}.{reference.FromVerse}";

            if (reference.ToVerse.HasValue)
            {
                referenceString = $"{referenceString}-FrePAS:{BookIndexConvertion[bookIndex]}.{reference.Chapter}.{reference.ToVerse}";
            }

            return referenceString;
        }
    }
}
