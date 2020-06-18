using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenGl;
using Tao.FreeGlut;
namespace Jusin.ThreeDLib.ObjectRender
{
    public class RenderForSelection : IObjectRender
    {
        private static RenderForSelection instance = null;
        public static RenderForSelection getRenderForSelection()
        {
            if (instance == null)
                instance = new RenderForSelection();
            return instance;
        }
        public void Render(ModelBase.ModelEntityBase entities)
        {
            if (entities.triangles == null || entities.triangles.Length == 0) return;




            // Draw every triangle in the entity
            Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);
            
            int number = 0;
            foreach (ModelBase.Triangle tri in entities.triangles)
            {
                Gl.glBegin(Gl.GL_TRIANGLES);
                // Vertex 1
                if (entities.normed) Gl.glNormal3d(tri.v1.norm.x, tri.v1.norm.y, tri.v1.norm.z);

                Gl.glVertex3d(tri.v1.v.x, tri.v1.v.y, tri.v1.v.z);

                // Vertex 2
                if (entities.normed) Gl.glNormal3d(tri.v2.norm.x, tri.v2.norm.y, tri.v2.norm.z);
              //  if (material != null && material.TextureId >= 0) Gl.glTexCoord2f(tri.v2.tex_coord.U, tri.v2.tex_coord.V);
                Gl.glVertex3d(tri.v2.v.x, tri.v2.v.y, tri.v2.v.z);
                // Vertex 3
                if (entities.normed) Gl.glNormal3d(tri.v3.norm.x, tri.v3.norm.y, tri.v3.norm.z);
                //if (material != null && material.TextureId >= 0) Gl.glTexCoord2f(tri.v3.tex_coord.U, tri.v3.tex_coord.V);
                Gl.glVertex3d(tri.v3.v.x, tri.v3.v.y, tri.v3.v.z);
                Gl.glEnd();
            }
          
        }
        public RenderKind getRenderKind()
        {
            return RenderKind.SelectionRender;
        }
    }
}
