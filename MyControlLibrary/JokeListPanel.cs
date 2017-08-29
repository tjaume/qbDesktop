using MyControlLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace MyControlLibrary
{
    public enum ControlStatus
    { 
        Normal,
        Hover,
        Down,
    }
    public partial class JokeListPanel :Control
    {
        public JokeListPanel()
            : base()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.ResizeRedraw, true);//设置样式控件样式风格

            this.UpdateStyles();//
            //把控件的宽度设置为不可更改
            this.MinimumSize = new System.Drawing.Size(620, 0);
            this.MaximumSize = new System.Drawing.Size(620,10000);
            defaultHeadImage =new Bitmap (DefaultHeadImage ,HEADIMAGESIZE ,HEADIMAGESIZE);
            //设置初始大小
            this.Size = new System.Drawing.Size(620, 50);
            this.BackColor = Color.White;
            jokeContentColor = Color.FromArgb(51, 51, 51);
            nickNameColor = Color.FromArgb(155, 136, 120);
            
            scrollColor = Color.FromArgb(226, 226, 224);
            sliderNormalColor = Color.FromArgb(202, 202, 202);
            sliderHoverColor = Color.FromArgb(135, 135, 134);
            nickNameFont = new Font("宋体", 10);
            jokeContentFont = new Font("Microsoft YaHei", 12);

            sliderStatus = ControlStatus.Normal;

            jokeItems = new List<JokeItem>();
        }

        private const int HEADIMAGESIZE = 50;//图片大小
        private const int CONTENTROWHEIGHT = 28;//行高
        private const int CONTENTWIDTH = 600;//内容的宽度

        private Color scrollColor;//滚动条的背景颜色
        private Color jokeContentColor;//笑话内容文本颜色
        private Color sliderNormalColor;//正常状态下或者是单击的滑块颜色
        private Color sliderHoverColor;//鼠标进入滑块时的颜色
        private Color nickNameColor;//昵称文本颜色
     
        private Font nickNameFont;//昵称文体
        private Font jokeContentFont;//笑话内容字体



        //笑话数据源
        private List<JokeItem> jokeItems;

        //是否绘制滚动条
        bool isDrawSlider=false;

        //滚动条
        Rectangle scrollBarRect;

        //滑块
        Rectangle sliderRect;

        //鼠标按下，记录滑块的位置
        Point sliderDownPoint;

        ControlStatus sliderStatus;//滑块状态

        int contentHight;//空间内容的高度

        int sliderValue;
        /// <summary>
        /// 滑块的位置
        /// </summary>
        private int SliderValue
        {
            get {
                return sliderValue;
            }
            set {

                if (this.sliderValue == value) return;
                if (value <= 0)
                {
                    this.sliderValue = 0;
                }
                else if (value >= this.contentHight - this.Height)
                {
                    this.sliderValue = this.contentHight - this.Height;
                }
                else
                {
                    this.sliderValue = value;
                }
                this.Invalidate();
            }
        }

        private Image defaultHeadImage=Resource.anony;
        /// <summary>
        /// 默认的头像图标 
        /// </summary>
        public Image DefaultHeadImage
        {
            get { return defaultHeadImage; }
            set { defaultHeadImage = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TranslateTransform(0, -SliderValue);//根据滑块的值设置绘制的原点坐标
            g.SmoothingMode = SmoothingMode.AntiAlias;//消除锯齿
            Rectangle rectList = new Rectangle(10, 20, 580, 20);
            Rectangle rectImageHead = new Rectangle();
            Rectangle rectnickName = new Rectangle();
            Rectangle rectJokeContent = new Rectangle();

            SolidBrush nickNameBrush = new SolidBrush(nickNameColor);
            SolidBrush jokeContentBrush = new SolidBrush(jokeContentColor);
            TextureBrush headImageBrush = null;
            try
            {
                foreach (JokeItem item in jokeItems)
                {
                    //绘制头部图像
                    if (item.HeadImage != null)
                    {
                        headImageBrush = new TextureBrush(item.HeadImage);

                    }
                    else
                    {
                        headImageBrush = new TextureBrush(defaultHeadImage);
                    }
                    headImageBrush.TranslateTransform(rectList.Left, rectList.Top);
                    rectImageHead = new Rectangle(rectList.Location, new Size(HEADIMAGESIZE, HEADIMAGESIZE));
                    g.FillEllipse(headImageBrush, rectImageHead);
                    headImageBrush.ResetTransform();//重置
                    item.HeadImageRect = rectImageHead;

                    //绘制昵称
                    rectnickName = Rectangle.Truncate(new RectangleF(new PointF(rectImageHead.Right + 10, rectList.Top + 20), g.MeasureString(item.NickName, nickNameFont)));
                    g.DrawString(item.NickName, nickNameFont, nickNameBrush, rectnickName.Location);
                    item.NickNameRect = rectnickName;
                    //绘制内容
                    rectList.Y = rectList.Top + HEADIMAGESIZE + 10;//空白区域
                    rectJokeContent.Location = rectList.Location;
                    List<string> lines = GetStringRows(g, jokeContentFont, item.JokeContent, CONTENTWIDTH);//获取文本需要绘制每行的文本
                    foreach (string line in lines)
                    {
                        g.DrawString(line, jokeContentFont, jokeContentBrush, rectList.Location);
                        rectList.Y = rectList.Top + CONTENTROWHEIGHT;
                    }
                    //绘制空白
                    rectList.Y = rectList.Top + 40;
                }
                //恢复坐标原点
                g.ResetTransform();
                //获取控件内容的高度
                this.contentHight = rectList.Bottom;
                if (this.contentHight > this.Height)//绘制滚动条
                {
                    isDrawSlider = true;
                    SolidBrush scrollBrush = new SolidBrush(scrollColor);
                    SolidBrush sliderBrush;
                    //判断滑块的绘制的颜色
                    switch (sliderStatus)
                    {
                        case ControlStatus.Down:
                            sliderBrush = new SolidBrush(sliderNormalColor);
                            break;
                        case ControlStatus.Normal:
                            sliderBrush = new SolidBrush(sliderNormalColor);
                            break;
                        case ControlStatus.Hover:
                            sliderBrush = new SolidBrush(sliderHoverColor);
                            break;
                        default:
                            sliderBrush = new SolidBrush(sliderNormalColor);
                            break;
                    }
                    try
                    {
                        //绘制滚动条
                        scrollBarRect = new Rectangle(this.Width - 10, 0, 10, this.Height);
                        g.FillRectangle(scrollBrush, scrollBarRect);

                        //绘制滑块
                        //计算滑块的位置和大小，初始化滑块矩形。每个像素代表
                        sliderRect.Width = scrollBarRect.Width;
                        sliderRect.Height = this.Width * 2 / contentHight;
                        if (sliderRect.Height < 50)
                        {
                            sliderRect.Height = 50;
                        }
                        sliderRect.X = scrollBarRect.X;
                        sliderRect.Y = SliderValue * (this.Height - sliderRect.Height) / (contentHight - this.Height);
                        g.FillRectangle(sliderBrush, sliderRect);
                    }
                        finally//释放资源
                    {
                        scrollBrush.Dispose();
                        sliderBrush.Dispose();
                    }
                }
                else//否则没有绘制滚动条
                {

                    isDrawSlider = false;
                    this.SliderValue = 0;//恢复绘制的坐标原点为（0,0）
                }
            }
            finally //释放资源
            {
                nickNameBrush.Dispose();
                jokeContentBrush.Dispose();

                if (headImageBrush != null)
                {
                    headImageBrush.Dispose();
                }
            }
            base.OnPaint(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (isDrawSlider)
            {
                if (e.Delta > 0)
                    this.SliderValue -= 50;
                if (e.Delta < 0)
                    this.SliderValue += 50;
            }
            base.OnMouseWheel(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (isDrawSlider)
            {
                if (sliderStatus == ControlStatus.Down)//拖动滑块
                {
                    int diffY = e.Location.Y - sliderDownPoint.Y;
                    this.SliderValue = this.SliderValue + diffY * (contentHight - this.Height) / (this.Height - sliderRect.Height);
                    this.sliderDownPoint = e.Location;
                    return;
                }
                if (sliderRect.Contains(e.Location))//鼠标进入滑块
                {
                    if (sliderStatus != ControlStatus.Hover)
                    {
                        sliderStatus = ControlStatus.Hover;
                        this.Invalidate();//重绘
                    }

                }
                else
                {
                    if (sliderStatus != ControlStatus.Normal)
                    {
                        sliderStatus = ControlStatus.Normal;
                        this.Invalidate();//重绘
                    }
                }
            }
            base.OnMouseMove(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (isDrawSlider)
            {
                if (e.Button == MouseButtons.Left)//左键按下
                {
                    if (sliderRect.Contains(e.Location))
                    {
                        if (sliderStatus != ControlStatus.Normal)
                        {
                            sliderStatus = ControlStatus.Down;
                            sliderDownPoint = e.Location;
                            this.Invalidate();//重绘
                        }
                    }
                    else if (scrollBarRect.Contains(e.Location))
                    {
                        if (e.Location.Y < sliderRect.Y)
                        {
                            //更改滑块的value值
                            this.SliderValue = e.Location.Y * (contentHight - this.Height) / (this.Height - sliderRect.Height);
                        }
                        else
                        {
                            this.SliderValue = (e.Location.Y - sliderRect.Height) * (contentHight - this.Height) / (this.Height - sliderRect.Height);
                        }
                    }

                }
            }
            this.Focus();
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (isDrawSlider)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (sliderRect.Contains(e.Location))//
                    {
                        sliderStatus = ControlStatus.Hover;
                        this.Invalidate();
                    }
                    else
                    {
                        if (sliderStatus != ControlStatus.Normal)
                        {
                            sliderStatus = ControlStatus.Normal;
                            this.Invalidate();
                        }
                    }
                }
            }
            base.OnMouseUp(e);
        }


        protected override void OnMouseLeave(EventArgs e)
        {
            if (isDrawSlider)
            {
                if (sliderStatus != ControlStatus.Normal)//是从滑块移动出界的，重绘。
                {
                    sliderStatus = ControlStatus.Normal;
                    this.Invalidate();
                }
            }

            base.OnMouseLeave(e);
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
                    index++;
                    RowBeginIndex = rowEndIndex + 2;
                }
                else if (graphic.MeasureString(text.Substring(RowBeginIndex, rowEndIndex - RowBeginIndex + 1), font).Width > width)
                {
                    textRows.Add(text.Substring(RowBeginIndex, rowEndIndex - RowBeginIndex));
                    RowBeginIndex = rowEndIndex;
                }
            }
            return textRows;
        }

        //对外接口
        public void AddItem(JokeItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("参数不能为空！");
            }
            else
            {
                this.jokeItems.Add(item);
                this.Invalidate();
            }
        }

        public void AddItems(List<JokeItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("参数不能为空！");
            }
            else
            {
                this.jokeItems.AddRange(items);
                this.Invalidate();
            }
        }

        public void ClearItems()
        {
            this.jokeItems.Clear();
            this.SliderValue = 0;
            this.Invalidate();
        }

        public void RemoveItem(JokeItem item)
        {
            this.jokeItems.Remove(item);
            this.Invalidate();
        }

        public int ItemsSize()
        {
            return this.jokeItems.Count;
        }

      
    }
}
