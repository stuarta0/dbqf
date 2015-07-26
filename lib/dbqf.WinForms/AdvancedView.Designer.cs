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
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.cboBuilders = new dbqf.WinForms.Controls.BindableComboBox();
            this.bsAdapter = new System.Windows.Forms.BindingSource(this.components);
            this.builderSourceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblField = new System.Windows.Forms.Label();
            this.lblOperator = new System.Windows.Forms.Label();
            this.lblValue = new System.Windows.Forms.Label();
            this.cboSubject = new dbqf.WinForms.Controls.BindableComboBox();
            this.subjectSourceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.layoutAndOr = new System.Windows.Forms.TableLayoutPanel();
            this.btnAnd = new System.Windows.Forms.Button();
            this.btnOr = new System.Windows.Forms.Button();
            this.pnlParameters = new System.Windows.Forms.Panel();
            this.fieldPathCombo = new dbqf.WinForms.FieldPathCombo();
            this.layoutMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsAdapter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.builderSourceBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subjectSourceBindingSource)).BeginInit();
            this.layoutAndOr.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.AutoSize = true;
            this.layoutMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.layoutMain.ColumnCount = 2;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Controls.Add(this.cboBuilders, 1, 2);
            this.layoutMain.Controls.Add(this.lblField, 0, 0);
            this.layoutMain.Controls.Add(this.lblOperator, 0, 2);
            this.layoutMain.Controls.Add(this.lblValue, 0, 3);
            this.layoutMain.Controls.Add(this.cboSubject, 1, 0);
            this.layoutMain.Controls.Add(this.layoutAndOr, 1, 4);
            this.layoutMain.Controls.Add(this.pnlParameters, 0, 5);
            this.layoutMain.Controls.Add(this.fieldPathCombo, 1, 1);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 6;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.layoutMain.Size = new System.Drawing.Size(281, 296);
            this.layoutMain.TabIndex = 0;
            // 
            // cboBuilders
            // 
            this.cboBuilders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboBuilders.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.bsAdapter, "SelectedBuilder", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cboBuilders.DataSource = this.builderSourceBindingSource;
            this.cboBuilders.DisplayMember = "Label";
            this.cboBuilders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBuilders.FormattingEnabled = true;
            this.cboBuilders.Location = new System.Drawing.Point(60, 30);
            this.cboBuilders.Name = "cboBuilders";
            this.cboBuilders.Size = new System.Drawing.Size(218, 21);
            this.cboBuilders.TabIndex = 4;
            // 
            // bsAdapter
            // 
            this.bsAdapter.DataSource = typeof(dbqf.WinForms.Advanced.WinFormsAdvancedAdapter);
            // 
            // builderSourceBindingSource
            // 
            this.builderSourceBindingSource.DataMember = "BuilderSource";
            this.builderSourceBindingSource.DataSource = this.bsAdapter;
            // 
            // lblField
            // 
            this.lblField.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblField.AutoSize = true;
            this.lblField.Location = new System.Drawing.Point(3, 7);
            this.lblField.Name = "lblField";
            this.lblField.Size = new System.Drawing.Size(32, 13);
            this.lblField.TabIndex = 0;
            this.lblField.Text = "Field:";
            // 
            // lblOperator
            // 
            this.lblOperator.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblOperator.AutoSize = true;
            this.lblOperator.Location = new System.Drawing.Point(3, 34);
            this.lblOperator.Name = "lblOperator";
            this.lblOperator.Size = new System.Drawing.Size(51, 13);
            this.lblOperator.TabIndex = 3;
            this.lblOperator.Text = "Operator:";
            // 
            // lblValue
            // 
            this.lblValue.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblValue.AutoSize = true;
            this.lblValue.Location = new System.Drawing.Point(3, 54);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(37, 13);
            this.lblValue.TabIndex = 5;
            this.lblValue.Text = "Value:";
            // 
            // cboSubject
            // 
            this.cboSubject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSubject.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.bsAdapter, "SelectedSubject", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cboSubject.DataSource = this.subjectSourceBindingSource;
            this.cboSubject.DisplayMember = "DisplayName";
            this.cboSubject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSubject.FormattingEnabled = true;
            this.cboSubject.Location = new System.Drawing.Point(60, 3);
            this.cboSubject.Name = "cboSubject";
            this.cboSubject.Size = new System.Drawing.Size(218, 21);
            this.cboSubject.TabIndex = 1;
            // 
            // subjectSourceBindingSource
            // 
            this.subjectSourceBindingSource.DataMember = "SubjectSource";
            this.subjectSourceBindingSource.DataSource = this.bsAdapter;
            // 
            // layoutAndOr
            // 
            this.layoutAndOr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.layoutAndOr.AutoSize = true;
            this.layoutAndOr.ColumnCount = 2;
            this.layoutAndOr.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutAndOr.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutAndOr.Controls.Add(this.btnAnd, 0, 0);
            this.layoutAndOr.Controls.Add(this.btnOr, 1, 0);
            this.layoutAndOr.Location = new System.Drawing.Point(189, 67);
            this.layoutAndOr.Margin = new System.Windows.Forms.Padding(0);
            this.layoutAndOr.Name = "layoutAndOr";
            this.layoutAndOr.RowCount = 1;
            this.layoutAndOr.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutAndOr.Size = new System.Drawing.Size(92, 29);
            this.layoutAndOr.TabIndex = 7;
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
            this.btnOr.TabIndex = 1;
            this.btnOr.Text = "OR";
            this.btnOr.UseVisualStyleBackColor = true;
            this.btnOr.Click += new System.EventHandler(this.btnOr_Click);
            // 
            // pnlParameters
            // 
            this.pnlParameters.AutoScroll = true;
            this.pnlParameters.BackColor = System.Drawing.Color.White;
            this.pnlParameters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.layoutMain.SetColumnSpan(this.pnlParameters, 2);
            this.pnlParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlParameters.Location = new System.Drawing.Point(3, 99);
            this.pnlParameters.Name = "pnlParameters";
            this.pnlParameters.Size = new System.Drawing.Size(275, 194);
            this.pnlParameters.TabIndex = 8;
            // 
            // fieldPathCombo
            // 
            this.fieldPathCombo.Adapter = null;
            this.fieldPathCombo.AutoSize = true;
            this.fieldPathCombo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fieldPathCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fieldPathCombo.Location = new System.Drawing.Point(57, 27);
            this.fieldPathCombo.Margin = new System.Windows.Forms.Padding(0);
            this.fieldPathCombo.Name = "fieldPathCombo";
            this.fieldPathCombo.Size = new System.Drawing.Size(224, 1);
            this.fieldPathCombo.TabIndex = 2;
            // 
            // AdvancedView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.layoutMain);
            this.Name = "AdvancedView";
            this.Size = new System.Drawing.Size(281, 296);
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsAdapter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.builderSourceBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subjectSourceBindingSource)).EndInit();
            this.layoutAndOr.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private dbqf.WinForms.Controls.BindableComboBox cboBuilders;
        private System.Windows.Forms.Label lblField;
        private System.Windows.Forms.Label lblOperator;
        private System.Windows.Forms.Label lblValue;
        private dbqf.WinForms.Controls.BindableComboBox cboSubject;
        private System.Windows.Forms.TableLayoutPanel layoutAndOr;
        private System.Windows.Forms.Button btnAnd;
        private System.Windows.Forms.Button btnOr;
        private System.Windows.Forms.Panel pnlParameters;
        private System.Windows.Forms.BindingSource bsAdapter;
        private System.Windows.Forms.BindingSource builderSourceBindingSource;
        private System.Windows.Forms.BindingSource subjectSourceBindingSource;
        private FieldPathCombo fieldPathCombo;
    }
}
