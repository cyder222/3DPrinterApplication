using System;
using System.Collections.Generic;
using System.Text;
using Jusin.ThreeDLib.ModelBase;
namespace Jusin.ObjectModel
{
    public interface ICollisionObject
    {
        Vector3d[] getBboxPoint();
        bool checkColl(ICollisionObject col);
        bool checkColl(Vector3d point);
    }
}
