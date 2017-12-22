using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Immanuel.WordChk.Models
{
    public class TextRank
    {
        public int WordPercentage { get; set; }
        public string ProfaneRate { get; set; }
    }

    public class Ranking
    {
        public List<TextRank> TextRank { get; set; }
    }
}