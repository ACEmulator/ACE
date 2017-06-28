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
            this.components = new System.ComponentModel.Container();
            this.timerDraw = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.lblDetail = new System.Windows.Forms.Label();
            this.picImage = new System.Windows.Forms.PictureBox();
            this.picZoom = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDetail = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // timerDraw
            // 
            this.timerDraw.Interval = 1000;
            this.timerDraw.Tick += new System.EventHandler(this.timersimulate_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Landblock Activty";
            // 
            // lblDetail
            // 
            this.lblDetail.AutoSize = true;
            this.lblDetail.Location = new System.Drawing.Point(288, 22);
            this.lblDetail.Name = "lblDetail";
            this.lblDetail.Size = new System.Drawing.Size(93, 13);
            this.lblDetail.TabIndex = 5;
            this.lblDetail.Text = "Landblock Detail: ";
            // 
            // picImage
            // 
            this.picImage.Image = global::ACE.Diagnostics.Properties.Resources.map;
            this.picImage.Location = new System.Drawing.Point(15, 38);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(255, 255);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picImage.TabIndex = 6;
            this.picImage.TabStop = false;
            this.picImage.Click += new System.EventHandler(this.picImage_Click);
            this.picImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picImage_MouseMove);
            // 
            // picZoom
            // 
            this.picZoom.Location = new System.Drawing.Point(291, 38);
            this.picZoom.Name = "picZoom";
            this.picZoom.Size = new System.Drawing.Size(256, 256);
            this.picZoom.TabIndex = 7;
            this.picZoom.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(572, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Landblock Detail: ";
            // 
            // txtDetail
            // 
            this.txtDetail.Location = new System.Drawing.Point(575, 38);
            this.txtDetail.Multiline = true;
            this.txtDetail.Name = "txtDetail";
            this.txtDetail.ReadOnly = true;
            this.txtDetail.Size = new System.Drawing.Size(387, 256);
            this.txtDetail.TabIndex = 9;
            // 
            // FrmMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(980, 303);
            this.Controls.Add(this.txtDetail);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.picZoom);
            this.Controls.Add(this.picImage);
            this.Controls.Add(this.lblDetail);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmMonitor";
            this.Text = "Landblock Monitor";
            this.Load += new System.EventHandler(this.FrmMonitor_Load);
            this.Move += new System.EventHandler(this.FrmMonitor_Move);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picZoom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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

