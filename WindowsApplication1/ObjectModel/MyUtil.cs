using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Jusin.ThreeDLib.ModelBase;
namespace WindowsApplication1.ObjectModel
{
    class MyUtil
    {
        /**
         * point‚ªtri‚Ì“à‘¤‚É‚ ‚é‚©‚Ç‚¤‚©‚ğ’²‚×‚é
         * 
         * **/
        public static bool PointInTriangle(Vector3d[] tri,Vector3d point)
        {
            //point‚ªtri‚Ìì‚é•½–Êã‚Ì“_‚©‚Ç‚¤‚©‚ğ’²‚×‚é

            Vector3d v1v2 = tri[1] - tri[0];
            Vector3d v2v3 = tri[2] = tri[1];
            Vector3d n = v1v2.cross(v2v3);
            n.Normalize();
            double D = n.Dot(point);
            double check = n.x * point.x + n.y * point.y + n.z * point.z + D * 1;
            if (Math.Abs(check) < 0.001)
            {
                //point‚ªtri‚Ì“à‘¤‚©‚Ç‚¤‚©’²‚×‚é
                Vector3d R = point - tri[0];
                Vector3d Q2 = tri[1] - tri[0];
                Vector3d Q3 = tri[2] = tri[0];
                double Q2_size = Q2.x * Q2.x + Q2.y * Q2.y + Q2.z * Q2.z;
                double Q3_size = Q3.x * Q3.x + Q3.y * Q3.y + Q3.z * Q3.z;
                double Q2Q3 = Q2.Dot(Q3);
                double RQ2 = R.Dot(Q2);
                double RQ3 = R.Dot(Q3);
                double[] M = new double[]{Q2_size,Q2Q3,Q2Q3,Q3_size};
                double inv_D = 1 / (Q2_size * Q3_size - Q2Q3 * Q2Q3);
                double[] inv_M = new double[] { Q3_size/inv_D, -Q2Q3/inv_D, -Q2Q3/inv_D, Q2_size/inv_D };
                //(a2,a3) = (RQ2,RQ3)inv_M---
                double a2 = RQ2 * inv_M[0] + RQ3 * inv_M[2];
                double a3 = RQ2 * inv_M[1] + RQ3 * inv_M[3];
                if (a2 >= 0 && a3 >= 0 && a2 + a3 <= 1)
                    return true;
             

            }
            return false;
        }
    }
}
