namespace Standalone.Forms
{
    partial class RetrieveFieldsView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.table = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tree = new System.Windows.Forms.TreeView();
            this.lstCustom = new System.Windows.Forms.ListBox();
            this.fieldsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.bsAdapter = new System.Windows.Forms.BindingSource(this.components);
            this.chkUseFields = new System.Windows.Forms.CheckBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.table.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsAdapter)).BeginInit();
            this.SuspendLayout();
            // 
            // table
            // 
            this.table.ColumnCount = 1;
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table.Controls.Add(this.splitContainer1, 0, 1);
            this.table.Controls.Add(this.chkUseFields, 0, 0);
            this.table.Controls.Add(this.btnReset, 0, 2);
            this.table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table.Location = new System.Drawing.Point(0, 0);
            this.table.Name = "table";
            this.table.RowCount = 3;
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.table.Size = new System.Drawing.Size(161, 207);
            this.table.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 23);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lstCustom);
            this.splitContainer1.Size = new System.Drawing.Size(161, 155);
            this.splitContainer1.SplitterDistance = 80;
            this.splitContainer1.TabIndex = 1;
            // 
            // tree
            // 
            this.tree.AllowDrop = true;
            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree.Location = new System.Drawing.Point(0, 0);
            this.tree.Name = "tree";
            this.tree.Size = new System.Drawing.Size(80, 155);
            this.tree.TabIndex = 0;
            this.tree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tree_BeforeExpand);
            this.tree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tree_ItemDrag);
            this.tree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tree_NodeMouseDoubleClick);
            this.tree.DragDrop += new System.Windows.Forms.DragEventHandler(this.tree_DragDrop);
            this.tree.DragEnter += new System.Windows.Forms.DragEventHandler(this.tree_DragEnter);
            this.tree.MouseEnter += new System.EventHandler(this.tree_MouseEnter);
            // 
            // lstCustom
            // 
            this.lstCustom.AllowDrop = true;
            this.lstCustom.DataSource = this.fieldsBindingSource;
            this.lstCustom.DisplayMember = "Description";
            this.lstCustom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstCustom.FormattingEnabled = true;
            this.lstCustom.IntegralHeight = false;
            this.lstCustom.Location = new System.Drawing.Point(0, 0);
            this.lstCustom.Name = "lstCustom";
            this.lstCustom.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstCustom.Size = new System.Drawing.Size(77, 155);
            this.lstCustom.TabIndex = 0;
            this.lstCustom.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstCustom_DragDrop);
            this.lstCustom.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstCustom_DragEnter);
            this.lstCustom.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstCustom_KeyUp);
            this.lstCustom.MouseEnter += new System.EventHandler(this.lstCustom_MouseEnter);
            this.lstCustom.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lstCustom_MouseMove);
            // 
            // fieldsBindingSource
            // 
            this.fieldsBindingSource.DataMember = "Fields";
            this.fieldsBindingSource.DataSource = this.bsAdapter;
            // 
            // bsAdapter
            // 
            this.bsAdapter.DataSource = typeof(Standalone.Forms.RetrieveFieldsViewAdapter);
            // 
            // chkUseFields
            // 
            this.chkUseFields.AutoSize = true;
            this.chkUseFields.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bsAdapter, "UseFields", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkUseFields.Location = new System.Drawing.Point(3, 3);
            this.chkUseFields.Name = "chkUseFields";
            this.chkUseFields.Size = new System.Drawing.Size(142, 17);
            this.chkUseFields.TabIndex = 2;
            this.chkUseFields.Text = "Use custom output fields";
            this.chkUseFields.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(83, 181);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 3;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // RetrieveFieldsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.table);
            this.Name = "RetrieveFieldsView";
            this.Size = new System.Drawing.Size(161, 207);
            this.table.ResumeLayout(false);
            this.table.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fieldsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsAdapter)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel table;
        private System.Windows.Forms.ListBox lstCustom;
        private System.Windows.Forms.BindingSource bsAdapter;
        private System.Windows.Forms.BindingSource fieldsBindingSource;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.CheckBox chkUseFields;
        private System.Windows.Forms.Button btnReset;
    }
}
