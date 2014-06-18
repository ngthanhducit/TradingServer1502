using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingServer.DBW
{
    public class DBWNews
    {
        private List<Business.News> MapNews(DS.NewsDataTable tbNews)
        {
            List<Business.News> result = new List<Business.News>();
            if (tbNews != null)
            {
                int count = tbNews.Count;
                for (int i = 0; i < count; i++)
                {
                    Business.News newNews = new Business.News();
                    newNews.Body = tbNews[i].Body;
                    newNews.Catetory = tbNews[i].Category;
                    newNews.DateCreated = tbNews[i].DateCreated;
                    newNews.ID = tbNews[i].ID;
                    newNews.Title = tbNews[i].Title;
                    result.Add(newNews);
                }
            }

            return result;
        }
        internal int InsertNews(string title,string body, DateTime timeAdd,string category)
        {
            int result = -1;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.NewsTableAdapter adap = new DSTableAdapters.NewsTableAdapter();
            try
            {
                adap.Connection = conn;
                conn.Open();
                result = int.Parse(adap.InsertNews(title, body, timeAdd, category).ToString());
            }
            catch (Exception ex)
            {
                result = -1;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result;
        }
        internal List<Business.News> GetTopNews()
        {
            List<Business.News> result = new List<Business.News>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DBConnection.DBConnection.Connection);
            DSTableAdapters.NewsTableAdapter adap = new DSTableAdapters.NewsTableAdapter();

            try
            {
                conn.Open();
                adap.Connection = conn;
                result = this.MapNews(adap.GetTopNews(10));
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                adap.Connection.Close();
                conn.Close();
            }

            return result;
        }
    }
}
