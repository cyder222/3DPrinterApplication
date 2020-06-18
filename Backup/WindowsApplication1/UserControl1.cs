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
namespace Tao_Sample
{
    public partial class UserControl1 : UserControl
    {
        protected ModelManager model_manager;
        public UserControl1()
        {
            model_manager = new ModelManager();
            model_manager.ModelsChange += new EventHandler(this.onModelsChange);
            InitializeComponent();
            this.ContextMenuStrip = normalMenuStrip;
            camera = new CCamera();
            is_mid_mouse_on = false;
            mag_speed = 3.0F;
            rotate_speed = 1.0F;
            clearColor = Color.FromArgb(0, 240, 240, 240);
           
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
            this.addModel("c:\\bmw.3ds");


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
            Jusin.ThreeDLib.ModelLoadPlugin.IModelImportPlugin importer = new Jusin.ThreeDLib.ModelLoadPlugin.ThreeDS.ThreeDSLoader();
            List<ModelEntityBase> entities = importer.importFromFile(filename);
            this.model_manager.addModel(new CLoadedObject(entities));
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
            Gl.glShadeModel(Gl.GL_SMOOTH);
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

            //Glu.gluSphere(new Glu.GLUquadric(), 1.0, 1, 1);
            
            //Gl.glDisable(Gl.GL_LIGHTING);
            //Gl.glDisable(Gl.GL_LIGHT0);
            //this.ClipRender();
            foreach (CLoadedObject model in this.model_manager.getModelList())
                model.Render();
            Gl.glPopMatrix();
            Gl.glDisable(Gl.GL_BLEND);
            Gl.glDisable(Gl.GL_DEPTH_TEST);
            //Draw XYZ axis
            this.setViewportAndCamera(this.ClientSize.Width - 50, 0, 50, 50);
            
           // Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            //Gl.glDisable(Gl.GL_BLEND);
            //Gl.glDisable(Gl.GL_POLYGON_SMOOTH);
            Gl.glLoadIdentity();
            Gl.glEnable(Gl.GL_DEPTH_TEST);
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
            
        }
        private void DRAW_XYZ()
        {
            Gl.glDisable(Gl.GL_LIGHTING);
            Gl.glDisable(Gl.GL_LIGHT0);
            Gl.glBegin(Gl.GL_LINES);

            Gl.glColor3d(0, 1, 0);//x(green)
            Gl.glVertex2d(-100, 0);
            Gl.glVertex2d(100, 0);

            Gl.glColor3d(1, 0, 0);//y(red)
            Gl.glVertex2d(0, 0);
            Gl.glVertex2d(0, 100);

            Gl.glColor3d(0, 0, 1);//z(blue)
            Gl.glVertex3d(0, 0, -100);
            Gl.glVertex3d(0, 0, 100);
            Gl.glEnd();
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
            double D = -15.0;
            double D2 = -0.0;
            
            Gl.glEnable(Gl.GL_CLIP_PLANE0);
           // Gl.glEnable(Gl.GL_CLIP_PLANE1);
            Gl.glClipPlane(Gl.GL_CLIP_PLANE0,new double[]{norm_vect.x,-1*norm_vect.y,norm_vect.z,-1*D});
            Gl.glEnable(Gl.GL_STENCIL_TEST);
            Gl.glClear(Gl.GL_STENCIL_BUFFER_BIT);
            Gl.glDisable(Gl.GL_DEPTH_TEST);
            Gl.glColorMask(Gl.GL_FALSE, Gl.GL_FALSE, Gl.GL_FALSE, Gl.GL_FALSE);
            //Gl.glClipPlane(Gl.GL_CLIP_PLANE1, new double[] { norm_vect_minus.x, norm_vect_minus.y, norm_vect_minus.z, D2 });
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
            Gl.glBegin(Gl.GL_QUADS); // rendering the plane quad. Note, it should be big 
            // enough to cover all clip edge area.
            float[][] verts = new float[][]{new float[]{100f,5.0f,100f},new float[]{-100.0f,100.0f,100.0f},new float[]{-100.0f,100.0f,-100.0f},new float[]{100.0f,100.0f,-100.0f}};
            for(int j=3; j>=0; j--) Gl.glVertex3fv(verts[j]);
            Gl.glEnd();
            //****** End rendering mesh's clip edge ****/
            //Now that the clip edge image has been rendered to the color and depth buffers, the final step is to render the earth surface mesh with the stencil test disabled.

            //****** Rendering mesh *********// 
            Gl.glDisable(Gl.GL_STENCIL_TEST);
            Gl.glEnable(Gl.GL_CLIP_PLANE0); // enabling clip plane again
            

            //Gl.glColorMask(Gl.GL_TRUE, Gl.GL_TRUE, Gl.GL_TRUE, Gl.GL_TRUE);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            //Gl.glClipPlane(Gl.GL_CLIP_PLANE0);
            
            
        }
        private void UserControl1_Load(object sender, EventArgs e)
        {

        }
        private void setViewportAndCamera(int x1, int y1, int x2, int y2)
        {
            //Gl.glPolygon
            Gl.glViewport(x1, y1,x2, y2);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(45, (double)(x2-x1) / (double)(y2-y1), 1.0, 1000.0);
            camera.gluLookAtLH();
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
        }
        private void UserControl1_Resize(object sender, EventArgs e)
        {
            Gl.glViewport(0, 0, this.Width, this.Height);

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(45, this.ClientSize.Width / this.ClientSize.Height, 1, 1000);
            camera.setViewPort(this.ClientSize.Width, this.ClientSize.Height);
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
            if (e.Button == MouseButtons.Middle)
            {
                is_mid_mouse_on = true;
                mouse_old_x = e.X;
                mouse_old_y = e.Y;
            }
            else if (e.Button == MouseButtons.Left)
            {
                this.model_manager.ClearSelectModel();
                /*Gl.glEnable(Gl.GL_DEPTH_TEST);
                double[] modelview = new double[16];
                Gl.glGetDoublev(Gl.GL_MODELVIEW_MATRIX,modelview);
                double[] projection = new double[16];
                Gl.glGetDoublev(Gl.GL_PROJECTION_MATRIX, projection);
                int[] viewport = new int[4];
                Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);
                double z = 0.0 ;
                double objX;
                double objY;
                double objZ;
                Gl.glReadPixels(e.X, this.Height - e.Y, 1, 1, Gl.GL_DEPTH_COMPONENT, Gl.GL_FLOAT, z);
                Glu.gluUnProject(e.X, this.Height - e.Y, (double)z, modelview, projection, viewport, out objX, out objY, out objZ);
                Gl.glDisable(Gl.GL_DEPTH_TEST);
                if (this.loaded.checkColl(new Vector3d(objX-camera.getEye().x, objY-camera.getEye().y, objZ-camera.getEye().z)))
                {
                    System.Console.Write("あいうえお");
                }*/
                //セレクションバッファを作成。
               /*
                int selectionBufferLength = 100;
                uint[] selectionBuff = new uint[selectionBufferLength];
                //OpenGLに渡す。
                Gl.glSelectBuffer(selectionBuff.Length, selectionBuff);
                Gl.glRenderMode(Gl.GL_SELECT);
               
                Gl.glPushName(-1);
               // Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, light);
                
                //マウスポインタの座標（単位：pixel）
                //（注：ビューポートなどと同様に、描画領域の左下が原点）
                double mouseX, mouseY;
                mouseX = e.X;
                mouseY = this.Height - e.Y - 1;
                //マウスポインタを中心として、ヒットする範囲の幅と高さ（単位：pixel）
                double width = 1.0;
                double height=1.0;
                //ビューポートも指定する必要があります。
                int[] viewport = new int[4];
               
                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glPushMatrix();
                Gl.glLoadIdentity();
                Gl.glViewport(0, 0, this.Width, this.Height);
                Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);
                Glu.gluPickMatrix(mouseX, mouseY, width, height, viewport);
                Glu.gluPerspective(45, (double)(this.Width-0 ) / (double)(this.Height - 0), 1.0, 1000.0);

               // Gl.glMatrixMode(Gl.GL_MODELVIEW);
                camera.gluLookAtLH();
                Gl.glMatrixMode(Gl.GL_PROJECTION);
                
              //  this.setViewportAndCamera(0, 0, this.Width, this.Height);

               
                //camera.gluLookAtLH();
                //camera.gluLookAtLH();
              
                Gl.glInitNames();
               
                //draw floor
                Gl.glEnable(Gl.GL_DEPTH_TEST);
                Gl.glPushName(1);
               /* for (int ii = 50; ii > -50; ii -= 5)
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


               // Gl.glLoadIdentity();
                /*
                //Glu.gluSphere(new Glu.GLUquadric(), 1.0, 1, 1);
                Gl.glPopName();
                Gl.glPushName(2);
                loaded.Render();
                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glPopName();
                
               
                Gl.glDisable(Gl.GL_BLEND);
                Gl.glDisable(Gl.GL_DEPTH_TEST);
                //Draw XYZ axis
               // this.setViewportAndCamera(this.ClientSize.Width - 50, 0, 50, 50);
                //Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);

                //Glu.gluPickMatrix(mouseX, mouseY, width, height, viewport);
                // Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
                //Gl.glDisable(Gl.GL_BLEND);
                //Gl.glDisable(Gl.GL_POLYGON_SMOOTH);
                //Gl.glLoadIdentity();
                Gl.glEnable(Gl.GL_DEPTH_TEST);
                //draw floor

                // model.sortEntities(camera);
                //  Gl.glEnable(Gl.GL_BLEND);
                //Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
                //Gl.glDepthMask(Gl.GL_FALSE);
                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glPopMatrix();
                Gl.glDisable(Gl.GL_DEPTH_TEST);
                Gl.glFlush();
                int hits = Gl.glRenderMode(Gl.GL_RENDER);
              */
                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glPushMatrix();
                Gl.glMatrixMode(Gl.GL_MODELVIEW);
                Gl.glPushMatrix();
                int[] viewport = new int[4];
                Gl.glViewport(0, 0, this.Width, this.Height);
                Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);
                TransformMatrixes trans = new TransformMatrixes();
                SelectionData selected = Selection.GetNearest(Selection.Pick<CLoadedObject>(model_manager.getSelectionDict(), new double[] { e.X, this.Height-e.Y-1 }, new double[] { 1, 1 }, camera,
                    viewport, false, out trans));
                if (selected != null)
                {
                    model_manager.SelectModel(selected.names[0]);
                   // (selected.item as CLoadedObject).changeState(new Jusin.ObjectModel.State.ModelStateSelected());
                 
                }
                
                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glPopMatrix();
                Gl.glMatrixMode(Gl.GL_MODELVIEW);
                Gl.glPopMatrix();
                this.Invalidate();
                }
        }
        private void MousePick(int x, int y)
        {
            
        }
        private void UserControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                is_mid_mouse_on = false;
            }
        }
        private void UserControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            int delta = e.Delta;
            this.setViewportAndCamera(0, 0, this.Width, this.Height);
            this.camera.zoomToWindowPos(e.X,this.Height-e.Y,(float)((float)delta/1000.0f));
            this.Invalidate();
        }
        private void UserControl1_MouseMove(object sender, MouseEventArgs e)
        {

            if (is_mid_mouse_on)
            {
                int sub_x = mouse_old_x - e.X;
                int sub_y = mouse_old_y - e.Y;
                double angle_x = Math.PI * sub_x / 180.0;
                double angle_y = Math.PI * sub_y / 180.0;
                if (Math.Abs(sub_x) < 1 && Math.Abs(sub_y) < 1) return;
                camera.eyeRotateAxis(new Vector3d(0, 0, 0), angle_x, camera.getUp());
                camera.eyeRotateAxis(new Vector3d(0, 0, 0), angle_y, camera.getUp().cross(camera.getEye() - camera.getCenter()));
                mouse_old_x = e.X;
                mouse_old_y = e.Y;

                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glLoadIdentity();
                Glu.gluPerspective(45, this.Width / this.Height, 1, 1000);
                camera.gluLookAtLH();
                Gl.glMatrixMode(Gl.GL_MODELVIEW);

                this.Invalidate();

            }
            else
            {
                foreach (CLoadedObject obj in model_manager.getModelList())
                {
                    obj.state.onMouseLeave(obj,e);
                }
                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glPushMatrix();
                Gl.glMatrixMode(Gl.GL_MODELVIEW);
                Gl.glPushMatrix();
                int[] viewport = new int[4];
                Gl.glViewport(0, 0, this.Width, this.Height);
                Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);
                TransformMatrixes trans = new TransformMatrixes();
                SelectionData selected = Selection.GetNearest(Selection.Pick<CLoadedObject>(model_manager.getSelectionDict(), new double[] { e.X, this.Height - e.Y - 1 }, new double[] { 1, 1 }, camera,
                    viewport, false, out trans));
                if (selected != null)
                {
                    (selected.item as CLoadedObject).state.onMouseOver(selected.item,e);
                    // (selected.item as CLoadedObject).changeState(new Jusin.ObjectModel.State.ModelStateSelected());

                }

                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glPopMatrix();
                Gl.glMatrixMode(Gl.GL_MODELVIEW);
                Gl.glPopMatrix();
               
            }
        }
        private void onModelsChange(object sender, EventArgs e)
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
