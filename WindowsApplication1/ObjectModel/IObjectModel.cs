using System;
using System.Collections.Generic;
using System.Text;
using Jusin.ThreeDLib.ModelBase;
namespace Jusin.ThreeDLib
{
    public enum RenderState
    {
        NORMAL,
        SELECTED,
        TRANSPARENT,
        NONE
    }
    interface IObjectModel
    {
        void Render();
        RenderState getRenderState();
        Vector3d[] getBboxPoint();

    }
}
