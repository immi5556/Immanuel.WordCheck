using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Immanuel.WordChk.Models
{
    public class WordsList
    {
        public WordsList()
        {
            Vulgar = new List<string>();
            Ugly = new List<string>();
            Positive = new List<string>();
            Offensive = new List<string>();
            Negative = new List<string>();
            Insult = new List<string>();
            Harsh = new List<string>();
            Apologize = new List<string>();
        }
        public List<string> Vulgar { get; set; }
        public List<string> Ugly { get; set; }
        public List<string> Positive { get; set; }
        public List<string> Offensive { get; set; }
        public List<string> Negative { get; set; }
        public List<string> Insult { get; set; }
        public List<string> Harsh { get; set; }
        public List<string> Apologize { get; set; }
        public Models.Ranking Rank { get; set; }
    }
}