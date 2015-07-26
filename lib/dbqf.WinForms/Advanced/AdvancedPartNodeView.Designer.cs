namespace dbqf.WinForms.Advanced
{
    partial class AdvancedPartNodeView
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
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblPrefix = new System.Windows.Forms.Label();
            this.bsNode = new System.Windows.Forms.BindingSource(this.components);
            this.lblDescription = new System.Windows.Forms.Label();
            this.layoutMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsNode)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.AutoSize = true;
            this.layoutMain.ColumnCount = 3;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutMain.Controls.Add(this.btnDelete, 2, 0);
            this.layoutMain.Controls.Add(this.lblPrefix, 0, 0);
            this.layoutMain.Controls.Add(this.lblDescription, 1, 0);
            this.layoutMain.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", this.bsNode, "NodeBackColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 1;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.layoutMain.Size = new System.Drawing.Size(122, 26);
            this.layoutMain.TabIndex = 0;
            // 
            // btnDelete
            // 
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Image = global::dbqf.WinForms.Properties.Resources.firefox_close_inactive;
            this.btnDelete.Location = new System.Drawing.Point(99, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(20, 20);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblPrefix
            // 
            this.lblPrefix.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPrefix.AutoSize = true;
            this.lblPrefix.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsNode, "Prefix", true));
            this.lblPrefix.Location = new System.Drawing.Point(3, 6);
            this.lblPrefix.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblPrefix.Name = "lblPrefix";
            this.lblPrefix.Size = new System.Drawing.Size(33, 13);
            this.lblPrefix.TabIndex = 0;
            this.lblPrefix.Text = "Prefix";
            // 
            // bsNode
            // 
            this.bsNode.DataSource = typeof(dbqf.WinForms.Advanced.WinFormsAdvancedPartNode);
            // 
            // lblDescription
            // 
            this.lblDescription.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDescription.AutoSize = true;
            this.lblDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsNode, "Description", true));
            this.lblDescription.Location = new System.Drawing.Point(36, 6);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(60, 13);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "Description";
            // 
            // AdvancedPartNodeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.layoutMain);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AdvancedPartNodeView";
            this.Size = new System.Drawing.Size(122, 26);
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsNode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.Label lblPrefix;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.BindingSource bsNode;
        private System.Windows.Forms.Button btnDelete;
    }
}
