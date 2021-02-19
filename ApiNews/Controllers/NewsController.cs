using ApiNews.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ApiNews.Controllers
{
    public class NewsController : ApiController
    {

        public string[] imgPaths = {
            "https://www.start-business-online.com/images/article_manager_uploads/blog.jpg",
                "https://openparachute.files.wordpress.com/2018/03/blog-feb.jpg",
                "https://techmeetups.com/wp-content/uploads/2019/06/blogging-SMB.png",
                "https://www.jennyboonewebstudio.com/wp-content/uploads/2018/01/blogStockPic.jpg",
                "https://introvertdear.com/wp-content/uploads/2019/07/why-INFJs-should-start-a-blog-770x470.jpg",
                "https://www.dalmatia.hr/edukacija/wp-content/uploads/2018/03/blogger-1200x630.jpg",
                "https://www.ning.com/blog/wp-content/uploads/2017/04/make-money-blogging.jpg",
                "https://www.telekom.com/resource/image/530616/landscape_ratio2x1/3000/1500/c3e6730fcd3f9223420a2ab53db0ede4/po/bi-blogschmuckbild-en.jpg",
                "https://moxafive.weebly.com/uploads/1/2/4/2/124201810/643831682.png",
                "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSfdsu9ycPYHaao9fQgr4pKp6e3tJrKRYtryQ&usqp=CAU"
        };
          
        public HttpResponseMessage Get()
        {
            string query = @"select ID, Title, NewsContent, ImgPath, DateAdded from dbo.News";

            DataTable table = new DataTable();
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            using (var cmd = new SqlCommand(query, conn))

            using (var adapter = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                adapter.Fill(table);
            }

            return Request.CreateResponse(HttpStatusCode.OK, table);
        }
    
        [HttpPost]
        public string AddNew(News news)
        {

            try
            {
                string query = @"insert into dbo.News (Title, NewsContent, ImgPath, DateAdded) values (@Title, @NewsContent, @ImgPath, @DateAdded)";

                DateTime added = DateTime.Now;

                DataTable table = new DataTable();
                SqlDataReader reader;
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                    using(var cmd = new SqlCommand(query, conn))
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    Random rand = new Random();
                    // Generate a random index less than the size of the array.  
                    int index = rand.Next(imgPaths.Length);

                    string imgPath = imgPaths[index];

                   

                    conn.Open();
                    cmd.Parameters.AddWithValue("@title", news.Title);
                    cmd.Parameters.AddWithValue("@NewsContent", news.NewsContent);
                    cmd.Parameters.AddWithValue("@ImgPath", imgPath);
                    cmd.Parameters.AddWithValue("@DateAdded", added);
                    cmd.CommandType = CommandType.Text;
                    
                    reader = cmd.ExecuteReader();
                    table.Load(reader);

                    reader.Close();
                    conn.Close();
                }
                return "Added Successfully!";
                

                
            }
            catch (Exception ex)
            {
                
                return "Problem occured! Failed to add new! Try again." + ex.Message;
            }
        }
        [HttpPut]
        public string EditNew(News news)
        {
            try
            {
                string query = @"update dbo.News set Title=@Title, NewsContent=@NewsContent, ImgPath=@ImgPath where ID=@ID";

                

                DataTable table = new DataTable();
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                using (var cmd = new SqlCommand(query, conn))
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    string imgPath = "https://www.start-business-online.com/images/article_manager_uploads/blog.jpg";
                    
                    cmd.Parameters.AddWithValue("@ID", news.ID);
                    cmd.Parameters.AddWithValue("@title", news.Title);
                    cmd.Parameters.AddWithValue("@NewsContent", news.NewsContent);
                    cmd.Parameters.AddWithValue("@ImgPath", imgPath);
                    cmd.CommandType = CommandType.Text;
                    adapter.Fill(table);
                }
                return "Edited Successfully!";


            }
            catch (Exception ex)
            {

                return "Problem occured! Failed to edit new! Try again." + ex.Message;
            }
        }
        [HttpDelete]
        public string DeleteNew(int id)
        {
            try
            {
                string query = @"delete from dbo.News where ID = " + id + @"";
                DateTime added = DateTime.Now;
                DateTime edited = DateTime.Now;

                DataTable table = new DataTable();
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                using (var cmd = new SqlCommand(query, conn))
                using (var adapter = new SqlDataAdapter(cmd))
                {
                   
                    cmd.CommandType = CommandType.Text;
                    adapter.Fill(table);
                }
                return "Deleted Successfully!";


            }
            catch (Exception)
            {

                return "Problem occured! Failed to delete new! Try again.";
            }
        }

      
    }
}
