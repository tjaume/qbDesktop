using MyControlLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace 获取糗事百科的笑话
{
    public partial class FrmMain : Form
    {
        private int currentIndex=1;
        private List <List<JokeItem >>jokePageList;
        public FrmMain()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<JokeItem> jokeList = JokeDataAdapter.GetJokeList(1);
            this.jokeListPanel1.AddItems(jokeList);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            Rectangle rMain = System.Windows.Forms.Screen.GetWorkingArea(this);
            int uiHieght=rMain.Height;
            this.Height =uiHieght ;
            this.jokeListPanel1.Height =this.ClientRectangle.Height;
            this.jokeListPanel1.Top = 0;
            this.jokeListPanel1.Left= (this.Width - 640) / 2;
            this.DesktopLocation = new Point((rMain.Width -this.Width)/2,0);

            //
            this.btnLeft.Left = 0;
            this.btnLeft.Top = (this.Height - 80) / 2;

            this.btnRight.Left = 85 + this.jokeListPanel1.Width;
            this.btnRight.Top = (this.Height - 80) / 2;

            //
            this.panel1.Location = this.jokeListPanel1.Location;
            this.panel1.Size = this.jokeListPanel1.Size;
            //
            this.loadBar.Location = new Point((this.panel1.Width - this.loadBar.Width) / 2, (this.panel1.Height - this.loadBar.Height) / 2);
            //
            this.lblStatus.Location = new Point(this.loadBar.Location.X + this.loadBar.Width + 5, this.loadBar.Location.Y + this.loadBar.Height / 2);
            //
            jokePageList = new List<List<JokeItem>>();
            CheckForIllegalCrossThreadCalls = false;
            //
            this.toolTip1.SetToolTip(this.btnLeft, currentIndex.ToString()+ "/35");
            this.toolTip1.SetToolTip(this.btnRight, currentIndex.ToString() + "/35");
            //
            this.panel1.Visible = true;
            this.loadBar.Start();
            this.btnLeft.Enabled = this.btnRight.Enabled = false;
            this.backgroundWorker1.RunWorkerAsync();
        }

      
        private void btnRight_Click(object sender, EventArgs e)
        {
            if (currentIndex + 1 > this.jokePageList.Count)
            {
                this.panel1.Visible = true;
                this.loadBar.Start();
                this.btnLeft.Enabled = this.btnRight.Enabled = false;
                currentIndex++;
                this.backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                currentIndex++;
                BindResult();
            }
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (currentIndex > 1)
            {
                currentIndex--;
                BindResult();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            jokePageList.Add(JokeDataAdapter.GetJokeList(currentIndex));
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.panel1.Visible = false;
            this.loadBar.Stop();
            this.btnRight.Enabled = this.btnLeft.Enabled = true;
            if (this.currentIndex <= 35)
            {
                BindResult();
            }
        }


        private void BindResult()
        {
            this.jokeListPanel1.ClearItems();
            this.jokeListPanel1.AddItems(jokePageList[currentIndex - 1]);
            this.Text = "糗百[" + currentIndex.ToString() + "/35]";
            this.toolTip1.SetToolTip(this.btnLeft, currentIndex.ToString() + "/35");
            this.toolTip1.SetToolTip(this.btnRight, currentIndex.ToString() + "/35");
            
        }

    }
}
