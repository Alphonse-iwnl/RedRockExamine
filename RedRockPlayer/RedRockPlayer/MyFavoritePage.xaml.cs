using RedRockPlayer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace RedRockPlayer
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MyFavoritePage : Page
    {
        public MyFavoritePage()
        {
            this.InitializeComponent();
            GetMyFavoriteList();
        }
        ObservableCollection<MyFavorite> MyList = new ObservableCollection<MyFavorite>();
        void GetMyFavoriteList()
        {
            using (var conn = AppDataBase.GetDbConnection())
            {
                var dbFavorite = conn.Table<MyFavorite>();
                foreach (var item in dbFavorite)
                    MyList.Add(new MyFavorite { songsName = item.songsName, singerName = item.singerName, imgUri = item.imgUri, imgUriB = item.imgUriB, songsUri = item.songsUri, tag = "Collapsed" });
            }
            MyFavoriteList.ItemsSource = MyList;
        }
        MyFavorite temp = new MyFavorite();
        private void MyFavoriteList_ItemClick(object sender, ItemClickEventArgs e)
        {
            temp = ((MyFavorite)e.ClickedItem);
            ((MyFavorite)e.ClickedItem).tag = "Visible";
        }
        bool firstTimeWrite = true;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = AppDataBase.GetDbConnection())
            {
                var dbFavorite = conn.Table<MyFavorite>();
                var dbSongs = conn.Table<PlayerList>();
                foreach (var item in dbFavorite)
                {
                    var addPlayerList = new PlayerList() { songsName = item.songsName, singerName = item.singerName, songsUri = item.songsUri, imgUri = item.imgUri, imgUriB = item.imgUriB, albumname = item.albumname, songid = item.songid };
                    var count = conn.Insert(addPlayerList);//将对象添加进表
                }
            }
        }
    }
}
