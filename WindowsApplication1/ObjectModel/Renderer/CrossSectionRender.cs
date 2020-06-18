using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jusin.ThreeDLib.ModelBase;
using Tao.OpenGl;
using Tao.FreeGlut;
using Jusin.ObjectModel;
namespace Jusin.ThreeDLib.Scene.Render
{
    /**
     * 二枚のcrossSectionを用いてレンダリングする
     * */
    public class DoubleCrossSectionRender
    {
        Vector3d norm_vect1, norm_vect2;
        double D1, D2;
        
        double Dim1
        {
            get
            {
                return D1;
            }
            set
            {
                D1 = value;
            }
        }
        double Dim2
        {
            get
            {
                return D2;
            }
            set
            {
                D2 = value;
            }
        }
        public DoubleCrossSectionRender(double D1, double D2,Vector3d norm_vect1,
            Vector3d norm_vect2)
        {
            this.norm_vect1 = norm_vect1;
            this.norm_vect2 = norm_vect2;
            this.D1 = D1;
            this.D2 = D2;
            
        }
        public DoubleCrossSectionRender(double D1, double D2)
        {
            this.norm_vect1 = new Vector3d(0.0, 1.0, 0.0);
            this.norm_vect2 = new Vector3d(0.0, 1.0, 0.0);
            this.D1 = D1;
            this.D2 = D2;
            
        }
        public void Render(ObjectModel.CLoadedObject model)
        {
                                Gl.glEnable(Gl.GL_CLIP_PLANE0);
                               // Gl.glEnable(Gl.GL_CLIP_PLANE1);
                    Gl.glClipPlane(Gl.GL_CLIP_PLANE0, new double[] { -1 * norm_vect1.x, -1 * norm_vect1.y, -1 * norm_vect1.z, -1 * D1 });
                    Gl.glEnable(Gl.GL_STENCIL_TEST);
                    Gl.glClear(Gl.GL_STENCIL_BUFFER_BIT);
                    Gl.glDisable(Gl.GL_DEPTH_TEST);
                    Gl.glColorMask(Gl.GL_FALSE, Gl.GL_FALSE, Gl.GL_FALSE, Gl.GL_FALSE);
                    Gl.glClipPlane(Gl.GL_CLIP_PLANE1, new double[] { norm_vect2.x, norm_vect2.y, norm_vect2.z, D2 });
                    // first pass: increment stencil buffer value on back faces
                    Gl.glStencilFunc(Gl.GL_ALWAYS, 0, 0);
                    Gl.glStencilOp(Gl.GL_KEEP, Gl.GL_KEEP, Gl.GL_INCR);
                    Gl.glCullFace(Gl.GL_FRONT); // render back faces only
                    model.Render();
                    /*Gl.glPushMatrix();
                    Gl.glLoadIdentity();
                    Gl.glTranslatef(model.vx, model.vy, model.vz);
                    Gl.glRotatef(model.x_dir, 1.0f, 0.0f, 0.0f);
                    Gl.glRotatef(model.y_dir, 0.0f, 1.0f, 0.0f);
                    Gl.glRotatef(model.z_dir, 0.0f, 0.0f, 1.0f);
                    
                    trianglesRender(tri_list.ToArray(), material, entities);
                    Gl.glPopMatrix();*/
                    // second pass: decrement stencil buffer value on front faces
                    Gl.glStencilOp(Gl.GL_KEEP, Gl.GL_KEEP, Gl.GL_DECR);
                    Gl.glCullFace(Gl.GL_BACK); // render front faces only
                    //Gl.glEnable(Gl.GL_CLIP_PLANE1);        
            /*Gl.glPushMatrix();
                    Gl.glLoadIdentity();
                    Gl.glTranslatef(model.vx, model.vy, model.vz);
                    Gl.glRotatef(model.x_dir, 1.0f, 0.0f, 0.0f);
                    Gl.glRotatef(model.y_dir, 0.0f, 1.0f, 0.0f);
                    Gl.glRotatef(model.z_dir, 0.0f, 0.0f, 1.0f);
                    
                    trianglesRender(tri_list.ToArray(), material, entities);
                    Gl.glPopMatrix();*/
                  //  Gl.glDisable(Gl.GL_CLIP_PLANE0);
                    //Gl.glDisable(Gl.GL_CLIP_PLANE1);
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
                    Gl.glDisable(Gl.GL_CLIP_PLANE1);
                    Gl.glStencilFunc(Gl.GL_NOTEQUAL, 0, ~0);
                    // stencil test will pass only when stencil buffer value = 0; 
                    // (~0 = 0x11...11)
                    /*Gl.glBegin(Gl.GL_QUADS); // rendering the plane quad. Note, it should be big 
                    // enough to cover all clip edge area.
                    float[][] verts = new float[][]{new float[]{1000f,5.0f,1000f},new float[]{-1000.0f,1000.0f,1000.0f},new float[]{-1000.0f,1000.0f,-1000.0f},new float[]{1000.0f,1000.0f,-1000.0f}};
                    for(int j=3; j>=0; j--) Gl.glVertex3fv(verts[j]);
                    Gl.glEnd();*/
                    //Gl.glCullFace(Gl.GL_BACK); // render front faces only

                    //****** End rendering mesh's clip edge ****/
                    //Now that the clip edge image has been rendered to the color and depth buffers, the final step is to render the earth surface mesh with the stencil test disabled.
                    /*Material material = model.entities[0].materials[0];
                    if (material.TextureId >= 0)
                    {
                        Gl.glEnable(Gl.GL_TEXTURE_2D);
                        Gl.glBindTexture(Gl.GL_TEXTURE_2D, material.TextureId);


                    }
                    

                    float trans = (float)material.Trans / 100f;
                    float[] ambient = new float[] { material.Ambient[0], material.Ambient[1], material.Ambient[2], trans };
                    float[] diffuse = new float[] { material.Diffuse[0], material.Diffuse[1], material.Diffuse[2], trans };
                    float[] specular = new float[] { material.Specular[0], material.Specular[1], material.Specular[2], trans };
                    Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT, ambient);
                    Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_DIFFUSE, diffuse);
                    Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, specular);
            
                    Gl.glColor3fv(diffuse);
                    Gl.glMaterialf(Gl.GL_FRONT_AND_BACK, Gl.GL_SHININESS, material.Shininess);
            */
                    float[] red = { 0.8f, 0.2f, 0.2f, 1.0f };
                    Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, red);
                    Gl.glEnable(Gl.GL_LIGHTING);
                    Gl.glPushMatrix();
                    Gl.glLoadIdentity();
                    Gl.glTranslatef(model.vx, model.vy, model.vz);
                    Gl.glRotatef(model.x_dir, 1.0f, 0.0f, 0.0f);
                    Gl.glRotatef(model.y_dir, 0.0f, 1.0f, 0.0f);
                    Gl.glRotatef(model.z_dir, 0.0f, 0.0f, 1.0f);
                    model.getBBox().DrawWire();
                    Gl.glPopMatrix();

                    //****** Rendering mesh *********// 
                    Gl.glDisable(Gl.GL_STENCIL_TEST);
                    Gl.glEnable(Gl.GL_CLIP_PLANE0); // enabling clip plane again
                    Gl.glEnable(Gl.GL_CLIP_PLANE1);

                    //Gl.glColorMask(Gl.GL_TRUE, Gl.GL_TRUE, Gl.GL_TRUE, Gl.GL_TRUE);
                    Gl.glEnable(Gl.GL_DEPTH_TEST);
                    //Gl.glDisable(Gl.GL_LIGHTING);
                    //Gl.glClipPlane(Gl.GL_CLIP_PLANE0);
                    /*Gl.glPushMatrix();
                    Gl.glLoadIdentity();
                    Gl.glTranslatef(model.vx, model.vy, model.vz);
                    Gl.glRotatef(model.x_dir, 1.0f, 0.0f, 0.0f);
                    Gl.glRotatef(model.y_dir, 0.0f, 1.0f, 0.0f);
                    Gl.glRotatef(model.z_dir, 0.0f, 0.0f, 1.0f);
                    *///trianglesRender(entities.triangles,material,entities);
                   // model.Render();
                    //Gl.glPopMatrix();

                    Gl.glDisable(Gl.GL_TEXTURE_2D);
              
        }

        public void RenderWithMaterial(ObjectModel.CLoadedObject model)
        {
            foreach (ModelEntityBase entities in model.entities)
            {
                if (entities.triangles == null || entities.triangles.Length == 0) continue;
                foreach (ModelBase.Material material in entities.materials)
                {
                                        List<Triangle> tri_list = new List<Triangle>();
                    foreach (ModelBase.Triangle tri in entities.triangles)
                    {
                        if (tri.material == material)
                        {
                            tri_list.Add(tri);
                        }
                    }
                    Gl.glEnable(Gl.GL_CLIP_PLANE0);
                    //Gl.glEnable(Gl.GL_CLIP_PLANE1);
                    Gl.glClipPlane(Gl.GL_CLIP_PLANE0, new double[] { -1 * norm_vect1.x, -1 * norm_vect1.y, -1 * norm_vect1.z, -1 * D1 });
                    Gl.glEnable(Gl.GL_STENCIL_TEST);
                    Gl.glClear(Gl.GL_STENCIL_BUFFER_BIT);
                    Gl.glDisable(Gl.GL_DEPTH_TEST);
                    Gl.glColorMask(Gl.GL_FALSE, Gl.GL_FALSE, Gl.GL_FALSE, Gl.GL_FALSE);
                    Gl.glClipPlane(Gl.GL_CLIP_PLANE1, new double[] { norm_vect2.x, norm_vect2.y, norm_vect2.z, D2 });
                    // first pass: increment stencil buffer value on back faces
                    Gl.glStencilFunc(Gl.GL_ALWAYS, 0, 0);
                    Gl.glStencilOp(Gl.GL_KEEP, Gl.GL_KEEP, Gl.GL_INCR);
                    Gl.glCullFace(Gl.GL_FRONT); // render back faces only
                    model.Render();
                    /*Gl.glPushMatrix();
                    Gl.glLoadIdentity();
                    Gl.glTranslatef(model.vx, model.vy, model.vz);
                    Gl.glRotatef(model.x_dir, 1.0f, 0.0f, 0.0f);
                    Gl.glRotatef(model.y_dir, 0.0f, 1.0f, 0.0f);
                    Gl.glRotatef(model.z_dir, 0.0f, 0.0f, 1.0f);
                    
                    trianglesRender(tri_list.ToArray(), material, entities);
                    Gl.glPopMatrix();*/
                    // second pass: decrement stencil buffer value on front faces
                    Gl.glStencilOp(Gl.GL_KEEP, Gl.GL_KEEP, Gl.GL_DECR);
                    Gl.glCullFace(Gl.GL_BACK); // render front faces only
                    /*Gl.glPushMatrix();
                    Gl.glLoadIdentity();
                    Gl.glTranslatef(model.vx, model.vy, model.vz);
                    Gl.glRotatef(model.x_dir, 1.0f, 0.0f, 0.0f);
                    Gl.glRotatef(model.y_dir, 0.0f, 1.0f, 0.0f);
                    Gl.glRotatef(model.z_dir, 0.0f, 0.0f, 1.0f);
                    
                    trianglesRender(tri_list.ToArray(), material, entities);
                    Gl.glPopMatrix();*/
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

                    //****** End rendering mesh's clip edge ****/
                    //Now that the clip edge image has been rendered to the color and depth buffers, the final step is to render the earth surface mesh with the stencil test disabled.
                    if (material.TextureId >= 0)
                    {
                        Gl.glEnable(Gl.GL_TEXTURE_2D);
                        Gl.glBindTexture(Gl.GL_TEXTURE_2D, material.TextureId);


                    }
                    

                    float trans = (float)material.Trans / 100f;
                    float[] ambient = new float[] { material.Ambient[0], material.Ambient[1], material.Ambient[2], trans };
                    float[] diffuse = new float[] { material.Diffuse[0], material.Diffuse[1], material.Diffuse[2], trans };
                    float[] specular = new float[] { material.Specular[0], material.Specular[1], material.Specular[2], trans };
                    Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT, ambient);
                    Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_DIFFUSE, diffuse);
                    Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, specular);

                    Gl.glColor3fv(diffuse);
                    Gl.glMaterialf(Gl.GL_FRONT_AND_BACK, Gl.GL_SHININESS, material.Shininess);

                    Gl.glEnable(Gl.GL_LIGHTING);
                    Gl.glPushMatrix();
                    Gl.glLoadIdentity();
                    Gl.glTranslatef(model.vx, model.vy, model.vz);
                    Gl.glRotatef(model.x_dir, 1.0f, 0.0f, 0.0f);
                    Gl.glRotatef(model.y_dir, 0.0f, 1.0f, 0.0f);
                    Gl.glRotatef(model.z_dir, 0.0f, 0.0f, 1.0f);
                    model.getBBox().DrawWire();
                    Gl.glPopMatrix();

                    //****** Rendering mesh *********// 
                    Gl.glDisable(Gl.GL_STENCIL_TEST);
                    Gl.glEnable(Gl.GL_CLIP_PLANE0); // enabling clip plane again
                    Gl.glEnable(Gl.GL_CLIP_PLANE1);

                    //Gl.glColorMask(Gl.GL_TRUE, Gl.GL_TRUE, Gl.GL_TRUE, Gl.GL_TRUE);
                    Gl.glEnable(Gl.GL_DEPTH_TEST);
                    //Gl.glDisable(Gl.GL_LIGHTING);
                    //Gl.glClipPlane(Gl.GL_CLIP_PLANE0);
                    /*Gl.glPushMatrix();
                    Gl.glLoadIdentity();
                    Gl.glTranslatef(model.vx, model.vy, model.vz);
                    Gl.glRotatef(model.x_dir, 1.0f, 0.0f, 0.0f);
                    Gl.glRotatef(model.y_dir, 0.0f, 1.0f, 0.0f);
                    Gl.glRotatef(model.z_dir, 0.0f, 0.0f, 1.0f);
                    *///trianglesRender(entities.triangles,material,entities);
                    model.Render();
                    //Gl.glPopMatrix();

                    Gl.glDisable(Gl.GL_TEXTURE_2D);
              
                }
              

               
            }
        }
        protected void trianglesRender(Triangle[] tris,Material material,ModelEntityBase entities)
        {
            
            foreach (Triangle tri in tris)
            {
                Gl.glBegin(Gl.GL_TRIANGLES);
                // Vertex 1
                if (entities.normed) Gl.glNormal3d(tri.v1.norm.x, tri.v1.norm.y, tri.v1.norm.z);
                if (material != null && material.TextureId >= 0)
                    Gl.glTexCoord2f(tri.v1.tex_coord.U, tri.v1.tex_coord.V);
                Gl.glVertex3d(tri.v1.v.x, tri.v1.v.y, tri.v1.v.z);

                // Vertex 2
                if (entities.normed) Gl.glNormal3d(tri.v2.norm.x, tri.v2.norm.y, tri.v2.norm.z);
                if (material != null && material.TextureId >= 0) Gl.glTexCoord2f(tri.v2.tex_coord.U, tri.v2.tex_coord.V);
                Gl.glVertex3d(tri.v2.v.x, tri.v2.v.y, tri.v2.v.z);
                // Vertex 3
                if (entities.normed) Gl.glNormal3d(tri.v3.norm.x, tri.v3.norm.y, tri.v3.norm.z);
                if (material != null && material.TextureId >= 0) Gl.glTexCoord2f(tri.v3.tex_coord.U, tri.v3.tex_coord.V);
                Gl.glVertex3d(tri.v3.v.x, tri.v3.v.y, tri.v3.v.z);
                Gl.glEnd();
                
            }
        }

        protected void entitiesRender(ModelEntityBase entities)
        {
            foreach (ModelBase.Triangle tri in entities.triangles)
            {
                ModelBase.Material material = null;
                //material = material_DIC[tri];
                if (tri.material != null)
                {
                    material = tri.material;
                    if (material.TextureId >= 0)
                    {
                        Gl.glEnable(Gl.GL_TEXTURE_2D);
                        Gl.glBindTexture(Gl.GL_TEXTURE_2D, material.TextureId);


                    }
                    float trans = (float)material.Trans / 100f;
                    float[] ambient = new float[] { material.Ambient[0], material.Ambient[1], material.Ambient[2], trans };
                    float[] diffuse = new float[] { material.Diffuse[0], material.Diffuse[1], material.Diffuse[2], trans };
                    float[] specular = new float[] { material.Specular[0], material.Specular[1], material.Specular[2], trans };
                    Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT, ambient);
                    Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_DIFFUSE, diffuse);
                    Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, specular);

                    Gl.glColor3fv(diffuse);
                    Gl.glMaterialf(Gl.GL_FRONT_AND_BACK, Gl.GL_SHININESS, material.Shininess);



                }
                //@TODO
                //TextureIdでtriangleをまとめて、glBegin,glEndの数を減らす
                // 2012/09/11では、glBeginとEndの間にglBindがあるとエラーを起こすため回避
                Gl.glBegin(Gl.GL_TRIANGLES);
                // Vertex 1
                if (entities.normed) Gl.glNormal3d(tri.v1.norm.x, tri.v1.norm.y, tri.v1.norm.z);
                if (material != null && material.TextureId >= 0)
                    Gl.glTexCoord2f(tri.v1.tex_coord.U, tri.v1.tex_coord.V);
                Gl.glVertex3d(tri.v1.v.x, tri.v1.v.y, tri.v1.v.z);

                // Vertex 2
                if (entities.normed) Gl.glNormal3d(tri.v2.norm.x, tri.v2.norm.y, tri.v2.norm.z);
                if (material != null && material.TextureId >= 0) Gl.glTexCoord2f(tri.v2.tex_coord.U, tri.v2.tex_coord.V);
                Gl.glVertex3d(tri.v2.v.x, tri.v2.v.y, tri.v2.v.z);
                // Vertex 3
                if (entities.normed) Gl.glNormal3d(tri.v3.norm.x, tri.v3.norm.y, tri.v3.norm.z);
                if (material != null && material.TextureId >= 0) Gl.glTexCoord2f(tri.v3.tex_coord.U, tri.v3.tex_coord.V);
                Gl.glVertex3d(tri.v3.v.x, tri.v3.v.y, tri.v3.v.z);
                Gl.glEnd();
                Gl.glDisable(Gl.GL_TEXTURE_2D);
            }
        }
      
        
    }
    
}
