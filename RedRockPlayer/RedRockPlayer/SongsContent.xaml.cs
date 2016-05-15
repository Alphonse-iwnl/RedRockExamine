using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedRockPlayer.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace RedRockPlayer
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SongsContent : Page
    {

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int _temp = (int)e.Parameter;
            using (var conn = AppDataBase.GetDbConnection())//连接数据库
            {
                PlayerList pl = Resources["pl"] as PlayerList;
                var dbSongs = conn.Table<PlayerList>();
                foreach (var item in dbSongs)
                    if (item.Id == _temp)
                    {
                        pl.imgUriB = item.imgUriB;
                        // SongsPic.Source = new BitmapImage(new Uri(item.imgUriB));
                        if (item.songsName != null)
                            songsName.Text = item.songsName;
                        else
                            songsName.Text = "未知歌曲";
                        if (item.singerName != null)
                            singerName.Text = item.singerName;
                        else
                            singerName.Text = "未知歌手";
                        if (item.albumname != null)
                            alubmName.Text = item.albumname;
                        else
                            alubmName.Text = "未知专辑";

                        HttpClient httpClient1 = new HttpClient();
                        string uri = $"http://route.showapi.com/213-2?musicid={item.songid}&showapi_sign=4eecf2bdcab441f5a10c634ef43f4a5c&showapi_appid=19015";

                        System.Net.Http.HttpResponseMessage response;
                        response = httpClient1.GetAsync(new Uri(uri)).Result;
                        if (response.StatusCode == HttpStatusCode.OK)
                            tempString = response.Content.ReadAsStringAsync().Result;

                        JObject jObject = (JObject)JsonConvert.DeserializeObject(tempString);
                        string json = jObject["showapi_res_body"].ToString();
                        JObject jObject1 = (JObject)JsonConvert.DeserializeObject(json);
                        string json1 = jObject1["lyric_txt"].ToString();
                        json1 = json1.Trim();
                        for (int j = 0; j < 3; j++)
                        {
                            for (int i = 0; i < json1.Length; i++)
                            {
                                if (json1[i] == ' ' && json1[i + 1] == ' ')
                                    json1.Remove(i, 1);
                            }
                        }
                        for (int i = 0; i < json1.Length; i++)
                        {
                            if (json1[i] == ' ' && json1[i + 1] != ' ')
                                json1.Replace(json1[i], '\n');
                        }
                        lyricText.Text = json1;
                    }
            }
        }
     
        string tempString;
        public SongsContent()
        {
            this.InitializeComponent();
            
            //GetPlayingSongs();
        }
        //void GetPlayingSongs()
        //{
        //    // int _temp = (int)e.Parameter;
        //    using (var conn = AppDataBase.GetDbConnection())//连接数据库
        //    {
        //        PlayerList pl = Resources["pl"] as PlayerList;
        //        var dbSongs = conn.Table<PlayerList>();
        //        foreach (var item in dbSongs)
        //            if (item.state == "on")
        //            {
        //                pl.imgUriB = item.imgUriB;
        //                //SongsPic.Source = new BitmapImage(new Uri(item.imgUriB));
        //                if (item.songsName != null)
        //                    songsName.Text = item.songsName;
        //                else
        //                    songsName.Text = "未知歌曲";
        //                if (item.singerName != null)
        //                    singerName.Text = item.singerName;
        //                else
        //                    singerName.Text = "未知歌手";
        //                if (item.albumname != null)
        //                    alubmName.Text = item.albumname;
        //                else
        //                    alubmName.Text = "未知专辑";
        //            }
        //    }
        //}
    }
}
