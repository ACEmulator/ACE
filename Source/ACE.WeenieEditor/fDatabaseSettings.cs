using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ACE.WeenieEditor
{
    public partial class fDatabaseSettings : Form
    {
        private WeenieEditorConfig config = null;

        public fDatabaseSettings()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // save the settings
            config.RootUsername = tbUsername.Text;
            config.RootPassword = tbPassword.Text;
            config.ServerIp = tbServerIp.Text;
            config.ServerPort = tbServerPort.Text;
            config.WorldDatabaseName = tbWorldDatabaseName.Text;
            config.Save();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void fDatabaseSettings_Load(object sender, EventArgs e)
        {
            // load the settings
            config = WeenieEditorConfig.Load();

            tbUsername.Text = config.RootUsername;
            tbPassword.Text = config.RootPassword;
            tbServerIp.Text = config.ServerIp;
            tbServerPort.Text = config.ServerPort;
            tbWorldDatabaseName.Text = config.WorldDatabaseName;
        }
    }
}
