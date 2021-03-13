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
                    gotCategories.Add(key, 0);

                if (gotCategories[key] < artCnt)
                {
                    gotCategories[key]++;
                    WriteReceivedArticle(art, lnk);
                    FileController.RewriteCollectionDict(artCategories, gotCategories);
                    try
                    {
                        dbController.ArticleInsert(art.GetHead(), art.GetBody(), art.GetDateTime().ToString(), art.GetCategory());
                        var end = "";
                        foreach (var item in gotCategories)
                            if (item.Value < artCnt)
                                end += String.Format($"{item.Key}-{item.Value} ");
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
        static string GetPath()
        {
            Console.WriteLine("Введите директорию для сохранения данных (будет создана отдельная папка)");
            string path = Console.ReadLine();
            while (!System.IO.Directory.Exists(path))
            {
                Console.WriteLine("Указанная директория не существует");
                path = Console.ReadLine();               
            }
            path += "/lenta_articles_parser/";
            System.IO.Directory.CreateDirectory(path);
            return path;
        }
        static int GetArtCount()
        {
            Console.WriteLine("Введите количество статей по каждоый категориии или 0 если нужны все");
            string input = Console.ReadLine();
            int artCnt = -1;
            while (!int.TryParse(input, out artCnt) || artCnt < 0)
            {
                Console.WriteLine("Некорректный ввод");
                input = Console.ReadLine();
            }
            if (artCnt == 0)
                artCnt = int.MaxValue;
            return artCnt;
        }
        static Dictionary<string, int> gotCategories;
        static HashSet<string> gotArticles;
        static DBController dbController;
        static int artCnt = 0;
        static string artCategories = "";
        static string artCollection = "";
        static string DBpath = "";

        static void Main(string[] args)
        {
            var path = GetPath();
            artCnt = GetArtCount();
            artCategories = path + "articlesCategories.txt";
            artCollection = path + "articlesCollection.txt";
            DBpath = path + "articles.db";
            gotCategories = FileController.GetCollectionDicti(artCategories);
            gotArticles = FileController.GetHashSet(artCollection);
            dbController = new DBController(DBpath);

            var baseUrl = "https://lenta.ru/";           
            for (DateTime dt = DateTime.Now; dt.Year >= 2001; dt = dt.AddDays(-1))
            {
                Runner(baseUrl + dt.Year.ToString()
                    + "/" + String.Format("{0:d2}", dt.Month)
                    + "/" + String.Format("{0:d2}", dt.Day)
                    + "/");
            }
        }
    }
}
