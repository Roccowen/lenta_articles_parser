using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

namespace s2._3
{
    class FileController
    {
        private static void FileCheck(string path)
        {
            if (!File.Exists(path))
                File.Create(path).Close();
        }
        public static void AppendLine(string pathToFile, string line)
        {
            FileController.FileCheck(pathToFile);            
            var writer = new StreamWriter(pathToFile, append: true);
            writer.WriteLine(line);
            writer.Close();
            writer.Dispose();
        }
        public static void RewriteDictionary(string pathToFile, Dictionary<string, int> collection)
        {
            FileController.FileCheck(pathToFile);
            var writer = new StreamWriter(pathToFile);
            foreach (var item in collection)
                writer.WriteLine("{0} {1}", item.Key, item.Value);           
            writer.Close();
            writer.Dispose();
        }
        public static HashSet<string> GetHashSet(string pathToCollect)
        {
            FileController.FileCheck(pathToCollect);
            var reader = new StreamReader(pathToCollect);           
            var collection = reader.ReadToEnd().Split().ToHashSet<string>();
            reader.Close();
            reader.Dispose();
            return collection;
        }
        public static Dictionary<string,int> GetDictionary(string pathToFile)
        {
            FileController.FileCheck(pathToFile);
            var reader = new StreamReader(pathToFile);
            var collection = new Dictionary<string, int>();
            string line;
            while ((line = reader.ReadLine()) != null)
                collection.Add(line.Split()[0], Convert.ToInt32(line.Split()[1]));
            reader.Close();
            reader.Dispose();
            return collection;
        }
    }
}
