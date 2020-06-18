using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WindowsApplication1.ControlMode;
using Jusin.ObjectModel;
using WindowsApplication1.Commands;
namespace WindowsApplication1.ControlMode
{
    public partial class MoveTab : UserControl
    {
        CLoadedObject target;
        float pre_vx, pre_vy, pre_vz,vx,vy,vz;
        float pre_rotate_x, pre_rotate_y, pre_rotate_z,r_x,r_y,r_z;
        float pre_mag,maged;
        public MoveTab(CLoadedObject target)
        {
            InitializeComponent();
            target.modelChange += new EventHandler(target_ObjectChange);
            this.target = target;
            this.pre_vx = target.vx;
            this.pre_vy = target.vy;
            this.pre_vz = target.vz;
            this.pre_rotate_x = target.x_dir;
            this.pre_rotate_y = target.y_dir;
            this.pre_rotate_z = target.z_dir;
            this.pre_mag = target.maged;
            this.updateValues();
        }

        void target_ObjectChange(object sender, EventArgs e)
        {
            updateValues();
        }
        void updateValues()
        {
            this.x_axis.Text = target.vx.ToString();
            this.y_axis.Text = target.vy.ToString();
            this.z_axis.Text = target.vz.ToString();
            this.x_rotate.Text = target.x_dir.ToString();
            this.y_rotate.Text = target.y_dir.ToString();
            this.z_rotate.Text = target.z_dir.ToString();
            this.maged_box.Text = target.maged.ToString();
        }

      

        private void okButton_Click(object sender, EventArgs e)
        {
            //targetの値をいったんもとに戻す。その前に移動先となる現在の値を別に記憶しておく
            float vx = this.vx;
            float vy = this.vy;
            float vz = this.vz;
            float r_x = this.r_x;
            float r_y = this.r_y;
            float r_z = this.r_z;
            float maged = this.maged;
            //元に戻す(undo(),redo()の実装のため)
            this.target.vx = pre_vx;
            this.target.vy = pre_vy;
            this.target.vz = pre_vz;
            this.target.x_dir = pre_rotate_x;
            this.target.y_dir = pre_rotate_y;
            this.target.z_dir = pre_rotate_z;
            this.target.maged = pre_mag;
            MainCommandManager.getInstance().execute(new MoveCommand(vx, vy, vz,r_x,r_y,r_z,maged, target));
            ControlModeChanger.getInstance().changeMode(new NormalMode());
        }
        private void moveTextValueChange(object sender, EventArgs e)
        {
            
            float value;
            if (float.TryParse(this.x_axis.Text, out value))
            {
                vx = value;
                this.target.vx = value;
            }

            if (float.TryParse(this.y_axis.Text, out value))
            {
                vy = value;
                this.target.vy = value;
            }
            if (float.TryParse(this.z_axis.Text, out value))
            {
                vz = value;
                this.target.vz = value;
            }
            if (float.TryParse(this.x_rotate.Text, out value))
            {
                r_x = value;
                this.target.x_dir = value;
            }
            if (float.TryParse(this.y_rotate.Text, out value))
            {
                r_y = value;
                this.target.y_dir = value;
            }
            if (float.TryParse(this.z_rotate.Text, out value))
            {
                r_z = value;
                this.target.z_dir = value;
            }
            if (float.TryParse(this.maged_box.Text, out value))
            {
                maged = value;
                this.target.maged = value;
            }
        }
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.target.vx = pre_vx;
            this.target.vy = pre_vy;
            this.target.vz = pre_vz;
            this.target.x_dir = pre_rotate_x;
            this.target.y_dir = pre_rotate_y;
            this.target.z_dir = pre_rotate_z;
            ControlModeChanger.getInstance().changeMode(new NormalMode());
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        
    }
}
