using Standalone.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using dbqf.Configuration;
using dbqf.Criterion;
using dbqf.WinForms;

namespace Standalone.Forms
{
    /// <summary>
    /// At the moment this class is acting as a sandbox.
    /// </summary>
    public partial class Main : Form
    {
        private class ConnectionMenuItem : ToolStripMenuItem
        {
            public Connection Connection { get; private set; }
            public ConnectionMenuItem(Connection c)
                : base(c.DisplayName)
            {
                Connection = c;
            }
        }

        private IGetParameter _currentAdapter;
        private MainAdapter _adapter;
        public Main(MainAdapter adapter)
        {
            InitializeComponent();
            _adapter = adapter;
            _currentAdapter = _adapter.Preset.Adapter;
            bsAdapter.DataSource = _adapter;
            _adapter.PropertyChanged += Adapter_PropertyChanged;

            // add connections to the menu
            foreach (var c in _adapter.Project.Connections)
            {
                var m = new ConnectionMenuItem(c);
                m.Checked = c == _adapter.Project.CurrentConnection;
                m.Click += (s, e) => _adapter.Project.CurrentConnection = c;
                connectionToolStripMenuItem.DropDownItems.Add(m);
            }
            _adapter.Project.CurrentConnectionChanged += (s, e) =>
            {
                foreach (ConnectionMenuItem c in connectionToolStripMenuItem.DropDownItems)
                    c.Checked = (c.Connection == ((Project)s).CurrentConnection);
            };

            // toolstrip combo has poor binding support so we need to wire it all up manually
            cboSearchFor.ComboBox.BindingContext = this.BindingContext;
            cboSearchFor.ComboBox.DataSource = _adapter.SubjectSource;
            cboSearchFor.SelectedIndexChanged += (s, e) => {
                _adapter.SelectedSubject = (ISubject)cboSearchFor.SelectedItem;
            };
            _adapter.SelectedSubjectChanged += (s, e) => {
                cboSearchFor.SelectedItem = _adapter.SelectedSubject;
            };

            // update datagridview when new data arrives
            _adapter.Result.DataSourceChanged += (s, e) => {
                var data = ((DataTable)_adapter.Result.DataSource);
                if (data != null)
                {
                    foreach (DataGridViewColumn c in dataGridView1.Columns)
                    {
                        var path = (FieldPath)data.Columns[c.DataPropertyName].ExtendedProperties["FieldPath"];
                        c.DefaultCellStyle.Format = path.Last.DisplayFormat;
                    }

                    dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                    tabResults.Text = String.Concat("Results (", data.Rows.Count, ")");
                }
                else
                {
                    tabResults.Text = "Results";
                }

                // the hack to fix redraw on parameter views affects TabControl text update, so force refresh on change
                this.Refresh();
            };
        }

        void Adapter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("ResultSQL".Equals(e.PropertyName))
                txtSql.Text = _adapter.ResultSQL;
        }

        /// <summary>
        /// This is the only way the WinForms.PresetView is able to rebuild it's UI without the application redrawing like a motherf***.
        /// SuspendLayout on any of the hierarchy, clearing controls before remove/dispose, calling WM_SETREDRAW with PInvoke, 
        /// making control visible=false, double buffering and/or optimized double buffer or all in wmpaint - none of these work.
        /// The following override at the form is the only way for it to not redraw (other than not using the TableLayoutPanel).
        /// http://stackoverflow.com/questions/3718380/winforms-double-buffering/3718648#3718648
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        private void tabControlParameters_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tab = tabControlParameters.SelectedTab.Tag;
            if (tab is PresetView)
                _currentAdapter = ((PresetView)tab).Adapter;
            else if (tab is StandardView)
                _currentAdapter = ((StandardView)tab).Adapter;
            else if (tab is AdvancedView)
                _currentAdapter = ((AdvancedView)tab).Adapter;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            _adapter.Search(_currentAdapter.GetParameter());
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _adapter.Reset();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            SetupView(_adapter.Preset, tabPreset);
            SetupView(_adapter.Standard, tabStandard);
            SetupView(_adapter.Advanced, tabAdvanced);
            SetupView(_adapter.RetrieveFields, tabOutput);

            // datagridview also has bad control designer, this has to be in the load event
            dataGridView1.DataSource = _adapter.Result;

            try { splitContainer1.SplitterDistance = Properties.Settings.Default.SplitterLocation; }
            catch { }
        }
        
        private void SetupView(Control control, TabPage parent)
        {
            control.Dock = DockStyle.Fill;
            parent.Controls.Add(control);
            parent.Tag = control;
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            Properties.Settings.Default.SplitterLocation = splitContainer1.SplitterDistance;
            Properties.Settings.Default.Save();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_adapter.Result.DataSource == null)
            {
                MessageBox.Show("There are no results to export.  Please perform a search first, then export.", "Export");
                return;
            }

            if (sfdExport.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                _adapter.Export(sfdExport.FileName);
                var result = MessageBox.Show(this, String.Concat("Do you want to open ", System.IO.Path.GetFileName(sfdExport.FileName), "?"), "Export", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    try { System.Diagnostics.Process.Start(sfdExport.FileName); }
                    catch (Exception ex) { MessageBox.Show(this, String.Concat("Unable to open file.", Environment.NewLine, ex.Message), "Export", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new AboutDialog();
            about.Show(this);
            about.FormClosed += (s2, e2) => about.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _adapter.CancelSearch();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _adapter.Open("");
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _adapter.Save("");
        }
    }
}
