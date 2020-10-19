using System.Data.SQLite;
using System.IO;

namespace s2._3
{
    class DBController
    {
        SQLiteConnection DBConnection;
        string Path;
        public DBController(string path)
        {
            Path = path;
            if (!File.Exists(Path))
                SQLiteConnection.CreateFile(Path);
            DBConnection = new SQLiteConnection(@"DataSource=" + Path + "; Version=3;");
            DBConnection.Open();

            string commandText = "CREATE TABLE IF NOT EXISTS [articles] ( " +
                                 "[id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                                 "[head] TEXT NOT NULL, " +
                                 "[body] TEXT NOT NULL, " +
                                 "[date] TEXT NOT NULL, " +
                                 "[theme] TEXT NOT NULL)";       
            ExecuteCommand(commandText);
        }
        private void ExecuteCommand(string commandText = null, SQLiteCommand sQLiteCommand = null)
        {
            if (sQLiteCommand == null)
                sQLiteCommand = new SQLiteCommand(commandText, DBConnection);
            sQLiteCommand.ExecuteNonQuery();
        }
        public void ArticleInsert(string head, string body, string date, string theme)
        {
            string commandText = "INSERT INTO [articles] ([head], [body], [date], [theme]) VALUES (@head, @body, @date, @theme)";
            var command = new SQLiteCommand(commandText, DBConnection);
            command.Parameters.AddWithValue("@head", head);
            command.Parameters.AddWithValue("@body", body);
            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@theme", theme);
            ExecuteCommand(commandText, command);
        }
    }
}
