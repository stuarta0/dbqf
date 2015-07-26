namespace dbqf.WinForms.Advanced
{
    partial class AdvancedPartJunctionView
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
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.layoutParts = new System.Windows.Forms.TableLayoutPanel();
            this.lblPrefix = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.layoutMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.AutoSize = true;
            this.layoutMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.layoutMain.ColumnCount = 3;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutMain.Controls.Add(this.layoutParts, 1, 0);
            this.layoutMain.Controls.Add(this.lblPrefix, 0, 0);
            this.layoutMain.Controls.Add(this.btnDelete, 2, 0);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 1;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Size = new System.Drawing.Size(65, 26);
            this.layoutMain.TabIndex = 0;
            // 
            // layoutParts
            // 
            this.layoutParts.AutoSize = true;
            this.layoutParts.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.layoutParts.ColumnCount = 1;
            this.layoutParts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutParts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutParts.Location = new System.Drawing.Point(39, 0);
            this.layoutParts.Margin = new System.Windows.Forms.Padding(0);
            this.layoutParts.Name = "layoutParts";
            this.layoutParts.RowCount = 1;
            this.layoutParts.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutParts.Size = new System.Drawing.Size(1, 26);
            this.layoutParts.TabIndex = 0;
            // 
            // lblPrefix
            // 
            this.lblPrefix.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPrefix.AutoSize = true;
            this.lblPrefix.Location = new System.Drawing.Point(3, 6);
            this.lblPrefix.Name = "lblPrefix";
            this.lblPrefix.Size = new System.Drawing.Size(33, 13);
            this.lblPrefix.TabIndex = 1;
            this.lblPrefix.Text = "Prefix";
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Image = global::dbqf.WinForms.Properties.Resources.firefox_close_inactive;
            this.btnDelete.Location = new System.Drawing.Point(42, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(20, 20);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // AdvancedPartJunctionView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.layoutMain);
            this.Name = "AdvancedPartJunctionView";
            this.Size = new System.Drawing.Size(65, 26);
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.TableLayoutPanel layoutParts;
        private System.Windows.Forms.Label lblPrefix;
        private System.Windows.Forms.Button btnDelete;
    }
}
