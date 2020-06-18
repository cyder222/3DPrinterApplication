using System;
using System.Collections.Generic;
using System.Text;

using Tao.OpenGl;
namespace Jusin.ThreeDLib.ModelBase
{
    /**
     * Texture Coordinate struct
     * **/
	public struct TexCoord 
	{
		public float U;
		public float V;

		public TexCoord ( float u , float v )
		{
			U = u;
			V = v;
		}
	}
    /**
     * Vertex Class
     * **/
    public class Vertex
    {
        public Vector3d v;
        public Vector3d norm;
        public TexCoord tex_coord;
        public Vertex()
        {
            v = new Vector3d();
            norm = new Vector3d();
        }
        public Vertex(float x,float y ,float z)
        {
            v = new Vector3d();
            norm = new Vector3d();
            v.x = x;
            v.y = y;
            v.z = z;
        }
    }
    public class Triangle
    {
        public Vertex v1;
        public Vertex v2;
        public Vertex v3;
        public Material material;
        public Triangle(Vertex v1, Vertex v2, Vertex v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }
    }
    public class Material
    {
        public float[] Ambient;
        public float[] Diffuse;
        public float[] Specular;
        public int Trans;
        public int Shininess;
        protected int texture_id;
        public int TextureId
        {
            get
            {
                return texture_id;
            }
            
        }
        public void setTextureIdForCopy(int texture_id)
        {
            this.texture_id = texture_id;
        }
        public void BindTexture(int width, int height, IntPtr data)
        {
            Gl.glEnable(Gl.GL_TEXTURE_2D);

            int[] textures = new int[1];
            Gl.glGenTextures(1, textures);
            texture_id = textures[0];
            //Console.WriteLine ( "GL Texture number: {0}", textures [0] );
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture_id);

            // repeat texture if neccessary
            //Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_CLAMP); 
            //Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_CLAMP);
            //Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);

            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            //Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST); 
            //Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST); 
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR_MIPMAP_NEAREST);

            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_REPLACE);

            // Finally we define the 2d texture
            //Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, width, height, 0, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, data);
            //Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA8, width, height, 0, Gl.GL_BGRA_EXT, Gl.GL_UNSIGNED_BYTE, data );
            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA8, width, height, 0, Gl.GL_BGRA_EXT, Gl.GL_UNSIGNED_BYTE, data);

            // And create 2d mipmaps for the minifying function
            Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, 4, width, height, Gl.GL_BGRA_EXT, Gl.GL_UNSIGNED_BYTE, data);

            Gl.glDisable(Gl.GL_TEXTURE_2D);
        }
    }
    public class ModelEntityBase
    {
        public Triangle[] triangles;
        public Vertex[] vertices;
        public Material[] materials;
        public bool normed = false;
        public void CalculateNormals()
        {
        }
        /**
         * 法線ベクトルを計算したかどうかを返却する
         * **/
        public bool isCalcedNorm()
        {
            return normed;
        }
    }


}
