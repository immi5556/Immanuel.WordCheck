using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Immanuel.Core.WordCheck.Controllers
{
    [ApiController]
    public class ProfaneController : ControllerBase
    {
        [HttpGet]
        [Route("Profane/Check/{text}")]
        public Models.TextRank Check(string text)
        {
            return GetRate(text);
        }

        [HttpPost]
        [Route("Profane/Check")]
        public Models.TextRank CheckPost()
        {
            string text = HttpContext.Request.Form["text"];
            return GetRate(text);
        }

        [HttpPost]
        [Route("Profane/Comments")]
        public List<Models.TextRank> CommentPost()
        {
            List<Models.TextRank> rest = new List<Models.TextRank>();
            string cmts = HttpContext.Request.Form["comments"];
            try
            {
                List<string> lcmts = JsonConvert.DeserializeObject<List<string>>(cmts);
                lcmts.ForEach(t =>
                {
                    try
                    {
                        string strcmt = "";
                        try
                        {
                            strcmt = System.Text.UTF8Encoding.UTF8.GetString(Convert.FromBase64String(t));
                        }
                        catch (FormatException ee)
                        {
                            strcmt = t;
                        }
                        rest.Add(GetRate(strcmt));
                    }
                    catch (Exception ee)
                    {
                        rest.Add(new Models.TextRank()
                        {
                            ProfaneRate = "UnKnown (Error Occured)"
                        });
                    }
                });
            }
            catch { throw; }
            return rest;
        }

        Models.TextRank GetRate(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;
            List<string> sents = text.Split('.').ToList();
            List<string> wrds = new List<string>();
            sents.ForEach(t => wrds.AddRange(t.Split(' ', StringSplitOptions.RemoveEmptyEntries).Distinct()));

            var vulgarinterword = App_Start.WordContext.Words.Vulgar.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            var vulgarintersent = App_Start.WordContext.Words.Vulgar.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            var uglyinterword = App_Start.WordContext.Words.Ugly.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            var uglyintersent = App_Start.WordContext.Words.Ugly.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            var positiveinterword = App_Start.WordContext.Words.Positive.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            var positiveintersent = App_Start.WordContext.Words.Positive.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            var offensiveinterword = App_Start.WordContext.Words.Offensive.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            var offensiveintersent = App_Start.WordContext.Words.Offensive.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            var negativeinterword = App_Start.WordContext.Words.Negative.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            var negativeintersent = App_Start.WordContext.Words.Negative.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            var insultinterword = App_Start.WordContext.Words.Insult.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            var insultintersent = App_Start.WordContext.Words.Insult.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            var harshinterword = App_Start.WordContext.Words.Harsh.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            var harshintersent = App_Start.WordContext.Words.Harsh.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            var apologizeinterword = App_Start.WordContext.Words.Apologize.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            var apologizeintersent = App_Start.WordContext.Words.Apologize.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            int totcnt = wrds.Count;
            int wrdcnt = vulgarinterword.Count + vulgarintersent.Count + uglyinterword.Count
                 + uglyintersent.Count + harshinterword.Count + insultintersent.Count;

            if (wrdcnt == 0)
                return App_Start.WordContext.Rank.TextRank[0];
            double pcnt = (double)wrdcnt / totcnt * 100;
            pcnt = pcnt > 100 ? 100 : pcnt;
            bool fnd = false;
            Models.TextRank tr = null;
            App_Start.WordContext.Rank.TextRank.ForEach(t =>
            {
                if (!fnd && pcnt <= t.WordPercentage && t.WordPercentage > 0)
                {
                    tr = t;
                    fnd = true;
                }
            });

            return tr;
        }
    }
}
