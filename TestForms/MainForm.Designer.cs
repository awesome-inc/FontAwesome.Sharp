using FontAwesome.Sharp;

namespace TestFontForms
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuHome = new FontAwesome.Sharp.IconMenuItem();
            this._openMenuItem = new FontAwesome.Sharp.IconMenuItem();
            this._saveMenuItem = new FontAwesome.Sharp.IconMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._quitMenuItem = new FontAwesome.Sharp.IconMenuItem();
            this._iconMenuItem1 = new FontAwesome.Sharp.IconMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSignIn = new FontAwesome.Sharp.IconButton();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.iconDropDown = new FontAwesome.Sharp.IconDropDownButton();
            this.toolStripSplitButton1 = new FontAwesome.Sharp.IconSplitButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.iconPictureBox3 = new FontAwesome.Sharp.IconPictureBox();
            this.iconPictureBox2 = new FontAwesome.Sharp.IconPictureBox();
            this.iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            this.iconButton1 = new FontAwesome.Sharp.IconPictureBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconButton1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHome,
            this._iconMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(544, 24);
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
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(544, 341);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(544, 412);
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
            this.statusStrip1.Size = new System.Drawing.Size(544, 22);
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
            this.iconDropDown.Icon = FontAwesome.Sharp.IconChar.Cog;
            this.iconDropDown.IconColor = System.Drawing.Color.DimGray;
            this.iconDropDown.IconSize = 16;
            this.iconDropDown.Image = ((System.Drawing.Image)(resources.GetObject("iconDropDown.Image")));
            this.iconDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.iconDropDown.Name = "iconDropDown";
            this.iconDropDown.Size = new System.Drawing.Size(29, 20);
            this.iconDropDown.Text = "toolStripDropDownButton1";
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
            this.splitContainer1.Panel2.Controls.Add(this.iconPictureBox3);
            this.splitContainer1.Panel2.Controls.Add(this.iconPictureBox2);
            this.splitContainer1.Panel2.Controls.Add(this.iconPictureBox1);
            this.splitContainer1.Panel2.Controls.Add(this.iconButton1);
            this.splitContainer1.Size = new System.Drawing.Size(544, 341);
            this.splitContainer1.SplitterDistance = 171;
            this.splitContainer1.TabIndex = 1;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Node2";
            treeNode1.Text = "Node2";
            treeNode2.Name = "Node1";
            treeNode2.Text = "Node1";
            treeNode3.Name = "Node3";
            treeNode3.Text = "Node3";
            treeNode4.Name = "Root";
            treeNode4.Text = "Root";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4});
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(171, 341);
            this.treeView1.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // iconPictureBox3
            // 
            this.iconPictureBox3.ActiveColor = System.Drawing.Color.DarkSlateGray;
            this.iconPictureBox3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.iconPictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox3.IconChar = FontAwesome.Sharp.IconChar.XingSquare;
            this.iconPictureBox3.InActiveColor = System.Drawing.Color.DarkGray;
            this.iconPictureBox3.Location = new System.Drawing.Point(146, 124);
            this.iconPictureBox3.Name = "iconPictureBox3";
            this.iconPictureBox3.Size = new System.Drawing.Size(85, 82);
            this.iconPictureBox3.TabIndex = 3;
            this.iconPictureBox3.TabStop = false;
            // 
            // iconPictureBox2
            // 
            this.iconPictureBox2.ActiveColor = System.Drawing.Color.Red;
            this.iconPictureBox2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.iconPictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox2.IconChar = FontAwesome.Sharp.IconChar.GooglePlusSquare;
            this.iconPictureBox2.InActiveColor = System.Drawing.Color.LightCoral;
            this.iconPictureBox2.Location = new System.Drawing.Point(39, 124);
            this.iconPictureBox2.Name = "iconPictureBox2";
            this.iconPictureBox2.Size = new System.Drawing.Size(85, 82);
            this.iconPictureBox2.TabIndex = 2;
            this.iconPictureBox2.TabStop = false;
            // 
            // iconPictureBox1
            // 
            this.iconPictureBox1.ActiveColor = System.Drawing.Color.Black;
            this.iconPictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.iconPictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.Github;
            this.iconPictureBox1.InActiveColor = System.Drawing.Color.Gray;
            this.iconPictureBox1.Location = new System.Drawing.Point(146, 23);
            this.iconPictureBox1.Name = "iconPictureBox1";
            this.iconPictureBox1.Size = new System.Drawing.Size(85, 82);
            this.iconPictureBox1.TabIndex = 1;
            this.iconPictureBox1.TabStop = false;
            // 
            // iconButton1
            // 
            this.iconButton1.ActiveColor = System.Drawing.Color.SteelBlue;
            this.iconButton1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.iconButton1.BackColor = System.Drawing.Color.Transparent;
            this.iconButton1.IconChar = FontAwesome.Sharp.IconChar.TwitterSquare;
            this.iconButton1.InActiveColor = System.Drawing.Color.LightSteelBlue;
            this.iconButton1.Location = new System.Drawing.Point(39, 23);
            this.iconButton1.Name = "iconButton1";
            this.iconButton1.Size = new System.Drawing.Size(85, 82);
            this.iconButton1.TabIndex = 0;
            this.iconButton1.TabStop = false;
            this.iconButton1.Click += new System.EventHandler(this.iconButton1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 412);
            this.Controls.Add(this.toolStripContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "TestForms";
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
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconButton1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private IconPictureBox iconButton1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private IconMenuItem mnuHome;
        private IconMenuItem _openMenuItem;
        private IconMenuItem _saveMenuItem;
        private IconMenuItem _quitMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private IconButton btnSignIn;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private IconDropDownButton iconDropDown;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private IconSplitButton toolStripSplitButton1;
        private IconPictureBox iconPictureBox3;
        private IconPictureBox iconPictureBox2;
        private IconPictureBox iconPictureBox1;
        private IconMenuItem _iconMenuItem1;
        private System.Windows.Forms.ImageList imageList1;
    }
}

