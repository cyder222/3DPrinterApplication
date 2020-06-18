using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace WindowsApplication1
{
   
    public partial class MainTabMenuControl : UserControl
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int iParam);
        public const int WM_SETREDRAW = 0x000B;
        public const int Win32False = 0;
        public const int Win32True = 1;
       
        /*
         * 一度に更新されるように、画面描画をいったん止める(Windows専用)
         * */
        public void BeginUpdate()
        {
            SendMessage(this.Handle, WM_SETREDRAW, Win32False, 0);
        }
        /**
         * 画面描画を再開する(Windows専用)
         * */
        public void EndUpdate()
        {
            SendMessage(this.Handle, WM_SETREDRAW, Win32True, 0);
            this.Invalidate();
        }
        public MainTabMenuControl()
        {
            InitializeComponent();
           // this.tabControl1.Controls.Remove(this.moveTabPage);
           // this.moveTabPage = new TabPage();
            //this.tabControl1.Controls.Add(this.moveTabPage);
            this.treeView1.Nodes.Add(Jusin.ObjectModel.ModelManager.getInstance().model_node);
            this.treeView1.AfterSelect += new TreeViewEventHandler(this.treeView1_Click);
            this.Invalidate();
        }
        public void setPropertyTabControl(TabPage tab_page,bool is_target = true)
        {
            Type type = this.moveTabPage.Controls[0].GetType() ;
            Type type2 = tab_page.Controls[0].GetType();
            if (type!=type2)
            {
                this.BeginUpdate();      
               this.tabControl1.Controls.Remove(this.moveTabPage);
               
                this.tabControl1.Controls.Add(tab_page);
                
                this.moveTabPage = tab_page;
                if(is_target)
                    this.tabControl1.SelectedTab = this.tabControl1.TabPages[1];
                
                //this.moveTabPage = tab_page;
                this.EndUpdate();
            }
        }
        protected void treeView1_Click(object sender, TreeViewEventArgs e)
        {
            int select_number;
            Jusin.ObjectModel.ModelManager.getInstance().ClearSelectModel();
            if (int.TryParse(e.Node.Name, out select_number) && Jusin.ObjectModel.ModelManager.getInstance().getSelectionDict().ContainsKey(select_number))
            {
                Jusin.ObjectModel.ModelManager.getInstance().SelectModel(int.Parse(e.Node.Name));

            }
            
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {
                
        }

    }
}
