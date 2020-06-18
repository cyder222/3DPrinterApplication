using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace WindowsApplication1.ContextMenus
{
    public partial class ModelSelectMenu : Component
    {
        public ModelSelectMenu()
        {
            InitializeComponent();
        }

        public ModelSelectMenu(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}
