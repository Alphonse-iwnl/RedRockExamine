using RedRockPlayer.Model;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace RedRockPlayer
{
    public static class AppDataBase
    {
        public readonly static string DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "PlayerListTest.db");

        public static SQLiteConnection GetDbConnection()
        {
            // 连接数据库，如果数据库文件不存在则创建一个空数据库。  
            var conn = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath);
            // 创建 BreakFast 模型对应的表，如果已存在，则忽略该操作。  
            conn.CreateTable<PlayerList>();
            conn.CreateTable<MyFavorite>();
            return conn;
        }
    }
}
