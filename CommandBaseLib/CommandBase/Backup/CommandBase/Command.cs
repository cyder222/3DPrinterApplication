using System;
using System.Collections.Generic;
using System.Text;

namespace Jusin.CommandBase
{
    public interface ICommand{
        void redo();
        void undo();
        void execute();
        bool isRedo();
        bool isUndo();
    }
    public class CommandExecuter{
        protected Stack<ICommand> redoBuffer;
        protected Stack<ICommand> undoBuffer;

        public void execute(ICommand command){
            command.execute();
            if(command.isUndo()){
                undoBuffer.Push(command);
                redoBuffer.Clear();
            }else{
                undoBuffer.Clear();
                redoBuffer.Clear();
            }
        }
        public void redo(){
            if(redoBuffer.Count==0)
                return;
            ICommand command = redoBuffer.Pop();
            command.redo();
            if(command.isUndo())
                undoBuffer.Push(command);
            else
                undoBuffer.Clear();
                
        }
        public void undo(){
            if(undoBuffer.Count==0)
                return;
            ICommand command = undoBuffer.Pop();
            command.undo();
            if(command.isRedo())
                redoBuffer.Push(command);
            else
               redoBuffer.Clear();

        }
    }

}