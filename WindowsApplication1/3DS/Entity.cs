// Title:	Entity.cs
// Author: 	Scott Ellington <scott.ellington@gmail.com>
//
// Copyright (C) 2006 Scott Ellington and authors
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using Tao.OpenGl;
using Jusin.Camera;
namespace ThreeD
{
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
	
	public class Entity
	{
		// TODO: OO this
		// fields should be private
		// constructor with verts and faces
		// normalize in ctor
		
		public Material material = new Material ();
       public  Dictionary<Triangle, Material> material_DIC = new Dictionary<Triangle, Material>();
        //The materials (multi sub object)
        public Material[] materials;
        // Material coordinates to indices
        public ushort[] mat_to_indices;
		// The stored vertices 
		public Vector[] vertices;

		// The calculated normals
		public Vector[] normals;
		
		// The indices of the triangles which point to vertices
		public Triangle[] indices;

		// The coordinates which map the texture onto the entity
		public TexCoord[] texcoords;
	
		bool normalized = false;
		
		public void CalculateNormals ()
		{
			if ( indices == null ) return;
			
			normals = new Vector [vertices.Length];

			Vector[] temps = new Vector [ indices.Length ];

			for ( int ii=0 ; ii < indices.Length ; ii++ )
			{
				Triangle tr = indices [ii];
				
				Vector v1 = vertices [ tr.vertex1 ] - vertices  [ tr.vertex2 ];
				Vector v2 = vertices [ tr.vertex2 ] - vertices  [ tr.vertex3 ];

				temps [ii] = v1.CrossProduct ( v2 );
				//Console.Write ("I");
			}

			for ( int ii = 0; ii < vertices.Length ; ii++ )
			{
				Vector v = new Vector ();
				int shared = 0;

				for ( int jj = 0; jj < indices.Length ; jj++ )
				{
					Triangle tr = indices [jj];
					if ( tr.vertex1 == ii || tr.vertex2 == ii || tr.vertex3 == ii )
					{
						v += temps [jj];
						shared++;
					}
				}

				normals [ii] = (v / shared).Normalize ();
			}
			//Console.WriteLine ( "Normals Calculated!" );
			normalized = true;
		}
        public void sortIndices(CCamera camera)
        {
            foreach (Triangle tri in indices)
            {
                tri.calcDist(vertices, camera); 
            }
            Array.Sort(indices);
        }
		public void Render ()
		{
			if ( indices == null ) return;

			

			// Draw every triangle in the entity
			Gl.glBegin ( Gl.GL_TRIANGLES);
            int number = 0;
			foreach ( Triangle tri in indices )
			{
                material = materials[mat_to_indices[number++]];
                //material = material_DIC[tri];
                if (material != null)
                {
                    float trans = (float)material.Trans / 100f;
                    float[] ambient = new float[] { material.Ambient[0], material.Ambient[1], material.Ambient[2], trans };
                    float[] diffuse = new float[] { material.Diffuse[0], material.Diffuse[1], material.Diffuse[2], trans };
                    float[] specular = new float[] { material.Specular[0], material.Specular[1], material.Specular[2], trans };
                    Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT,ambient);
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
				if (normalized) Gl.glNormal3d ( normals[tri.vertex1].X, normals[tri.vertex1].Y, normals[tri.vertex1].Z );
				if (material!=null&& material.TextureId >= 0 ) Gl.glTexCoord2f ( texcoords [ tri.vertex1 ].U, texcoords [ tri.vertex1 ].V);
				Gl.glVertex3d ( vertices[tri.vertex1].X, vertices[tri.vertex1].Y, vertices[tri.vertex1].Z );

				// Vertex 2
				if (normalized) Gl.glNormal3d ( normals[tri.vertex2].X, normals[tri.vertex2].Y, normals[tri.vertex2].Z );
				if (material!=null&& material.TextureId >= 0 ) Gl.glTexCoord2f ( texcoords [ tri.vertex2 ].U, texcoords [ tri.vertex2 ].V);
				Gl.glVertex3d ( vertices[tri.vertex2].X, vertices[tri.vertex2].Y, vertices[tri.vertex2].Z );

				// Vertex 3
				if (normalized) Gl.glNormal3d ( normals[tri.vertex3].X, normals[tri.vertex3].Y, normals[tri.vertex3].Z );
				if (material!=null&& material.TextureId >= 0 ) Gl.glTexCoord2f( texcoords [ tri.vertex3 ].U, texcoords [ tri.vertex3 ].V);
				Gl.glVertex3d ( vertices[tri.vertex3].X, vertices[tri.vertex3].Y, vertices[tri.vertex3].Z );
			}
			
			Gl.glEnd();
			Gl.glDisable( Gl.GL_TEXTURE_2D );
			//Console.WriteLine ( Gl.glGetError () );
		}	
	}
}
