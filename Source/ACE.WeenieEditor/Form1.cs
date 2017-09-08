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
using ACE.Entity;
using ACE.Entity.Enum.Properties;
using Microsoft.VisualBasic;

namespace ACE.WeenieEditor
{
    public partial class Form1 : Form
    {
        private AceObject _weenie = null;
        private EditMode _stringEditMode = EditMode.None;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tvStrings.Height += pEditString.Height;
            pEditString.Visible = false;

            cbStringProperties.SetEnumDataSource<PropertyString>(true);
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
            string weenieId = Interaction.InputBox("Enter the Weenie ID", "ACE");
            uint id;
            if (uint.TryParse(weenieId, out id))
            {
                var db = new WorldDatabase();
                var config = WeenieEditorConfig.Load();
                db.Initialize(config.ServerIp, uint.Parse(config.ServerPort), config.RootUsername, config.RootPassword, config.WorldDatabaseName);

                _weenie = db.GetWeenie(id);
                LoadWeenie();
            }
        }

        private void byNameToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void LoadWeenie()
        {
            if (_weenie == null)
                return;
            
            _weenie.StringProperties.ForEach(sProp =>
            {
                tvStrings.Nodes.Add(CreateStringNode(sProp));
            });
        }

        private TreeNode CreateStringNode(AceObjectPropertiesString prop)
        {
            TreeNode node = new TreeNode();
            node.Text = ((PropertyString)prop.PropertyId).ToString() + " - " + prop.PropertyValue;
            node.Tag = prop;
            node.ContextMenuStrip = cmsStrings;
            return node;
        }

        private void editStringMenuItem_Click(object sender, EventArgs e)
        {
            if (_stringEditMode != EditMode.None)
            {
                MessageBox.Show("Cancel or Save current changes first.");
                return;
            }

            var menuItem = sender as ToolStripMenuItem;
            var source = (menuItem.Owner as ContextMenuStrip).SourceControl as TreeView;
            EditString(source?.SelectedNode);
        }

        private void deleteStringMenuItem_Click(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            var source = (menuItem.Owner as ContextMenuStrip).SourceControl as TreeView;

            _weenie.StringProperties.Remove(source?.SelectedNode.Tag as AceObjectPropertiesString);
            // TODO: save change to change-list

            source.Nodes.Remove(source?.SelectedNode);
        }

        private void addStringMenuItem_Click(object sender, EventArgs e)
        {
            if (_stringEditMode != EditMode.None)
            {
                MessageBox.Show("Cancel or Save current changes first.");
                return;
            }

            AddString();
        }
        
        private void tvStrings_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ((TreeView)sender).SelectedNode = e.Node;
        }

        private void btnCancelString_Click(object sender, EventArgs e)
        {
            tvStrings.Height += pEditString.Height;
            pEditString.Visible = false;
        }

        private void btnSaveString_Click(object sender, EventArgs e)
        {
            TreeNode node = null;
            AceObjectPropertiesString prop = null;

            if (_stringEditMode == EditMode.Edit)
            {
                node = (TreeNode)tbStringValue.Tag;
                prop = node.Tag as AceObjectPropertiesString;
                prop.PropertyValue = tbStringValue.Text;
                node.Text = ((PropertyString)prop.PropertyId) + " - " + prop.PropertyValue;
            }
            else
            {
                prop = new AceObjectPropertiesString();
                prop.AceObjectId = _weenie.WeenieClassId;
                prop.PropertyId = Convert.ToUInt16((int)cbStringProperties.SelectedValue);
                prop.PropertyValue = tbStringValue.Text;
                _weenie.StringProperties.Add(prop);
                node = CreateStringNode(prop);
                tvStrings.Nodes.Add(node);
            }

            // TODO: save change to change-list.  object structure will be dirty

            tvStrings.Height += pEditString.Height;
            pEditString.Visible = false;
            _stringEditMode = EditMode.None;
        }

        private void tvStrings_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            EditString((sender as TreeView)?.SelectedNode);
        }

        private void EditString(TreeNode sourceNode)
        {
            AceObjectPropertiesString prop = sourceNode.Tag as AceObjectPropertiesString;
            _stringEditMode = EditMode.Edit;
            tvStrings.Height -= pEditString.Height;
            pEditString.Visible = true;
            cbStringProperties.SelectedValue = (int?)prop.PropertyId;
            tbStringValue.Text = prop.PropertyValue;
            tbStringValue.Tag = sourceNode;
            cbStringProperties.Enabled = false;
        }

        private void AddString()
        {
            _stringEditMode = EditMode.Add;
            tvStrings.Height -= pEditString.Height;
            pEditString.Visible = true;

            cbStringProperties.SelectedValue = -1;
            tbStringValue.Text = "";
            cbStringProperties.Enabled = true;
        }
    }
}
