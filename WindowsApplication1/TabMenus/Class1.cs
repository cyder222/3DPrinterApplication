using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace WindowsApplication1.TabMenus
{
    class ModelTreeNode: TreeNode
    {
        public ModelTreeNode()
        {
           
        }
        public void Select()
        {
            this.BackColor = System.Drawing.Color.Blue;
            this.ForeColor = System.Drawing.Color.White;
        }
        public void cancelSelect()
        {
            this.BackColor = System.Drawing.Color.White;
            this.ForeColor = System.Drawing.Color.Black;
        }
    }
}
