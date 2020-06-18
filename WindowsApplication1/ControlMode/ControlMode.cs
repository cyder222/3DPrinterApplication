using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Jusin.ObjectModel;
using Jusin.Camera;
namespace WindowsApplication1.ControlMode
{
    public class ControlModeChanger
    {
        private static ControlModeChanger __instance = null;

        public static ControlModeChanger getInstance()
        {
            if (__instance == null)
            {
                __instance = new ControlModeChanger();
            }
            return __instance;
        }
        protected ControlMode current_mode;
        public event EventHandler ModeChange;
        protected ModelManager manager;
        private ControlModeChanger()
        {
            this.current_mode = new ControlMode();
        }
        public void setModelManager(ModelManager manager)
        {
            this.manager = manager;
        }
        public void changeMode(ControlMode new_mode)
        {
            if (this.current_mode.GetType() == new_mode.GetType())
                return;
            this.current_mode.leave(new_mode);
            new_mode.invoke(current_mode);
            this.current_mode = new_mode;
            if(this.ModeChange!=null)
                this.ModeChange(this, new EventArgs());
        }
        public ControlMode getCurrentMode()
        {
            return this.current_mode;
        }
    }
    public class ControlMode
    {
        public TabPage propertyTabPage;
        public ContextMenuStrip GLContextMenu;
        protected bool is_mid_mouse_on;
        protected CCamera target_camera;
        public ControlMode()
        {
            intializeTabPage();
            intializeGLContextMenu();
            is_mid_mouse_on = false;
            target_camera = null;
        }
        virtual public CCamera getTargetCamera()
        {
            return this.target_camera;
        }
        virtual protected void intializeTabPage()
        {
        }
        virtual protected void intializeGLContextMenu()
        {
        }
        virtual public void invoke(ControlMode fromMode = null)
        {
        }
        virtual public void leave(ControlMode toMode = null)
        {
        }
        virtual public void MouseClick(ModelManager model_manager, CCamera camera,Control control, MouseEventArgs e)
        {
        }
        virtual public void MouseMove(ModelManager model_manager, CCamera camera,Control control , MouseEventArgs e)
        {
        }
        virtual public void MouseWheel(ModelManager model_manager, CCamera camera, Control control, MouseEventArgs e)
        {
        }
        virtual public void MouseUp(ModelManager model_manager, CCamera camera, Control control, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                is_mid_mouse_on = false;
            }
        }

    }
   
    
    public class ModelMoveMode : NormalMode
    {
        protected CLoadedObject target;
        protected bool is_left_mouse_on;
        public ModelMoveMode(CLoadedObject target)
        {
           //this.model_manager = manager;
            this.target = target;
            intializeTabPage();
        }
        protected void intializeTabPage()
        {
            this.propertyTabPage = new TabPage();
            this.propertyTabPage.Text = "プロパティ";

            propertyTabPage.Controls.Add( new MoveTab(target) );
        }
        protected void intializeGLContextMenu()
        {
          
        }
        public override void MouseMove(ModelManager model_manager, CCamera camera, Control control, MouseEventArgs e)
        {
            base.MouseMove(model_manager, camera, control, e);
            if (is_left_mouse_on)
            {
                int dx = (e.X - mouse_old_x)/2;
                int dy = (e.Y - mouse_old_y)/2;
                this.target.vx += dx;
                this.target.vy -= dy;
                mouse_old_x = e.X;
                mouse_old_y = e.Y;

            }
        }
        public override void MouseUp(ModelManager model_manager, CCamera camera, Control control, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                is_mid_mouse_on = false;
            }
            if (e.Button == MouseButtons.Left)
            {
                is_left_mouse_on = false;
            }
        }
        override public void MouseClick(ModelManager model_manager, CCamera camera, Control control, MouseEventArgs e)
        {
            this.target_camera = camera;
            if (e.Button == MouseButtons.Left)
            {
                is_left_mouse_on = true;
                mouse_old_x = e.X;
                mouse_old_y = e.Y;
            }
            if (e.Button == MouseButtons.Middle)
            {
                is_mid_mouse_on = true;
                mouse_old_x = e.X;
                mouse_old_y = e.Y;
            }
        } 
    }
}
    