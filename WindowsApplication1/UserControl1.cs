using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using Tao.OpenGl;
using Tao.Platform.Windows;
using Jusin.ThreeDLib.ModelLoadPlugin.ThreeDS;
using Jusin.ThreeDLib.ModelBase;
using Jusin.ObjectModel;
using Jusin.ThreeDLib;
using Jusin.Camera;
using Jusin.ThreeDLib.Scene.Render;
using Jusin.ThreeDLib.Scene;
using WindowsApplication1.ControlMode;
using WindowsApplication1.Commands;
namespace Tao_Sample
{
    public partial class UserControl1 : UserControl
    {
        protected ModelManager model_manager;
        protected CLoadedObject xyz_axis;
        public UserControl1()
        {
            model_manager = ModelManager.getInstance();
            camera = new CCamera();
            camera.Change += new EventHandler(this.onCameraChange);
            model_manager.ModelsChange += new EventHandler(this.onModelsChange);

            InitializeComponent();
            this.ContextMenuStrip = normalMenuStrip;
            
            is_mid_mouse_on = false;
            mag_speed = 45F;
            rotate_speed = 1.0F;
            clearColor = Color.FromArgb(0, 240, 240, 240);
            Jusin.ThreeDLib.ModelLoadPlugin.IModelImportPlugin importer = new Jusin.ThreeDLib.ModelLoadPlugin.ThreeDS.ThreeDSLoader();
            List<ModelEntityBase> entities = importer.importFromFile("c:\\xyz_axis.3ds");
            xyz_axis = new CLoadedObject(entities);
        }
        public void setCamera(Vector3d eye,Vector3d center,Vector3d up)
        {
            this.camera.set(eye.x,eye.y,eye.z,center.x,center.y,center.z,up.x,up.y,up.z);
        }
        CLoadedObject loaded;
        IntPtr hRC;
        IntPtr hDC;
        Color clearColor;

        //マウスコントロール
        CCamera camera;
        bool is_mid_mouse_on;
        int mouse_old_x, mouse_old_y; //前回のマウス座標
        float mag_speed,rotate_speed; //マウスの拡大縮小，回転速度
        protected override void OnHandleCreated(EventArgs e)
        {
            
            base.OnHandleCreated(e);

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, false);
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            Gdi.PIXELFORMATDESCRIPTOR pfd = new Gdi.PIXELFORMATDESCRIPTOR();
            //            OpenGLをサポート         ウィンドウに描画         ダブルバッファ
            pfd.dwFlags = Gdi.PFD_SUPPORT_OPENGL | Gdi.PFD_DRAW_TO_WINDOW | Gdi.PFD_DOUBLEBUFFER;
            pfd.iPixelType = Gdi.PFD_TYPE_RGBA; //RGBAフォーマット
            pfd.cColorBits = 32; // 32bit/pixel
            pfd.cAlphaBits = 8; // アルファチャンネル8bit (0にするとアルファチャンネル無しになる)
            pfd.cDepthBits = 16; // デプスバッファ16bit
            pfd.cStencilBits = 8;//ステンシルバッファを利用するときはこれを追加しなければならない
            //デバイスコンテキストのハンドルを取得。
            this.hDC = User.GetDC(this.Handle);

            //ピクセルフォーマットを選択。
            int pixFormat = Gdi.ChoosePixelFormat(this.hDC, ref pfd);
            if (pixFormat <= 0) throw new Exception("ChoosePixelFormat failed.");

            //ピクセルフォーマットの正確な設定を取得
            Wgl.wglDescribePixelFormat(this.hDC, pixFormat, 40, ref pfd);

            //デバイスコンテキストにピクセルフォーマットを設定。
            bool valid = Gdi.SetPixelFormat(this.hDC, pixFormat, ref pfd);
            if (!valid) throw new Exception("SetPixelFormat failed");

            //OpenGLのレンダリングコンテキストを作成。
            this.hRC = Wgl.wglCreateContext(this.hDC);
            if (this.hRC == IntPtr.Zero) throw new Exception("wglCreateContext failed.");

            //作成したコンテキストをカレントに設定。
            Wgl.wglMakeCurrent(this.hDC, this.hRC);

            //レンダリングコンテキストを作成、カレントに設定したら、
            //１度だけこれを呼び出しておく。
            //Tao.OpenGl.GL、Tao.Platform.Windows.WGLの仕様。
            Gl.ReloadFunctions();
            Wgl.ReloadFunctions();

            //一応、エラーがないか確認。
            int err = Gl.glGetError();
            if (err != Gl.GL_NO_ERROR)
            {
                throw new Exception("Error code = " + err.ToString());
            }
            //this.addModel("c:\\users\\taiki\\bmw.3ds");


            camera.setViewPort(this.Width, this.Height);
            this.SetupGL();
        }
        public void changeModel(string filename)
        {
            Jusin.ThreeDLib.ModelLoadPlugin.IModelImportPlugin importer = new Jusin.ThreeDLib.ModelLoadPlugin.ThreeDS.ThreeDSLoader();
            List<ModelEntityBase> entities = importer.importFromFile(filename);
            this.loaded.entities = entities;
           // this.loaded = new CLoadedObject(model);
        }
        public void addModel(string filename)
        {
            MainCommandManager.getInstance().execute(new AddModelCommand(this.model_manager, filename));
            //this.loaded.entities = entities;
        }

        /// <summary>
        /// レンダリングコンテキストを解放する。
        /// </summary>
        private void DeleteContext()
        {
            Wgl.wglDeleteContext(hRC);
        }


        private void SetupGL()
        {
            Gl.glEnable(Gl.GL_DEPTH_TEST);
           // Gl.glEnable(Gl.GL_BLEND);
            Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);
            //Gl.glPolygonMode ( Gl.GL_FRONT_AND_BACK, Gl.GL_LINE );

            Gl.glClearColor((float)clearColor.R/255f, (float)clearColor.G/255f, (float)clearColor.B/255f, (float)clearColor.A/255f);
            Gl.glShadeModel(Gl.GL_FLAT);
            Gl.glEnable(Gl.GL_CULL_FACE);
            Gl.glCullFace(Gl.GL_BACK);
            //Gl.glBlendFunc(Gl.GL_SRC_ALPHA_SATURATE, Gl.GL_ONE);

            // enable lighting
            Gl.glEnable(Gl.GL_LIGHT0);
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glLightModeli(Gl.GL_LIGHT_MODEL_TWO_SIDE, Gl.GL_TRUE);
            UserControl1_Resize(this, new EventArgs());
            //Gl.glEnable( Gl.GL_TEXTURE_2D );
        }

        protected void RenderClip()
        {

        }
        protected override void OnPaint(PaintEventArgs e)
        {
        
            //base.OnPaint( e );
            Wgl.wglMakeCurrent(this.hDC, this.hRC);

            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT | Gl.GL_STENCIL_BUFFER_BIT);
           
		    float[] light = new float[] {200, 100000, 40};
            float[] light2 = new float[] { 200, -100000, 40 };
           
            int err = Gl.glGetError();
            if (err != Gl.GL_NO_ERROR)
            {
                err = err;
            }
            //Gl.glDisable(Gl.GL_BLEND);
            //Gl.glDisable(Gl.GL_POLYGON_SMOOTH);
           Gl.glLoadIdentity();
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, light);
            this.setViewportAndCamera(0, 0, this.Width, this.Height);

            //draw floor
            
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            /*for (int ii = 50; ii > -50; ii -= 5)
            {
                Gl.glBegin(Gl.GL_TRIANGLE_STRIP);
                Gl.glNormal3f(0.0f, 1.0f, 0.0f);

                for (int jj = -50; jj < 50; jj += 5)
                {
                    Gl.glColor3f(0.4f, 0.6f, 0.8f);
                    Gl.glVertex3f(jj, -10, ii);
                    Gl.glVertex3f(jj, -10, ii + 5);
                }

                Gl.glEnd();
            }*/
            //model.sortEntities(camera);
            //Gl.glEnable(Gl.GL_BLEND);
            //Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
            //Gl.glDepthMask(Gl.GL_FALSE);


            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glPushMatrix();
            Gl.glLoadIdentity();
          
            // enough to cover all clip edge area.
            
            //Glu.gluSphere(new Glu.GLUquadric(), 1.0, 1, 1);
            
            //Gl.glDisable(Gl.GL_LIGHTING);
            //Gl.glDisable(Gl.GL_LIGHT0);
            
            //this.ClipRender();
            //DoubleCrossSectionRender cross_render = new DoubleCrossSectionRender(-20.0, -19.9);
            //foreach (CLoadedObject model in this.model_manager.getModelList())
            //cross_render.Render(model);
            foreach (CLoadedObject model in this.model_manager.getModelList())
            {
                model.Render();
            }
            err = Gl.glGetError();
            if (err != Gl.GL_NO_ERROR)
            {

                err = err;
            }

            Gl.glPopMatrix();
            Gl.glDisable(Gl.GL_BLEND);
            Gl.glDisable(Gl.GL_CLIP_PLANE0); // enabling clip plane again
            Gl.glDisable(Gl.GL_CLIP_PLANE1);
            Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_LINE);
            Gl.glLineWidth(1.0f);
            Gl.glDisable(Gl.GL_LIGHTING);
            Gl.glColor4f(1.0f, 0.48f, (float)0x00,0.5f);
            
           /* foreach (CLoadedObject model in this.model_manager.getModelList())
            {

                Gl.glPushMatrix();
                Gl.glLoadIdentity();
                Gl.glTranslatef(model.vx, model.vy, model.vz);
                Gl.glRotatef(model.x_dir, 1.0f, 0.0f, 0.0f);
                Gl.glRotatef(model.y_dir, 0.0f, 1.0f, 0.0f);
                Gl.glRotatef(model.z_dir, 0.0f, 0.0f, 1.0f);
                model.getBBox().DrawWire();
                Gl.glPopMatrix();
            }*/
          
            Gl.glPushMatrix();
            Gl.glLoadIdentity();
            Gl.glTranslatef(0, 100, 0);
            this.myBox(100.0, 100.0, 100.0);
            Gl.glPopMatrix();
            Gl.glDisable(Gl.GL_DEPTH_TEST);
            //Draw XYZ axis
            
            
           // Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            //Gl.glDisable(Gl.GL_BLEND);
            //Gl.glDisable(Gl.GL_POLYGON_SMOOTH);
            Gl.glLoadIdentity();
            this.setSmallCameraViewport(this.ClientSize.Width - 100, 0, 100, 100);
            
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_LIGHTING);
            //draw floor

            // model.sortEntities(camera);
            //  Gl.glEnable(Gl.GL_BLEND);
            //Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
            //Gl.glDepthMask(Gl.GL_FALSE);
            
            DRAW_XYZ();
            Gl.glDisable(Gl.GL_DEPTH_TEST);
            //Gl.glDepthMask(Gl.GL_TRUE);
            Gl.glFlush(); //シングルバッファの場合は必要。
            Wgl.wglSwapBuffers(this.hDC); //シングルバッファの場合は、この呼び出しは無効。
            this.setViewportAndCamera(0, 0, this.Width, this.Height);
        }
        /*
         * 直方体を描く
         */
        private void myBox(double x, double y, double z)
        {
          double[][] vertex = new double[][]{
            new double[]{ -x, -y, -z },
             new double[]{  x, -y, -z },
             new double[]{  x,  y, -z },
             new double[]{ -x,  y, -z },
             new double[]{ -x, -y,  z },
            new double[] {  x, -y,  z },
            new double[] {  x,  y,  z }, 
            new double[] { -x,  y,  z }
          };

           int[][] face = new int[][]{
            new int[] { 0, 1, 2, 3 },
           new int[] { 1, 5, 6, 2 },
           new int[] { 5, 4, 7, 6 },
           new int[] { 4, 0, 3, 7 },
           new int[] { 4, 5, 1, 0 },
           new int[] { 3, 2, 6, 7 }
          };

           double[][]normal = new double[][]{
            new double[] { 0.0, 0.0,-1.0 },
            new double[] { 1.0, 0.0, 0.0 },
            new double[] { 0.0, 0.0, 1.0 },
            new double[] {-1.0, 0.0, 0.0 },
            new double[] { 0.0,-1.0, 0.0 },
            new double[] { 0.0, 1.0, 0.0 }
          };

           float[] red = { 0.8f, 0.2f, 0.2f, 1.0f };

          int i, j;

          /* 材質を設定する */
          Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, red);
            
          Gl.glBegin(Gl.GL_QUADS);
          for (j = 0; j < 6; ++j) {
              Gl.glNormal3dv(normal[j]);
            for (i = 4; --i >= 0;) {
                Gl.glVertex3dv(vertex[face[j][i]]);
            }
          }
          Gl.glEnd();
        }

        private void DRAW_PRINTAREA(double x, double y, double z)
        {
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex2d(-100, 0);
        }
        private void DRAW_XYZ()
        {
           // Gl.glDisable(Gl.GL_LIGHTING);
            //Gl.glDisable(Gl.GL_LIGHT0);
            /*Gl.glBegin(Gl.GL_LINES);

            Gl.glColor3d(0, 1, 0);//x(green)
            Gl.glVertex2d(-100, 0);
            Gl.glVertex2d(100, 0);

            Gl.glColor3d(1, 0, 0);//y(red)
            Gl.glVertex2d(0, 0);
            Gl.glVertex2d(0, 100);

            Gl.glColor3d(0, 0, 1);//z(blue)
            Gl.glVertex3d(0, 0, -100);
            Gl.glVertex3d(0, 0, 100);
            Gl.glEnd();*/
            Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);
            xyz_axis.Render();
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);
        }
        public void toTransparent()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent; // 透明
            this.clearColor = Color.FromArgb(255, 255, 255, 255);

            //this.BackColor = Color.FromArgb(100, 255, 255, 255); // 半透明
        }
        private void ClipRender()
        {
            // Clip plane setup
            Vector3d[] vec = new Vector3d[4]; // quad, defining our plane
            Vector3d norm_vect = new Vector3d(0.0, 1.0, 0.0);
            Vector3d norm_vect_minus = new Vector3d(0.0,1.0,0.0);
            double D = -20.0;
            double D2 = -19.9;
            
            Gl.glEnable(Gl.GL_CLIP_PLANE0);
            //Gl.glEnable(Gl.GL_CLIP_PLANE1);
            Gl.glClipPlane(Gl.GL_CLIP_PLANE0,new double[]{norm_vect.x,-1*norm_vect.y,norm_vect.z,-1*D});
            Gl.glEnable(Gl.GL_STENCIL_TEST);
            Gl.glClear(Gl.GL_STENCIL_BUFFER_BIT);
            Gl.glDisable(Gl.GL_DEPTH_TEST);
            Gl.glColorMask(Gl.GL_FALSE, Gl.GL_FALSE, Gl.GL_FALSE, Gl.GL_FALSE);
            Gl.glClipPlane(Gl.GL_CLIP_PLANE1, new double[] { norm_vect_minus.x, norm_vect_minus.y, norm_vect_minus.z, D2 });
            // first pass: increment stencil buffer value on back faces
            Gl.glStencilFunc(Gl.GL_ALWAYS, 0, 0);
            Gl.glStencilOp(Gl.GL_KEEP, Gl.GL_KEEP, Gl.GL_INCR);
            Gl.glCullFace(Gl.GL_FRONT); // render back faces only

            foreach (CLoadedObject model in this.model_manager.getModelList())
                model.Render();
           
            // second pass: decrement stencil buffer value on front faces
            Gl.glStencilOp(Gl.GL_KEEP, Gl.GL_KEEP, Gl.GL_DECR);
            Gl.glCullFace(Gl.GL_BACK); // render front faces only
            foreach (CLoadedObject model in this.model_manager.getModelList())
                model.Render();    
            //****** Rendering the mesh's clip edge ****//
            Gl.glEnable(Gl.GL_STENCIL_TEST);
            //Gl.glClear(Gl.GL_STENCIL_BUFFER_BIT);
            Gl.glDisable(Gl.GL_DEPTH_TEST);
           Gl.glColorMask(Gl.GL_FALSE, Gl.GL_FALSE, Gl.GL_FALSE, Gl.GL_FALSE);
           
            // drawing clip planes masked by stencil buffer content
            Gl.glColorMask(Gl.GL_TRUE, Gl.GL_TRUE, Gl.GL_TRUE, Gl.GL_TRUE);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glDisable(Gl.GL_CLIP_PLANE0);
            //Gl.glDisable(Gl.GL_CLIP_PLANE1);
            Gl.glStencilFunc(Gl.GL_NOTEQUAL, 0, ~0); 
            // stencil test will pass only when stencil buffer value = 0; 
            // (~0 = 0x11...11)
            /*Gl.glBegin(Gl.GL_QUADS); // rendering the plane quad. Note, it should be big 
            // enough to cover all clip edge area.
            float[][] verts = new float[][]{new float[]{1000f,5.0f,1000f},new float[]{-1000.0f,1000.0f,1000.0f},new float[]{-1000.0f,1000.0f,-1000.0f},new float[]{1000.0f,1000.0f,-1000.0f}};
            for(int j=3; j>=0; j--) Gl.glVertex3fv(verts[j]);
            Gl.glEnd();*/
            //Gl.glCullFace(Gl.GL_BACK); // render front faces only
            foreach (CLoadedObject model in this.model_manager.getModelList())
            {

                Gl.glPushMatrix();
                Gl.glLoadIdentity();
                Gl.glTranslatef(model.vx, model.vy, model.vz);
                Gl.glRotatef(model.x_dir, 1.0f, 0.0f, 0.0f);
                Gl.glRotatef(model.y_dir, 0.0f, 1.0f, 0.0f);
                Gl.glRotatef(model.z_dir, 0.0f, 0.0f, 1.0f);
                model.getBBox().DrawWire();
                Gl.glPopMatrix();
            }   
            //****** End rendering mesh's clip edge ****/
            //Now that the clip edge image has been rendered to the color and depth buffers, the final step is to render the earth surface mesh with the stencil test disabled.

            //****** Rendering mesh *********// 
            Gl.glDisable(Gl.GL_STENCIL_TEST);
            Gl.glEnable(Gl.GL_CLIP_PLANE0); // enabling clip plane again
            Gl.glEnable(Gl.GL_CLIP_PLANE1);

            //Gl.glColorMask(Gl.GL_TRUE, Gl.GL_TRUE, Gl.GL_TRUE, Gl.GL_TRUE);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glDisable(Gl.GL_LIGHTING);
            //Gl.glClipPlane(Gl.GL_CLIP_PLANE0);
        }
        private void UserControl1_Load(object sender, EventArgs e)
        {

        }
        public void setViewportAndCamera(int x1, int y1, int x2, int y2)
        {
            //Gl.glPolygon
            Gl.glViewport(x1, y1,x2, y2);
            camera.setViewPort(x2, y2);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            camera.ApplyProjection();
            camera.gluLookAtLH();
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
        }
        public void setSmallCameraViewport(int x1,int y1,int x2, int y2)
        {
            Gl.glViewport(x1, y1, x2, y2);
           // camera.setViewPort(x2, y2);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            CBBox box = xyz_axis.getBBox();
            camera.ApplyProjection(5.0,x2,y2);

           // Glu.gluLookAt(100 * camera.getUp().x+box.getCenter().x, 100 * camera.getUp().y+box.getCenter().y, 100 * camera.getUp().z+box.getCenter().z, box.getCenter().x, box.getCenter().y, box.getCenter().z, camera.getUp().x, camera.getUp().y, camera.getUp().z);
            Vector3d eye_vect = camera.getEye() - camera.getCenter();
            //eye_vect.Normalize();
            Glu.gluLookAt(eye_vect.x,eye_vect.y,eye_vect.z,0,0,0 ,camera.getUp().x, camera.getUp().y, camera.getUp().z);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
        }
        private void UserControl1_Resize(object sender, EventArgs e)
        {
            Gl.glViewport(0, 0, this.Width, this.Height);

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            this.camera.setViewPort(this.ClientSize.Width, this.ClientSize.Height);
            camera.ApplyProjection();
            
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            camera.gluLookAtLH();
          
            
        }
        struct st_Selection
        {
            int name;
            double min_z;
            double max_z;
        }
     
       
        private void UserControl1_MouseDown(object sender, MouseEventArgs e)
        {
            ControlModeChanger.getInstance().getCurrentMode().MouseClick(this.model_manager, this.camera, this, e);
        }
        private void MousePick(int x, int y)
        {
            
        }
        private void UserControl1_MouseUp(object sender, MouseEventArgs e)
        {
            ControlModeChanger.getInstance().getCurrentMode().MouseUp(this.model_manager, camera, this, e);
        }
        private void UserControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            ControlModeChanger.getInstance().getCurrentMode().MouseWheel(this.model_manager, camera, this, e);
        }
        private void UserControl1_MouseMove(object sender, MouseEventArgs e)
        {

            ControlModeChanger.getInstance().getCurrentMode().MouseMove(this.model_manager, camera, this, e);
        }
        private void onModelsChange(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        private void onCameraChange(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void moveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void modelSelectMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "moveToolStripMenuItem":

                    break;
                case "magedToolStripMenuItem":
                    break;
                default :
                    break;
            }
        }

    }
  

}
