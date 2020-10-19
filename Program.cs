using System;
using System.Collections.Generic;
using System.Linq;

namespace s2._3
{
    class Program
    {                
        static void Runner(string lnk)
        {
            if (!gotArticles.Contains(lnk))
            {
                gotArticles.Add(lnk);
                FileController.AppendLine(artCollection, lnk);                
                LentaArticle art;
                try
                {
                    art = new LentaArticle(lnk);
                }
                catch (Exception e)
                {
                    WriteExecption(e, String.Format("Ошибка при получении статьи {0}.", lnk));
                    return;
                }
                var key = art.GetCategory();             
                if (!gotCategories.ContainsKey(key))
                {
                    gotCategories.Add(key, 0);                    
                }
                    
                if (gotCategories[key] < artCnt)
                {
                    gotCategories[key]++;
                    WriteReceivedArticle(art, lnk);
                    FileController.RewriteDictionary(artCategories, gotCategories);
                    try
                    {
                        dbController.ArticleInsert(art.GetHead(), art.GetBody(), art.GetDateTime().ToString(), art.GetCategory());
                        var end = "";
                        foreach (var item in gotCategories)
                            if (item.Value < artCnt)
                                end += String.Format($" \"{item.Key}\" - {item.Value}");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(end);
                        Console.ResetColor();
                    }
                    catch (Exception e)
                    {
                        WriteExecption(e, "Ошибка при добавлении записи в базу данных.");
                    }
                    foreach (var url in art.GetLinksNext())
                        Runner(url);
                }               
            }          
        }
        static void WriteExecption(Exception e, string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message + " " + text);
            Console.ResetColor();
        }
        static void WriteReceivedArticle(LentaArticle art, string lnk)
        {
            Console.Write($"{gotArticles.Count()}. {art.GetHead()} - {art.GetCategory()} ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(lnk + "\n\r");
            Console.ResetColor();
        }
        
        static Dictionary<string, int> gotCategories;
        static HashSet<string> gotArticles;
        static DBController dbController;
        static int artCnt = 10000;
        static int days = 365;
        static string artCategories = @"C:\articlesDS\articlesCategories.txt";
        static string artCollection = @"C:\articlesDS\articlesCollection.txt";
        static string DBpath = @"C:\articlesDS\articles.db";

        static void Main(string[] args)
        {
            gotCategories = FileController.GetDictionary(artCategories);
            gotArticles = FileController.GetHashSet(artCollection);
            dbController = new DBController(DBpath);

            var baseUrl = "https://lenta.ru/";
            DateTime dt = DateTime.Now;

            for (int i = 0; i < days; i++)
            {
                dt = dt.AddDays(-1);
                Runner(baseUrl + dt.Year.ToString()
                    + "/" + String.Format("{0:d2}", dt.Month)
                    + "/" + String.Format("{0:d2}", dt.Day)
                    + "/");
            }
        }
    }
}
