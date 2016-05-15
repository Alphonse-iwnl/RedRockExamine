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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace RedRockPlayer
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        bool Music = false, adminSong = true;
        int temp = 0, _temp, temp_ = 1, clickTimes;
        public ObservableCollection<HotSongsModel> playListModel = new ObservableCollection<HotSongsModel>();
        public MainPage()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                var state = "VisualState";
                var state1 = "VisualState1";
                if (e.NewSize.Width <= 700)
                    VisualStateManager.GoToState(this, state, true);
                else if (e.NewSize.Width >700)
                    VisualStateManager.GoToState(this, state1, true);
            }; 
      
        frame.Navigate(typeof(HotSongsPage));
            soundsSlider.Value = 100;
            //最后添加的一首歌为默认播放源
            GetLastSongsPic();
        }

        void GetPlayList()
        {
            playListModel.Clear();
            using (var conn = AppDataBase.GetDbConnection())
            {
                var dbSongs = conn.Table<PlayerList>();
                //foreach (var item in dbSongs)
                //{
                //    item.Id= temp_;
                //    temp_++;
                //}
                foreach (var item in dbSongs)
                {
                    playListModel.Add(new HotSongsModel { songname = item.songsName, singername = item.singerName, downUrl = item.songsUri,/* tag = "Collapsed" */tag = "Collapsed", id = item.Id.ToString(), albumpic_small = item.imgUri });
                }
            }
            foreach (var item in playListModel)
            {
                item.ID = temp_.ToString();
                temp_++;
            }
            temp_ = 1;

            PlayListFlyout.ItemsSource = playListModel;
        }
        void GetLastSongsPic()
        {
            using (var conn = AppDataBase.GetDbConnection())
            {
                var dbSongs = conn.Table<PlayerList>();
                foreach (var item in dbSongs)
                    if (item.Id > _temp)
                    {
                        _temp = item.Id;
                    }
            }
            using (var conn = AppDataBase.GetDbConnection())//连接数据库
            {
                var dbSongs = conn.Table<PlayerList>();
                foreach (var item in dbSongs)
                    if (item.Id == _temp)
                    {
                        _temp = item.Id;
                        if (item.imgUri != null)
                            SongsImg.Source = new BitmapImage(new Uri(item.imgUri));
                        //item.state = "on";
                        //conn.Update(item);
                    }
            }
        }
        void Del_GetLastSongsPic()
        {
            using (var conn = AppDataBase.GetDbConnection())
            {
                var dbSongs = conn.Table<PlayerList>();
                foreach (var item in dbSongs)
                    if (item.Id > _temp)
                    {
                        _temp = item.Id;
                    }
            }
            using (var conn = AppDataBase.GetDbConnection())//连接数据库
            {
                var dbSongs = conn.Table<PlayerList>();
                foreach (var item in dbSongs)
                    if (item.Id == _temp-1)
                    {
                        _temp = item.Id;
                        if (item.imgUri != null)
                            SongsImg.Source = new BitmapImage(new Uri(item.imgUri));
                        //item.state = "on";
                        //conn.Update(item);
                    }
            }
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HotMusic.IsSelected)
                this.frame.Navigate(typeof(HotSongsPage));
            if (FindMusic.IsSelected)
                this.frame.Navigate(typeof(SearchPage));
            if (LoveMusic.IsSelected)
                this.frame.Navigate(typeof(MyFavoritePage));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Temps.Visibility == Visibility.Visible)
                Temps.Visibility = Visibility.Collapsed;
            else
                Temps.Visibility = Visibility.Visible;
            MySpliteView.IsPaneOpen = !MySpliteView.IsPaneOpen;
        }

        private void MySpliteView_PaneClosed(SplitView sender, object args)
        {
            Temps.Visibility = Visibility.Collapsed;
        }

        private void SongsImg_Tapped(object sender, TappedRoutedEventArgs e)
        {
            frame.Navigate(typeof(SongsContent), _temp);
        }

        void GetLastSong()
        {
            using (var conn = AppDataBase.GetDbConnection())//连接数据库
            {
                var dbSongs = conn.Table<PlayerList>();
                foreach (var item in dbSongs)
                    if (item.Id > _temp)
                    {
                        _temp = item.Id;
                    }
            }
            using (var conn = AppDataBase.GetDbConnection())//连接数据库
            {
                var dbSongs = conn.Table<PlayerList>();
                foreach (var item in dbSongs)
                    if (item.Id == _temp)
                    {
                        _temp = item.Id;
                        player.Source = new Uri(item.songsUri, UriKind.Absolute);
                    }
            }

        }
        void Del_GetLastSong()
        {
            using (var conn = AppDataBase.GetDbConnection())//连接数据库
            {
                var dbSongs = conn.Table<PlayerList>();
                foreach (var item in dbSongs)
                    if (item.Id > _temp)
                    {
                        _temp = item.Id;
                    }
            }
            using (var conn = AppDataBase.GetDbConnection())//连接数据库
            {
                var dbSongs = conn.Table<PlayerList>();
                foreach (var item in dbSongs)
                    if (item.Id == _temp-1)
                    {
                        _temp = item.Id;
                        player.Source = new Uri(item.songsUri, UriKind.Absolute);
                    }
            }

        }
        private void soundsSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            player.Volume = soundsSlider.Value / 100;
        }

        private void playAndStopButton_Click(object sender, RoutedEventArgs e)
        {
            if (temp == 0 && adminSong)
            { GetLastSong(); temp++; }
            if (Music)
            {
                player.Pause();
                Music = false;
            }
            else
            {
                player.Play();
                Music = true;
            }
        }

        private void delButton_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = AppDataBase.GetDbConnection())
            {
                var dbSongs = conn.Table<PlayerList>();
                foreach (var item in dbSongs)
                {
                    if (item.songsName == tempModel.songname)
                    {
                        if (player.Source.AbsoluteUri == item.songsUri)
                        { player.Stop(); Del_GetLastSongsPic(); Del_GetLastSong(); }
                        conn.Delete(item);
                    }                   
                }
            }
            //ReValueId();
            tempModel.tag = "Collapsed";
            GetPlayList();
        }

        HotSongsModel tempModel = new HotSongsModel();

        private void PlayListFlyout_ItemClick(object sender, ItemClickEventArgs e)
        {
            tempModel = (HotSongsModel)e.ClickedItem;
            ((HotSongsModel)e.ClickedItem).tag = "Visible";
        }


        private void DelAllButton_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = AppDataBase.GetDbConnection())
            {
                conn.DeleteAll<PlayerList>();
            }
            GetPlayList();
        }
        int panduan = 0;
        private void player_MediaEnded(object sender, RoutedEventArgs e)
        {
            using (var conn = AppDataBase.GetDbConnection())
            {
                var dbSongs = conn.Table<PlayerList>();
                foreach (var item in dbSongs)
                {
                    if (item.songsUri == player.Source.AbsoluteUri)
                        panduan++;
                    else if (panduan == 1 && item.songsUri != null)
                    {
                        player.Source = new Uri(item.songsUri, UriKind.Absolute);
                        SongsImg.Source = new BitmapImage(new Uri(item.imgUri));
                        break;
                    }
                }
            }
        }
        MyFavorite temp1 = new MyFavorite();
        PlayerList temp2 = new PlayerList();
        string tempuri;
        //有毒
        //private void LoveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if(player.Source.AbsoluteUri!=null)
        //        tempuri = player.Source.AbsoluteUri;            
        //        using (var conn = AppDataBase.GetDbConnection())
        //        {
        //            var dbSongs = conn.Table<PlayerList>();
        //            foreach (var item in dbSongs)
        //                if (tempuri == item.songsUri)
        //                    temp2 = item;
        //        }

        //        using (var conn = AppDataBase.GetDbConnection())
        //        {
        //            //var dbFavorite = conn.Table<MyFavorite>();
        //            var newsongs = new MyFavorite() { songsName = temp2.songsName, singerName = temp2.singerName, imgUri = temp2.imgUri, imgUriB = temp2.imgUriB, Id = temp2.Id, albumname = temp2.albumname };
        //            var count = conn.Insert(newsongs);
        //        }
            
        //}

        //void ReValueId()
        //{using (var conn = AppDataBase.GetDbConnection())
        //    {
        //        var dbSongs = conn.Table<PlayerList>();
        //        foreach (var item in dbSongs)
        //        {
        //            item.Id = temp_;
        //            temp_++;
        //        }
        //    }
        //}
        private void mySongsButton_Click(object sender, RoutedEventArgs e)
        {
            //ReValueId();
            GetPlayList();
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            tempModel.tag = "Collapsed";
             
            //ReValueId();
            player.Source = new Uri(tempModel.downUrl, UriKind.Absolute);
            SongsImg.Source = new BitmapImage(new Uri(tempModel.albumpic_small));
            player.Play();
            _temp = int.Parse(tempModel.id);
            if (frame.CanGoBack)
                frame.Navigate(typeof(SongsContent), _temp);
        }

        private void lastSongButton_Click(object sender, RoutedEventArgs e)
        {
            adminSong = false;
            //using (var conn = AppDataBase.GetDbConnection())
            //{
            //    var dbSongs = conn.Table<PlayerList>();
            //    foreach (var item in dbSongs)
            //    {
            //        if (item.state == "on")
            //        {
            //            item.state = "off";
            //            conn.Update(item);
            //        }
            //    }
            //}
            using (var conn = AppDataBase.GetDbConnection())//连接数据库
            {
                var dbSongs = conn.Table<PlayerList>();
                foreach (var item in dbSongs)
                    if (item.Id == _temp - 1)
                    {
                        _temp = item.Id;
                        if (item.imgUri != null)
                        {
                            SongsImg.Source = new BitmapImage(new Uri(item.imgUri));
                            //item.state = "on";
                            //conn.Update(item);
                        }
                        player.Source = new Uri(item.songsUri, UriKind.Absolute);

                    }
            }
            //TODU:有点问题 暂时想不通
            if (frame.CanGoBack)
                frame.Navigate(typeof(SongsContent), _temp);

        }
        private void nextSongButton_Click(object sender, RoutedEventArgs e)
        {
            adminSong = false;
            //using (var conn = AppDataBase.GetDbConnection())
            //{
            //    var dbSongs = conn.Table<PlayerList>();
            //    foreach (var item in dbSongs)
            //    {
            //        if (item.state == "on")
            //        {
            //            item.state = "off";
            //            conn.Update(item);
            //        }
            //    }
            //}
            using (var conn = AppDataBase.GetDbConnection())//连接数据库
            {
                var dbSongs = conn.Table<PlayerList>();
                foreach (var item in dbSongs)
                    if (item.Id == _temp + 1)
                    {
                        _temp = item.Id;
                        if (item.imgUri != null)
                        {
                            SongsImg.Source = new BitmapImage(new Uri(item.imgUri));
                            //item.state = "on";
                            //conn.Update(item);
                        }
                        player.Source = new Uri(item.songsUri, UriKind.Absolute);
                        break;
                    }
            }
            if (frame.CanGoBack)
                frame.Navigate(typeof(SongsContent), _temp);
        }
        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{

        //}
    }
}
