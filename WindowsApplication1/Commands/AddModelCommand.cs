using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jusin.CommandBase;
using Jusin.ObjectModel;
using Jusin.ThreeDLib.ModelBase;
using WindowsApplication1.ControlMode;
namespace WindowsApplication1.Commands
{
    class AddModelCommand : ICommand 
    {
        ModelManager model_manager;
        String filename;
        CLoadedObject added_model;
        public AddModelCommand(ModelManager manager, String filename)
        {
            this.model_manager = manager;
            this.filename = filename;
            Jusin.ThreeDLib.ModelLoadPlugin.IModelImportPlugin importer = new Jusin.ThreeDLib.ModelLoadPlugin.ThreeDS.ThreeDSLoader();
            List<ModelEntityBase> entities = importer.importFromFile(filename);
            CLoadedObject new_model = new CLoadedObject(entities);
            this.added_model = new_model;
            this.model_manager.addModel(new_model);
            this.model_manager.ClearSelectModel();
            this.model_manager.SelectModel((int)new_model.name);
            ControlModeChanger.getInstance().changeMode(new ModelMoveMode(this.model_manager.getSelectedModels()[0]));
        }
        public bool isUndo()
        {
            return true;
        }
        public bool isRedo()
        {
            return true;
        }
        public void undo()
        {
            this.model_manager.removeModel(added_model);
        }
        public void redo()
        {
            this.model_manager.addModel(added_model);
            this.model_manager.ClearSelectModel();
            this.model_manager.SelectModel((int)added_model.name);
            
        }
        public void execute()
        {
           
        }
    }
}
