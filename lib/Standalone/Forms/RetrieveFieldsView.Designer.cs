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
            this.rdoPredefined = new System.Windows.Forms.RadioButton();
            this.rdoCustom = new System.Windows.Forms.RadioButton();
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
            this.table.ColumnCount = 2;
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table.Controls.Add(this.splitContainer1, 0, 1);
            this.table.Controls.Add(this.rdoPredefined, 0, 0);
            this.table.Controls.Add(this.rdoCustom, 1, 0);
            this.table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table.Location = new System.Drawing.Point(0, 0);
            this.table.Name = "table";
            this.table.RowCount = 2;
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.table.Size = new System.Drawing.Size(161, 207);
            this.table.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.table.SetColumnSpan(this.splitContainer1, 2);
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
            this.splitContainer1.Size = new System.Drawing.Size(161, 184);
            this.splitContainer1.SplitterDistance = 80;
            this.splitContainer1.TabIndex = 1;
            // 
            // tree
            // 
            this.tree.AllowDrop = true;
            this.tree.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bsAdapter, "FieldsEnabled", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree.Location = new System.Drawing.Point(0, 0);
            this.tree.Name = "tree";
            this.tree.Size = new System.Drawing.Size(80, 184);
            this.tree.TabIndex = 0;
            this.tree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tree_BeforeExpand);
            this.tree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tree_ItemDrag);
            this.tree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tree_NodeMouseDoubleClick);
            // 
            // lstCustom
            // 
            this.lstCustom.AllowDrop = true;
            this.lstCustom.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bsAdapter, "FieldsEnabled", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lstCustom.DataSource = this.fieldsBindingSource;
            this.lstCustom.DisplayMember = "Description";
            this.lstCustom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstCustom.FormattingEnabled = true;
            this.lstCustom.IntegralHeight = false;
            this.lstCustom.Location = new System.Drawing.Point(0, 0);
            this.lstCustom.Name = "lstCustom";
            this.lstCustom.Size = new System.Drawing.Size(77, 184);
            this.lstCustom.TabIndex = 0;
            this.lstCustom.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstCustom_DragDrop);
            this.lstCustom.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstCustom_DragEnter);
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
            // rdoPredefined
            // 
            this.rdoPredefined.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rdoPredefined.AutoSize = true;
            this.rdoPredefined.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bsAdapter, "RetrieveFieldMethod", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rdoPredefined.Location = new System.Drawing.Point(3, 3);
            this.rdoPredefined.Name = "rdoPredefined";
            this.rdoPredefined.Size = new System.Drawing.Size(76, 17);
            this.rdoPredefined.TabIndex = 0;
            this.rdoPredefined.TabStop = true;
            this.rdoPredefined.Text = "Predefined";
            this.rdoPredefined.UseVisualStyleBackColor = true;
            // 
            // rdoCustom
            // 
            this.rdoCustom.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rdoCustom.AutoSize = true;
            this.rdoCustom.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bsAdapter, "RetrieveFieldMethod", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rdoCustom.Location = new System.Drawing.Point(85, 3);
            this.rdoCustom.Name = "rdoCustom";
            this.rdoCustom.Size = new System.Drawing.Size(60, 17);
            this.rdoCustom.TabIndex = 0;
            this.rdoCustom.TabStop = true;
            this.rdoCustom.Text = "Custom";
            this.rdoCustom.UseVisualStyleBackColor = true;
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
        private System.Windows.Forms.RadioButton rdoPredefined;
        private System.Windows.Forms.RadioButton rdoCustom;
        private System.Windows.Forms.ListBox lstCustom;
        private System.Windows.Forms.BindingSource bsAdapter;
        private System.Windows.Forms.BindingSource fieldsBindingSource;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView tree;
    }
}
