using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication1.ControlMode
{
    public class ControlMode
    {
        public TabPage propertyTabPage;
        public ContextMenu GLContextMenu;

        public ControlMode()
        {
            intializeTabPage();
            intializeGLContextMenu();
        }
        protected void intializeTabPage()
        {
        }
        protected void intializeGLContextMenu()
        {
        }

    }
    public class ModelMoveMode : ControlMode
    {
        public ModelMoveMode()
        {
           
        }
         protected void intializeTabPage()
        {
            this.propertyTabPage = new TabPage();
            propertyTabPage.Controls.Add( new MoveTab() );
        }
         protected void intializeGLContextMenu()
        {
        }
    }
}
