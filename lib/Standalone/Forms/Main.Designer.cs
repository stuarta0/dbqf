namespace Standalone.Forms
{
    partial class Main
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sfdExport = new System.Windows.Forms.SaveFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControlParameters = new System.Windows.Forms.TabControl();
            this.tabPreset = new System.Windows.Forms.TabPage();
            this.tabStandard = new System.Windows.Forms.TabPage();
            this.tabAdvanced = new System.Windows.Forms.TabPage();
            this.tabOutput = new System.Windows.Forms.TabPage();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.cboSearchFor = new System.Windows.Forms.ToolStripComboBox();
            this.btnSearch = new System.Windows.Forms.ToolStripButton();
            this.btnReset = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnEnhance = new System.Windows.Forms.ToolStripButton();
            this.tabControlResults = new System.Windows.Forms.TabControl();
            this.tabResults = new System.Windows.Forms.TabPage();
            this.pnlLoading = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.picLoading = new System.Windows.Forms.PictureBox();
            this.bsAdapter = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabResultsSQL = new System.Windows.Forms.TabPage();
            this.txtSql = new FastColoredTextBoxNS.FastColoredTextBox();
            this.resultBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ofdLoad = new System.Windows.Forms.OpenFileDialog();
            this.sfdSave = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlParameters.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.tabControlResults.SuspendLayout();
            this.tabResults.SuspendLayout();
            this.pnlLoading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLoading)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsAdapter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabResultsSQL.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSql)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.connectionToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1084, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "&Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveAsToolStripMenuItem.Text = "&Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exportToolStripMenuItem.Text = "&Export...";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // connectionToolStripMenuItem
            // 
            this.connectionToolStripMenuItem.Name = "connectionToolStripMenuItem";
            this.connectionToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.connectionToolStripMenuItem.Text = "&Connection";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // sfdExport
            // 
            this.sfdExport.Filter = "Comma-separated (*.csv)|*.csv|Tab-delimited (*.txt)|*.txt";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControlParameters);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControlResults);
            this.splitContainer1.Size = new System.Drawing.Size(1084, 658);
            this.splitContainer1.SplitterDistance = 354;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // tabControlParameters
            // 
            this.tabControlParameters.Controls.Add(this.tabPreset);
            this.tabControlParameters.Controls.Add(this.tabStandard);
            this.tabControlParameters.Controls.Add(this.tabAdvanced);
            this.tabControlParameters.Controls.Add(this.tabOutput);
            this.tabControlParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlParameters.Location = new System.Drawing.Point(0, 25);
            this.tabControlParameters.Name = "tabControlParameters";
            this.tabControlParameters.SelectedIndex = 0;
            this.tabControlParameters.Size = new System.Drawing.Size(354, 633);
            this.tabControlParameters.TabIndex = 0;
            this.tabControlParameters.SelectedIndexChanged += new System.EventHandler(this.tabControlParameters_SelectedIndexChanged);
            // 
            // tabPreset
            // 
            this.tabPreset.Location = new System.Drawing.Point(4, 22);
            this.tabPreset.Name = "tabPreset";
            this.tabPreset.Padding = new System.Windows.Forms.Padding(3);
            this.tabPreset.Size = new System.Drawing.Size(346, 607);
            this.tabPreset.TabIndex = 0;
            this.tabPreset.Text = "Preset";
            this.tabPreset.UseVisualStyleBackColor = true;
            // 
            // tabStandard
            // 
            this.tabStandard.Location = new System.Drawing.Point(4, 22);
            this.tabStandard.Name = "tabStandard";
            this.tabStandard.Padding = new System.Windows.Forms.Padding(3);
            this.tabStandard.Size = new System.Drawing.Size(346, 607);
            this.tabStandard.TabIndex = 1;
            this.tabStandard.Text = "Standard";
            this.tabStandard.UseVisualStyleBackColor = true;
            // 
            // tabAdvanced
            // 
            this.tabAdvanced.Location = new System.Drawing.Point(4, 22);
            this.tabAdvanced.Name = "tabAdvanced";
            this.tabAdvanced.Padding = new System.Windows.Forms.Padding(3);
            this.tabAdvanced.Size = new System.Drawing.Size(346, 607);
            this.tabAdvanced.TabIndex = 2;
            this.tabAdvanced.Text = "Advanced";
            this.tabAdvanced.UseVisualStyleBackColor = true;
            // 
            // tabOutput
            // 
            this.tabOutput.Location = new System.Drawing.Point(4, 22);
            this.tabOutput.Name = "tabOutput";
            this.tabOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tabOutput.Size = new System.Drawing.Size(346, 607);
            this.tabOutput.TabIndex = 3;
            this.tabOutput.Text = "Output";
            this.tabOutput.UseVisualStyleBackColor = true;
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cboSearchFor,
            this.btnSearch,
            this.btnReset,
            this.toolStripSeparator1,
            this.btnEnhance});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(354, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // cboSearchFor
            // 
            this.cboSearchFor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSearchFor.Name = "cboSearchFor";
            this.cboSearchFor.Size = new System.Drawing.Size(121, 25);
            // 
            // btnSearch
            // 
            this.btnSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(46, 22);
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnReset
            // 
            this.btnReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnReset.Image = ((System.Drawing.Image)(resources.GetObject("btnReset.Image")));
            this.btnReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(39, 22);
            this.btnReset.Text = "Reset";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnEnhance
            // 
            this.btnEnhance.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnEnhance.Enabled = false;
            this.btnEnhance.Image = ((System.Drawing.Image)(resources.GetObject("btnEnhance.Image")));
            this.btnEnhance.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEnhance.Name = "btnEnhance";
            this.btnEnhance.Size = new System.Drawing.Size(56, 22);
            this.btnEnhance.Text = "Enhance";
            // 
            // tabControlResults
            // 
            this.tabControlResults.Controls.Add(this.tabResults);
            this.tabControlResults.Controls.Add(this.tabResultsSQL);
            this.tabControlResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlResults.Location = new System.Drawing.Point(0, 0);
            this.tabControlResults.Name = "tabControlResults";
            this.tabControlResults.SelectedIndex = 0;
            this.tabControlResults.Size = new System.Drawing.Size(726, 658);
            this.tabControlResults.TabIndex = 0;
            // 
            // tabResults
            // 
            this.tabResults.Controls.Add(this.pnlLoading);
            this.tabResults.Controls.Add(this.dataGridView1);
            this.tabResults.Location = new System.Drawing.Point(4, 22);
            this.tabResults.Name = "tabResults";
            this.tabResults.Padding = new System.Windows.Forms.Padding(3);
            this.tabResults.Size = new System.Drawing.Size(718, 632);
            this.tabResults.TabIndex = 0;
            this.tabResults.Text = "Results";
            this.tabResults.UseVisualStyleBackColor = true;
            // 
            // pnlLoading
            // 
            this.pnlLoading.BackColor = System.Drawing.Color.White;
            this.pnlLoading.Controls.Add(this.btnCancel);
            this.pnlLoading.Controls.Add(this.picLoading);
            this.pnlLoading.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.bsAdapter, "IsSearching", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.pnlLoading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLoading.Location = new System.Drawing.Point(3, 3);
            this.pnlLoading.Name = "pnlLoading";
            this.pnlLoading.Size = new System.Drawing.Size(712, 626);
            this.pnlLoading.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.Location = new System.Drawing.Point(321, 350);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // picLoading
            // 
            this.picLoading.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picLoading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picLoading.Image = global::Standalone.Properties.Resources.stopwatch_gif_preloader;
            this.picLoading.Location = new System.Drawing.Point(309, 248);
            this.picLoading.Name = "picLoading";
            this.picLoading.Size = new System.Drawing.Size(96, 96);
            this.picLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picLoading.TabIndex = 1;
            this.picLoading.TabStop = false;
            // 
            // bsAdapter
            // 
            this.bsAdapter.DataSource = typeof(Standalone.Forms.MainAdapter);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(712, 626);
            this.dataGridView1.TabIndex = 0;
            // 
            // tabResultsSQL
            // 
            this.tabResultsSQL.Controls.Add(this.txtSql);
            this.tabResultsSQL.Location = new System.Drawing.Point(4, 22);
            this.tabResultsSQL.Name = "tabResultsSQL";
            this.tabResultsSQL.Padding = new System.Windows.Forms.Padding(3);
            this.tabResultsSQL.Size = new System.Drawing.Size(718, 632);
            this.tabResultsSQL.TabIndex = 1;
            this.tabResultsSQL.Text = "SQL";
            this.tabResultsSQL.UseVisualStyleBackColor = true;
            // 
            // txtSql
            // 
            this.txtSql.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.txtSql.AutoIndentCharsPatterns = "";
            this.txtSql.AutoScrollMinSize = new System.Drawing.Size(2, 14);
            this.txtSql.BackBrush = null;
            this.txtSql.CharHeight = 14;
            this.txtSql.CharWidth = 8;
            this.txtSql.CommentPrefix = "--";
            this.txtSql.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSql.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSql.IsReplaceMode = false;
            this.txtSql.Language = FastColoredTextBoxNS.Language.SQL;
            this.txtSql.LeftBracket = '(';
            this.txtSql.Location = new System.Drawing.Point(3, 3);
            this.txtSql.Name = "txtSql";
            this.txtSql.Paddings = new System.Windows.Forms.Padding(0);
            this.txtSql.RightBracket = ')';
            this.txtSql.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSql.Size = new System.Drawing.Size(712, 626);
            this.txtSql.TabIndex = 0;
            this.txtSql.Zoom = 100;
            // 
            // resultBindingSource
            // 
            this.resultBindingSource.DataMember = "Result";
            this.resultBindingSource.DataSource = this.bsAdapter;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 682);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsAdapter, "ApplicationTitle", true));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Database Query Framework";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControlParameters.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.tabControlResults.ResumeLayout(false);
            this.tabResults.ResumeLayout(false);
            this.pnlLoading.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLoading)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsAdapter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabResultsSQL.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtSql)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControlResults;
        private System.Windows.Forms.TabPage tabResults;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabPage tabResultsSQL;
        private System.Windows.Forms.TabControl tabControlParameters;
        private System.Windows.Forms.TabPage tabPreset;
        private System.Windows.Forms.TabPage tabStandard;
        private System.Windows.Forms.TabPage tabAdvanced;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnSearch;
        private System.Windows.Forms.ToolStripButton btnReset;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnEnhance;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox cboSearchFor;
        private System.Windows.Forms.BindingSource bsAdapter;
        private System.Windows.Forms.BindingSource resultBindingSource;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem connectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog sfdExport;
        private FastColoredTextBoxNS.FastColoredTextBox txtSql;
        private System.Windows.Forms.PictureBox picLoading;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Panel pnlLoading;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabPage tabOutput;
        private System.Windows.Forms.OpenFileDialog ofdLoad;
        private System.Windows.Forms.SaveFileDialog sfdSave;
    }
}