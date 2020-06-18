using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication1
{
    public partial class MainTabMenuControl : UserControl
    {
        public MainTabMenuControl()
        {
            InitializeComponent();
           // this.tabControl1.Controls.Remove(this.moveTabPage);
           // this.moveTabPage = new TabPage();
            //this.tabControl1.Controls.Add(this.moveTabPage);
            this.Invalidate();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
