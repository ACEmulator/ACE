namespace ACE.Diagnostics
{
    partial class FrmMonitor
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
            components = new System.ComponentModel.Container();
            timerDraw = new System.Windows.Forms.Timer(components);
            label1 = new System.Windows.Forms.Label();
            lblDetail = new System.Windows.Forms.Label();
            picImage = new System.Windows.Forms.PictureBox();
            picZoom = new System.Windows.Forms.PictureBox();
            label2 = new System.Windows.Forms.Label();
            txtDetail = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(picImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(picZoom)).BeginInit();
            SuspendLayout();
            timerDraw.Interval = 1000;
            timerDraw.Tick += new System.EventHandler(timersimulate_Tick);

            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 22);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(92, 13);
            label1.TabIndex = 3;
            label1.Text = "Landblock Activty";

            lblDetail.AutoSize = true;
            lblDetail.Location = new System.Drawing.Point(288, 22);
            lblDetail.Name = "lblDetail";
            lblDetail.Size = new System.Drawing.Size(93, 13);
            lblDetail.TabIndex = 5;
            lblDetail.Text = "Landblock Detail: ";

            picImage.Image = Properties.Resources.map;
            picImage.Location = new System.Drawing.Point(15, 38);
            picImage.Name = "picImage";
            picImage.Size = new System.Drawing.Size(256, 256);
            picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            picImage.TabIndex = 6;
            picImage.TabStop = false;
            picImage.Click += new System.EventHandler(picImage_Click);
            picImage.MouseMove += new System.Windows.Forms.MouseEventHandler(picImage_MouseMove);

            picZoom.Location = new System.Drawing.Point(291, 38);
            picZoom.Name = "picZoom";
            picZoom.Size = new System.Drawing.Size(256, 256);
            picZoom.TabIndex = 7;
            picZoom.TabStop = false;

            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(572, 22);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(93, 13);
            label2.TabIndex = 8;
            label2.Text = "Landblock Detail: ";

            txtDetail.Location = new System.Drawing.Point(575, 38);
            txtDetail.Multiline = true;
            txtDetail.Name = "txtDetail";
            txtDetail.ReadOnly = true;
            txtDetail.Size = new System.Drawing.Size(387, 256);
            txtDetail.TabIndex = 9;

            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Control;
            ClientSize = new System.Drawing.Size(980, 303);
            Controls.Add(txtDetail);
            Controls.Add(label2);
            Controls.Add(picZoom);
            Controls.Add(picImage);
            Controls.Add(lblDetail);
            Controls.Add(label1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "FrmMonitor";
            Text = "Landblock Monitor";
            Load += new System.EventHandler(FrmMonitor_Load);
            Move += new System.EventHandler(FrmMonitor_Move);
            ((System.ComponentModel.ISupportInitialize)(picImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(picZoom)).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timerDraw;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDetail;
        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.PictureBox picZoom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDetail;
    }
}

