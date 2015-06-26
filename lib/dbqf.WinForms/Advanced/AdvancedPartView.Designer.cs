namespace dbqf.WinForms.Advanced
{
    partial class AdvancedPartView
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
            this.cboBuilders = new dbqf.WinForms.Controls.BindableComboBox();
            this.bsAdvancedPart = new System.Windows.Forms.BindingSource(this.components);
            this.buildersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboSubject = new dbqf.WinForms.Controls.BindableComboBox();
            this.subjectsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.pathsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.layout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsAdvancedPart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buildersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subjectsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pathsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // layout
            // 
            this.layout.AutoSize = true;
            this.layout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.layout.ColumnCount = 2;
            this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layout.Controls.Add(this.cboBuilders, 1, 2);
            this.layout.Controls.Add(this.label1, 0, 0);
            this.layout.Controls.Add(this.label2, 0, 1);
            this.layout.Controls.Add(this.label3, 0, 2);
            this.layout.Controls.Add(this.label4, 0, 3);
            this.layout.Controls.Add(this.cboSubject, 1, 0);
            this.layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layout.Location = new System.Drawing.Point(0, 0);
            this.layout.Name = "layout";
            this.layout.RowCount = 5;
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layout.Size = new System.Drawing.Size(281, 86);
            this.layout.TabIndex = 0;
            // 
            // cboBuilders
            // 
            this.cboBuilders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboBuilders.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.bsAdvancedPart, "SelectedBuilder", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cboBuilders.DataSource = this.buildersBindingSource;
            this.cboBuilders.DisplayMember = "Label";
            this.cboBuilders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBuilders.FormattingEnabled = true;
            this.cboBuilders.Location = new System.Drawing.Point(60, 49);
            this.cboBuilders.Name = "cboBuilders";
            this.cboBuilders.Size = new System.Drawing.Size(218, 21);
            this.cboBuilders.TabIndex = 21;
            // 
            // bsAdvancedPart
            // 
            this.bsAdvancedPart.DataSource = typeof(dbqf.WinForms.Advanced.WinFormsAdvancedPartNode);
            // 
            // buildersBindingSource
            // 
            this.buildersBindingSource.DataMember = "Builders";
            this.buildersBindingSource.DataSource = this.bsAdvancedPart;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Subject:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 33);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Field:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Operator:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Value:";
            // 
            // cboSubject
            // 
            this.cboSubject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSubject.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.bsAdvancedPart, "SelectedSubject", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cboSubject.DataSource = this.subjectsBindingSource;
            this.cboSubject.DisplayMember = "DisplayName";
            this.cboSubject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSubject.FormattingEnabled = true;
            this.cboSubject.Location = new System.Drawing.Point(60, 3);
            this.cboSubject.Name = "cboSubject";
            this.cboSubject.Size = new System.Drawing.Size(218, 21);
            this.cboSubject.TabIndex = 1;
            // 
            // subjectsBindingSource
            // 
            this.subjectsBindingSource.DataMember = "Subjects";
            this.subjectsBindingSource.DataSource = this.bsAdvancedPart;
            // 
            // pathsBindingSource
            // 
            this.pathsBindingSource.DataMember = "Paths";
            this.pathsBindingSource.DataSource = this.bsAdvancedPart;
            // 
            // AdvancedPartView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.layout);
            this.Name = "AdvancedPartView";
            this.Size = new System.Drawing.Size(281, 86);
            this.layout.ResumeLayout(false);
            this.layout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsAdvancedPart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buildersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subjectsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pathsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layout;
        private dbqf.WinForms.Controls.BindableComboBox cboBuilders;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private dbqf.WinForms.Controls.BindableComboBox cboSubject;
        private System.Windows.Forms.BindingSource bsAdvancedPart;
        private System.Windows.Forms.BindingSource buildersBindingSource;
        private System.Windows.Forms.BindingSource pathsBindingSource;
        private System.Windows.Forms.BindingSource subjectsBindingSource;
    }
}
