using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MyControlLibrary
{
    public class JokeItem
    {
        private string nickName;
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName
        {
            get { return nickName; }
            set { nickName = value; }
        }

        private Image headImage;
        /// <summary>
        /// 头像
        /// </summary>
        public Image HeadImage
        {
            get { return headImage; }
            set { headImage = value; }
        }
        private string jokeContent;
        /// <summary>
        /// 笑话内容
        /// </summary>
        public string JokeContent
        {
            get { return jokeContent; }
            set { jokeContent = value; }
        }

        private string jokeUrl;
        /// <summary>
        /// 笑话地址
        /// </summary>
        public string JokeUrl
        {
            get { return jokeUrl; }
            set { jokeUrl = value; }
        }

        private Rectangle headImageRect;
        /// <summary>
        /// 头部图像区域
        /// </summary>
        public Rectangle HeadImageRect
        {
            get { return headImageRect; }
            set { headImageRect = value; }
        }

        private Rectangle nickNameRect;
        /// <summary>
        /// 昵称区域
        /// </summary>
        public Rectangle NickNameRect
        {
            get { return nickNameRect; }
            set { nickNameRect = value; }
        }

        private Rectangle jokeContentRect;
        /// <summary>
        /// 笑话正文区域
        /// </summary>
        public Rectangle JokeContentRect
        {
            get { return jokeContentRect; }
            set { jokeContentRect = value; }
        }
    }
}
