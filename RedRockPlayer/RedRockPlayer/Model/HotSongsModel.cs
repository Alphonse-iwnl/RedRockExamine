using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedRockPlayer.Model
{
    public class HotSongsModel : INotifyPropertyChanged
    {
        public string songname { get; set; }//歌曲名称

        public string id { get; set; } 
        public string ID { get; set; }//显示ID
        public string songid { get; set; }//歌曲id
        public string singername { get; set; }//歌手名字
        public string singerid { get; set; }//歌手id
        public string m4a { get; set; }//流媒体地址
        public string downUrl { get; set; }//下载地址
        public string albumpic_small { get; set; }//封面大图
        public string albumpic_big { get; set; }//封面小图
        public string albumname { get; set; }//专辑名字
        public string tag
        {
            get { return Tag; }
            set
            {
                Tag = value;
               RaisePropertyChanged("tag");
            }
        }
        private string Tag;

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
