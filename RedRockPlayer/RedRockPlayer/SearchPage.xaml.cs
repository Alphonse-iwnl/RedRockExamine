using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedRockPlayer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class SearchPage : Page
    {
        string tempUri = "&showapi_sign=4eecf2bdcab441f5a10c634ef43f4a5c&showapi_appid=19015", tempString, x;
        int tempid = 1;
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text != "")
                x = SearchBox.Text;
            else if (SearchBox.Text == "")
            {
                string msg = "请输入搜索内容~";
                await new MessageDialog(msg).ShowAsync();
            }
            HttpGetJson(x);
        }
        bool firstTimeWrite = true;

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
                if (firstTimeWrite)
                    using (var conn = AppDataBase.GetDbConnection())//连接数据库
                    {
                        var addSongs = new PlayerList() { songsName = temp.songname, singerName = temp.singername, songsUri = temp.downUrl, imgUri = temp.albumpic_small, imgUriB = temp.albumpic_big, albumname = temp.albumname, songid = temp.songid, state = "off" };
                        var count = conn.Insert(addSongs);//将对象添加进表
                    }
                firstTimeWrite = true;

                temp.tag = "Collapsed";
                //player.Source = new Uri(songUri, UriKind.Absolute);
                //player.Play();
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
        private void SearchList_ItemClick(object sender, ItemClickEventArgs e)
        {
            temp = ((HotSongsModel)e.ClickedItem);
            ((HotSongsModel)e.ClickedItem).tag = "Visible";
        }

        public SearchPage()
        {
            this.InitializeComponent();
             
        }
        void HttpGetJson(string x)
        {
            HttpClient httpClient1 = new HttpClient();
            string uri = $"http://route.showapi.com/213-1?keyword={x}" + tempUri;
       
            System.Net.Http.HttpResponseMessage response;
            response = httpClient1.GetAsync(new Uri(uri)).Result;
            if (response.StatusCode == HttpStatusCode.OK)
                tempString = response.Content.ReadAsStringAsync().Result;

            JObject jObject1 = (JObject)JsonConvert.DeserializeObject(tempString);
            string json1 = jObject1["showapi_res_body"].ToString();
            JObject jArray1 = (JObject)JsonConvert.DeserializeObject(json1);
            string json = jArray1["pagebean"].ToString();
            JObject jArray2 = (JObject)JsonConvert.DeserializeObject(json);
            string json2 = jArray2["contentlist"].ToString();
            JArray jArray = (JArray)JsonConvert.DeserializeObject(json2);
            List<HotSongsModel> tempList = JsonConvert.DeserializeObject<List<HotSongsModel>>(jArray.ToString());
            foreach (var item in tempList)
            {
                item.id = tempid.ToString();
                tempid++;
            }
            foreach (var item in tempList)
            {
                item.tag = "Collapsed";
            }
            SearchList.ItemsSource = tempList;
             
        }
    }
}
