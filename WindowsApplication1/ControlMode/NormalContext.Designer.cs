namespace WindowsApplication1.ControlMode
{
    partial class NormalContext
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
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
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MoveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShoumenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.後面ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.moveToolStipMenu2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.移動ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.カメラToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.正面ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.RightSideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.moveToolStipMenu2.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MoveToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(126, 32);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // MoveToolStripMenuItem
            // 
            this.MoveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShoumenToolStripMenuItem,
            this.後面ToolStripMenuItem,
            this.topToolStripMenuItem,
            this.toolStripSeparator1});
            this.MoveToolStripMenuItem.Name = "MoveToolStripMenuItem";
            this.MoveToolStripMenuItem.Size = new System.Drawing.Size(125, 28);
            this.MoveToolStripMenuItem.Text = "カメラ";
            // 
            // ShoumenToolStripMenuItem
            // 
            this.ShoumenToolStripMenuItem.Name = "ShoumenToolStripMenuItem";
            this.ShoumenToolStripMenuItem.Size = new System.Drawing.Size(125, 28);
            this.ShoumenToolStripMenuItem.Text = "正面";
            this.ShoumenToolStripMenuItem.Click += new System.EventHandler(this.ShoumenToolStripMenuItem_Click);
            // 
            // 後面ToolStripMenuItem
            // 
            this.後面ToolStripMenuItem.Name = "後面ToolStripMenuItem";
            this.後面ToolStripMenuItem.Size = new System.Drawing.Size(125, 28);
            this.後面ToolStripMenuItem.Text = "右側面";
            this.後面ToolStripMenuItem.Click += new System.EventHandler(this.RightSideToolStripMenuItem_Click);
            // 
            // topToolStripMenuItem
            // 
            this.topToolStripMenuItem.Name = "topToolStripMenuItem";
            this.topToolStripMenuItem.Size = new System.Drawing.Size(125, 28);
            this.topToolStripMenuItem.Text = "上面";
            this.topToolStripMenuItem.Click += new System.EventHandler(this.topToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(122, 6);
            // 
            // moveToolStipMenu2
            // 
            this.moveToolStipMenu2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.移動ToolStripMenuItem,
            this.カメラToolStripMenuItem});
            this.moveToolStipMenu2.Name = "SelectedContextMenuStrip";
            this.moveToolStipMenu2.Size = new System.Drawing.Size(153, 82);
            this.moveToolStipMenu2.Opening += new System.ComponentModel.CancelEventHandler(this.moveToolStipMenu2_Opening);
            // 
            // 移動ToolStripMenuItem
            // 
            this.移動ToolStripMenuItem.Name = "移動ToolStripMenuItem";
            this.移動ToolStripMenuItem.Size = new System.Drawing.Size(152, 28);
            this.移動ToolStripMenuItem.Text = "移動";
            this.移動ToolStripMenuItem.Click += new System.EventHandler(this.MoveToolStripMenuItem_Click);
            // 
            // カメラToolStripMenuItem
            // 
            this.カメラToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.正面ToolStripMenuItem1,
            this.RightSideToolStripMenuItem,
            this.topToolStripMenuItem1});
            this.カメラToolStripMenuItem.Name = "カメラToolStripMenuItem";
            this.カメラToolStripMenuItem.Size = new System.Drawing.Size(152, 28);
            this.カメラToolStripMenuItem.Text = "カメラ";
            // 
            // 正面ToolStripMenuItem1
            // 
            this.正面ToolStripMenuItem1.Name = "正面ToolStripMenuItem1";
            this.正面ToolStripMenuItem1.Size = new System.Drawing.Size(125, 28);
            this.正面ToolStripMenuItem1.Text = "正面";
            // 
            // RightSideToolStripMenuItem
            // 
            this.RightSideToolStripMenuItem.Name = "RightSideToolStripMenuItem";
            this.RightSideToolStripMenuItem.Size = new System.Drawing.Size(125, 28);
            this.RightSideToolStripMenuItem.Text = "右側面";
            this.RightSideToolStripMenuItem.Click += new System.EventHandler(this.RightSideToolStripMenuItem_Click);
            // 
            // topToolStripMenuItem1
            // 
            this.topToolStripMenuItem1.Name = "topToolStripMenuItem1";
            this.topToolStripMenuItem1.Size = new System.Drawing.Size(125, 28);
            this.topToolStripMenuItem1.Text = "上面";
            this.topToolStripMenuItem1.Click += new System.EventHandler(this.topToolStripMenuItem_Click);
            // 
            // NormalContext
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "NormalContext";
            this.contextMenuStrip1.ResumeLayout(false);
            this.moveToolStipMenu2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MoveToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip moveToolStipMenu2;
        private System.Windows.Forms.ToolStripMenuItem 移動ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ShoumenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 後面ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem topToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem カメラToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 正面ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem RightSideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem topToolStripMenuItem1;
    }
}
