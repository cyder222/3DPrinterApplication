using System;
using System.Collections.Generic;
using System.Text;
using Jusin.ThreeDLib;
using Tao.OpenGl;
using Tao.FreeGlut;
namespace Jusin.ThreeDLib.ObjectRender
{
    enum RenderKind
    {
        NormalRender
    }
    public interface IObjectRender
    {
        void Render(ModelBase.ModelEntityBase entities);
        RenderKind getRenderKind();
    }
    public class NormalObjectRender : IObjectRender
    {
        void Render(ModelBase.ModelEntityBase entities)
        {
            if (entities.triangles == null || entities.triangles.Length == 0) return;



            // Draw every triangle in the entity
            Gl.glBegin(Gl.GL_TRIANGLES);
            int number = 0;
            foreach (ModelBase.Triangle tri in entities.triangles)
            {
                //material = material_DIC[tri];
                if (tri.material != null)
                {
                    ModelBase.Material material = tri.material;
                    float trans = (float)material.Trans / 100f;
                    float[] ambient = new float[] { material.Ambient[0], material.Ambient[1], material.Ambient[2], trans };
                    float[] diffuse = new float[] { material.Diffuse[0], material.Diffuse[1], material.Diffuse[2], trans };
                    float[] specular = new float[] { material.Specular[0], material.Specular[1], material.Specular[2], trans };
                    Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT, ambient);
                    Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_DIFFUSE, diffuse);
                    Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, specular);
                    Gl.glColor3fv(diffuse);
                    Gl.glMaterialf(Gl.GL_FRONT_AND_BACK, Gl.GL_SHININESS, material.Shininess);


                    if (material.TextureId >= 0)
                    {
                        Gl.glBindTexture(Gl.GL_TEXTURE_2D, material.TextureId);
                        Gl.glEnable(Gl.GL_TEXTURE_2D);
                    }
                }
                // Vertex 1
                if (entities.normed) Gl.glNormal3d(tri.v1.norm.x, tri.v1.norm.y, tri.v1.norm.z);
                if (material != null && material.TextureId >= 0) Gl.glTexCoord2f(tri.v1.tex_coord.U, tri.v1.tex_coord.V);
                Gl.glVertex3d(tri.v1.x, tri.v1.y, tri.v1.z);

                // Vertex 2
                if (entities.normed) Gl.glNormal3d(tri.v2.norm.x, tri.v2.norm.y, tri.v2.norm.z);
                if (material != null && material.TextureId >= 0) Gl.glTexCoord2f(tri.v2.tex_coord.U, tri.v2.tex_coord.V);
                Gl.glVertex3d(tri.v2.x, tri.v2.y, tri.v2.z);
                // Vertex 3
                if (entities.normed) Gl.glNormal3d(tri.v3.norm.x, tri.v3.norm.y, tri.v3.norm.z);
                if (material != null && material.TextureId >= 0) Gl.glTexCoord2f(tri.v3.tex_coord.U, tri.v3.tex_coord.V);
                Gl.glVertex3d(tri.v3.x, tri.v3.y, tri.v3.z);
            }

            Gl.glEnd();
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            //Console.WriteLine ( Gl.glGetError () );
        }
        RenderKind getRenderKind()
        {
            return RenderKind.NormalRender;
        }
    }
}
