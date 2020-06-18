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
    public class CLoadedObject : ISelectable
    {
        public List<ModelEntityBase> entities;
        public float _vx,_vy,_vz; //åªç›ÇÃà íu
        public float _x_dir,_y_dir,_z_dir;//åªç›ÇÃå¸Ç´(âÒì]äpìx)
        public float _maged; //ÉIÉuÉWÉFÉNÉgÇÃägëÂó¶(1.0Ç≈ìôî{)
        protected IObjectRender renderer;
        public event EventHandler modelChange;
        public Vector3d[] bbox_point;
        public event EventHandler ObjectChange;
        public ModelState state;
        bool selectable = false;
        uint select_name;
        public static RenderForSelection select_render = new RenderForSelection();
        public void DrawSceneForSelectionMode(uint name)
        {
            foreach(ModelEntityBase entity in this.entities)
                select_render.Render(entity);
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
        protected void ObjectChangeEvent()
        {
            if (modelChange != null)
                modelChange(this, EventArgs.Empty);
        }
        public float vx{
            set{
                _vx = value;
                ObjectChangeEvent();
            }
            get{
                return _vx;
            }
        }
         public float vy{
            set{
                _vy = value;
                ObjectChangeEvent();
            }
            get{
                return _vy;
            }
        }
        public float vz{
            set{
                _vz = value;
                ObjectChangeEvent();
            }
            get{
                return _vz;
            }
        }
        public float x_dir{
            set{
                _x_dir = value;
                ObjectChangeEvent();
            }
            get{
                return _x_dir;
            }
        }
         public float y_dir{
            set{
                _y_dir = value;
                ObjectChangeEvent();
            }
            get{
                return _y_dir;
            }
        }
         public float z_dir{
            set{
                _z_dir = value;
                ObjectChangeEvent();
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
        }
        public void Render()
        {
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glPushMatrix();
            Gl.glLoadIdentity();
            Gl.glRotatef(x_dir,1.0f,0.0f,0.0f);
            Gl.glRotatef(y_dir, 0.0f, 1.0f, 0.0f);
            Gl.glRotatef(z_dir, 0.0f, 0.0f, 1.0f);
            Gl.glTranslatef(vx, vy, vz);
            foreach(ModelEntityBase entity in this.entities)
                this.state.Render(entity);
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
