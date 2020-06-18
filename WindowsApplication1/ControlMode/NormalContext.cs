using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Jusin.ObjectModel;
namespace WindowsApplication1.ControlMode
{
    public partial class NormalContext : UserControl
    {
        ModelManager manager;
        public NormalContext()
        {
            InitializeComponent();
            manager = ModelManager.getInstance();
        }
        public ContextMenuStrip getContextMenu()
        {
            return this.contextMenuStrip1;
        }
        public ContextMenuStrip getSelectedContextMenu()
        {
            return this.moveToolStipMenu2;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void MoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (manager.getSelectedModels().Count != 0)
            {
                CLoadedObject obj = manager.getSelectedModels()[0];
                ControlModeChanger.getInstance().changeMode(new ModelMoveMode(obj));
            }
        }

        private void ShoumenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CLoadedObject obj = manager.getSelectedModels().Count!=0?manager.getSelectedModels()[0]:null;
            CameraControl.setToShoumen(ControlModeChanger.getInstance().getCurrentMode().getTargetCamera(), obj);
        }
        private void RightSideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CLoadedObject obj = manager.getSelectedModels().Count != 0 ? manager.getSelectedModels()[0] : null;
            CameraControl.setToSide(ControlModeChanger.getInstance().getCurrentMode().getTargetCamera(),obj);
        }

        private void topToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CLoadedObject obj = manager.getSelectedModels().Count != 0 ? manager.getSelectedModels()[0] : null;
            CameraControl.setToTop(ControlModeChanger.getInstance().getCurrentMode().getTargetCamera(), obj);
        }

        private void moveToolStipMenu2_Opening(object sender, CancelEventArgs e)
        {

        }

      
      
    }
}
