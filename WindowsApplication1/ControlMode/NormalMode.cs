using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Tao.OpenGl;
using Tao.Platform.Windows;
using Jusin.ThreeDLib.ModelLoadPlugin.ThreeDS;
using Jusin.ThreeDLib.ModelBase;
using Jusin.ObjectModel;
using Jusin.ThreeDLib;
using Jusin.Camera;
namespace WindowsApplication1.ControlMode
{
    public class NormalMode : ControlMode
    {
        protected bool is_mid_mouse_on;
        protected int mouse_old_x, mouse_old_y; //前回のマウス座標
        protected float mag_speed, rotate_speed; //マウスの拡大縮小，回転速度
      
       
        public NormalMode()
        {
            intializeTabPage();
            intializeGLContextMenu();
        }
        override public void MouseClick(ModelManager model_manager, CCamera camera, Control control, MouseEventArgs e)
        {
            this.target_camera = camera;
              if (e.Button == MouseButtons.Middle)
            {
                is_mid_mouse_on = true;
                mouse_old_x = e.X;
                mouse_old_y = e.Y;
            }
              else if (e.Button == MouseButtons.Left)
              {
                
                  model_manager.ClearSelectModel();
                  Gl.glMatrixMode(Gl.GL_PROJECTION);
                  Gl.glPushMatrix();
                  Gl.glMatrixMode(Gl.GL_MODELVIEW);
                  Gl.glPushMatrix();
                  int[] viewport = new int[4];
                  Gl.glViewport(0, 0, control.Width, control.Height);
                  Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);
                  TransformMatrixes trans = new TransformMatrixes();
                  List<SelectionData> picks;
                  try
                  {
                      picks = Selection.Pick<CLoadedObject>(model_manager.getSelectionDict(), new double[] { e.X, control.Height - e.Y - 1 }, new double[] { 1, 1 }, camera,
                          viewport, false, out trans);
                  }
                  catch (Exception except)
                  {
                      return;
                  }
                  SelectionData selected = Selection.GetNearest(picks);
                  if (selected != null)
                  {
                      model_manager.SelectModel(selected.names[0]);
                      // (selected.item as CLoadedObject).changeState(new Jusin.ObjectModel.State.ModelStateSelected());
                      ControlModeChanger.getInstance().changeMode(new NormalModeSelectedModel());
                  }
                  else
                  {
                      ControlModeChanger.getInstance().changeMode(new NormalMode());
                  }

                  Gl.glMatrixMode(Gl.GL_PROJECTION);
                  Gl.glPopMatrix();
                  Gl.glMatrixMode(Gl.GL_MODELVIEW);
                  Gl.glPopMatrix();
                  control.Invalidate();
              }
        }

        override public void MouseMove(ModelManager model_manager, CCamera camera,Control control, MouseEventArgs e)
        {

            if (is_mid_mouse_on)
            {
                //ミドルボタンが押されているときはカメラの回転処理
                //この時、モデルが選ばれていたらモデルの中心点を中心に回転
                int sub_x = mouse_old_x - e.X;
                int sub_y = mouse_old_y - e.Y;
                double angle_x = Math.PI * sub_x / 180.0;
                double angle_y = Math.PI * sub_y / 180.0;
                if (Math.Abs(sub_x) < 1 && Math.Abs(sub_y) < 1) return;
                 Vector3d center;
                 if (model_manager.getModelList().Count > 0)
                     center = model_manager.getModelList()[0].getCenterPoint();
                 else
                     center = new Vector3d(0, 0, 0);
                camera.eyeRotateAxis(center, angle_x, camera.getUp());
                Vector3d axis = camera.getUp().cross(camera.getEye() - camera.getCenter());
                axis.Normalize();

                camera.eyeRotateAxis(center, angle_y, axis);
                mouse_old_x = e.X;
                mouse_old_y = e.Y;

                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glLoadIdentity();
                camera.ApplyProjection();
                camera.gluLookAtLH();
                Gl.glMatrixMode(Gl.GL_MODELVIEW);

                control.Invalidate();

            }
            else
            {
                foreach (CLoadedObject obj in model_manager.getModelList())
                {
                    obj.state.onMouseLeave(obj, e);
                }
                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glPushMatrix();
                Gl.glMatrixMode(Gl.GL_MODELVIEW);
                Gl.glPushMatrix();
                int[] viewport = new int[4];
                Gl.glViewport(0, 0, control.Width, control.Height);
                Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);
                TransformMatrixes trans = new TransformMatrixes();
                List<SelectionData> picks;
                try
                {
                    picks= Selection.Pick<CLoadedObject>(model_manager.getSelectionDict(), new double[] { e.X, control.Height - e.Y - 1 }, new double[] { 1, 1 }, camera,
                        viewport, false, out trans);
                }
                catch (Exception except)
                {
                    return;
                }
                SelectionData selected = Selection.GetNearest(picks);
                if (selected != null)
                {
                    (selected.item as CLoadedObject).state.onMouseOver(selected.item, e);
                    // (selected.item as CLoadedObject).changeState(new Jusin.ObjectModel.State.ModelStateSelected());

                }

                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glPopMatrix();
                Gl.glMatrixMode(Gl.GL_MODELVIEW);
                Gl.glPopMatrix();

            }
        }
        override public void MouseWheel(ModelManager model_manager, CCamera camera, Control control, MouseEventArgs e)
        {
            int delta = e.Delta;
            //this.setViewportAndCamera(0, 0, control.Width, control.Height);
            //Gl.glPolygon
            int x1 = 0; int y1 = 0; int x2 = control.Width; int y2 = control.Height;
            Gl.glViewport(x1, y1, x2, y2);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
           // Glu.gluPerspective(30, (double)(x2 - x1) / (double)(y2 - y1), 1.0, 1000.0);
            camera.ApplyProjection();
            camera.gluLookAtLH();
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            camera.zoomToWindowPos(e.X, control.Height - e.Y, (float)((float)delta / 10.0f));
            control.Invalidate();
        }
        public override void MouseUp(ModelManager model_manager, CCamera camera, Control control, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                is_mid_mouse_on = false;
            }
        }
        
        virtual protected void intializeTabPage()
        {
            this.propertyTabPage = new TabPage();
            propertyTabPage.Controls.Add(new NormalTab());
            propertyTabPage.Text = "プロパティ";
        }
        virtual protected void intializeGLContextMenu()
        {
            NormalContext context = new NormalContext();
            this.GLContextMenu = context.getContextMenu();

        }

    }
    public class NormalModeSelectedModel : NormalMode
    {
        protected override void  intializeGLContextMenu()
        {
            NormalContext context = new NormalContext();
            this.GLContextMenu = context.getSelectedContextMenu();
        }
    }
}
