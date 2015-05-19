namespace dbqf.WinForms
{
    partial class FieldPathCombo
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
            this.layoutFieldPaths = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // layoutFieldPaths
            // 
            this.layoutFieldPaths.AutoSize = true;
            this.layoutFieldPaths.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.layoutFieldPaths.ColumnCount = 1;
            this.layoutFieldPaths.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutFieldPaths.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutFieldPaths.Location = new System.Drawing.Point(0, 0);
            this.layoutFieldPaths.Margin = new System.Windows.Forms.Padding(0);
            this.layoutFieldPaths.Name = "layoutFieldPaths";
            this.layoutFieldPaths.RowCount = 1;
            this.layoutFieldPaths.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutFieldPaths.Size = new System.Drawing.Size(122, 44);
            this.layoutFieldPaths.TabIndex = 4;
            // 
            // FieldPathCombo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.layoutFieldPaths);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "FieldPathCombo";
            this.Size = new System.Drawing.Size(122, 44);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutFieldPaths;
    }
}
