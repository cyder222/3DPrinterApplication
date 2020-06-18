using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Jusin.ThreeDLib.ModelBase;
namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //this.TransparencyKey = Color.FromArgb(255,255,255,255);
            this.ContextMenuStrip = this.contextMenuStrip1;
            Vector3d eye = new Vector3d(0.0,-200.0,0.0);
            Vector3d center = new Vector3d(0.0,0.0,0.0);
            Vector3d up = new Vector3d(0.0,0.0,1.0);
            this.userControl11.setCamera(eye,
                center,
                up);
            this.open3DFileDialog1.FileOk +=new CancelEventHandler(open3DFileDialog1_FileOk);
           // this.contextMenuStrip1 = new ContextMenus.ModelSelectMenu(this);
            
          
        }
        private void open3DFileDialog1_FileOk(object sender, EventArgs e)
        {
            this.userControl11.changeModel(open3DFileDialog1.FileName);
        }
        private void userControl11_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
           // ViewPort1.Size = new System.Drawing.Size(this.ClientSize.Width, this.ClientSize.Height);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
          
        }

        private void ファイルToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 新規ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenu_ItemClick(object sender, EventArgs e)
        {
            this.open3DFileDialog1.ShowDialog();
        }

        private void closeToolStipMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.contextMenuStrip1.Show();
        }

        private void userControl11_Load_1(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void property_tab_Click(object sender, EventArgs e)
        {

        }

       

      
    }
}