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
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using Microsoft.VisualBasic;

namespace ACE.WeenieEditor
{
    public partial class Form1 : Form
    {
        private AceObject _weenie = null;
        private EditMode _stringEditMode = EditMode.None;
        private EditMode _intEditMode = EditMode.None;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbWeenieType.SetEnumDataSource<WeenieType>(true);

            tvStrings.Height += pEditString.Height;
            pEditString.Visible = false;
            cbStringProperties.SetEnumDataSource<PropertyString>(true);

            tvInts.Height += pEditInt.Height;
            pEditInt.Visible = false;
            cbIntProperties.SetEnumDataSource<PropertyInt>(true);
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Empty function
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
            catch (Exception ex)
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
            // Empty function
        }

        private void LoadWeenie()
        {
            if (_weenie == null)
                return;
            
            _weenie.StringProperties.ForEach(sProp =>
            {
                tvStrings.Nodes.Add(CreateStringNode(sProp));
            });

            _weenie.IntProperties.ForEach(sProp =>
            {
                tvInts.Nodes.Add(CreateIntNode(sProp));
            });

            lblWeenidId.Text = _weenie.WeenieClassId.ToString();
            cbWeenieType.SelectedValue = _weenie.WeenieType;
        }

        private TreeNode CreateStringNode(AceObjectPropertiesString prop)
        {
            TreeNode node = new TreeNode();
            node.Text = ((PropertyString)prop.PropertyId).ToString() + " - " + prop.PropertyValue;
            node.Tag = prop;
            node.ContextMenuStrip = cmsString;
            return node;
        }

        private TreeNode CreateIntNode(AceObjectPropertiesInt prop)
        {
            TreeNode node = new TreeNode();
            node.Text = ((PropertyInt)prop.PropertyId).ToString() + " - " + prop.PropertyValue;
            node.Tag = prop;
            node.ContextMenuStrip = cmsInt;
            return node;
        }

        private void editStringMenuItem_Click(object sender, EventArgs e)
        {
            if (_stringEditMode != EditMode.None)
            {
                MessageBox.Show("Cancel or Save current String changes first.");
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
                MessageBox.Show("Cancel or Save current String changes first.");
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
            if (_stringEditMode != EditMode.None)
            {
                if (tbStringValue.Text != (((tbStringValue.Tag as TreeNode).Tag as AceObjectPropertiesString).PropertyValue))
                {
                    // unsaved changes
                    MessageBox.Show("Unsaved String changes detected.  Save or Cancel first.");
                    return;
                }
            }
            else
            {
                // only adjust if we're opening it from a closed state
                tvStrings.Height -= pEditString.Height;
            }

            AceObjectPropertiesString prop = sourceNode.Tag as AceObjectPropertiesString;
            _stringEditMode = EditMode.Edit;
            pEditString.Visible = true;
            cbStringProperties.SelectedValue = (int?)prop.PropertyId;
            tbStringValue.Text = prop.PropertyValue;
            tbStringValue.Tag = sourceNode;
            cbStringProperties.Enabled = false;
        }

        private void EditInt(TreeNode sourceNode)
        {
            if (_intEditMode != EditMode.None)
            {
                if ((uint)tbIntValue.Value != (((tbIntValue.Tag as TreeNode).Tag as AceObjectPropertiesInt).PropertyValue))
                {
                    // unsaved changes
                    MessageBox.Show("Unsaved Int changes detected.  Save or Cancel first.");
                    return;
                }
            }
            else
            {
                // only adjust if we're opening it from a closed state
                tvInts.Height -= pEditInt.Height;
            }

            AceObjectPropertiesInt prop = sourceNode.Tag as AceObjectPropertiesInt;
            _intEditMode = EditMode.Edit;
            pEditInt.Visible = true;
            cbIntProperties.SelectedValue = (int?)prop.PropertyId;
            tbIntValue.Value = prop.PropertyValue ?? 0;
            tbIntValue.Tag = sourceNode;
            cbIntProperties.Enabled = false;
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

        private void AddInt()
        {
            _intEditMode = EditMode.Add;
            tvInts.Height -= pEditInt.Height;
            pEditInt.Visible = true;

            cbIntProperties.SelectedValue = -1;
            tbIntValue.Text = "";
            cbIntProperties.Enabled = true;
        }

        private void tvInts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            EditInt((sender as TreeView)?.SelectedNode);
        }

        private void tvInts_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ((TreeView)sender).SelectedNode = e.Node;
        }

        private void btnCancelInt_Click(object sender, EventArgs e)
        {
            tvInts.Height += pEditInt.Height;
            pEditInt.Visible = false;
        }

        private void btnSaveInt_Click(object sender, EventArgs e)
        {
            TreeNode node = null;
            AceObjectPropertiesInt prop = null;

            if (_intEditMode == EditMode.Edit)
            {
                node = (TreeNode)tbIntValue.Tag;
                prop = node.Tag as AceObjectPropertiesInt;
                prop.PropertyValue = (uint)tbIntValue.Value;
                node.Text = ((PropertyString)prop.PropertyId) + " - " + prop.PropertyValue;
            }
            else
            {
                prop = new AceObjectPropertiesInt();
                prop.AceObjectId = _weenie.WeenieClassId;
                prop.PropertyId = Convert.ToUInt16((int)cbStringProperties.SelectedValue);
                prop.PropertyValue = (uint)tbIntValue.Value;
                _weenie.IntProperties.Add(prop);
                node = CreateIntNode(prop);
                tvStrings.Nodes.Add(node);
            }

            // TODO: save change to change-list.  object structure will be dirty

            tvInts.Height += pEditInt.Height;
            pEditInt.Visible = false;
            _intEditMode = EditMode.None;
        }

        private void addIntMenuItem_Click(object sender, EventArgs e)
        {
            if (_intEditMode != EditMode.None)
            {
                MessageBox.Show("Cancel or Save current Int changes first.");
                return;
            }

            AddInt();
        }

        private void editIntMenuItem_Click(object sender, EventArgs e)
        {
            if (_intEditMode != EditMode.None)
            {
                MessageBox.Show("Cancel or Save current Int changes first.");
                return;
            }

            var menuItem = sender as ToolStripMenuItem;
            var source = (menuItem.Owner as ContextMenuStrip).SourceControl as TreeView;
            EditInt(source?.SelectedNode);
        }

        private void deleteIntMenuItem_Click(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            var source = (menuItem.Owner as ContextMenuStrip).SourceControl as TreeView;

            _weenie.IntProperties.Remove(source?.SelectedNode.Tag as AceObjectPropertiesInt);
            // TODO: save change to change-list

            source.Nodes.Remove(source?.SelectedNode);
        }

        private void exportWeeniesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var config = WeenieEditorConfig.Load();

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select a folder to export to.";
            fbd.SelectedPath = config.ExportFolder;

            var dr = fbd.ShowDialog(this);

            if (dr == DialogResult.OK)
            {
                config.ExportFolder = fbd.SelectedPath;
                config.Save();


            }
        }
    }
}
