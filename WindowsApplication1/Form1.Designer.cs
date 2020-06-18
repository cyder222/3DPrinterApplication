namespace WindowsApplication1
{
    partial class Form1
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
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.kkkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.モデルの追加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.デバッグ断面出力ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.デバッグシェルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LightTabImageList = new System.Windows.Forms.ImageList(this.components);
            this.open3DFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.キャンセルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Add3DFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.mainTabMenuControl1 = new WindowsApplication1.MainTabMenuControl();
            this.userControl11 = new Tao_Sample.UserControl1();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kkkToolStripMenuItem,
            this.toolStripSeparator2});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(111, 38);
            this.contextMenuStrip1.Text = "kkk";
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // kkkToolStripMenuItem
            // 
            this.kkkToolStripMenuItem.Enabled = false;
            this.kkkToolStripMenuItem.Name = "kkkToolStripMenuItem";
            this.kkkToolStripMenuItem.Size = new System.Drawing.Size(110, 28);
            this.kkkToolStripMenuItem.Text = "移動";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(107, 6);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルToolStripMenuItem,
            this.editToolStripMenuItem,
            this.displayToolStripMenuItem,
            this.insertToolStripMenuItem,
            this.toolToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1217, 31);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ファイルToolStripMenuItem
            // 
            this.ファイルToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.toolStripSeparator1,
            this.モデルの追加ToolStripMenuItem,
            this.デバッグ断面出力ToolStripMenuItem,
            this.デバッグシェルToolStripMenuItem});
            this.ファイルToolStripMenuItem.Name = "ファイルToolStripMenuItem";
            this.ファイルToolStripMenuItem.Size = new System.Drawing.Size(105, 27);
            this.ファイルToolStripMenuItem.Text = "ファイル(F)";
            this.ファイルToolStripMenuItem.Click += new System.EventHandler(this.ファイルToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(200, 28);
            this.newToolStripMenuItem.Text = "新規(N)";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(200, 28);
            this.openToolStripMenuItem.Text = "開く(O)";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenu_ItemClick);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(200, 28);
            this.closeToolStripMenuItem.Text = "閉じる(C)";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStipMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(197, 6);
            // 
            // モデルの追加ToolStripMenuItem
            // 
            this.モデルの追加ToolStripMenuItem.Name = "モデルの追加ToolStripMenuItem";
            this.モデルの追加ToolStripMenuItem.Size = new System.Drawing.Size(200, 28);
            this.モデルの追加ToolStripMenuItem.Text = "モデルの追加";
            this.モデルの追加ToolStripMenuItem.Click += new System.EventHandler(this.addModelToolStripMenuItem_Click);
            // 
            // デバッグ断面出力ToolStripMenuItem
            // 
            this.デバッグ断面出力ToolStripMenuItem.Name = "デバッグ断面出力ToolStripMenuItem";
            this.デバッグ断面出力ToolStripMenuItem.Size = new System.Drawing.Size(200, 28);
            this.デバッグ断面出力ToolStripMenuItem.Text = "デバッグ断面出力";
            // 
            // デバッグシェルToolStripMenuItem
            // 
            this.デバッグシェルToolStripMenuItem.Name = "デバッグシェルToolStripMenuItem";
            this.デバッグシェルToolStripMenuItem.Size = new System.Drawing.Size(200, 28);
            this.デバッグシェルToolStripMenuItem.Text = "デバッグシェル";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(75, 27);
            this.editToolStripMenuItem.Text = "編集(E)";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // displayToolStripMenuItem
            // 
            this.displayToolStripMenuItem.Name = "displayToolStripMenuItem";
            this.displayToolStripMenuItem.Size = new System.Drawing.Size(76, 27);
            this.displayToolStripMenuItem.Text = "表示(V)";
            this.displayToolStripMenuItem.Click += new System.EventHandler(this.displayToolStripMenuItem_Click);
            // 
            // insertToolStripMenuItem
            // 
            this.insertToolStripMenuItem.Name = "insertToolStripMenuItem";
            this.insertToolStripMenuItem.Size = new System.Drawing.Size(72, 27);
            this.insertToolStripMenuItem.Text = "挿入(I)";
            // 
            // toolToolStripMenuItem
            // 
            this.toolToolStripMenuItem.Name = "toolToolStripMenuItem";
            this.toolToolStripMenuItem.Size = new System.Drawing.Size(91, 27);
            this.toolToolStripMenuItem.Text = "ツール(T)";
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(126, 27);
            this.windowToolStripMenuItem.Text = "ウィンドウ(W)";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(92, 27);
            this.helpToolStripMenuItem.Text = "ヘルプ(H)";
            // 
            // LightTabImageList
            // 
            this.LightTabImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("LightTabImageList.ImageStream")));
            this.LightTabImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.LightTabImageList.Images.SetKeyName(0, "icon_016.png");
            // 
            // open3DFileDialog1
            // 
            this.open3DFileDialog1.FileName = "openFileDialog1";
            this.open3DFileDialog1.Filter = "3DSファイル(*.3ds)|*.3ds|All files(*.*)|*.*";
            this.open3DFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.キャンセルToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(156, 32);
            // 
            // キャンセルToolStripMenuItem
            // 
            this.キャンセルToolStripMenuItem.Name = "キャンセルToolStripMenuItem";
            this.キャンセルToolStripMenuItem.Size = new System.Drawing.Size(155, 28);
            this.キャンセルToolStripMenuItem.Text = "キャンセル";
            // 
            // Add3DFileDialog1
            // 
            this.Add3DFileDialog1.FileName = "Add3DFileDialog1";
            this.Add3DFileDialog1.Filter = "3DSファイル(*.3ds)|*.3ds|All files(*.*)|*.*";
            this.Add3DFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.Add3DFileDialog_FileOk);
            // 
            // mainTabMenuControl1
            // 
            this.mainTabMenuControl1.AutoSize = true;
            this.mainTabMenuControl1.Location = new System.Drawing.Point(0, 36);
            this.mainTabMenuControl1.Margin = new System.Windows.Forms.Padding(5);
            this.mainTabMenuControl1.Name = "mainTabMenuControl1";
            this.mainTabMenuControl1.Size = new System.Drawing.Size(277, 564);
            this.mainTabMenuControl1.TabIndex = 4;
            this.mainTabMenuControl1.Resize += new System.EventHandler(this.mainTabMenuControl1_Resize);
            // 
            // userControl11
            // 
            this.userControl11.Location = new System.Drawing.Point(287, 36);
            this.userControl11.Margin = new System.Windows.Forms.Padding(5);
            this.userControl11.Name = "userControl11";
            this.userControl11.Size = new System.Drawing.Size(930, 500);
            this.userControl11.TabIndex = 3;
            this.userControl11.Load += new System.EventHandler(this.userControl11_Load_1);
            this.userControl11.KeyDown += new System.Windows.Forms.KeyEventHandler(this.onKeyDown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1217, 578);
            this.Controls.Add(this.mainTabMenuControl1);
            this.Controls.Add(this.userControl11);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem kkkToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ファイルToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem displayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ImageList LightTabImageList;
        private System.Windows.Forms.OpenFileDialog open3DFileDialog1;
        private Tao_Sample.UserControl1 userControl11;
        private MainTabMenuControl mainTabMenuControl1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem キャンセルToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem モデルの追加ToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog Add3DFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem デバッグ断面出力ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem デバッグシェルToolStripMenuItem;
    }
}

