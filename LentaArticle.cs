using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using HtmlAgilityPack;

namespace s2._3
{
    class LentaArticle : HtmlDocument
    {       
        HtmlDocument Document;
        public LentaArticle(string lnk)
        {
            Thread.Sleep(new Random().Next(120, 900));
            Document = new HtmlWeb().Load(lnk);
        }
        static private string Clear(string str) => str.Replace('«', '"').Replace('»', '"').Replace("&nbsp;", " ").Trim(new char[] { '[', ']' });
        public string GetHead()
        {
            string txt = "";
            try
            {
                txt = Clear(Document.DocumentNode.SelectSingleNode("//meta[1]").GetAttributeValue("content", ""));
                txt = txt.Substring(0, txt.IndexOf(':'));
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message + " Ошибка в извлечении заголовка статьи.");
                Console.ResetColor();
            }
            return txt;           
        }       
        public string GetBody()
        {
            string txt = "";
            try
            {              
                foreach (var a in Document.DocumentNode.SelectNodes("//div[@itemprop = \"articleBody\"]/p"))
                    txt += Clear(a.InnerText);             
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message + " Ошибка в извлечении тела статьи.");
                Console.ResetColor();
            }
            return txt;
        }
        public DateTime GetDateTime()
        {
            DateTime datetime;
            var a = Document.DocumentNode.SelectSingleNode("//time[@class = \"g-date\"]");
            datetime = DateTime.Parse(a.GetAttributeValue("datetime", ""));
            return datetime;
        }
        public HashSet<string> GetLinksNext()
        {
            var lks = new HashSet<string>();
            string lnk = "";
            foreach (var a in Document.DocumentNode.SelectNodes("//a")) 
            {
                lnk = a.GetAttributeValue("href", "");
                if (Regex.IsMatch(lnk, @"\/(articles|news)\/") && !Regex.IsMatch(lnk, @"https|www|http|comments"))
                    lks.Add("https://lenta.ru" + lnk);
            }
            return lks;
        }
        public string GetCategory() => Regex.Match(Document.DocumentNode
            .SelectSingleNode("//head/descendant::script[text()[contains(.,\"window.Lenta.bloc_slug = \")]]")
            .InnerText, "\"(.*)\"")
            .Groups[1]
            .Value;
    }
}
