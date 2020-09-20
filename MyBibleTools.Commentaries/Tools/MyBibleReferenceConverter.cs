using Common.Commentaries.Model;
using Common.Commentaries.Tools;
using System.Collections.Generic;

namespace MyBibleTools.Commentaries.Tools
{
    public class MyBibleReferenceConverter : BaseReferenceConverter
    {
        public override string ReferenceToBookLink(Reference reference)
        {
            return $"B:{ToStringReference(reference)}";
        }

        public override string ReferenceToCommentaryLink(Reference reference)
        {
            return $"C:@{ToStringReference(reference)}";
        }

        private string ToStringReference(Reference reference)
        {
            int bookIndex = BookNumberFromAbbreviation(reference.Book);

            if (bookIndex >= 0)
            {
                if (reference.FromVerse == reference.ToVerse)
                    return $"{BookIndexConvertion[bookIndex]}.{reference.Chapter}.{reference.FromVerse}";
                else
                    return $"{BookIndexConvertion[bookIndex]}.{reference.Chapter}.{reference.FromVerse}-{reference.ToVerse}";
            }
            else
            {
                return string.Empty;
            }
        }

        public static int ConvertBookIndex(int bookIndex)
        {
            return BookIndexConvertion[bookIndex];
        }

        // MyBible use non-standard book number, see https://mybible.zone/code-eng.php
        private static readonly Dictionary<int, int> BookIndexConvertion = new Dictionary<int, int>()
        {
            {1, 10 },
            {2, 20 },
            {3, 30 },
            {4, 40 },
            {5, 50 },
            {6, 60 },
            {7, 70 },
            {8, 80 },
            {9, 90 },
            {10, 100 },
            {11, 110 },
            {12, 120 },
            {13, 130 },
            {14, 140 },
            {15, 150 },
            {16, 160 }, // Nehemiah
            {17, 190 }, // Esther
            {18, 220 },
            {19, 230 },
            {20, 240 },
            {21, 250 },
            {22, 260 }, // Song
            {23, 290 },
            {24, 300 },
            {25, 310 }, //Lamentations
            {26, 330 }, //Ezekiel
            {27, 340 },
            {28, 350 },
            {29, 360 },
            {30, 370 },
            {31, 380 },
            {32, 390 },
            {33, 400 },
            {34, 410 },
            {35, 420 },
            {36, 430 },
            {37, 440 },
            {38, 450 },
            {39, 460 },
            {40, 470 }, //Matthew
            {41, 480 },
            {42, 490 },
            {43, 500 },
            {44, 510 }, //Acts
            {45, 520 },
            {46, 530 },
            {47, 540 },
            {48, 550 },
            {49, 560 },
            {50, 570 },
            {51, 580 },
            {52, 590 },
            {53, 600 },
            {54, 610 },
            {55, 620 },
            {56, 630 },
            {57, 640 },
            {58, 650 },
            {59, 660 },
            {60, 670 },
            {61, 680 },
            {62, 690 },
            {63, 700 },
            {64, 710 },
            {65, 720 },
            {66, 730 },
        };
    }
}