using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
using RedRockPlayer.Model;
// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace RedRockPlayer
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HotSongsPage : Page
    {
        string tempString, tempUri = "&showapi_sign=4eecf2bdcab441f5a10c634ef43f4a5c&showapi_appid=19015";
        int tempid = 01;
        string songUri;
        public HotSongsPage()
        {
            this.InitializeComponent();
            GetList();

        }
        void GetList()
        {
            HttpGetJson("3");
            HttpGetJson("5");
            HttpGetJson("6");
            HttpGetJson("16");
            HttpGetJson("17");
            HttpGetJson("18");
            HttpGetJson("19");
            HttpGetJson("23");
            HttpGetJson("26");
        }
        List<HotSongsModel> temp_List = new List<HotSongsModel>();
        void HttpGetJson(string x)
        {
            HttpClient httpClient1 = new HttpClient();
            string uri = $"http://route.showapi.com/213-4?topid={x}" + tempUri;
            //httpClient1.DefaultRequestHeaders.Add("apikey", "46a3f59d544a4b264496c0b394485816");
            System.Net.Http.HttpResponseMessage response;
            response = httpClient1.GetAsync(new Uri(uri)).Result;
            if (response.StatusCode == HttpStatusCode.OK)
                tempString = response.Content.ReadAsStringAsync().Result;

            //什么鬼接口 fuck
            JObject jObject1 = (JObject)JsonConvert.DeserializeObject(tempString);
            string json1 = jObject1["showapi_res_body"].ToString();
            JObject jArray1 = (JObject)JsonConvert.DeserializeObject(json1);
            string json = jArray1["pagebean"].ToString();
            JObject jArray2 = (JObject)JsonConvert.DeserializeObject(json);
            string json2 = jArray2["songlist"].ToString();
            JArray jArray = (JArray)JsonConvert.DeserializeObject(json2);
            List<HotSongsModel> tempList = JsonConvert.DeserializeObject<List<HotSongsModel>>(jArray.ToString());
            temp_List = tempList;
            foreach (var item in tempList)
            {
                item.id = tempid.ToString();
                tempid++;
            }
            tempid = 1;
            foreach (var item in tempList)
            {
                if (item.singername == null)
                    item.singername = "未知歌手";
            }
            foreach (var item in tempList)
            {
                item.tag = "Collapsed";
            }
            switch (x)
            {
                case "3":
                    AmericanHotSongsList.ItemsSource = tempList;
                    break;
                case "5":
                    ChineseHotSongsList.ItemsSource = tempList;
                    break;
                case "6":
                    HKTWHotSongsList.ItemsSource = tempList;
                    break;
                case "16":
                    koreaHotSongsList.ItemsSource = tempList;
                    break;
                case "17":
                    JapanHotSongsList.ItemsSource = tempList;
                    break;
                case "18":
                    minyaoHotSongsList.ItemsSource = tempList;
                    break;
                case "19":
                    RockHotSongsList.ItemsSource = tempList;
                    break;
                case "23":
                    SaleHotSongsList.ItemsSource = tempList;
                    break;
                case "26":
                    HotSongsList.ItemsSource = tempList;
                    break;
            }
        }
        MyFavorite temp1 = new MyFavorite();

        private void downloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (temp1 != null)
            {
                using (var conn = AppDataBase.GetDbConnection())//连接数据库
                {
                    var dbFavorite = conn.Table<MyFavorite>();
                    foreach (var item in dbFavorite)
                    {
                        if (item.songsName == temp1.songsName)
                            firstTimeWrite = false;
                    }
                }
                if (firstTimeWrite)
                    using (var conn = AppDataBase.GetDbConnection())//连接数据库
                    {
                        var addSongs = new MyFavorite() { songsName = temp1.songsName, singerName = temp1.singerName, songsUri = temp1.songsUri, imgUri = temp1.imgUri, imgUriB = temp1.imgUriB, albumname = temp1.albumname, songid = temp1.songid };
                        var count = conn.Insert(addSongs);//将对象添加进表
                    }
                firstTimeWrite = true;               
            }
            temp.tag = "Collapsed";
        }
        HotSongsModel temp = new HotSongsModel();
        private void AmericanHotSongsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            temp = ((HotSongsModel)e.ClickedItem);
            //temp1 = ((MyFavorite)e.ClickedItem);
            ((HotSongsModel)e.ClickedItem).tag ="Visible";
            //songUri = temp.downUrl;
            //Frame.Navigate(typeof(MainPage),((HotSongsModel)e.ClickedItem));
        }
        bool firstTimeWrite = true;

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            using (var conn = AppDataBase.GetDbConnection())//连接数据库
            {
                foreach (var item in temp_List)
                { 
                var addSongs = new PlayerList() { songsName = item.songname, singerName = item.singername, songsUri = item.downUrl, imgUri = item.albumpic_small, imgUriB = item.albumpic_big, albumname = item.albumname, songid = item.songid, state = "off" };
                var count = conn.Insert(addSongs);//将对象添加进表
                }
            }
        }

        private void HKTWHotSongsList_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
             
        }

        //private void HotSongsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if(!firstTimeWrite)
        //    ((HotSongsModel)e.OriginalSource).tag = "Collapsed";
        //}

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            if (temp != null)
            {
                using (var conn = AppDataBase.GetDbConnection())//连接数据库
                {
                    var dbSongs = conn.Table<PlayerList>();
                    foreach (var item in dbSongs)
                    {
                        if (item.songsName == temp.songname)
                            firstTimeWrite = false;
                    }
                }
                if(firstTimeWrite)
                    using (var conn = AppDataBase.GetDbConnection())//连接数据库
                    {
                        var addSongs = new PlayerList() { songsName=temp.songname,singerName=temp.singername, songsUri=temp.downUrl, imgUri=temp.albumpic_small,imgUriB=temp.albumpic_big, albumname=temp.albumname, songid =temp.songid,state="off"};
                        var count = conn.Insert(addSongs);//将对象添加进表
                    }
                firstTimeWrite = true;
                
                temp.tag = "Collapsed";
                //player.Source = new Uri(songUri, UriKind.Absolute);
                //player.Play();
            }
        }
    }
}
