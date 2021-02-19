using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiNews.Models
{
    public class News
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string NewsContent { get; set; }
        public string ImgPath { get; set; }
        public DateTime DateAdded { get; set; }
    }
}