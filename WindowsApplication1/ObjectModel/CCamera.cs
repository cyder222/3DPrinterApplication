using System;
using System.Collections.Generic;
using System.Text;

using Tao.OpenGl;
using Jusin.ThreeDLib;
using Jusin.ThreeDLib.ModelBase;
namespace Jusin.Camera
{
    public class CCamera
    {
        Vector3d eye;
        Vector3d center;
        Vector3d up;
        protected int Width;
        public int width
        {
            get
            {
                return this.Width;
            }
        }
        protected int Height;
        public int height
        {
            get
            {
                return this.Height;
            }
        }
        public event EventHandler Change;
        protected bool perspective;
        protected double zoom_factor;
        protected float base_zoom_factor = 0.05f;
        public CCamera()
        {
            eye = new Vector3d(0.0,0.0,-200.0);
            center = new Vector3d(0.0,0.0,0.0);
            up = new Vector3d(0.0,1.0,0.0);
            set(eye.x, eye.y, eye.z,
                center.x, center.y, center.z,
                up.x, up.y, up.z);
            this.perspective = false;
            this.zoom_factor = 1.0f;
        }
        public void setViewPort(int Width,int Height)
        {
            this.Width = Width;
            this.Height = Height;
        }
        public void ApplyProjection()
        {
            if (this.perspective)
                Glu.gluPerspective(30, (double)(this.Width - 0) / (double)(this.Height - 0), 1.0, 10000.0);
            else
                Gl.glOrtho(-this.Width * base_zoom_factor * this.zoom, this.Width * base_zoom_factor * this.zoom, -this.Height * base_zoom_factor * this.zoom, this.Height * base_zoom_factor * this.zoom, 1.0, 10000.0);
        }
        public void ApplyProjection(double zoom,int width,int height)
        {
           // Glu.gluPerspective(30, (double)(this.Width - 0) / (double)(this.Height - 0), 1.0, 10000.0);
            Gl.glOrtho(-width * base_zoom_factor * zoom, width * base_zoom_factor * zoom, -height * base_zoom_factor * zoom, height * base_zoom_factor * zoom, 1.0, 10000.0);
        }
        public Vector3d getEye()
        {
            return this.eye;
        }
        public Vector3d getUp()
        {
            return this.up;
        }
        public Vector3d getCenter()
        {
            return this.center;
        }
        public CCamera(Vector3d eye,Vector3d center,Vector3d up){
            set(eye.x,eye.y,eye.z,
                center.x,center.y,center.z,
                up.x,up.y,up.z);
        }
        public void set(double eye_x, double eye_y, double eye_z,
                           double center_x, double center_y, double center_z,
                            double up_x, double up_y, double up_z)
        {
            eye.set(eye_x, eye_y, eye_z);
            center.set(center_x, center_y, center_z);
            up.set(up_x, up_y, up_z);
            ChangeEvent();
        }
        public void gluLookAtLH()
        {
            Glu.gluLookAt(eye.x, eye.y, eye.z, center.x, center.y, center.z, up.x, up.y, up.z);
        }
 
        public void setCameraStatus()
        {

        }
        public void eyeRotateAxis(Vector3d _center, double angle, Vector3d axe)
        {
            this.eye -= _center;
            this.eye.rotateAxis(angle,axe.x,axe.y,axe.z);
            this.eye += _center;
            this.up.rotateAxis(angle,axe.x,axe.y,axe.z);
            this.center -= _center;
            this.center.rotateAxis(angle,axe.x,axe.y,axe.z);
            this.center += _center;
        }
        public void eyeRotateX(Vector3d  _center,  double angle)
        {
          this.eye -= _center;
          this.eye.rotateX(angle);
          this.eye +=  _center;
          this.up.rotateX(angle);
        }
        public void eyeRotateY(Vector3d _center, double angle)
        {
            this.eye -= _center;
            this.eye.rotateY(angle);
            this.eye += _center;
            this.up.rotateY(angle);
        }
        public void eyeRotateZ(Vector3d _center, double angle)
        {
            this.eye -= _center;
            this.eye.rotateZ(angle);
            this.eye += _center;
            this.up.rotateZ(angle);
        }
        public void moveDelta(float delta_x,float delta_y,float delta_z)
        {
        }
        public void moveTo(float x,float y , float z)
        {
            this.eye.x = x;
            this.eye.y = y;
            this.eye.z = z;
        }
        public void rotateDelta(float delta_rot_x,float delta_rot_y,float delta_rot_z)
        {
        }
        public void rotateTo(float rot_x,float rot_y, float rot_z)
        {
        }
        /**
         * window座標 x,yからの視線ベクトルに向かってズームする．
         * @param y 左下原点のy座標
         * @param zoom 拡大率(カメラの位置から視線ベクトルの先までの距離*zoom分近づく)zoom = 0.0f ~ 0.5f
         * 
         * **/
        public void zoomToWindowPos(int x, int y,float zoom)
        {
            this.zoom += zoom / 100.0f;
            
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            double[] modelview = new double[16];
            Gl.glGetDoublev(Gl.GL_MODELVIEW_MATRIX, modelview);
            double[] projection = new double[16];
            Gl.glGetDoublev(Gl.GL_PROJECTION_MATRIX, projection);
            int[] viewport = new int[4];
            Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);
            double z = 0.0;
            double objX;
            double objY;
            double objZ;
            Gl.glReadPixels(x, y, 1, 1, Gl.GL_DEPTH_COMPONENT, Gl.GL_FLOAT, z);
            Glu.gluUnProject(x,y, (double)z, modelview, projection, viewport, out objX, out objY, out objZ);
            Gl.glDisable(Gl.GL_DEPTH_TEST);
             Vector3d moveVect;
            if(zoom<0)
               moveVect = new Vector3d((this.eye.x - objX) * zoom/100.0, (this.eye.y - objY) * zoom/100.0, (this.eye.z - objZ) * zoom/100.0);
            else
                moveVect = new Vector3d((this.eye.x - center.x) * zoom / 100.0, (this.eye.y - center.y) * zoom / 100.0, (this.eye.z - center.z) * zoom / 100.0);
 
            this.eye += moveVect;
           // this.eye += centerMoveVect;

            this.center += moveVect;
            
           // this.center += center_move_x.mul(new Vector3d(zoom, zoom, zoom)) ;
           // this.eye += center_move_x.mul(new Vector3d(zoom,zoom,zoom));
            //this.center += center_move_x;
            //this.eye += center_move_x;
            
           

        }

        // FOR ANIMATION CONTROL --------------------------------------
        private void ChangeEvent()
        {
            if (Change != null)
            {
                Change(this, EventArgs.Empty);
            }
        }
        public double eye_x
        {
            set
            {
                this.eye.x = value;
                ChangeEvent();
            }
            get
            {
                return this.eye.x;
            }
        }
        public double eye_y
        {
            set
            {
                this.eye.y = value;
                ChangeEvent();
            }
            get
            {
                return this.eye.y;
            }
        }
        public double eye_z
        {
            set
            {
                this.eye.z = value;
                ChangeEvent();
            }
            get
            {
                return this.eye.z;
            }
        }
        public double up_x
        {
            set
            {
                this.up.x = value;
                ChangeEvent();
            }
            get
            {
                return this.up.x;
            }
        }
        public double up_y
        {
            set
            {
                this.up.y = value;
                ChangeEvent();
            }
            get
            {
                return this.up.y;
            }
        }
        public double up_z
        {
            set
            {
                this.up.z = value;
                ChangeEvent();
            }
            get
            {
                return this.up.z;
            }
        }
        public double center_x
        {
            set
            {
                this.center.x = value;
                ChangeEvent();
            }
            get
            {
                return this.center.x;
            }
        }
        public double center_y
        {
            set
            {
                this.center.y = value;
                ChangeEvent();
            }
            get
            {
                return this.center.y;
            }
        }
        public double center_z
        {
            set
            {
                this.center.z = value;
                ChangeEvent();
            }
            get
            {
                return this.center.z;
            }
        }
        public float base_zoom
        {
            get
            {
                return this.base_zoom_factor;
            }
        }
        public double zoom
        {
            get
            {
                return this.zoom_factor;
            }
            set
            {
                
                this.zoom_factor = value;
                if (this.zoom_factor <= 0.1)
                {
                    this.zoom_factor = 0.1;
                }
                ChangeEvent();
            }
            
        }

    }
}
