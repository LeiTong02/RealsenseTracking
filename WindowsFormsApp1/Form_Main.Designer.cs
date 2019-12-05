namespace mycontracts
{
    partial class Form_Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolstrip_add = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_edit = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_delete = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_search = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDown_DataCollection = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItem_views = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton_openDir = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(0, 28);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(906, 435);
            this.dataGridView1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolstrip_add,
            this.toolStrip_edit,
            this.toolStrip_delete,
            this.toolStrip_search,
            this.toolStripDropDown_DataCollection,
            this.toolStripButton_openDir});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(906, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolstrip_add
            // 
            this.toolstrip_add.Image = ((System.Drawing.Image)(resources.GetObject("toolstrip_add.Image")));
            this.toolstrip_add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstrip_add.Name = "toolstrip_add";
            this.toolstrip_add.Size = new System.Drawing.Size(49, 22);
            this.toolstrip_add.Text = "Add";
            this.toolstrip_add.Click += new System.EventHandler(this.toolstrip_add_Click);
            // 
            // toolStrip_edit
            // 
            this.toolStrip_edit.Image = ((System.Drawing.Image)(resources.GetObject("toolStrip_edit.Image")));
            this.toolStrip_edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStrip_edit.Name = "toolStrip_edit";
            this.toolStrip_edit.Size = new System.Drawing.Size(47, 22);
            this.toolStrip_edit.Text = "Edit";
            this.toolStrip_edit.Click += new System.EventHandler(this.toolStrip_edit_Click);
            // 
            // toolStrip_delete
            // 
            this.toolStrip_delete.Image = ((System.Drawing.Image)(resources.GetObject("toolStrip_delete.Image")));
            this.toolStrip_delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStrip_delete.Name = "toolStrip_delete";
            this.toolStrip_delete.Size = new System.Drawing.Size(60, 22);
            this.toolStrip_delete.Text = "Delete";
            this.toolStrip_delete.Click += new System.EventHandler(this.toolStrip_delete_Click);
            // 
            // toolStrip_search
            // 
            this.toolStrip_search.Image = ((System.Drawing.Image)(resources.GetObject("toolStrip_search.Image")));
            this.toolStrip_search.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStrip_search.Name = "toolStrip_search";
            this.toolStrip_search.Size = new System.Drawing.Size(62, 22);
            this.toolStrip_search.Text = "Search";
            this.toolStrip_search.Click += new System.EventHandler(this.toolStrip_search_Click);
            // 
            // toolStripDropDown_DataCollection
            // 
            this.toolStripDropDown_DataCollection.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_views,
            this.toolStripMenuItem1});
            this.toolStripDropDown_DataCollection.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripDropDown_DataCollection.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDown_DataCollection.Image")));
            this.toolStripDropDown_DataCollection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDown_DataCollection.Name = "toolStripDropDown_DataCollection";
            this.toolStripDropDown_DataCollection.Size = new System.Drawing.Size(117, 22);
            this.toolStripDropDown_DataCollection.Text = "Data Collection";
            // 
            // toolStripMenuItem_views
            // 
            this.toolStripMenuItem_views.Name = "toolStripMenuItem_views";
            this.toolStripMenuItem_views.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem_views.Text = "Body Plane";
            this.toolStripMenuItem_views.Click += new System.EventHandler(this.toolStripMenuItem_views_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem1.Text = "Lissajous Figure";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripButton_openDir
            // 
            this.toolStripButton_openDir.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_openDir.Image")));
            this.toolStripButton_openDir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_openDir.Name = "toolStripButton_openDir";
            this.toolStripButton_openDir.Size = new System.Drawing.Size(92, 22);
            this.toolStripButton_openDir.Text = "Open Folder";
            this.toolStripButton_openDir.Click += new System.EventHandler(this.toolStripButton_openDir_Click);
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(906, 463);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form_Main";
            this.Text = "Form Main";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolstrip_add;
        private System.Windows.Forms.ToolStripButton toolStrip_edit;
        private System.Windows.Forms.ToolStripButton toolStrip_delete;
        private System.Windows.Forms.ToolStripButton toolStrip_search;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDown_DataCollection;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_views;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripButton toolStripButton_openDir;
    }
}

