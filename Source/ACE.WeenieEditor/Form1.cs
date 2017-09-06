using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACE.Database;

namespace ACE.WeenieEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void databaseConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new fDatabaseSettings().ShowDialog(this);
        }

        private void testDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var db = new WorldDatabase();
            try
            {
                var config = WeenieEditorConfig.Load();
                db.Initialize(config.ServerIp, uint.Parse(config.ServerPort), config.RootUsername, config.RootPassword, config.WorldDatabaseName, false);
                MessageBox.Show(this, "Connection to database successful!", "ACE", MessageBoxButtons.OK);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error connecting to database." + Environment.NewLine + ex, "ACE", MessageBoxButtons.OK);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void byIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var db = new WorldDatabase();
            var config = WeenieEditorConfig.Load();
            db.Initialize(config.ServerIp, uint.Parse(config.ServerPort), config.RootUsername, config.RootPassword, config.WorldDatabaseName);
        }

        private void byNameToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
