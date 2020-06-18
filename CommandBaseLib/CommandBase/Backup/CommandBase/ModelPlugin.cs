using System;
using System.Collections.Generic;
using System.Text;
using Jusin.ThreeDLib.ModelBase;
using Jusin.CommandBase;
namespace Jusin.ThreeDLib.ModelLoadPlugin
{
    public interface IModelImportPlugin
    {
         List<ModelEntityBase> importFromFile(String filename);
    }
}
namespace Jusin.ThreeDLibModel.EditPlugin
{
    public interface IModelEditPlugin
    {
         void Edit(ModelEntityBase model);
    }
}
