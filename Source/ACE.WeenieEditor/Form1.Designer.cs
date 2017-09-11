﻿namespace ACE.WeenieEditor
{
    partial class Form1
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
            this.mnuMainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gitHubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prepareUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tcProperties = new System.Windows.Forms.TabControl();
            this.tpSummary = new System.Windows.Forms.TabPage();
            this.tpStrings = new System.Windows.Forms.TabPage();
            this.pEditString = new System.Windows.Forms.Panel();
            this.btnCancelString = new System.Windows.Forms.Button();
            this.btnSaveString = new System.Windows.Forms.Button();
            this.tbStringValue = new System.Windows.Forms.TextBox();
            this.cbStringProperties = new System.Windows.Forms.ComboBox();
            this.tvStrings = new System.Windows.Forms.TreeView();
            this.tpInts = new System.Windows.Forms.TabPage();
            this.tpInt64s = new System.Windows.Forms.TabPage();
            this.tpDoubles = new System.Windows.Forms.TabPage();
            this.tpBools = new System.Windows.Forms.TabPage();
            this.tpDIDs = new System.Windows.Forms.TabPage();
            this.tpIIDs = new System.Windows.Forms.TabPage();
            this.tpLocations = new System.Windows.Forms.TabPage();
            this.tpAttributes = new System.Windows.Forms.TabPage();
            this.tpSkills = new System.Windows.Forms.TabPage();
            this.tpBook = new System.Windows.Forms.TabPage();
            this.cmsString = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addStringMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editStringMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteStringMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblWeenidId = new System.Windows.Forms.Label();
            this.cbWeenieType = new System.Windows.Forms.ComboBox();
            this.pEditInt = new System.Windows.Forms.Panel();
            this.btnCancelInt = new System.Windows.Forms.Button();
            this.btnSaveInt = new System.Windows.Forms.Button();
            this.cbIntProperties = new System.Windows.Forms.ComboBox();
            this.tvInts = new System.Windows.Forms.TreeView();
            this.tbIntValue = new System.Windows.Forms.NumericUpDown();
            this.cmsInt = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addIntMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editIntMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteIntMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMainMenu.SuspendLayout();
            this.tcProperties.SuspendLayout();
            this.tpSummary.SuspendLayout();
            this.tpStrings.SuspendLayout();
            this.pEditString.SuspendLayout();
            this.tpInts.SuspendLayout();
            this.cmsString.SuspendLayout();
            this.pEditInt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbIntValue)).BeginInit();
            this.cmsInt.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuMainMenu
            // 
            this.mnuMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.gitHubToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mnuMainMenu.Location = new System.Drawing.Point(0, 0);
            this.mnuMainMenu.Name = "mnuMainMenu";
            this.mnuMainMenu.Size = new System.Drawing.Size(713, 24);
            this.mnuMainMenu.TabIndex = 0;
            this.mnuMainMenu.Text = "File";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.databaseConnectionToolStripMenuItem,
            this.testDatabaseToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // databaseConnectionToolStripMenuItem
            // 
            this.databaseConnectionToolStripMenuItem.Name = "databaseConnectionToolStripMenuItem";
            this.databaseConnectionToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.databaseConnectionToolStripMenuItem.Text = "&Database Connection";
            this.databaseConnectionToolStripMenuItem.Click += new System.EventHandler(this.databaseConnectionToolStripMenuItem_Click);
            // 
            // testDatabaseToolStripMenuItem
            // 
            this.testDatabaseToolStripMenuItem.Name = "testDatabaseToolStripMenuItem";
            this.testDatabaseToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.testDatabaseToolStripMenuItem.Text = "&Test Database";
            this.testDatabaseToolStripMenuItem.Click += new System.EventHandler(this.testDatabaseToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(184, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.byIDToolStripMenuItem,
            this.byNameToolStripMenuItem});
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.searchToolStripMenuItem.Text = "&Search";
            this.searchToolStripMenuItem.Click += new System.EventHandler(this.searchToolStripMenuItem_Click);
            // 
            // byIDToolStripMenuItem
            // 
            this.byIDToolStripMenuItem.Name = "byIDToolStripMenuItem";
            this.byIDToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.byIDToolStripMenuItem.Text = "By &ID";
            this.byIDToolStripMenuItem.Click += new System.EventHandler(this.byIDToolStripMenuItem_Click);
            // 
            // byNameToolStripMenuItem
            // 
            this.byNameToolStripMenuItem.Name = "byNameToolStripMenuItem";
            this.byNameToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.byNameToolStripMenuItem.Text = "By &Name";
            this.byNameToolStripMenuItem.Click += new System.EventHandler(this.byNameToolStripMenuItem_Click);
            // 
            // gitHubToolStripMenuItem
            // 
            this.gitHubToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getUpdatesToolStripMenuItem,
            this.prepareUpdateToolStripMenuItem});
            this.gitHubToolStripMenuItem.Name = "gitHubToolStripMenuItem";
            this.gitHubToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.gitHubToolStripMenuItem.Text = "&GitHub";
            // 
            // getUpdatesToolStripMenuItem
            // 
            this.getUpdatesToolStripMenuItem.Name = "getUpdatesToolStripMenuItem";
            this.getUpdatesToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.getUpdatesToolStripMenuItem.Text = "&Update my Data";
            // 
            // prepareUpdateToolStripMenuItem
            // 
            this.prepareUpdateToolStripMenuItem.Name = "prepareUpdateToolStripMenuItem";
            this.prepareUpdateToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.prepareUpdateToolStripMenuItem.Text = "&Send Update";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // tcProperties
            // 
            this.tcProperties.Controls.Add(this.tpSummary);
            this.tcProperties.Controls.Add(this.tpStrings);
            this.tcProperties.Controls.Add(this.tpInts);
            this.tcProperties.Controls.Add(this.tpInt64s);
            this.tcProperties.Controls.Add(this.tpDoubles);
            this.tcProperties.Controls.Add(this.tpBools);
            this.tcProperties.Controls.Add(this.tpDIDs);
            this.tcProperties.Controls.Add(this.tpIIDs);
            this.tcProperties.Controls.Add(this.tpLocations);
            this.tcProperties.Controls.Add(this.tpAttributes);
            this.tcProperties.Controls.Add(this.tpSkills);
            this.tcProperties.Controls.Add(this.tpBook);
            this.tcProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcProperties.Location = new System.Drawing.Point(0, 24);
            this.tcProperties.Name = "tcProperties";
            this.tcProperties.SelectedIndex = 0;
            this.tcProperties.Size = new System.Drawing.Size(713, 489);
            this.tcProperties.TabIndex = 1;
            // 
            // tpSummary
            // 
            this.tpSummary.Controls.Add(this.cbWeenieType);
            this.tpSummary.Controls.Add(this.lblWeenidId);
            this.tpSummary.Controls.Add(this.label2);
            this.tpSummary.Controls.Add(this.label1);
            this.tpSummary.Location = new System.Drawing.Point(4, 22);
            this.tpSummary.Name = "tpSummary";
            this.tpSummary.Padding = new System.Windows.Forms.Padding(3);
            this.tpSummary.Size = new System.Drawing.Size(705, 463);
            this.tpSummary.TabIndex = 0;
            this.tpSummary.Text = "Summary";
            this.tpSummary.UseVisualStyleBackColor = true;
            // 
            // tpStrings
            // 
            this.tpStrings.Controls.Add(this.pEditString);
            this.tpStrings.Controls.Add(this.tvStrings);
            this.tpStrings.Location = new System.Drawing.Point(4, 22);
            this.tpStrings.Name = "tpStrings";
            this.tpStrings.Padding = new System.Windows.Forms.Padding(3);
            this.tpStrings.Size = new System.Drawing.Size(705, 463);
            this.tpStrings.TabIndex = 1;
            this.tpStrings.Text = "Strings";
            this.tpStrings.UseVisualStyleBackColor = true;
            // 
            // pEditString
            // 
            this.pEditString.Controls.Add(this.btnCancelString);
            this.pEditString.Controls.Add(this.btnSaveString);
            this.pEditString.Controls.Add(this.tbStringValue);
            this.pEditString.Controls.Add(this.cbStringProperties);
            this.pEditString.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pEditString.Location = new System.Drawing.Point(3, 326);
            this.pEditString.Name = "pEditString";
            this.pEditString.Size = new System.Drawing.Size(699, 134);
            this.pEditString.TabIndex = 6;
            this.pEditString.Visible = false;
            // 
            // btnCancelString
            // 
            this.btnCancelString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelString.Location = new System.Drawing.Point(538, 100);
            this.btnCancelString.Name = "btnCancelString";
            this.btnCancelString.Size = new System.Drawing.Size(76, 28);
            this.btnCancelString.TabIndex = 6;
            this.btnCancelString.Text = "&Cancel";
            this.btnCancelString.UseVisualStyleBackColor = true;
            this.btnCancelString.Click += new System.EventHandler(this.btnCancelString_Click);
            // 
            // btnSaveString
            // 
            this.btnSaveString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveString.Location = new System.Drawing.Point(620, 100);
            this.btnSaveString.Name = "btnSaveString";
            this.btnSaveString.Size = new System.Drawing.Size(76, 28);
            this.btnSaveString.TabIndex = 5;
            this.btnSaveString.Text = "&Save";
            this.btnSaveString.UseVisualStyleBackColor = true;
            this.btnSaveString.Click += new System.EventHandler(this.btnSaveString_Click);
            // 
            // tbStringValue
            // 
            this.tbStringValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbStringValue.Location = new System.Drawing.Point(3, 31);
            this.tbStringValue.Multiline = true;
            this.tbStringValue.Name = "tbStringValue";
            this.tbStringValue.Size = new System.Drawing.Size(693, 63);
            this.tbStringValue.TabIndex = 4;
            // 
            // cbStringProperties
            // 
            this.cbStringProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbStringProperties.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStringProperties.FormattingEnabled = true;
            this.cbStringProperties.Location = new System.Drawing.Point(3, 4);
            this.cbStringProperties.Name = "cbStringProperties";
            this.cbStringProperties.Size = new System.Drawing.Size(693, 21);
            this.cbStringProperties.TabIndex = 3;
            // 
            // tvStrings
            // 
            this.tvStrings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvStrings.Location = new System.Drawing.Point(0, 0);
            this.tvStrings.Name = "tvStrings";
            this.tvStrings.Size = new System.Drawing.Size(705, 324);
            this.tvStrings.TabIndex = 0;
            this.tvStrings.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvStrings_NodeMouseClick);
            this.tvStrings.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tvStrings_MouseDoubleClick);
            // 
            // tpInts
            // 
            this.tpInts.Controls.Add(this.pEditInt);
            this.tpInts.Controls.Add(this.tvInts);
            this.tpInts.Location = new System.Drawing.Point(4, 22);
            this.tpInts.Name = "tpInts";
            this.tpInts.Size = new System.Drawing.Size(705, 463);
            this.tpInts.TabIndex = 2;
            this.tpInts.Text = "Int32s";
            this.tpInts.UseVisualStyleBackColor = true;
            // 
            // tpInt64s
            // 
            this.tpInt64s.Location = new System.Drawing.Point(4, 22);
            this.tpInt64s.Name = "tpInt64s";
            this.tpInt64s.Size = new System.Drawing.Size(705, 463);
            this.tpInt64s.TabIndex = 3;
            this.tpInt64s.Text = "Int64s";
            this.tpInt64s.UseVisualStyleBackColor = true;
            // 
            // tpDoubles
            // 
            this.tpDoubles.Location = new System.Drawing.Point(4, 22);
            this.tpDoubles.Name = "tpDoubles";
            this.tpDoubles.Size = new System.Drawing.Size(705, 463);
            this.tpDoubles.TabIndex = 4;
            this.tpDoubles.Text = "Doubles";
            this.tpDoubles.UseVisualStyleBackColor = true;
            // 
            // tpBools
            // 
            this.tpBools.Location = new System.Drawing.Point(4, 22);
            this.tpBools.Name = "tpBools";
            this.tpBools.Size = new System.Drawing.Size(705, 463);
            this.tpBools.TabIndex = 5;
            this.tpBools.Text = "Bools";
            this.tpBools.UseVisualStyleBackColor = true;
            // 
            // tpDIDs
            // 
            this.tpDIDs.Location = new System.Drawing.Point(4, 22);
            this.tpDIDs.Name = "tpDIDs";
            this.tpDIDs.Size = new System.Drawing.Size(705, 463);
            this.tpDIDs.TabIndex = 7;
            this.tpDIDs.Text = "DIDs";
            this.tpDIDs.UseVisualStyleBackColor = true;
            // 
            // tpIIDs
            // 
            this.tpIIDs.Location = new System.Drawing.Point(4, 22);
            this.tpIIDs.Name = "tpIIDs";
            this.tpIIDs.Size = new System.Drawing.Size(705, 463);
            this.tpIIDs.TabIndex = 8;
            this.tpIIDs.Text = "IIDs";
            this.tpIIDs.UseVisualStyleBackColor = true;
            // 
            // tpLocations
            // 
            this.tpLocations.Location = new System.Drawing.Point(4, 22);
            this.tpLocations.Name = "tpLocations";
            this.tpLocations.Size = new System.Drawing.Size(705, 463);
            this.tpLocations.TabIndex = 6;
            this.tpLocations.Text = "Locations";
            this.tpLocations.UseVisualStyleBackColor = true;
            // 
            // tpAttributes
            // 
            this.tpAttributes.Location = new System.Drawing.Point(4, 22);
            this.tpAttributes.Name = "tpAttributes";
            this.tpAttributes.Size = new System.Drawing.Size(705, 463);
            this.tpAttributes.TabIndex = 9;
            this.tpAttributes.Text = "Attributes";
            this.tpAttributes.UseVisualStyleBackColor = true;
            // 
            // tpSkills
            // 
            this.tpSkills.Location = new System.Drawing.Point(4, 22);
            this.tpSkills.Name = "tpSkills";
            this.tpSkills.Size = new System.Drawing.Size(705, 463);
            this.tpSkills.TabIndex = 10;
            this.tpSkills.Text = "Skills";
            this.tpSkills.UseVisualStyleBackColor = true;
            // 
            // tpBook
            // 
            this.tpBook.Location = new System.Drawing.Point(4, 22);
            this.tpBook.Name = "tpBook";
            this.tpBook.Size = new System.Drawing.Size(705, 463);
            this.tpBook.TabIndex = 12;
            this.tpBook.Text = "Book";
            this.tpBook.UseVisualStyleBackColor = true;
            // 
            // cmsString
            // 
            this.cmsString.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addStringMenuItem,
            this.editStringMenuItem,
            this.deleteStringMenuItem});
            this.cmsString.Name = "cmsStrings";
            this.cmsString.Size = new System.Drawing.Size(108, 70);
            // 
            // addStringMenuItem
            // 
            this.addStringMenuItem.Name = "addStringMenuItem";
            this.addStringMenuItem.Size = new System.Drawing.Size(107, 22);
            this.addStringMenuItem.Text = "Add";
            this.addStringMenuItem.Click += new System.EventHandler(this.addStringMenuItem_Click);
            // 
            // editStringMenuItem
            // 
            this.editStringMenuItem.Name = "editStringMenuItem";
            this.editStringMenuItem.Size = new System.Drawing.Size(107, 22);
            this.editStringMenuItem.Text = "Edit";
            this.editStringMenuItem.Click += new System.EventHandler(this.editStringMenuItem_Click);
            // 
            // deleteStringMenuItem
            // 
            this.deleteStringMenuItem.Name = "deleteStringMenuItem";
            this.deleteStringMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteStringMenuItem.Text = "Delete";
            this.deleteStringMenuItem.Click += new System.EventHandler(this.deleteStringMenuItem_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Weenie Id:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Weenie Type:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblWeenidId
            // 
            this.lblWeenidId.Location = new System.Drawing.Point(134, 22);
            this.lblWeenidId.Name = "lblWeenidId";
            this.lblWeenidId.Size = new System.Drawing.Size(120, 20);
            this.lblWeenidId.TabIndex = 2;
            this.lblWeenidId.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cbWeenieType
            // 
            this.cbWeenieType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWeenieType.FormattingEnabled = true;
            this.cbWeenieType.Location = new System.Drawing.Point(137, 39);
            this.cbWeenieType.Name = "cbWeenieType";
            this.cbWeenieType.Size = new System.Drawing.Size(121, 21);
            this.cbWeenieType.TabIndex = 3;
            // 
            // pEditInt
            // 
            this.pEditInt.Controls.Add(this.tbIntValue);
            this.pEditInt.Controls.Add(this.btnCancelInt);
            this.pEditInt.Controls.Add(this.btnSaveInt);
            this.pEditInt.Controls.Add(this.cbIntProperties);
            this.pEditInt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pEditInt.Location = new System.Drawing.Point(0, 422);
            this.pEditInt.Name = "pEditInt";
            this.pEditInt.Size = new System.Drawing.Size(705, 41);
            this.pEditInt.TabIndex = 8;
            this.pEditInt.Visible = false;
            // 
            // btnCancelInt
            // 
            this.btnCancelInt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelInt.Location = new System.Drawing.Point(544, 7);
            this.btnCancelInt.Name = "btnCancelInt";
            this.btnCancelInt.Size = new System.Drawing.Size(76, 28);
            this.btnCancelInt.TabIndex = 6;
            this.btnCancelInt.Text = "&Cancel";
            this.btnCancelInt.UseVisualStyleBackColor = true;
            this.btnCancelInt.Click += new System.EventHandler(this.btnCancelInt_Click);
            // 
            // btnSaveInt
            // 
            this.btnSaveInt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveInt.Location = new System.Drawing.Point(626, 7);
            this.btnSaveInt.Name = "btnSaveInt";
            this.btnSaveInt.Size = new System.Drawing.Size(76, 28);
            this.btnSaveInt.TabIndex = 5;
            this.btnSaveInt.Text = "&Save";
            this.btnSaveInt.UseVisualStyleBackColor = true;
            this.btnSaveInt.Click += new System.EventHandler(this.btnSaveInt_Click);
            // 
            // cbIntProperties
            // 
            this.cbIntProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbIntProperties.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIntProperties.FormattingEnabled = true;
            this.cbIntProperties.Location = new System.Drawing.Point(8, 11);
            this.cbIntProperties.Name = "cbIntProperties";
            this.cbIntProperties.Size = new System.Drawing.Size(319, 21);
            this.cbIntProperties.TabIndex = 3;
            // 
            // tvInts
            // 
            this.tvInts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvInts.Location = new System.Drawing.Point(0, 0);
            this.tvInts.Name = "tvInts";
            this.tvInts.Size = new System.Drawing.Size(705, 423);
            this.tvInts.TabIndex = 7;
            this.tvInts.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvInts_NodeMouseClick);
            this.tvInts.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tvInts_MouseDoubleClick);
            // 
            // tbIntValue
            // 
            this.tbIntValue.Location = new System.Drawing.Point(333, 13);
            this.tbIntValue.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.tbIntValue.Name = "tbIntValue";
            this.tbIntValue.Size = new System.Drawing.Size(140, 20);
            this.tbIntValue.TabIndex = 9;
            // 
            // cmsInt
            // 
            this.cmsInt.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addIntMenuItem,
            this.editIntMenuItem,
            this.deleteIntMenuItem});
            this.cmsInt.Name = "cmsStrings";
            this.cmsInt.Size = new System.Drawing.Size(108, 70);
            // 
            // addIntMenuItem
            // 
            this.addIntMenuItem.Name = "addIntMenuItem";
            this.addIntMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addIntMenuItem.Text = "Add";
            this.addIntMenuItem.Click += new System.EventHandler(this.addIntMenuItem_Click);
            // 
            // editIntMenuItem
            // 
            this.editIntMenuItem.Name = "editIntMenuItem";
            this.editIntMenuItem.Size = new System.Drawing.Size(152, 22);
            this.editIntMenuItem.Text = "Edit";
            this.editIntMenuItem.Click += new System.EventHandler(this.editIntMenuItem_Click);
            // 
            // deleteIntMenuItem
            // 
            this.deleteIntMenuItem.Name = "deleteIntMenuItem";
            this.deleteIntMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteIntMenuItem.Text = "Delete";
            this.deleteIntMenuItem.Click += new System.EventHandler(this.deleteIntMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 513);
            this.Controls.Add(this.tcProperties);
            this.Controls.Add(this.mnuMainMenu);
            this.MainMenuStrip = this.mnuMainMenu;
            this.Name = "Form1";
            this.Text = "ACE Weenie Editor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.mnuMainMenu.ResumeLayout(false);
            this.mnuMainMenu.PerformLayout();
            this.tcProperties.ResumeLayout(false);
            this.tpSummary.ResumeLayout(false);
            this.tpStrings.ResumeLayout(false);
            this.pEditString.ResumeLayout(false);
            this.pEditString.PerformLayout();
            this.tpInts.ResumeLayout(false);
            this.cmsString.ResumeLayout(false);
            this.pEditInt.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbIntValue)).EndInit();
            this.cmsInt.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuMainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseConnectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byIDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gitHubToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem prepareUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testDatabaseToolStripMenuItem;
        private System.Windows.Forms.TabControl tcProperties;
        private System.Windows.Forms.TabPage tpSummary;
        private System.Windows.Forms.TabPage tpStrings;
        private System.Windows.Forms.TabPage tpInts;
        private System.Windows.Forms.TabPage tpInt64s;
        private System.Windows.Forms.TabPage tpDoubles;
        private System.Windows.Forms.TabPage tpBools;
        private System.Windows.Forms.TabPage tpLocations;
        private System.Windows.Forms.TabPage tpDIDs;
        private System.Windows.Forms.TabPage tpIIDs;
        private System.Windows.Forms.TabPage tpAttributes;
        private System.Windows.Forms.TabPage tpSkills;
        private System.Windows.Forms.TabPage tpBook;
        private System.Windows.Forms.ContextMenuStrip cmsString;
        private System.Windows.Forms.ToolStripMenuItem addStringMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editStringMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteStringMenuItem;
        private System.Windows.Forms.Panel pEditString;
        private System.Windows.Forms.Button btnCancelString;
        private System.Windows.Forms.Button btnSaveString;
        private System.Windows.Forms.TextBox tbStringValue;
        private System.Windows.Forms.ComboBox cbStringProperties;
        public System.Windows.Forms.TreeView tvStrings;
        private System.Windows.Forms.ComboBox cbWeenieType;
        private System.Windows.Forms.Label lblWeenidId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pEditInt;
        private System.Windows.Forms.Button btnCancelInt;
        private System.Windows.Forms.Button btnSaveInt;
        private System.Windows.Forms.ComboBox cbIntProperties;
        public System.Windows.Forms.TreeView tvInts;
        private System.Windows.Forms.NumericUpDown tbIntValue;
        private System.Windows.Forms.ContextMenuStrip cmsInt;
        private System.Windows.Forms.ToolStripMenuItem addIntMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editIntMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteIntMenuItem;
    }
}

