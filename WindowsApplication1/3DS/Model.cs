// Title:	Model.cs
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
using WindowsApplication1.ObjectModel;
namespace ThreeD
{

    public class Model
	{
		public List<Entity> Entities = new List<Entity> ();

		public void Render ()
		{
			foreach ( Entity e in Entities )
				e.Render ();
		}
        public RenderState getRenderState()
        {
            return RenderState.NORMAL;
        }
        /**
         * カメラから各エンティティ内のポリゴンまでの距離を計算して，遠い順にソートする（Zソートでの半透明対応用）
         * 
         * **/
        public void sortEntities(CCamera camera)
        {
            foreach (Entity entity in Entities)
            {
                entity.sortIndices(camera);
            }
        }
        /**
         * バウンディングボックス情報を返却する
         * @return [0] min_point(x,y,z) [1] max_point(x,y,z)それぞれ
         * 
         * **/
        public Vector3d[] getBboxPoint()
        {
            double max_x, max_y , max_z;
            double min_x , min_y , min_z;
            max_x = max_y = max_z =   -10000000.0;
            min_x = min_y = min_z =   10000000.0;
            foreach (Entity e in Entities)
            {
               
                for (int i = 0; i < e.vertices.Length; i++)
                {
                    Vector vertics = e.vertices[i];
                    if (vertics.X < min_x)
                    {
                        min_x = vertics.X;
                    }
                    if (vertics.Y < min_y)
                        min_y = vertics.Y;
                    if (vertics.Z < min_z)
                        min_z = vertics.Z;
                    if (vertics.X > max_x)
                        max_x = vertics.X;
                    if (vertics.Y > max_y)
                        max_y = vertics.Y;
                    if (vertics.Z > max_z)
                        max_z = vertics.Z;

                }
                
            }
            Vector3d min_vec = new Vector3d(min_x, min_y, min_z);
            Vector3d max_vec = new Vector3d(max_x, max_y, max_z);
            Vector3d[] ret = new Vector3d[] { min_vec, max_vec };
            return ret;
        }
    }
}
