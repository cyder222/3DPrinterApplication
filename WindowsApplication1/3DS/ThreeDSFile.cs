// Title:	ThreeDSFile.cs
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
using System.IO;
using System.Text;
using System.Collections.Generic;

using System.Drawing;

namespace ThreeD
{
	public class ThreeDSFile
	{	
		enum Groups
		{
			C_PRIMARY      = 0x4D4D,
			C_OBJECTINFO   = 0x3D3D, 
			C_VERSION      = 0x0002,
			C_EDITKEYFRAME = 0xB000,          
			C_MATERIAL     = 0xAFFF,    
			C_MATNAME      = 0xA000, 
			C_MATAMBIENT   = 0xA010,
			C_MATDIFFUSE   = 0xA020,
			C_MATSPECULAR  = 0xA030,
			C_MATSHININESS = 0xA040,
            C_MAT_TRANSPARENCY = 0xA050,
			C_MATMAP       = 0xA200,
			C_MATMAPFILE   = 0xA300,
			C_OBJECT       = 0x4000,   
			C_OBJECT_MESH  = 0x4100,
			C_OBJECT_VERTICES    = 0x4110, 
			C_OBJECT_FACES       = 0x4120,
			C_OBJECT_MATERIAL    = 0x4130,
			C_OBJECT_UV		= 0x4140
		}

		class ThreeDSChunk
		{
			public ushort ID;
			public uint Length;
			public int BytesRead;

			public ThreeDSChunk ( BinaryReader reader )
			{
				// 2 byte ID
				ID = reader.ReadUInt16();
				//Console.WriteLine ("ID: {0}", ID.ToString("x"));

				// 4 byte length
				Length = reader.ReadUInt32 ();
				//Console.WriteLine ("Length: {0}", Length);

				// = 6
				BytesRead = 6; 
			}
		}

		BinaryReader reader;

		Model model = new Model ();
		public Model ThreeDSModel {
			get {
				return model;
			}
		}
		
		Dictionary < string, Material > materials = new Dictionary < string, Material > ();
		
		string base_dir;
		
		public ThreeDSFile ( string file_name )
		{
			base_dir =  new FileInfo ( file_name ).DirectoryName + "/";
			
			FileStream file;
			file = new FileStream(file_name, FileMode.Open, FileAccess.Read); 

			reader = new BinaryReader ( file );
			reader.BaseStream.Seek (0, SeekOrigin.Begin); 

			ThreeDSChunk chunk = new ThreeDSChunk ( reader );
			if ( chunk.ID != (short) Groups.C_PRIMARY )
				throw new ApplicationException ( "Not a proper 3DS file." );

			ProcessChunk ( chunk );

			reader.Close ();
			file.Close ();
		}

		void ProcessChunk ( ThreeDSChunk chunk )
		{
			while ( chunk.BytesRead < chunk.Length )
			{
				ThreeDSChunk child = new ThreeDSChunk ( reader );

				switch ((Groups) child.ID)
				{
					case Groups.C_VERSION:

						int version = reader.ReadInt32 ();
						child.BytesRead += 4;

						Console.WriteLine ( "3DS File Version: {0}", version );
						break;

					case Groups.C_OBJECTINFO:

						ThreeDSChunk obj_chunk = new ThreeDSChunk ( reader );

						// not sure whats up with this chunk
						SkipChunk ( obj_chunk );
						child.BytesRead += obj_chunk.BytesRead;

						ProcessChunk ( child );

						break;					

					case Groups.C_MATERIAL:

						ProcessMaterialChunk ( child );
						//SkipChunk ( child );
						break;

					case Groups.C_OBJECT:

						//SkipChunk ( child );
						string name = ProcessString ( child );
						Console.WriteLine ("OBJECT NAME: {0}", name);

						Entity e = ProcessObjectChunk ( child );
						e.CalculateNormals ();
						model.Entities.Add ( e );

						break;

					default:

						SkipChunk ( child );
						break;

				}

				chunk.BytesRead += child.BytesRead;
				//Console.WriteLine ( "ID: {0} Length: {1} Read: {2}", chunk.ID.ToString("x"), chunk.Length , chunk.BytesRead );
			}
		}

		void ProcessMaterialChunk ( ThreeDSChunk chunk )
		{
			string name = string.Empty;
			Material m = new Material ();
			
			while ( chunk.BytesRead < chunk.Length )
			{
				ThreeDSChunk child = new ThreeDSChunk ( reader );
				
				switch ((Groups) child.ID)
				{
					case Groups.C_MATNAME:

						name = ProcessString ( child );
						Console.WriteLine ( "Material: {0}", name );
						break;
				
					case Groups.C_MATAMBIENT:

						m.Ambient = ProcessColorChunk ( child );
						break;
						
					case Groups.C_MATDIFFUSE:

						m.Diffuse = ProcessColorChunk ( child );
						break;
						
					case Groups.C_MATSPECULAR:

						m.Specular = ProcessColorChunk ( child );
						break;
						
					case Groups.C_MATSHININESS:

						m.Shininess = ProcessPercentageChunk ( child );
						//Console.WriteLine ( "SHININESS: {0}", m.Shininess );
						break;
				    case Groups.C_MAT_TRANSPARENCY:
                        m.Trans = 100-ProcessPercentageChunk(child);
                        break;
					case Groups.C_MATMAP:

						ProcessPercentageChunk ( child );
	
						//SkipChunk ( child );
						ProcessTexMapChunk ( child , m );
						
						break;
						
					default:

						SkipChunk ( child );
						break;
				}
				chunk.BytesRead += child.BytesRead;
			}
			materials.Add ( name, m );
		}

		void ProcessTexMapChunk ( ThreeDSChunk chunk, Material m )
		{
			while ( chunk.BytesRead < chunk.Length )
			{
				ThreeDSChunk child = new ThreeDSChunk ( reader );
				switch ((Groups) child.ID)
				{
					case Groups.C_MATMAPFILE:

						string name = ProcessString ( child );
						Console.WriteLine ( "	Texture File: {0}", name );

						//FileStream fStream;
						Bitmap bmp;
						try 
						{
							//fStream = new FileStream(base_dir + name, FileMode.Open, FileAccess.Read);
							bmp = new Bitmap ( base_dir + name );
						}
						catch ( Exception e )
						{
							// couldn't find the file
							Console.WriteLine ( "	ERROR: could not load file '{0}'", base_dir + name );
							break;
						}

						// Flip image (needed so texture are the correct way around!)
						bmp.RotateFlip(RotateFlipType.RotateNoneFlipY); 
						
						System.Drawing.Imaging.BitmapData imgData = bmp.LockBits ( new Rectangle(new Point(0, 0), bmp.Size), 
								System.Drawing.Imaging.ImageLockMode.ReadOnly,
								System.Drawing.Imaging.PixelFormat.Format32bppArgb);								
//								System.Drawing.Imaging.PixelFormat.Format24bppRgb ); 
									
						m.BindTexture ( imgData.Width, imgData.Height, imgData.Scan0 );
						
						bmp.UnlockBits(imgData);
						bmp.Dispose();
						
						/*
						BinaryReader br = new BinaryReader(fStream);

						br.ReadBytes ( 14 ); // skip file header
					
						uint offset = br.ReadUInt32 (  );
						//br.ReadBytes ( 4 ); // skip image header
						uint biWidth = br.ReadUInt32 ();
						uint biHeight = br.ReadUInt32 ();
						Console.WriteLine ( "w {0} h {1}", biWidth, biHeight );
						br.ReadBytes ( (int) offset - 12  ); // skip rest of image header
						
						byte[,,] tex = new byte [ biHeight , biWidth , 4 ];
						
						for ( int ii=0 ; ii <  biHeight ; ii++ )
						{
							for ( int jj=0 ; jj < biWidth ; jj++ )
							{
								tex [ ii, jj, 0 ] = br.ReadByte();
								tex [ ii, jj, 1 ] = br.ReadByte();
								tex [ ii, jj, 2 ] = br.ReadByte();
								tex [ ii, jj, 3 ] = 255;
								//Console.Write ( ii + " " );
							}
						}

						br.Close();
						fStream.Close();
						m.BindTexture ( (int) biWidth, (int) biHeight, tex );
						*/
						break;

					default:

						SkipChunk ( child );
						break;

				}
				chunk.BytesRead += child.BytesRead;
			}
		}

		float[] ProcessColorChunk ( ThreeDSChunk chunk )
		{
			ThreeDSChunk child = new ThreeDSChunk ( reader );
			float[] c = new float[] { (float) reader.ReadByte() / 256 , (float) reader.ReadByte() / 256 , (float) reader.ReadByte() / 256 };
			//Console.WriteLine ( "R {0} G {1} B {2}", c.R, c.B, c.G );
			chunk.BytesRead += (int) child.Length;	
			return c;
		}

		int ProcessPercentageChunk ( ThreeDSChunk chunk )
		{
			ThreeDSChunk child = new ThreeDSChunk ( reader );
			int per = reader.ReadUInt16 ();
			child.BytesRead += 2;
			chunk.BytesRead += child.BytesRead;
			return per;
		}

		Entity ProcessObjectChunk ( ThreeDSChunk chunk )
		{
			return ProcessObjectChunk ( chunk, new Entity() );
		}

		Entity ProcessObjectChunk ( ThreeDSChunk chunk, Entity e )
		{
            if(e.materials == null)
                e.materials = new Material[materials.Count];
            ushort mat_count = 0;
			while ( chunk.BytesRead < chunk.Length )
			{
				ThreeDSChunk child = new ThreeDSChunk ( reader );

				switch ((Groups) child.ID)
				{
					case Groups.C_OBJECT_MESH:

						ProcessObjectChunk ( child , e );
						break;

					case Groups.C_OBJECT_VERTICES:

						e.vertices = ReadVertices ( child );
						break;

					case Groups.C_OBJECT_FACES:

						e.indices = ReadIndices ( child );
                        e.mat_to_indices = new ushort[e.indices.Length];
                       
						if ( child.BytesRead < child.Length )
							ProcessObjectChunk ( child, e );
						break;

					case Groups.C_OBJECT_MATERIAL:

						string name2 = ProcessString ( child );
						Console.WriteLine ( "	Uses Material: {0}", name2 );
                        
						Material mat;
						if ( materials.TryGetValue ( name2, out mat ) )
							e.materials[mat_count] = mat;
						else
							Console.WriteLine ( " Warning: Material '{0}' not found. ", name2 );
							//throw new Exception ( "Material not found!" );
                        
						
						   int nfaces = reader.ReadUInt16 ();
						   child.BytesRead += 2;
						   Console.WriteLine ( nfaces );

						   for ( int ii=0; ii< nfaces; ii++)
						   {
                               int face_number = reader.ReadUInt16();
						        //Console.Write ( face_number + " " );
                                e.mat_to_indices[face_number] = mat_count;
                                e.material_DIC.Add(e.indices[face_number],e.materials[mat_count]);
						        child.BytesRead += 2;

						   }
                           mat_count++;
							SkipChunk ( child );
						break;

					case Groups.C_OBJECT_UV:

						int cnt = reader.ReadUInt16 ();
						child.BytesRead += 2;

						Console.WriteLine ( "	TexCoords: {0}", cnt );
						e.texcoords = new TexCoord [ cnt ];
						for ( int ii=0; ii<cnt; ii++ )
							e.texcoords [ii] = new TexCoord ( reader.ReadSingle (), reader.ReadSingle () );
						
						child.BytesRead += ( cnt * ( 4 * 2 ) );
						
						break;
						
					default:

						SkipChunk ( child );
						break;

				}
				chunk.BytesRead += child.BytesRead;
				//Console.WriteLine ( "	ID: {0} Length: {1} Read: {2}", chunk.ID.ToString("x"), chunk.Length , chunk.BytesRead );
			}
           
          
			return e;
		}

		void SkipChunk ( ThreeDSChunk chunk )
		{
			int length = (int) chunk.Length - chunk.BytesRead;
			reader.ReadBytes ( length );
			chunk.BytesRead += length;			
		}

		string ProcessString ( ThreeDSChunk chunk )
		{
			StringBuilder sb = new StringBuilder ();

			byte b = reader.ReadByte ();
			int idx = 0;
			while ( b != 0 )
			{
				sb.Append ( (char) b);
				b = reader.ReadByte ();
				idx++;
			}
			chunk.BytesRead += idx+1;

			return sb.ToString();
		}

		Vector[] ReadVertices ( ThreeDSChunk chunk )
		{
			ushort numVerts = reader.ReadUInt16 ();
			chunk.BytesRead += 2;
			Console.WriteLine ( "	Vertices: {0}", numVerts );
			Vector[] verts = new Vector[numVerts];

			for ( int ii=0; ii < verts.Length ; ii++ )
			{
				float f1 = reader.ReadSingle();
				float f2 = reader.ReadSingle();
				float f3 = reader.ReadSingle();

				verts[ii] = new Vector ( f1, f3, -f2 );
				//Console.WriteLine ( verts [ii] );
			}

			//Console.WriteLine ( "{0}   {1}", verts.Length * ( 3 * 4 ), chunk.Length - chunk.BytesRead );

			chunk.BytesRead += verts.Length * ( 3 * 4 ) ;
			//chunk.BytesRead = (int) chunk.Length;
			//SkipChunk ( chunk );

			return verts;
		}

		Triangle[] ReadIndices ( ThreeDSChunk chunk )
		{
			ushort numIdcs = reader.ReadUInt16 ();
			chunk.BytesRead += 2;
			Console.WriteLine ( "	Indices: {0}", numIdcs );
			Triangle[] idcs = new Triangle[numIdcs];

			for ( int ii=0; ii < idcs.Length ; ii++ )
			{
				idcs [ii] = new Triangle ( reader.ReadUInt16(), reader.ReadUInt16(), reader.ReadUInt16() );
				//Console.WriteLine ( idcs [ii] );

				// flags
				reader.ReadUInt16 ();
			}
			chunk.BytesRead += ( 2 * 4 ) * idcs.Length;
			//Console.WriteLine ( "b {0} l {1}", chunk.BytesRead, chunk.Length);

			//chunk.BytesRead = (int) chunk.Length;
			//SkipChunk ( chunk );

			return idcs;
		}

		/*
		   public static void Main (string[] argv)
		   {
		   if (argv.Length <= 0) return;
		   new ThreeDSFile ( argv[0] );
		   }
		   */
	}
}
