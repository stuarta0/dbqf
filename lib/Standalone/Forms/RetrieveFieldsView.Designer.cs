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
            this.rdoPredefined = new System.Windows.Forms.RadioButton();
            this.bsAdapter = new System.Windows.Forms.BindingSource(this.components);
            this.rdoCustom = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lstCustom = new System.Windows.Forms.ListBox();
            this.fieldsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.table.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsAdapter)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // table
            // 
            this.table.ColumnCount = 1;
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table.Controls.Add(this.rdoPredefined, 0, 0);
            this.table.Controls.Add(this.rdoCustom, 0, 1);
            this.table.Controls.Add(this.tableLayoutPanel1, 0, 2);
            this.table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table.Location = new System.Drawing.Point(0, 0);
            this.table.Name = "table";
            this.table.RowCount = 3;
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.table.Size = new System.Drawing.Size(280, 389);
            this.table.TabIndex = 0;
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
            // bsAdapter
            // 
            this.bsAdapter.DataSource = typeof(Standalone.Forms.RetrieveFieldsViewAdapter);
            // 
            // rdoCustom
            // 
            this.rdoCustom.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rdoCustom.AutoSize = true;
            this.rdoCustom.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bsAdapter, "RetrieveFieldMethod", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rdoCustom.Location = new System.Drawing.Point(3, 26);
            this.rdoCustom.Name = "rdoCustom";
            this.rdoCustom.Size = new System.Drawing.Size(60, 17);
            this.rdoCustom.TabIndex = 0;
            this.rdoCustom.TabStop = true;
            this.rdoCustom.Text = "Custom";
            this.rdoCustom.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.lstCustom, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnMoveUp, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnMoveDown, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnDelete, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 49);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(274, 337);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // lstCustom
            // 
            this.lstCustom.DataSource = this.fieldsBindingSource;
            this.lstCustom.DisplayMember = "Description";
            this.lstCustom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstCustom.FormattingEnabled = true;
            this.lstCustom.Location = new System.Drawing.Point(3, 3);
            this.lstCustom.Name = "lstCustom";
            this.tableLayoutPanel1.SetRowSpan(this.lstCustom, 4);
            this.lstCustom.Size = new System.Drawing.Size(239, 331);
            this.lstCustom.TabIndex = 0;
            // 
            // fieldsBindingSource
            // 
            this.fieldsBindingSource.DataMember = "Fields";
            this.fieldsBindingSource.DataSource = this.bsAdapter;
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Location = new System.Drawing.Point(248, 3);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(23, 23);
            this.btnMoveUp.TabIndex = 1;
            this.btnMoveUp.Text = "^";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Location = new System.Drawing.Point(248, 32);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(23, 23);
            this.btnMoveDown.TabIndex = 1;
            this.btnMoveDown.Text = "v";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(248, 61);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(23, 23);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "x";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // RetrieveFieldsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.table);
            this.Name = "RetrieveFieldsView";
            this.Size = new System.Drawing.Size(280, 389);
            this.table.ResumeLayout(false);
            this.table.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsAdapter)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fieldsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel table;
        private System.Windows.Forms.RadioButton rdoPredefined;
        private System.Windows.Forms.RadioButton rdoCustom;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListBox lstCustom;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.BindingSource bsAdapter;
        private System.Windows.Forms.BindingSource fieldsBindingSource;
    }
}
