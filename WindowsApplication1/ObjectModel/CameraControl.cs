using System;
using System.Collections.Generic;
using System.Text;
using Jusin.Camera;
using Jusin.ObjectModel;
using Jusin.ThreeDLib.ModelBase;
using Youryella.Windows.Forms.Animation;
namespace Jusin.ObjectModel
{
    public class CameraControl
    {
       
       /* private Vector3d[] calcShoumenCameraPos(CLoadedObject target)
        {
            
            Vector3d up = new Vector3d(0.0, 1.0, 0.0);
            Vector3d center;
            CBBox box = target.getBBox();
            if (target != null)
            {
                center = box.getCenter();
            }
            else
            {
                center = new Vector3d(0, 0, 0);
            }
            Vector3d eye = new Vector3d();
            
            
        }
        private Vector3d[] calcUpCameraPos(CLoadedObject target)
        {
        }
        private Vector3d[] calcSideCameraPos(CLoadedObject target)
        {
        }*/
        protected static double calcWindowFitZoomSide(CCamera camera, CLoadedObject target)
        {
            double width = 200.0;
            double height = 200.0;
            double view_width = camera.width;
            double view_height = camera.height;
            if (target != null)
            {
                width = target.getBBox().getSize().z ;
                height = target.getBBox().getSize().y;
            }
            width = width * 1.05 / camera.base_zoom / 2;
            height = height * 1.05 / camera.base_zoom / 2;
            double tmp_zoom = width/view_width;
            if (tmp_zoom * view_height <=height)
                tmp_zoom = height / view_height;

            return tmp_zoom;

        }
        protected static double calcWindowFitZoomTop(CCamera camera, CLoadedObject target)
        {
            double width = 200.0;
            double height = 200.0;
            double view_width = camera.width;
            double view_height = camera.height;
            if (target != null)
            {
                width = target.getBBox().getSize().z;
                height = target.getBBox().getSize().x;
            }
            width = width * 1.05 / camera.base_zoom / 2;
            height = height * 1.05 / camera.base_zoom / 2;
            double tmp_zoom = width / view_width;
            if (tmp_zoom * view_height <= height)
                tmp_zoom = height / view_height;

            return tmp_zoom;
        }
        protected static double calcWindowFitZoomShoumen(CCamera camera, CLoadedObject target)
        {
            double width=200.0;
            double height = 200.0;
            double view_width = camera.width;
            double view_height = camera.height;
            if (target != null)
            {
                width = target.getBBox().getSize().x ;
                height = target.getBBox().getSize().y ;
            }
            width = width * 1.05/camera.base_zoom/2;
            height = height * 1.05/camera.base_zoom/2;
            double tmp_zoom = width/view_width;
            if (tmp_zoom * view_height <= height)
                tmp_zoom = height / view_height;

            return tmp_zoom;

        }
        /**
         * 上正面からの視点に変更する
         * @param camera 移動するカメラオブジェクト
         * @param target 中心のターゲットとなるオブジェクト
         * 
         * **/
        public static void setToTop(CCamera camera, CLoadedObject target = null, bool isAnimation = true)
        {
            ParallelAnimation pAnimation = new ParallelAnimation();
            TimeSpan duration = TimeSpan.FromSeconds(0.5);
             double zoom = CameraControl.calcWindowFitZoomTop(camera, target);
            if (isAnimation)
            {
                if(target==null)
                {
                    pAnimation.AddChild(new DoubleAnimation(camera, "eye_x", 0.0, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "eye_y", 300.0, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "eye_z", 0.0, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "center_x", 0.0, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "center_y", 0.0, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "center_z", 0.0, duration));
                }else{
                    pAnimation.AddChild(new DoubleAnimation(camera, "eye_x", target.getBBox().getCenter().x+target.vx, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "eye_y", target.getBBox().getCenter().y+300+target.vy, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "eye_z", target.getBBox().getCenter().z+target.vz, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "center_x", target.getBBox().getCenter().x+target.vx, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "center_y", target.getBBox().getCenter().y+target.vy, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "center_z", target.getBBox().getCenter().z+target.vz, duration));
                }
                pAnimation.AddChild(new DoubleAnimation(camera, "up_x", -1.0, duration));
                pAnimation.AddChild(new DoubleAnimation(camera, "up_y", 0.0, duration));
                pAnimation.AddChild(new DoubleAnimation(camera, "up_z", 0.0, duration));
                 pAnimation.AddChild(new DoubleAnimation(camera, "zoom", zoom, duration));
                pAnimation.Begin();
            }
            else
            {
                camera.set(0.0, 100.0, 200.0,
                    0.0, 100.0, 0.0,
                    0.0, 1.0, 0.0);
            }
        }
        /**
         * カメラを側面からの視点に変更する
         * 
         * **/
        public static void setToSide(CCamera camera, CLoadedObject target = null, bool isAnimation = true)
        {
            ParallelAnimation pAnimation = new ParallelAnimation();
            TimeSpan duration = TimeSpan.FromSeconds(0.5);
            double zoom = CameraControl.calcWindowFitZoomSide(camera, target);
            if (isAnimation)
            {
                if (target == null)
                {
                    pAnimation.AddChild(new DoubleAnimation(camera, "eye_x", 100.0, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "eye_y", 100.0, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "eye_z", 0.0, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "center_x", 0.0, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "center_y", 100.0, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "center_z", 0.0, duration));
                }
                else
                {
                    pAnimation.AddChild(new DoubleAnimation(camera, "eye_x", target.getBBox().getCenter().x+100+target.vx, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "eye_y", target.getBBox().getCenter().y+target.vy, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "eye_z", target.getBBox().getCenter().z+target.vz, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "center_x", target.getBBox().getCenter().x+target.vx, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "center_y", target.getBBox().getCenter().y+target.vy, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "center_z", target.getBBox().getCenter().z+target.vz, duration));
                }
                pAnimation.AddChild(new DoubleAnimation(camera, "up_x", 0.0, duration));
                pAnimation.AddChild(new DoubleAnimation(camera, "up_y", 1.0, duration));
                pAnimation.AddChild(new DoubleAnimation(camera, "up_z", 0.0, duration));
                pAnimation.AddChild(new DoubleAnimation(camera, "zoom", zoom, duration));
                pAnimation.Begin();
            }
            else
            {
                camera.set(0.0, 100.0, 200.0,
                    0.0, 100.0, 0.0,
                    0.0, 1.0, 0.0);
            }
        }
        /**
         * カメラを正面からの視点に変更する
         * 
         * **/
        public static void setToShoumen(CCamera camera,CLoadedObject target = null, bool isAnimation = true)
        {
            ParallelAnimation pAnimation = new ParallelAnimation();
            TimeSpan duration = TimeSpan.FromSeconds(0.5);
            double zoom = CameraControl.calcWindowFitZoomShoumen(camera, target);
            if (isAnimation)
            {
                if (target == null)
                {
                    pAnimation.AddChild(new DoubleAnimation(camera, "eye_x", 0.0, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "eye_y", 100.0, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "eye_z", 200.0, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "center_x", 0.0, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "center_y", 100.0, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "center_z", 0.0, duration));
                }
                else
                {
                    pAnimation.AddChild(new DoubleAnimation(camera, "eye_x", target.getBBox().getCenter().x+target.vx, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "eye_y", target.getBBox().getCenter().y+target.vy, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "eye_z", target.getBBox().getCenter().z+200.0+target.vz, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "center_x", target.getBBox().getCenter().x+target.vx, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "center_y", target.getBBox().getCenter().y+target.vy, duration));
                    pAnimation.AddChild(new DoubleAnimation(camera, "center_z", target.getBBox().getCenter().z+target.vz, duration));
                }
                pAnimation.AddChild(new DoubleAnimation(camera, "up_x", 0.0, duration));
                pAnimation.AddChild(new DoubleAnimation(camera, "up_y", 1.0, duration));
                pAnimation.AddChild(new DoubleAnimation(camera, "up_z", 0.0, duration));
                pAnimation.AddChild(new DoubleAnimation(camera,"zoom",zoom,duration));
                pAnimation.Begin();
            }
            else
            {
                camera.set(0.0, 100.0, 200.0,
                    0.0, 100.0, 0.0, 
                    0.0, 1.0, 0.0);
            }

        }

    }
}
