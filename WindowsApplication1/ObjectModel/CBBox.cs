using System;
using System.Collections.Generic;
using System.Text;
using Jusin.ThreeDLib.ModelBase;
using Tao.OpenGl;
namespace Jusin.ObjectModel
{
    public class CBBox
    {
        Vector3d center;
        Vector3d size;
        public CBBox(Vector3d center, Vector3d size)
        {
            this.center = center;
            this.size = size;
        }
        public Vector3d getCenter()
        {
            return this.center;
        }
        public Vector3d getSize()
        {
            return this.size;
        }
        public void DrawWire()
        {
            double x = size.x / 2.0;
            double y = size.y / 2.0;
            double z = size.z / 2.0;
          double[][] vertex = new double[][]{
            new double[]{ -x+center.x, -y+center.y, -z+center.z },
             new double[]{  x+center.x, -y+center.y, -z+center.z },
             new double[]{  x+center.x,  y+center.y, -z+center.z },
             new double[]{ -x+center.x,  y+center.y, -z+center.z },
             new double[]{ -x+center.x, -y+center.y,  z+center.z },
            new double[] {  x+center.x, -y+center.y,  z+center.z },
            new double[] {  x+center.x,  y+center.y,  z+center.z }, 
            new double[] { -x+center.x,  y+center.y,  z+center.z }
          };

           int[][] face = new int[][]{
            new int[] { 0, 1, 2, 3 },
           new int[] { 1, 5, 6, 2 },
           new int[] { 5, 4, 7, 6 },
           new int[] { 4, 0, 3, 7 },
           new int[] { 4, 5, 1, 0 },
           new int[] { 3, 2, 6, 7 }
          };

           double[][]normal = new double[][]{
            new double[] { 0.0, 0.0,-1.0 },
            new double[] { 1.0, 0.0, 0.0 },
            new double[] { 0.0, 0.0, 1.0 },
            new double[] {-1.0, 0.0, 0.0 },
            new double[] { 0.0,-1.0, 0.0 },
            new double[] { 0.0, 1.0, 0.0 }
          };

          // float[] red = { 0.8f, 0.2f, 0.2f, 1.0f };

          int i, j;

          /* çﬁéøÇê›íËÇ∑ÇÈ */
          //Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, red);
            
          Gl.glBegin(Gl.GL_QUADS);
          for (j = 0; j < 6; ++j) {
              Gl.glNormal3dv(normal[j]);
            for (i = 4; --i >= 0;) {
                Gl.glVertex3dv(vertex[face[j][i]]);
            }
          }
          Gl.glEnd();
        }
       
    }
}
