using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace Jusin.ObjectModel
{
    public class ModelsEventErgs : EventArgs
    {
        public CLoadedObject target_model;
        public ModelsEventErgs(CLoadedObject target_model)
        {
            this.target_model = target_model;
        }
    }
    public class ModelManager
    {
        protected List<CLoadedObject> models;
        protected Dictionary<int,CLoadedObject> selection_dict;
        protected List<CLoadedObject> selected_models;
        protected TreeNode tree_nodes;
        public TreeNode model_node
        {
            get
            {
                return this.tree_nodes;
            }
        }
        public event EventHandler ModelsChange;
        protected int last_id;
        static ModelManager __instance = null;
        protected ModelManager()
        {
            models = new List<CLoadedObject>();
            selected_models = new List<CLoadedObject>();
            selection_dict = new Dictionary<int,CLoadedObject>();
            tree_nodes = new TreeNode("モデル一覧");
            last_id = 0;
        }
        public static ModelManager getInstance()
        {
            if (__instance == null)
            {
                __instance = new ModelManager();
            }
            return __instance;
        }
        public Dictionary<int, CLoadedObject> getSelectionDict()
        {
            return this.selection_dict;
        }
        public List<CLoadedObject> getModelList()
        {
            last_id = 0;
            return models;
        }
        public CLoadedObject getModelBySelectionNumber(int selection_number)
        {
            return selection_dict[selection_number];
        }
        protected int getNewId()
        {
            while (selection_dict.ContainsKey(++last_id)) ;
            return last_id;
        }
        protected void AddModelToNode(CLoadedObject model)
        {
            tree_nodes.Nodes.Add(model.name.ToString(), model.name.ToString());
        }
        public CLoadedObject addModel(CLoadedObject model)
        {
            int key = getNewId();
            model.name = (uint)key;
            model.glDisplayList = key;
            models.Add(model);
            model.Selectable = true;
            selection_dict.Add(key, model);
            model.modelChange += new EventHandler(this.onModelChange);
            this.AddModelToNode(model);
            this.ModelsChange(this, new ModelsEventErgs(model));
            return model;
        }
        public CLoadedObject addNonSelectModel(CLoadedObject model)
        {
            models.Add(model);
            model.Selectable = false;
            model.modelChange += new EventHandler(this.onModelChange);
            this.ModelsChange(this, new ModelsEventErgs(model));
            return model;
        }
        public void removeModel(CLoadedObject model)
        {
            if (!models.Contains(model))
                return;
            models.Remove(model);
            if(model.Selectable)
                selection_dict.Remove((int)model.name);
            if (selected_models.Contains(model))
                selected_models.Remove(model);
            this.tree_nodes.Nodes.Remove(tree_nodes.Nodes[model.name.ToString()]);
            model.clearCallList();
            this.ModelsChange(this, new ModelsEventErgs(model));
        }
        public void SelectModel(int selection_number)
        {
            if (!selection_dict.ContainsKey(selection_number))
                return;
            CLoadedObject model = selection_dict[selection_number];
            model.changeState(new Jusin.ObjectModel.State.ModelStateSelected()); 
            selected_models.Add(selection_dict[selection_number]);
            if (tree_nodes.Nodes.ContainsKey(selection_number.ToString()))
            {
                tree_nodes.TreeView.SelectedNode = tree_nodes.Nodes[selection_number.ToString()];
            }
            
        }
        public void ClearSelectModel()
        {
            foreach (CLoadedObject obj in selected_models)
            {
                obj.changeState(new Jusin.ObjectModel.State.ModelStateNormal());
            }
           
            selected_models.Clear();
            tree_nodes.TreeView.SelectedNode = null;
        }
        public void onModelChange(object sender, EventArgs e)
        {
            this.ModelsChange(this, new ModelsEventErgs((CLoadedObject)sender));
        }
        public IEnumerable<CLoadedObject> getAllModelForEach
        {
            get
            {
                foreach (CLoadedObject obj in this.models)
                    yield return obj;
            }
        }
        public List<CLoadedObject> getSelectedModels()
        {
            return this.selected_models;
        }
        public IEnumerable<int> getSelectionKeysForEach
        {
            get
            {
                foreach (int key in this.selection_dict.Keys)
                    yield return key;
            }
        }
        public IEnumerable<CLoadedObject> getSelectedModelForEach
        {
            get
            {
                foreach (CLoadedObject obj in this.selected_models)
                    yield return obj;
            }
        }
        
    
    }
}
