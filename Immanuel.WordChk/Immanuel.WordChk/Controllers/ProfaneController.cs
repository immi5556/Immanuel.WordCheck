using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Immanuel.WordChk.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProfaneController : ApiController
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
            string text = System.Web.HttpContext.Current.Request.Form["text"];
            return GetRate(text);
        }

        [HttpPost]
        [Route("Profane/Comments")]
        public List<Models.TextRank> CommentPost()
        {
            List<Models.TextRank> rest = new List<Models.TextRank>();
            string cmts = System.Web.HttpContext.Current.Request.Form["comments"];
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
            sents.ForEach(t => wrds.AddRange(t.Split(' ')));

            var vulgarinterword = WordChk.App_Start.WordContext.Words.Vulgar.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            var vulgarintersent = WordChk.App_Start.WordContext.Words.Vulgar.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            var uglyinterword = WordChk.App_Start.WordContext.Words.Ugly.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            var uglyintersent = WordChk.App_Start.WordContext.Words.Ugly.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            var positiveinterword = WordChk.App_Start.WordContext.Words.Positive.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            var positiveintersent = WordChk.App_Start.WordContext.Words.Positive.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            var offensiveinterword = WordChk.App_Start.WordContext.Words.Offensive.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            var offensiveintersent = WordChk.App_Start.WordContext.Words.Offensive.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            var negativeinterword = WordChk.App_Start.WordContext.Words.Negative.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            var negativeintersent = WordChk.App_Start.WordContext.Words.Negative.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            var insultinterword = WordChk.App_Start.WordContext.Words.Insult.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            var insultintersent = WordChk.App_Start.WordContext.Words.Insult.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            var harshinterword = WordChk.App_Start.WordContext.Words.Harsh.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            var harshintersent = WordChk.App_Start.WordContext.Words.Harsh.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            var apologizeinterword = WordChk.App_Start.WordContext.Words.Apologize.Intersect(wrds, StringComparer.OrdinalIgnoreCase).ToList();
            var apologizeintersent = WordChk.App_Start.WordContext.Words.Apologize.Intersect(sents, StringComparer.OrdinalIgnoreCase).ToList();

            int totcnt = wrds.Count;
            int wrdcnt = vulgarinterword.Count + vulgarintersent.Count + uglyinterword.Count;
            //     + uglyintersent.Count + harshinterword.Count + insultintersent.Count;

            if (wrdcnt == 0)
                return WordChk.App_Start.WordContext.Rank.TextRank[0];
            double pcnt = (double)wrdcnt / totcnt * 100;
            bool fnd = false;
            Models.TextRank tr = null;
            WordChk.App_Start.WordContext.Rank.TextRank.ForEach(t =>
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