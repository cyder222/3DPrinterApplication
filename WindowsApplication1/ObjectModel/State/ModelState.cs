using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Jusin.ThreeDLib.ObjectRender;
namespace Jusin.ObjectModel.State
{

    public enum ModelStateType
    {
        Normal,
        Selected,
        MOUSE_OVER,
        count
    }
    public class ModelStateSelected : ModelState
    {
        public ModelStateSelected()
        {
            renderes = new List<IObjectRender>();
            this.model_state = ModelStateType.Selected;
           // this.renderes.Add(this.render_factory.create(RenderKind.WireFrameRender));
          //  this.renderes.Add(this.render_factory.create(RenderKind.NormalRender));
            this.renderes.Add(this.render_factory.create(RenderKind.BlueRender));
        }
    }
    public class ModelStateMouseOver : ModelState
    {
        public ModelStateMouseOver()
        {
            renderes = new List<IObjectRender>();
            this.model_state = ModelStateType.MOUSE_OVER;
            this.renderes.Add(this.render_factory.create(RenderKind.NormalRender));
            this.renderes.Add(this.render_factory.create(RenderKind.WireFrameRender));
           
        }
        public override void onMouseLeave(object target_model, MouseEventArgs e)
        {
            (target_model as CLoadedObject).changeState(new ModelStateNormal());
        }
    }
    public class ModelStateNormal : ModelState
    {
        public ModelStateNormal()
        {
            renderes = new List<IObjectRender>();
            this.model_state = ModelStateType.Normal;
            //this.renderes.Add(this.render_factory.create(RenderKind.WireFrameRender));
            this.renderes.Add(this.render_factory.create(RenderKind.NormalRender));
          
        }
        override public void onMouseOver(object target_model, MouseEventArgs e)
        {
            (target_model as CLoadedObject).changeState(new ModelStateMouseOver());
        }
        
    }

    public class ModelState
    {
        protected RenderFactory render_factory = new RenderFactory();
        protected ModelStateType model_state;
        protected List<IObjectRender> renderes;
        public ModelState()
        {
            renderes = new List<IObjectRender>();
            this.model_state = ModelStateType.Normal;
            //this.renderes.Add(this.render_factory.create(RenderKind.WireFrameRender));
            this.renderes.Add(this.render_factory.create(RenderKind.NormalRender));
          
        }
        public void Render(Jusin.ThreeDLib.ModelBase.ModelEntityBase entity)
        {
            foreach (IObjectRender render in renderes)
            {
                System.Diagnostics.Debug.WriteLine(render.getRenderKind());
                render.Render(entity);
            }
        }
        virtual public void onMouseClick(object target_model,MouseEventArgs e)
        {
        }
        virtual public void onMouseLeave(object target_model,MouseEventArgs e)
        {
        }
        virtual public void onMouseRClick(object target_model, MouseEventArgs e)
        {
        }
        virtual public void onMouseOver(object target_model, MouseEventArgs e)
        {
           
        }
        virtual public void onKeyLeft(object target_model, MouseEventArgs e)
        {
        }
        virtual public void onKeyRight(object target_model, MouseEventArgs e)
        {
        }
        virtual public void onKeyUp(object target_model, MouseEventArgs e)
        {
        }
        virtual public void onKeyDown(object target_model, MouseEventArgs e)
        {
        }
        virtual public ModelStateType getCurrentState()
        {
            return model_state;
        }
    }
}
