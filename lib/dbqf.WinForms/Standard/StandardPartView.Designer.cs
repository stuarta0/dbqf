namespace dbqf.WinForms.Standard
{
    partial class StandardPartView
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
            this.layout = new System.Windows.Forms.TableLayoutPanel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.cboPath = new dbqf.WinForms.Controls.BindableComboBox();
            this.bsPart = new System.Windows.Forms.BindingSource(this.components);
            this.pathsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cboBuilder = new dbqf.WinForms.Controls.BindableComboBox();
            this.buildersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.layout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsPart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pathsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buildersBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // layout
            // 
            this.layout.AutoSize = true;
            this.layout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.layout.ColumnCount = 4;
            this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.36364F));
            this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.27273F));
            this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.36364F));
            this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layout.Controls.Add(this.btnDelete, 3, 0);
            this.layout.Controls.Add(this.cboPath, 0, 0);
            this.layout.Controls.Add(this.cboBuilder, 1, 0);
            this.layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layout.Location = new System.Drawing.Point(0, 0);
            this.layout.Margin = new System.Windows.Forms.Padding(0);
            this.layout.Name = "layout";
            this.layout.RowCount = 1;
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layout.Size = new System.Drawing.Size(488, 27);
            this.layout.TabIndex = 0;
            // 
            // btnDelete
            // 
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Image = global::dbqf.WinForms.Properties.Resources.firefox_close_inactive;
            this.btnDelete.Location = new System.Drawing.Point(465, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(20, 20);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // cboPath
            // 
            this.cboPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboPath.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.bsPart, "SelectedPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cboPath.DataSource = this.pathsBindingSource;
            this.cboPath.DisplayMember = "Description";
            this.cboPath.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPath.FormattingEnabled = true;
            this.cboPath.Location = new System.Drawing.Point(3, 3);
            this.cboPath.Name = "cboPath";
            this.cboPath.Size = new System.Drawing.Size(162, 21);
            this.cboPath.TabIndex = 1;
            // 
            // bsPart
            // 
            this.bsPart.DataSource = typeof(dbqf.WinForms.Standard.WinFormsStandardPart);
            // 
            // pathsBindingSource
            // 
            this.pathsBindingSource.DataMember = "Paths";
            this.pathsBindingSource.DataSource = this.bsPart;
            // 
            // cboBuilder
            // 
            this.cboBuilder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboBuilder.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.bsPart, "SelectedBuilder", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cboBuilder.DataSource = this.buildersBindingSource;
            this.cboBuilder.DisplayMember = "Label";
            this.cboBuilder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBuilder.FormattingEnabled = true;
            this.cboBuilder.Location = new System.Drawing.Point(171, 3);
            this.cboBuilder.Name = "cboBuilder";
            this.cboBuilder.Size = new System.Drawing.Size(120, 21);
            this.cboBuilder.TabIndex = 2;
            // 
            // buildersBindingSource
            // 
            this.buildersBindingSource.DataMember = "Builders";
            this.buildersBindingSource.DataSource = this.bsPart;
            // 
            // StandardPartView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.layout);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "StandardPartView";
            this.Size = new System.Drawing.Size(488, 27);
            this.layout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsPart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pathsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buildersBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layout;
        private System.Windows.Forms.Button btnDelete;
        private dbqf.WinForms.Controls.BindableComboBox cboPath;
        private dbqf.WinForms.Controls.BindableComboBox cboBuilder;
        private System.Windows.Forms.BindingSource bsPart;
        private System.Windows.Forms.BindingSource pathsBindingSource;
        private System.Windows.Forms.BindingSource buildersBindingSource;
    }
}
