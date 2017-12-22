using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Immanuel.WordChk.App_Start
{
    public class WordContext
    {
        //static InitContext()
        //{
        //    //Start();
        //}
        static string pPath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/en-EN");
        static public Models.WordsList Words { get; set; }
        static public Models.Ranking Rank { get; set; }
        public static void Start()
        {
            Words = new Models.WordsList();
            JObject dd = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(Path.Combine(pPath, "Vulgar.json")));
            Words.Vulgar = JsonConvert.DeserializeObject<List<string>>(dd.GetValue("vulgar").ToString());

            dd = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(Path.Combine(pPath, "Ugly.json")));
            Words.Ugly = JsonConvert.DeserializeObject<List<string>>(dd.GetValue("ugly").ToString());

            dd = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(Path.Combine(pPath, "Positive.json")));
            Words.Positive = JsonConvert.DeserializeObject<List<string>>(dd.GetValue("positive").ToString());

            dd = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(Path.Combine(pPath, "Offensive.json")));
            Words.Offensive = JsonConvert.DeserializeObject<List<string>>(dd.GetValue("offensive").ToString());

            dd = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(Path.Combine(pPath, "Negative.json")));
            Words.Negative = JsonConvert.DeserializeObject<List<string>>(dd.GetValue("negative").ToString());

            dd = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(Path.Combine(pPath, "Insult.json")));
            Words.Insult = JsonConvert.DeserializeObject<List<string>>(dd.GetValue("insult").ToString());

            dd = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(Path.Combine(pPath, "Harsh.json")));
            Words.Harsh = JsonConvert.DeserializeObject<List<string>>(dd.GetValue("harsh").ToString());

            dd = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(Path.Combine(pPath, "Apologizing.json")));
            Words.Apologize = JsonConvert.DeserializeObject<List<string>>(dd.GetValue("apologize").ToString());

            Rank = JsonConvert.DeserializeObject<Models.Ranking>(File.ReadAllText(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data"), "config.json")));
            Rank.TextRank = Rank.TextRank.OrderBy(t => t.WordPercentage).Select(t => t).ToList();
        }
    }
}