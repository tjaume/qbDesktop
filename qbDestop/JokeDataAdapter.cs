using MyControlLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace 获取糗事百科的笑话
{
    class JokeDataAdapter
    {
        const string qsbkMainUrl = "http://www.qiushibaike.com";


        /// <summary>
        /// 获取笑话列表
        /// </summary>
        /// <param name="htmlContent"></param>
        public static List<JokeItem> GetJokeList(int pageIndex)
        {
            string htmlContent = GetUrlContent(GetWBJokeUrl(pageIndex));
            List<JokeItem> jokeList = new List<JokeItem>();
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            HtmlNode rootNode = htmlDoc.DocumentNode;
            string xpathOfJokeDiv = "//div[@class='article block untagged mb15']";
            string xpathOfJokeContent = "./a/div[@class='content']/span";
            string xpathOfImg = "./div[@class='author clearfix']/a/img";
            try
            {
                HtmlNodeCollection jokeCollection = rootNode.SelectNodes(xpathOfJokeDiv);
                int jokeCount = jokeCollection.Count;
                JokeItem joke;
                foreach (HtmlNode jokeNode in jokeCollection)
                {
                    joke = new JokeItem();
                    HtmlNode contentNode = jokeNode.SelectSingleNode(xpathOfJokeContent);
                    if (contentNode != null)
                    {
                        joke.JokeContent = Regex.Replace(contentNode.InnerText, "(\r\n)+", "\r\n");
                    }
                    else
                    {
                        joke.JokeContent = "";
                    }
                    HtmlNode imgornameNode = jokeNode.SelectSingleNode(xpathOfImg);
                    if (imgornameNode != null)
                    {
                        joke.NickName = imgornameNode.GetAttributeValue("alt", "");
                        joke.HeadImage = GetWebImage("http:" + imgornameNode.GetAttributeValue("src", ""));
                        joke.HeadImage = joke.HeadImage != null ? new Bitmap(joke.HeadImage, 50, 50) : null;
                    }
                    else
                    {
                        joke.NickName = "匿名用户";
                        joke.HeadImage = null;
                    }
                    jokeList.Add(joke);
                }

            }
            catch { }
            return jokeList;
        }

        /// <summary>
        /// 根据糗事百科笑话页面索引获取笑话页的html源码
        /// </summary>
        /// <param name="pageIndex">页面索引</param>
        /// <returns></returns>
        private static string GetWBJokeUrl(int pageIndex)
        {
            StringBuilder url = new StringBuilder();
            url.Append(qsbkMainUrl);
            url.Append("/textnew/page/");
            url.Append(pageIndex.ToString());
            return url.ToString();
        }
        /// <summary>
        /// 根据网页的url获取网页的html源码
        /// </summary>
        /// <param name="url">网页链接</param>
        /// <returns></returns>
        private static string GetUrlContent(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Maxthon/4.4.8.1000 Chrome/30.0.1599.101 Safari/537.36";
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            catch { return null; }
        }

        private static Image GetWebImage(string webUrl)
        {
            try
            {
                Encoding encode = Encoding.GetEncoding("utf-8");//网页编码==Encoding.UTF8  
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri(webUrl));
                HttpWebResponse ress = (HttpWebResponse)req.GetResponse();
                Stream sstreamRes = ress.GetResponseStream();
                return System.Drawing.Image.FromStream(sstreamRes);
            }
            catch { return null; }
        }

    }
}
