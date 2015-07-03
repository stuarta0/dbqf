namespace dbqf.WinForms
{
    partial class AdvancedView
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
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboSubject = new dbqf.WinForms.Controls.BindableComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAnd = new System.Windows.Forms.Button();
            this.btnOr = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bsAdapter = new System.Windows.Forms.BindingSource(this.components);
            this.subjectSourceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.builderSourceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.layout.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsAdapter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subjectSourceBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.builderSourceBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // layout
            // 
            this.layout.AutoSize = true;
            this.layout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.layout.ColumnCount = 2;
            this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layout.Controls.Add(this.cboBuilders, 1, 2);
            this.layout.Controls.Add(this.label1, 0, 0);
            this.layout.Controls.Add(this.label3, 0, 2);
            this.layout.Controls.Add(this.label4, 0, 3);
            this.layout.Controls.Add(this.cboSubject, 1, 0);
            this.layout.Controls.Add(this.tableLayoutPanel1, 1, 4);
            this.layout.Controls.Add(this.panel1, 0, 5);
            this.layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layout.Location = new System.Drawing.Point(0, 0);
            this.layout.Name = "layout";
            this.layout.RowCount = 6;
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.layout.Size = new System.Drawing.Size(281, 296);
            this.layout.TabIndex = 0;
            // 
            // cboBuilders
            // 
            this.cboBuilders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboBuilders.DataSource = this.builderSourceBindingSource;
            this.cboBuilders.DisplayMember = "Label";
            this.cboBuilders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBuilders.FormattingEnabled = true;
            this.cboBuilders.Location = new System.Drawing.Point(60, 30);
            this.cboBuilders.Name = "cboBuilders";
            this.cboBuilders.Size = new System.Drawing.Size(218, 21);
            this.cboBuilders.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Field:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Operator:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Value:";
            // 
            // cboSubject
            // 
            this.cboSubject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSubject.DataSource = this.subjectSourceBindingSource;
            this.cboSubject.DisplayMember = "DisplayName";
            this.cboSubject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSubject.FormattingEnabled = true;
            this.cboSubject.Location = new System.Drawing.Point(60, 3);
            this.cboSubject.Name = "cboSubject";
            this.cboSubject.Size = new System.Drawing.Size(218, 21);
            this.cboSubject.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.btnAnd, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnOr, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(189, 67);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(92, 29);
            this.tableLayoutPanel1.TabIndex = 23;
            // 
            // btnAnd
            // 
            this.btnAnd.Location = new System.Drawing.Point(3, 3);
            this.btnAnd.Name = "btnAnd";
            this.btnAnd.Size = new System.Drawing.Size(40, 23);
            this.btnAnd.TabIndex = 0;
            this.btnAnd.Text = "AND";
            this.btnAnd.UseVisualStyleBackColor = true;
            this.btnAnd.Click += new System.EventHandler(this.btnAnd_Click);
            // 
            // btnOr
            // 
            this.btnOr.Location = new System.Drawing.Point(49, 3);
            this.btnOr.Name = "btnOr";
            this.btnOr.Size = new System.Drawing.Size(40, 23);
            this.btnOr.TabIndex = 0;
            this.btnOr.Text = "OR";
            this.btnOr.UseVisualStyleBackColor = true;
            this.btnOr.Click += new System.EventHandler(this.btnOr_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.layout.SetColumnSpan(this.panel1, 2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 99);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(275, 194);
            this.panel1.TabIndex = 24;
            // 
            // bsAdapter
            // 
            this.bsAdapter.DataSource = typeof(dbqf.WinForms.Advanced.WinFormsAdvancedAdapter);
            // 
            // subjectSourceBindingSource
            // 
            this.subjectSourceBindingSource.DataMember = "SubjectSource";
            this.subjectSourceBindingSource.DataSource = this.bsAdapter;
            // 
            // builderSourceBindingSource
            // 
            this.builderSourceBindingSource.DataMember = "BuilderSource";
            this.builderSourceBindingSource.DataSource = this.bsAdapter;
            // 
            // AdvancedView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.layout);
            this.Name = "AdvancedView";
            this.Size = new System.Drawing.Size(281, 296);
            this.layout.ResumeLayout(false);
            this.layout.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsAdapter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subjectSourceBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.builderSourceBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layout;
        private dbqf.WinForms.Controls.BindableComboBox cboBuilders;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private dbqf.WinForms.Controls.BindableComboBox cboSubject;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnAnd;
        private System.Windows.Forms.Button btnOr;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.BindingSource bsAdapter;
        private System.Windows.Forms.BindingSource builderSourceBindingSource;
        private System.Windows.Forms.BindingSource subjectSourceBindingSource;
    }
}
