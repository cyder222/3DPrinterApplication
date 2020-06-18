using System;
using System.Collections.Generic;
using System.Text;

using Tao.OpenGl;
using Jusin.ThreeDLib.ModelBase;
using Jusin.ThreeDLib.ObjectRender;
using Jusin.ObjectModel.State;
using Jusin.ThreeDLib;
namespace Jusin.ObjectModel
{
    /// <summary>
    /// CLoaded Object.ロードされたモデルを表す。 
    /// </summary>
    public class CLoadedObject : ISelectable
    {
        public List<ModelEntityBase> entities;
        public float _vx,_vy,_vz; //現在の位置
        public float _x_dir,_y_dir,_z_dir;//現在の向き(回転角度)
        public float _maged; //オブジェクトの拡大率(1.0で等倍)
        protected CBBox bbox;
        protected IObjectRender renderer;
        public event EventHandler modelChange;
        public Vector3d[] bbox_point;
        public event EventHandler ObjectChange;
        public ModelState state;
        bool selectable = false;
        uint select_name;
        public int glDisplayList;
        public static RenderForSelection select_render = new RenderForSelection();
        /// <summary>
        /// ISelectableで使う内部描画関数。この表示をもとにセレクトする
        /// </summary>
        /// <param name="name"></param>
        public void DrawSceneForSelectionMode(uint name)
        {
            
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glPushMatrix();
           
           
            Gl.glTranslatef(vx, vy, vz);
            Gl.glRotatef(x_dir, 1.0f, 0.0f, 0.0f);
            Gl.glRotatef(y_dir, 0.0f, 1.0f, 0.0f);
            Gl.glRotatef(z_dir, 0.0f, 0.0f, 1.0f);
            Gl.glScalef(maged, maged, maged);
            foreach (ModelEntityBase entity in this.entities)
                select_render.Render(entity);
            Gl.glLoadIdentity();
            Gl.glPopMatrix();
        }
        public bool Selectable{
            get{
                return this.selectable;
            }
            set{
                selectable = value;
            }
        }
       public uint name{
            get{
                return select_name;
            }
            set{
                select_name = value;
            }
        }
        public ISelectable GettHitObject(SelectionData selectionData)
        {
            return this;
        }
        public void changeState(ModelState new_state)
        {
            this.state = new_state;
            ObjectChangeEvent();
        }
        protected Vector3d center_point;
        public CBBox getBBox()
        {
            if (bbox != null)
                return bbox;
            
            double max_x = 0,min_x = 0;
            double max_y = 0,min_y = 0;
            double max_z = 0,min_z = 0;
            bool first=true;
            foreach (ModelEntityBase entity in this.entities)
            {
                foreach (Vertex vert in entity.vertices)
                {
                    if (first)
                    {
                        max_x = min_x = vert.v.x;
                        max_y = min_y = vert.v.y;
                        max_z = min_z = vert.v.z;
                        first = false;
                    }
                    else
                    {
                        if (vert.v.x > max_x)
                            max_x = vert.v.x;
                        if (vert.v.x < min_x)
                            min_x = vert.v.x;
                        if (vert.v.y > max_y)
                            max_y = vert.v.y;
                        if (vert.v.y < min_y)
                            min_y = vert.v.y;
                        if (vert.v.z > max_z)
                            max_z = vert.v.z;
                        if (vert.v.z < min_z)
                            min_z = vert.v.z;
                    }

                }
            }
            Vector3d center = new Vector3d(maged*(max_x + min_x) / 2.0, maged*(max_y + min_y) / 2.0, maged*(max_z + min_z) / 2.0);
           
           
            
            Vector3d size = new Vector3d(maged*(max_x - min_x), maged*(max_y - min_y), maged*(max_z - min_z));

            bbox = new CBBox(center, size);

            return bbox;
        }
        public Vector3d getCenterPoint()
        {
            if (this.center_point == null)
            {
                Vector3d center = new Vector3d(0,0,0);
                
                foreach (ModelEntityBase entity in this.entities)
                {
                    foreach (Vertex vert in entity.vertices)
                    {
                        center += vert.v;
                    }
                    center.x = center.x / entity.vertices.Length;
                    center.y = center.y / entity.vertices.Length;
                    center.z = center.z / entity.vertices.Length;
                }
                this.center_point = center;
            }
           
            return center_point+(new Vector3d(vx,vy,vz));
        }
        protected void ObjectChangeEvent()
        {
            if (modelChange != null)
                modelChange(this, EventArgs.Empty);
        }
        public float vx{
            set{
                if (_vx != value)
                {
                    _vx = value;
                    ObjectChangeEvent();
                }
            }
            get{
                return _vx;
            }
        }
         public float vy{
            set{
                if (_vy != value)
                {
                    _vy = value;
                    ObjectChangeEvent();
                }
            }
            get{
                return _vy;
            }
        }
        public float vz{
            set{
                if (_vz != value)
                {
                    _vz = value;
                    ObjectChangeEvent();
                }
            }
            get{
                return _vz;
            }
        }
        public float x_dir{
            set{
                if (_x_dir != value)
                {
                    _x_dir = value;
                    ObjectChangeEvent();
                }
            }
            get{
                return _x_dir;
            }
        }
         public float y_dir{
            set{
                if (_y_dir != value)
                {
                    _y_dir = value;
                    ObjectChangeEvent();
                }
            }
            get{
                return _y_dir;
            }
        }
         public float z_dir{
            set{
                if (_z_dir != value)
                {
                    _z_dir = value;
                    ObjectChangeEvent();
                }
            }
            get{
                return _z_dir;
            }
        }
         public float maged{
            set{
                _maged = value;
                ObjectChangeEvent();
            }
            get{
                return _maged;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="render"></param>
        public void setRender(IObjectRender render){
            this.renderer = render;
            ObjectChangeEvent();
        }
        public CLoadedObject(List<ModelEntityBase> entities)
        {
            this.entities = entities;
            vx = vy = vz = 0.0f;
            x_dir = y_dir = z_dir = 0.0f;
            maged = 1.0f;
            bbox_point = null;
            this.state = new ModelStateNormal();
            this.center_point = null;
            int count = (int)(ModelStateType.count);
            this.glDisplayList = 0;//Gl.glGenLists(count);
            //int number = this.glDisplayList*(int)(ModelStateType.count) + (int)(this.getCurrentState());
           
        }
        public void Render()
        {
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glPushMatrix();
            Gl.glLoadIdentity();
           
           
            Gl.glTranslatef(vx, vy, vz);
            Gl.glRotatef(x_dir, 1.0f, 0.0f, 0.0f);
            Gl.glRotatef(y_dir, 0.0f, 1.0f, 0.0f);
            Gl.glRotatef(z_dir, 0.0f, 0.0f, 1.0f);
            Gl.glScalef(maged, maged, maged);
          /*  if (Gl.GL_TRUE == Gl.glIsList(1+this.glDisplayList*(int)(ModelStateType.count) + (int)(this.getCurrentState())))
            {
                int number = 1+this.glDisplayList*(int)(ModelStateType.count)  + (int)(this.getCurrentState());
                Gl.glCallList(number);
            }
            else if (this.glDisplayList != 0)
            {
                Gl.glNewList(1+this.glDisplayList*(int)(ModelStateType.count) + (int)(this.getCurrentState()), Gl.GL_COMPILE_AND_EXECUTE);


                foreach (ModelEntityBase entity in this.entities)
                    this.state.Render(entity);

                Gl.glEndList();
            }*/
            //else
            {
                foreach (ModelEntityBase entity in this.entities)
                    this.state.Render(entity);
            }
            Gl.glPopMatrix();
        }
        public RenderKind getRenderKind()
        {
            return this.renderer.getRenderKind();
        }
        public ModelStateType getCurrentState()
        {
            return this.state.getCurrentState();
        }
        public void clearCallList()
        {
            if (this.glDisplayList != 0)
            {
                Gl.glDeleteLists(this.glDisplayList * (int)(ModelStateType.count), (int)(ModelStateType.count));
            }
        }
       /* public Vector3d[] getBboxPoint()
        {
            if (bbox_point == null)
            {
                this.updateBboxPoint();     
            }
            return this.bbox_point;
        }
        public void updateBboxPoint()
        {
            //this.bbox_point = this.model.getBboxPoint();
        }
        public bool checkColl(ICollisionObject model)
        {
            Vector3d[] points = model.getBboxPoint();
            return false;
        }
        public bool checkColl(Vector3d point)
        {
            List<Entity> entities = ((Model)this.model).Entities;
            foreach (Entity e in entities)
            {
                foreach (Triangle tri in e.indices)
                {
                    Vector3d p1 = new Vector3d(e.vertices[tri.vertex1]);
                    Vector3d p2 = new Vector3d(e.vertices[tri.vertex2]);
                    Vector3d p3 = new Vector3d(e.vertices[tri.vertex3]);
                    Vector3d[] vertices = new Vector3d[] { p1, p2, p3 };
                    if (MyUtil.PointInTriangle(vertices, point))
                    {
                        return true;
                    }
                }
            }
            /*Vector3d[] points = model.getBboxPoint();
            if (points[0].x <= point.x && points[0].y <= point.y && points[0].z <= point.z)
                if (points[1].x >= point.x && points[1].y >= point.y && points[1].z >= point.z)
                    return true;
             */
            
          /*  return false;
        }
        */

    }
}
