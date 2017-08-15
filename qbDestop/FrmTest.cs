using MyControlLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace 获取糗事百科的笑话
{
    public partial class FrmTest : Form
    {
        public FrmTest()
        {
            InitializeComponent();
        }

        private const int IMAGESIZE = 40;

        private int startXLaction = 0;
        private int nameXLaction = 0;
        private Image head;
        List<string> textList = null;
        string name = "韵律！";
        TextureBrush brush;

        SolidBrush brushNmae;
        SolidBrush brushContent;
        
        private void InitParams()
        {
            startXLaction = (this.Width - 600) / 2;
            nameXLaction = startXLaction +IMAGESIZE +10;
            string text = "博君一笑：\r\n妻子\r\n我今天拿药的时候遇到老严他说准备办出院手续，不想再治疗了，家里为给他治病借了很多的钱，再继续下去怕治不好给老婆孩子留下太多的债无法还清。\r\n\r\n丈夫\r\n就是你老和我说的那个和我差不多年龄的大学老师吗？\r\n\r\n妻子\r\n\r\n是的，说再治疗下去就要卖房了，所以，我还是想给你买份大病保险。\r\n\r\n丈夫\r\n不是说过很多次了嘛，买保险没啥必要，那是在花冤枉钱。\r\n\r\n妻子\r\n\r\n我经常去医院，看到有太多的人借钱都想买保险，我咨询过了，一旦生病，保险就买不了了，所以，趁现在还健康，赶紧把保险买好免得将来买不了了。\r\n\r\n丈夫\r\n我现在身体好好的，买啥保险啊，那些倒霉的事不会发生在我身上的。\r\n\r\n妻子\r\n\r\n那我问你五个问题吧。\r\n\r\n丈夫\r\n好，老婆有什么问题可以随\r\n便问哈。\r\n\r\n妻子\r\n第一个问题：现在环境污染那么严重，咱们经常能从微信上，电视，报纸上看到得癌症的人也是越来越多，你能确认你一定不会得病吗？\r\n\r\n丈夫\r\n人吃五谷杂粮怎能不得病，我当然也不例外。\r\n\r\n妻子\r\n\r\n第二个问题：得病了就要治，是不是要花咱们的存款？\r\n\r\n丈夫\r\n那是自然啊，谁能替咱们花这钱。\r\n\r\n妻子\r\n\r\n第三个问题：你现在收入高是因为你的奖金提成高，一旦你生病不能上班了，你还能挣那么多钱吗？\r\n\r\n丈夫\r\n如果那样收入要缩减2/3，只能拿基本工资了。\r\n\r\n妻子\r\n\r\n第四个问题：如果真是那样，咱家的房贷能还上吗？还能保证你妈每个月治病的开销？还有儿子上学的费用吗？这还没算家里的开销呢？\r\n\r\n丈夫\r\n是啊，我要只拿基本工资的话，房贷肯定是还不上了，没准看病的钱都不够呢，想想也确实可怕！\r\n\r\n妻子\r\n\r\n第五个问题：真要那样，咱家三口就要坐吃山空只能花存款了，你觉得咱们能花几年呢？那时你妈治病要花钱，你也要治病花钱，真要存款都花没了，家里两个病人你让我一个人怎么办啊？最后咱们的房子估计都保不住了，倒霉的还不是咱们全家人吗？大人苦点无所谓，难道你忍心还把咱们的儿子也搭进去吗？\r\n\r\n丈夫\r\n。。。。。真是太可怕了，看来保险是必须要买了，还是老婆想的周全，那明天咱们就去买。";
            if (Regex.IsMatch(text, @"(\r\n)+"))
            { 
            
            }
           // text = Regex.Replace(text, @"(\r\n)+", "\r\n");
            textList = GetStringRows(this.CreateGraphics(), new Font("宋体", 12), text, 600);

            head = new Bitmap(Image.FromFile("01.jpg"), IMAGESIZE, IMAGESIZE);

            brush = new TextureBrush(head,new Rectangle(0,0,IMAGESIZE,IMAGESIZE));
            brushNmae = new SolidBrush(Color.FromArgb(155, 136, 120));
            brushContent = new SolidBrush(Color.FromArgb(51,51,51));
            brush.TranslateTransform(startXLaction, 50); 
        }

        /// <summary>
        /// 计算字符串，以600px像素的宽度，看需要分为几行
        /// </summary>
        /// <param name="graphic">测量画布</param>
        /// <param name="font">字体大小</param>
        /// <param name="text">文字</param>
        /// <param name="width"></param>
        /// <returns></returns>
        private List<string> GetStringRows(Graphics graphic, Font font, string text, int width)
        {
            int RowBeginIndex = 0;
            int rowEndIndex = 0;
            int textLength = text.Length;
            List<string> textRows = new List<string>();
           
            for (int index = 0; index < textLength; index++)
            {
                rowEndIndex = index;

                if (index == textLength - 1)
                {
                    textRows.Add(text.Substring(RowBeginIndex));
                }
                else if (rowEndIndex + 1 < text.Length && text.Substring(rowEndIndex, 2) == "\r\n")//碰到了\r\n
                {
                    if (rowEndIndex == RowBeginIndex)
                    {
                        textRows.Add("");//绘制空行
                    }
                    else
                    {
                        textRows.Add(text.Substring(RowBeginIndex, rowEndIndex - RowBeginIndex));
                    }
                    index ++;
                    RowBeginIndex = rowEndIndex+2;
                }
                else if (graphic.MeasureString(text.Substring(RowBeginIndex, rowEndIndex - RowBeginIndex + 1), font).Width > width)
                {
                    textRows.Add(text.Substring(RowBeginIndex, rowEndIndex - RowBeginIndex));
                    RowBeginIndex = rowEndIndex;
                }
            }
            graphic.Dispose();
            //StringBuilder sb = new StringBuilder();
            //foreach (string s in textRows)
            //{
            //    sb.Append(s + "\r\n");
            //}
            //string b= sb.ToString().Substring(0, sb.Length - 2);
            return textRows;
        }

         private void Form1_Paint(object sender, PaintEventArgs e)
         {
             
             Graphics g = e.Graphics;
             g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
             g.FillRectangle(Brushes.White, new Rectangle(startXLaction-20,0,640,this.Height));
             
             g.FillEllipse(brush, new Rectangle(startXLaction, 50, IMAGESIZE, IMAGESIZE));

             brush.ResetTransform();
             g.DrawString(name, new Font("宋体", 10), brushNmae, new Point(nameXLaction, 65));
             int rowWidth=28;
             int startY = 110;
             g.DrawLine(Pens.Black, new Point(startXLaction, 95), new Point(startXLaction + 600, 95));
             foreach (string s in textList)
             {
                 g.DrawString(s, new Font("Microsoft YaHei", 12), Brushes.Black, new Point(startXLaction, startY));
                 startY += rowWidth;
             }
             //g.DrawString(GetTransformText("我是陕西人，我们这打麻将把自摸叫抠了，昨晚上我们三女的一男的打牌，一把牌，\r\n一女的自摸了，说我这么好的牌，都没人打的叫我胡，还要叫我自己揭，那男的说，那就是我们最后把你逼抠了，我说：不许说流氓话。瞬间笑倒一片。"), new Font("Microsoft YaHei", 12), Brushes.Black, new Point(startXLaction, startY));
         }

         private void Form1_Load(object sender, EventArgs e)
         {
             InitParams();
             this.Invalidate();
         }
         List<JokeItem> jokeList;
         private void button1_Click(object sender, EventArgs e)
         {
             jokeList = JokeDataAdapter.GetJokeList(2);
             textList = GetStringRows(this.CreateGraphics(), new Font("宋体", 12), jokeList[i].JokeContent, 600);
             name = jokeList[i].NickName;
             if (jokeList[i].HeadImage != null)
             {
                 head = new Bitmap(jokeList[i].HeadImage, IMAGESIZE, IMAGESIZE);
                 brush = new TextureBrush(head, new Rectangle(0, 0, IMAGESIZE, IMAGESIZE));
             }
                 brush.TranslateTransform(startXLaction, 50);
             
             this.Invalidate();
         }



         /// <summary>
         /// 控件大小固定为600px,由此计算笑话内容的高度。
         /// </summary>
         /// <param name="graphic">画布</param>
         /// <param name="font">字体</param>
         /// <param name="text">颜色</param>
         /// <returns></returns>
         private string GetTransformText(string text)
         {
             int width = 600;//默认控件大小是600；
             Font font = new Font("Microsoft YaHei", 12);
             Graphics graphic = this.CreateGraphics();
             int RowBeginIndex = 0;
             int rowEndIndex = 0;
             int textLength = text.Length;
             text = text.Replace("<br/>", "\r\n");
             List<string> textRows = new List<string>();

             for (int index = 0; index < textLength; index++)
             {
                 rowEndIndex = index;

                 if (index == textLength - 1)
                 {
                     textRows.Add(text.Substring(RowBeginIndex));
                 }
                 else if (rowEndIndex + 1 < text.Length && text.Substring(rowEndIndex, 2) == "\r\n")//碰到换行
                 {
                     textRows.Add(text.Substring(RowBeginIndex, rowEndIndex - RowBeginIndex));
                     rowEndIndex = index += 2;
                     RowBeginIndex = rowEndIndex;
                 }
                 else if (graphic.MeasureString(text.Substring(RowBeginIndex, rowEndIndex - RowBeginIndex + 1), font).Width > width)
                 {
                     textRows.Add(text.Substring(RowBeginIndex, rowEndIndex - RowBeginIndex));
                     RowBeginIndex = rowEndIndex;
                 }
             }
             graphic.Dispose();
             StringBuilder sb = new StringBuilder();
             foreach (string s in textRows)
             {
                 sb.Append(s + "\r\n");
             }
             return sb.ToString().Substring(0, sb.Length - 2);//返回最终字符
         }

         int i = 0;
         private void button2_Click(object sender, EventArgs e)
         {
             i++;
             if (i == jokeList.Count)
             {
                 i = 0;
             }
             textList = GetStringRows(this.CreateGraphics(), new Font("宋体", 12), jokeList[i].JokeContent, 600);
             name = jokeList[i].NickName;


             if (jokeList[i].HeadImage != null)
             {
                 head = new Bitmap(jokeList[i].HeadImage, IMAGESIZE, IMAGESIZE);
                 brush = new TextureBrush(head, new Rectangle(0, 0, IMAGESIZE, IMAGESIZE));
             }
             brush.TranslateTransform(startXLaction, 50);
             this.Invalidate();
         }

         private void button3_Click(object sender, EventArgs e)
         {
             MessageBox.Show(JokeDataAdapter.GetJokeList(1).Count .ToString ());
         }
       
    }
}
