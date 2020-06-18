namespace Tao_Sample
{
    partial class UserControl1
    {
        /// <summary> 
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            this.DeleteContext();
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.normalMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.modelSelectMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.moveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.magedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modelSelectMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // normalMenuStrip
            // 
            this.normalMenuStrip.Name = "normalMenuStrip";
            this.normalMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // modelSelectMenuStrip
            // 
            this.modelSelectMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveToolStripMenuItem,
            this.magedToolStripMenuItem});
            this.modelSelectMenuStrip.Name = "modelSelectMenuStrip";
            this.modelSelectMenuStrip.Size = new System.Drawing.Size(125, 48);
            this.modelSelectMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.modelSelectMenuStrip_ItemClicked);
            // 
            // moveToolStripMenuItem
            // 
            this.moveToolStripMenuItem.Name = "moveToolStripMenuItem";
            this.moveToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.moveToolStripMenuItem.Text = "移動";
            this.moveToolStripMenuItem.Click += new System.EventHandler(this.moveToolStripMenuItem_Click);
            // 
            // magedToolStripMenuItem
            // 
            this.magedToolStripMenuItem.Name = "magedToolStripMenuItem";
            this.magedToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.magedToolStripMenuItem.Text = "拡大縮小";
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(255, 365);
            this.Load += new System.EventHandler(this.UserControl1_Load);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.UserControl1_MouseWheel);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UserControl1_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.UserControl1_MouseDown);
            this.Resize += new System.EventHandler(this.UserControl1_Resize);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.UserControl1_MouseUp);
            this.modelSelectMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip normalMenuStrip;
        private System.Windows.Forms.ContextMenuStrip modelSelectMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem moveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem magedToolStripMenuItem;

    }
}
