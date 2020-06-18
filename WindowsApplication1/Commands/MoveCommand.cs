using System;
using System.Collections.Generic;
using System.Text;
using Jusin.CommandBase;
using Jusin.ObjectModel;
namespace WindowsApplication1.Commands
{
    class MoveCommand : ICommand
    {
        float x;
        float y;
        float z;
        float rotate_x;
        float rotate_y;
        float rotate_z;
        float pre_x;
        float pre_y;
        float pre_z;
        float pre_rotate_x;
        float pre_rotate_y;
        float pre_rotate_z;
        float maged, pre_maged;
        CLoadedObject target;
        public MoveCommand(float x, float y, float z, CLoadedObject target)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.rotate_x = target.x_dir;
            this.rotate_y = target.y_dir;
            this.rotate_z = target.z_dir;
            this.maged = target.maged;
            this.target = target;
        }
        public MoveCommand(float x, float y, float z,float rotate_x,float rotate_y,float rotate_z, CLoadedObject target)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.rotate_x = rotate_x;
            this.rotate_y = rotate_y;
            this.rotate_z = rotate_z;
            this.maged = target.maged;
            this.target = target;
        }
        public MoveCommand(float x, float y, float z, float rotate_x, float rotate_y, float rotate_z, float maged,CLoadedObject target)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.rotate_x = rotate_x;
            this.rotate_y = rotate_y;
            this.rotate_z = rotate_z;
            this.maged = maged;
            this.target = target;
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
            this.target.vx = this.pre_x;
            this.target.vy = this.pre_y;
            this.target.vz = this.pre_z;
            this.target.x_dir = this.pre_rotate_x;
            this.target.y_dir = this.pre_rotate_y;
            this.target.z_dir = this.pre_rotate_z;
            this.target.maged = this.pre_maged;
        }
        public void redo()
        {
            target.vx = x;
            target.vy = y;
            target.vz = z;
            target.x_dir = this.rotate_x;
            target.y_dir = this.rotate_y;
            target.z_dir = this.rotate_z;
            target.maged = this.maged;
        }
        public void execute()
        {
            this.pre_x = target.vx;
            this.pre_y = target.vy;
            this.pre_z = target.vz;
            this.pre_rotate_x = target.x_dir;
            this.pre_rotate_y = target.y_dir;
            this.pre_rotate_z = target.z_dir;
            this.pre_maged = target.maged;
            target.vx = x;
            target.vy = y;
            target.vz = z;
            target.x_dir = this.rotate_x;
            target.y_dir = this.rotate_y;
            target.z_dir = this.rotate_z;
            target.maged = this.maged;
        }
    }
}
