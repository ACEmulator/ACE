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
            this.picLandblockDiag = new System.Windows.Forms.Panel();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // picLandblockDiag
            // 
            this.picLandblockDiag.Location = new System.Drawing.Point(3, 7);
            this.picLandblockDiag.Name = "picLandblockDiag";
            this.picLandblockDiag.Size = new System.Drawing.Size(777, 777);
            this.picLandblockDiag.TabIndex = 2;
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timersimulate_Tick);
            // 
            // FrmMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(784, 783);
            this.Controls.Add(this.picLandblockDiag);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmMonitor";
            this.Text = "Landblock Monitor";
            this.Load += new System.EventHandler(this.FrmMonitor_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel picLandblockDiag;
        private System.Windows.Forms.Timer timer;
    }
}

