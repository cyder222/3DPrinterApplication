using System;
using System.Collections.Generic;
using System.Text;
using Jusin.CommandBase;
namespace WindowsApplication1.Commands
{
    class MainCommandManager : CommandExecuter
    {
        private static  MainCommandManager __instance = null;

        protected MainCommandManager()
        {
            this.redoBuffer = new Stack<ICommand>();
            this.undoBuffer = new Stack<ICommand>();
        }
        public static MainCommandManager getInstance(){
            if (__instance == null)
            {
                __instance = new MainCommandManager();
            }
            return __instance;
        }
       
    }
}
