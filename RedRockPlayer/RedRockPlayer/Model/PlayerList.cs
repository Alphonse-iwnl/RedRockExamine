using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedRockPlayer.Model
{
    public class PlayerList
    {
        [PrimaryKey]// 主键。
        [AutoIncrement]// 自动增长。
        public int Id
        {
            get;
            set;
        }
        public string songsName { get; set; }
        public string imgUri { get; set; }
        public string imgUriB { get; set; }
        public string singerName { get; set; }
        public string albumname { get; set; }
        public string songsUri { get; set; }
        public string songid { get; set; }
        public string state { get; set; }
    }
}
