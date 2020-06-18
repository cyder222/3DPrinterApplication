using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Jusin.ThreeDLib.ModelLoadPlugin;
using ThirdParty.ThreeDLib.ThreeD;
namespace Jusin.ThreeDLib.ModelLoadPlugin.ThreeDS
{
    public class ThreeDSLoader : IModelImportPlugin
    {

        public ThreeDSLoader()
        {
        }
        public List<Jusin.ThreeDLib.ModelBase.ModelEntityBase> importFromFile(String filename)
        {
            List<Jusin.ThreeDLib.ModelBase.ModelEntityBase> entities = new List<Jusin.ThreeDLib.ModelBase.ModelEntityBase>();
            Model model = new ThreeDSFile(filename).ThreeDSModel;
            foreach (ThirdParty.ThreeDLib.ThreeD.Entity e in model.Entities)
            {
                entities.Add(convertEntity(e));
            }
            return entities;

        }
        /**
         * ThreedLoader‚ÌEntity‚ðModelEntityBase‚É•ÏŠ·‚µ‚Ä•Ô‹p‚·‚é.
         * 
         * **/
        protected Jusin.ThreeDLib.ModelBase.ModelEntityBase convertEntity(ThirdParty.ThreeDLib.ThreeD.Entity e)
        {
            Jusin.ThreeDLib.ModelBase.ModelEntityBase e_base = new Jusin.ThreeDLib.ModelBase.ModelEntityBase();
            Jusin.ThreeDLib.ModelBase.Triangle[] new_triangles = new  Jusin.ThreeDLib.ModelBase.Triangle[e.indices.Length];
            Jusin.ThreeDLib.ModelBase.Vertex[] new_vertexes = new Jusin.ThreeDLib.ModelBase.Vertex[e.vertices.Length];
            Jusin.ThreeDLib.ModelBase.Material[] new_materials = new Jusin.ThreeDLib.ModelBase.Material[e.materials.Length];
             Jusin.ThreeDLib.ModelBase.TexCoord[] new_texcoords;
             if (e.texcoords != null)
                 new_texcoords = new Jusin.ThreeDLib.ModelBase.TexCoord[e.texcoords.Length];
             else
                 new_texcoords = null;
            int i =0;
            foreach (Vector vect in e.vertices)
            {
                Jusin.ThreeDLib.ModelBase.Vertex vert = new Jusin.ThreeDLib.ModelBase.Vertex((float)vect.X, (float)vect.Y, (float)vect.Z);
                new_vertexes[i] = vert;
                if (e.Normalized)
                {
                    vert.norm = new Jusin.ThreeDLib.ModelBase.Vector3d(e.normals[i].X,e.normals[i].Y,e.normals[i].Z);
                }
                
                i++;
            }
           
            i=0;
            foreach (Material mat in e.materials)
            {
                if (mat != null)
                {
                    new_materials[i] = new Jusin.ThreeDLib.ModelBase.Material();
                    new_materials[i].Ambient = mat.Ambient;
                    new_materials[i].Diffuse = mat.Diffuse;
                    new_materials[i].Shininess = mat.Shininess;
                    new_materials[i].Specular = mat.Specular;
                    new_materials[i].Trans = mat.Trans;
                    new_materials[i].setTextureIdForCopy(mat.TextureId);

                    i++;
                }
            }
          
            i=0;
            foreach (Triangle indice in e.indices)
            {
                Jusin.ThreeDLib.ModelBase.Triangle tri = new Jusin.ThreeDLib.ModelBase.Triangle(new_vertexes[indice.vertex1], new_vertexes[indice.vertex2], new_vertexes[indice.vertex3]);
                if(new_materials.Length!=0)
                    tri.material = new_materials[e.mat_to_indices[i]];
                new_triangles[i] = tri;
                i++;
            }
           
            i=0;
            if (new_texcoords != null)
            {
                foreach (TexCoord coord in e.texcoords)
                {
                    new_texcoords[i].U = coord.U;
                    new_texcoords[i].V = coord.V;
                    new_vertexes[i].tex_coord = new_texcoords[i];
                    i++;
                }
            }
            e_base.vertices = new_vertexes;
            e_base.materials = new_materials;
            e_base.triangles = new_triangles;
            e_base.normed = e.Normalized;
            return e_base;
        }
       

    }
}
