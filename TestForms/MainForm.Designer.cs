using FontAwesome.Sharp;

namespace TestForms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node2");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node1", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Node3");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Root", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3});
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.iconMenuItem1 = new FontAwesome.Sharp.IconMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuHome = new FontAwesome.Sharp.IconMenuItem();
            this._openMenuItem = new FontAwesome.Sharp.IconMenuItem();
            this._saveMenuItem = new FontAwesome.Sharp.IconMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._quitMenuItem = new FontAwesome.Sharp.IconMenuItem();
            this._iconMenuItem1 = new FontAwesome.Sharp.IconMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSignIn = new FontAwesome.Sharp.IconToolStripButton();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.iconDropDown = new FontAwesome.Sharp.IconDropDownButton();
            this.iconMenuItem2 = new FontAwesome.Sharp.IconMenuItem();
            this.toolStripSplitButton1 = new FontAwesome.Sharp.IconSplitButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnXing = new FontAwesome.Sharp.IconButton();
            this.btnGooglePlus = new FontAwesome.Sharp.IconButton();
            this.boxOctocat = new FontAwesome.Sharp.IconPictureBox();
            this.boxTwitter = new FontAwesome.Sharp.IconPictureBox();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.boxOctocat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boxTwitter)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.iconMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(159, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(158, 22);
            this.toolStripMenuItem1.Text = "menuItem1";
            this.toolStripMenuItem1.ToolTipText = "Regular menu item";
            // 
            // iconMenuItem1
            // 
            this.iconMenuItem1.Icon = FontAwesome.Sharp.IconChar.Wpforms;
            this.iconMenuItem1.IconColor = System.Drawing.Color.Black;
            this.iconMenuItem1.IconSize = 16;
            this.iconMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("iconMenuItem1.Image")));
            this.iconMenuItem1.Name = "iconMenuItem1";
            this.iconMenuItem1.Size = new System.Drawing.Size(158, 22);
            this.iconMenuItem1.Text = "iconMenuItem1";
            this.iconMenuItem1.ToolTipText = "wpforms icon (fa 4.6.1)";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHome,
            this._iconMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(341, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuHome
            // 
            this.mnuHome.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._openMenuItem,
            this._saveMenuItem,
            this.toolStripSeparator1,
            this._quitMenuItem});
            this.mnuHome.Icon = FontAwesome.Sharp.IconChar.Home;
            this.mnuHome.IconColor = System.Drawing.Color.DarkBlue;
            this.mnuHome.IconSize = 16;
            this.mnuHome.Image = ((System.Drawing.Image)(resources.GetObject("mnuHome.Image")));
            this.mnuHome.Name = "mnuHome";
            this.mnuHome.Size = new System.Drawing.Size(53, 20);
            this.mnuHome.Text = "&File";
            // 
            // _openMenuItem
            // 
            this._openMenuItem.BackColor = System.Drawing.Color.Transparent;
            this._openMenuItem.Icon = FontAwesome.Sharp.IconChar.FileO;
            this._openMenuItem.IconColor = System.Drawing.Color.Black;
            this._openMenuItem.IconSize = 16;
            this._openMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_openMenuItem.Image")));
            this._openMenuItem.Name = "_openMenuItem";
            this._openMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this._openMenuItem.Size = new System.Drawing.Size(146, 22);
            this._openMenuItem.Text = "&Open";
            // 
            // _saveMenuItem
            // 
            this._saveMenuItem.Icon = FontAwesome.Sharp.IconChar.FloppyO;
            this._saveMenuItem.IconColor = System.Drawing.Color.Black;
            this._saveMenuItem.IconSize = 16;
            this._saveMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_saveMenuItem.Image")));
            this._saveMenuItem.Name = "_saveMenuItem";
            this._saveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this._saveMenuItem.Size = new System.Drawing.Size(146, 22);
            this._saveMenuItem.Text = "&Save";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // _quitMenuItem
            // 
            this._quitMenuItem.Icon = FontAwesome.Sharp.IconChar.TimesCircle;
            this._quitMenuItem.IconColor = System.Drawing.Color.Black;
            this._quitMenuItem.IconSize = 16;
            this._quitMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_quitMenuItem.Image")));
            this._quitMenuItem.Name = "_quitMenuItem";
            this._quitMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this._quitMenuItem.Size = new System.Drawing.Size(146, 22);
            this._quitMenuItem.Text = "&Quit";
            // 
            // _iconMenuItem1
            // 
            this._iconMenuItem1.Icon = FontAwesome.Sharp.IconChar.QuestionCircle;
            this._iconMenuItem1.IconColor = System.Drawing.Color.DarkBlue;
            this._iconMenuItem1.IconSize = 17;
            this._iconMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("_iconMenuItem1.Image")));
            this._iconMenuItem1.Name = "_iconMenuItem1";
            this._iconMenuItem1.Size = new System.Drawing.Size(60, 20);
            this._iconMenuItem1.Text = "&Help";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSignIn});
            this.toolStrip1.Location = new System.Drawing.Point(3, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(75, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnSignIn
            // 
            this.btnSignIn.Icon = FontAwesome.Sharp.IconChar.SignIn;
            this.btnSignIn.IconColor = System.Drawing.Color.Black;
            this.btnSignIn.IconSize = 16;
            this.btnSignIn.Image = ((System.Drawing.Image)(resources.GetObject("btnSignIn.Image")));
            this.btnSignIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSignIn.Name = "btnSignIn";
            this.btnSignIn.Size = new System.Drawing.Size(63, 22);
            this.btnSignIn.Text = "Sign In";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(341, 224);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(341, 295);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.iconDropDown,
            this.toolStripSplitButton1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(341, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 17);
            this.lblStatus.Text = "Status";
            // 
            // iconDropDown
            // 
            this.iconDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.iconDropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iconMenuItem2});
            this.iconDropDown.Icon = FontAwesome.Sharp.IconChar.Cog;
            this.iconDropDown.IconColor = System.Drawing.Color.DimGray;
            this.iconDropDown.IconSize = 16;
            this.iconDropDown.Image = ((System.Drawing.Image)(resources.GetObject("iconDropDown.Image")));
            this.iconDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.iconDropDown.Name = "iconDropDown";
            this.iconDropDown.Size = new System.Drawing.Size(29, 20);
            this.iconDropDown.Text = "toolStripDropDownButton1";
            // 
            // iconMenuItem2
            // 
            this.iconMenuItem2.Icon = FontAwesome.Sharp.IconChar.Star;
            this.iconMenuItem2.IconColor = System.Drawing.Color.Black;
            this.iconMenuItem2.IconSize = 16;
            this.iconMenuItem2.Image = ((System.Drawing.Image)(resources.GetObject("iconMenuItem2.Image")));
            this.iconMenuItem2.Name = "iconMenuItem2";
            this.iconMenuItem2.Size = new System.Drawing.Size(158, 22);
            this.iconMenuItem2.Text = "iconMenuItem2";
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.Icon = FontAwesome.Sharp.IconChar.Glass;
            this.toolStripSplitButton1.IconColor = System.Drawing.Color.DarkViolet;
            this.toolStripSplitButton1.IconSize = 16;
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(32, 20);
            this.toolStripSplitButton1.Text = "toolStripSplitButton1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnXing);
            this.splitContainer1.Panel2.Controls.Add(this.btnGooglePlus);
            this.splitContainer1.Panel2.Controls.Add(this.boxOctocat);
            this.splitContainer1.Panel2.Controls.Add(this.boxTwitter);
            this.splitContainer1.Size = new System.Drawing.Size(341, 224);
            this.splitContainer1.SplitterDistance = 107;
            this.splitContainer1.TabIndex = 1;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode1.ContextMenuStrip = this.contextMenuStrip1;
            treeNode1.Name = "Node2";
            treeNode1.Text = "Node2";
            treeNode2.ContextMenuStrip = this.contextMenuStrip1;
            treeNode2.Name = "Node1";
            treeNode2.Text = "Node1";
            treeNode3.Name = "Node3";
            treeNode3.Text = "Node3";
            treeNode4.ContextMenuStrip = this.contextMenuStrip1;
            treeNode4.Name = "Root";
            treeNode4.Text = "Root";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4});
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(107, 224);
            this.treeView1.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // btnXing
            // 
            this.btnXing.ContextMenuStrip = this.contextMenuStrip1;
            this.btnXing.Icon = FontAwesome.Sharp.IconChar.XingSquare;
            this.btnXing.IconColor = System.Drawing.Color.DarkSlateGray;
            this.btnXing.IconSize = 32;
            this.btnXing.Image = ((System.Drawing.Image)(resources.GetObject("btnXing.Image")));
            this.btnXing.Location = new System.Drawing.Point(121, 121);
            this.btnXing.Name = "btnXing";
            this.btnXing.Size = new System.Drawing.Size(75, 57);
            this.btnXing.TabIndex = 3;
            this.btnXing.TabStop = false;
            // 
            // btnGooglePlus
            // 
            this.btnGooglePlus.AutoSize = true;
            this.btnGooglePlus.ContextMenuStrip = this.contextMenuStrip1;
            this.btnGooglePlus.Icon = FontAwesome.Sharp.IconChar.GooglePlusSquare;
            this.btnGooglePlus.IconColor = System.Drawing.Color.Red;
            this.btnGooglePlus.IconSize = 32;
            this.btnGooglePlus.Image = ((System.Drawing.Image)(resources.GetObject("btnGooglePlus.Image")));
            this.btnGooglePlus.Location = new System.Drawing.Point(14, 121);
            this.btnGooglePlus.Name = "btnGooglePlus";
            this.btnGooglePlus.Size = new System.Drawing.Size(85, 57);
            this.btnGooglePlus.TabIndex = 2;
            this.btnGooglePlus.TabStop = false;
            // 
            // boxOctocat
            // 
            this.boxOctocat.BackColor = System.Drawing.Color.Transparent;
            this.boxOctocat.ContextMenuStrip = this.contextMenuStrip1;
            this.boxOctocat.ForeColor = System.Drawing.Color.Black;
            this.boxOctocat.Height = 85;
            this.boxOctocat.IconChar = FontAwesome.Sharp.IconChar.Github;
            this.boxOctocat.Location = new System.Drawing.Point(121, 12);
            this.boxOctocat.Name = "boxOctocat";
            this.boxOctocat.Size = new System.Drawing.Size(85, 85);
            this.boxOctocat.TabIndex = 1;
            this.boxOctocat.TabStop = false;
            this.boxOctocat.UseIconCache = true;
            this.boxOctocat.Width = 85;
            // 
            // boxTwitter
            // 
            this.boxTwitter.BackColor = System.Drawing.Color.Transparent;
            this.boxTwitter.ForeColor = System.Drawing.Color.SteelBlue;
            this.boxTwitter.Height = 85;
            this.boxTwitter.IconChar = FontAwesome.Sharp.IconChar.TwitterSquare;
            this.boxTwitter.Location = new System.Drawing.Point(14, 12);
            this.boxTwitter.Name = "boxTwitter";
            this.boxTwitter.Size = new System.Drawing.Size(85, 85);
            this.boxTwitter.TabIndex = 0;
            this.boxTwitter.TabStop = false;
            this.boxTwitter.UseIconCache = true;
            this.boxTwitter.Width = 85;
            this.boxTwitter.Click += new System.EventHandler(this.iconButton1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 295);
            this.Controls.Add(this.toolStripContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "TestForms";
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.boxOctocat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boxTwitter)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private IconPictureBox boxTwitter;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private IconMenuItem mnuHome;
        private IconMenuItem _openMenuItem;
        private IconMenuItem _saveMenuItem;
        private IconMenuItem _quitMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private IconToolStripButton btnSignIn;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private IconDropDownButton iconDropDown;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private IconSplitButton toolStripSplitButton1;
        private IconButton btnXing;
        private IconButton btnGooglePlus;
        private IconPictureBox boxOctocat;
        private IconMenuItem _iconMenuItem1;
        private System.Windows.Forms.ImageList imageList1;
        private IconMenuItem iconMenuItem2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private IconMenuItem iconMenuItem1;
    }
}

