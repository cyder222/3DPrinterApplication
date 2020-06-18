using System;
using System.Collections.Generic;
using System.Text;

namespace Jusin.ThreeDLib.ModelBase
{
    public class Vector3d
    {
        public double x, y, z;
        public Vector3d()
        {

        }
        public Vector3d(Vector3d vec)
        {
            this.set(vec.x, vec.y, vec.z);
        }
        public Vector3d(double x, double y, double z)
        {
            this.set(x, y, z);
        }
        public Vector3d(double[] xyz)
        {
            this.set(xyz);
        }
        public void set(double x, double y, double z)
        {
            this.x = x; this.y = y; this.z = z;
        }
        public void set(double[] xyz)
        {
            this.set(xyz[0], xyz[1], xyz[2]);
        }
        /*public static void glVertex(Vector3d vect)
        {
            Gl.glVertex3d(vect.x, vect.y, vect.z);
        }
        public static void glTranslate(Vector3d vect)
        {
            Gl.glTranslated(vect.x, vect.y, vect.z);
        }*/
        public void rotateX(double angle)
        {
            Vector3d[] mat = new Vector3d[3];
            mat[0] = new Vector3d();
            mat[1] = new Vector3d();
            mat[2] = new Vector3d();
            mat[0].set(1.0, 0.0, 0.0);
            mat[1].set(0.0, Math.Cos(angle), Math.Sin(angle));
            mat[2].set(0.0, -Math.Sin(angle), Math.Cos(angle));
            this.multiply(mat);
        }
        public void rotateY(double angle)
        {
            Vector3d[] mat = new Vector3d[3];
            mat[0] = new Vector3d();
            mat[1] = new Vector3d();
            mat[2] = new Vector3d();
            mat[0].set(Math.Cos(angle), 0.0, -Math.Sin(angle));
            mat[1].set(0.0, 1.0, 0.0);
            mat[2].set(Math.Sin(angle), 0.0, Math.Cos(angle));
            this.multiply(mat);
        }
        public void rotateZ(double angle)
        {
            Vector3d[] mat = new Vector3d[3];
            mat[0] = new Vector3d();
            mat[1] = new Vector3d();
            mat[2] = new Vector3d();
            mat[0].set(Math.Cos(angle), Math.Sin(angle), 0.0);
            mat[1].set(-Math.Sin(angle), Math.Cos(angle), 0.0);
            mat[2].set(0.0, 0.0, 1.0);
            this.multiply(mat);
        }
        public void rotateAxis(double angle, double axe_x, double axe_y, double axe_z)
        {
            Vector3d axe = new Vector3d();
            double theta;
            double phai;

            if (axe_y == 0) { axe.set(axe_x, 0.000000000000000000000000001, axe_z); }
            else { axe.set(axe_x, axe_y, axe_z); }

            //é≤ÇxyïΩñ Ç…ç~ÇÎÇµÇΩÉxÉNÉgÉãÇ∆Yé≤Ç∆ÇÃäpìxtheta
            theta = Math.Atan(axe.x / axe.y);
            theta = -theta;//ê≥âÒì]
            if (axe.y < 0) theta += Math.PI;

            //Vector3d tmp=axe;
            //axeÇzé≤íÜêSÇ…-theta âÒì]Ç…ìØÇ∂
            axe.set(0.0, Math.Sqrt(Math.Pow(axe.y, 2) + Math.Pow(axe.x, 2)), axe.z);
            // Yé≤Ç∆ÇÃäpìx phai. îÕàÕÇÕ-M_PI/2 Å` M_PI/2 Ç≈è\ï™
            phai = Math.Atan(axe.z / axe.y);

            //vÇZé≤íÜêSÇ…-thetaâÒì]Ç≥ÇπÇÈ
            this.rotateZ(-theta);
            //vÇXé≤íÜêSÇ…-phaiâÒì]Ç≥ÇπÇÈ
            this.rotateX(-phai);
            //vÇYé≤íÜêSÇ…angleâÒì]Ç≥ÇπÇÈ
            this.rotateY(angle);
            //ñﬂÇ∑
            this.rotateX(phai);
            this.rotateZ(theta);
        }
        /**
         * é©ï™Ç∆ÇÃäOêœÇãÅÇﬂÇƒï‘ãpÇ∑ÇÈÅié©ï™é©êgÇÕÇ©ÇÌÇÁÇ»Ç¢)
         * */
        public Vector3d cross(Vector3d mat)
        {
            return new Vector3d(y * mat.z - z * mat.y, z * mat.x - x * mat.z, x * mat.y - y * mat.x);
        }
        public double Dot(Vector3d mat)
        {
            return this.x * mat.x + this.y * mat.y + this.z * mat.z;
        }
        //ê≥ãKâªÇ∑ÇÈ
        public void Normalize()
        {
            double size = this.x * this.x + this.y * this.y + this.z * this.z;
            this.x = this.x / size;
            this.y = this.y / size;
            this.z = this.z / size;
        }
        /**
         * çsóÒìØémÇÇ©ÇØÇÈ
         * @param Vector3d[] mat (3éüå≥îzóÒ,âÒì]çsóÒ)
         * 
         * */
        public Vector3d multiply(Vector3d[] mat)
        {
            double x = this.x;
            double y = this.y;
            double z = this.z;
            this.x = x * mat[0].x + y * mat[1].x + z * mat[2].x;
            this.y = x * mat[0].y + y * mat[1].y + z * mat[2].y;
            this.z = x * mat[0].z + y * mat[1].z + z * mat[2].z;
            return this;
        }
        public Vector3d mul(Vector3d mat)
        {
            this.x *= mat.x;
            this.y *= mat.y;
            this.z *= mat.z;
            return this;
        }
        public Vector3d div(Vector3d mat)
        {
            this.x /= mat.x;
            this.y /= mat.y;
            this.z /= mat.z;
            return this;
        }
        public static Vector3d operator -(Vector3d mat1, Vector3d mat2)
        {
            Vector3d ret = new Vector3d();
            ret.x = mat1.x - mat2.x;
            ret.y = mat1.y - mat2.y;
            ret.z = mat1.z - mat2.z;
            return ret;
        }
        public static Vector3d operator +(Vector3d mat1, Vector3d mat2)
        {
            Vector3d ret = new Vector3d(); ;
            ret.x = mat1.x + mat2.x;
            ret.y = mat1.y + mat2.y;
            ret.z = mat1.z + mat2.z;
            return ret;
        }

    }
}
