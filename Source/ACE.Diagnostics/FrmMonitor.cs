using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ACE.Diagnostics
{
    public partial class FrmMonitor : Form
    {
        public FrmMonitor()
        {
            InitializeComponent(); 
        }

        private bool initdraw = false;
        private LandBlockStatusFlag[,] TempLandBlockKeys = new LandBlockStatusFlag[256, 256];
        private BackgroundWorker bwUpdateLandblockGrid = new BackgroundWorker();

        private Bitmap bitmapdiag = new Bitmap(256, 256, PixelFormat.Format32bppPArgb);
        private int blocksize = 1;

        private Brush brushIdleUnloaded = Brushes.Transparent;
        private Brush brushIdleLoaded = Brushes.LightGreen;
        private Brush brushIdleLoading = Brushes.LightYellow;
        private Brush brushIdleUnloading = Brushes.WhiteSmoke;

        private Brush brushInUseLow = Brushes.Green;
        private Brush brushInUseMed = Brushes.Yellow;
        private Brush brushInuseHigh = Brushes.Orange;
        private Brush brushGenericError = Brushes.DarkRed;

        private int zoomFactor = 5;

        private Color backColor;
        private int selrow = 0;
        private int selcol = 0;

        private void FrmMonitor_Load(object sender, EventArgs e)
        {

            backColor = Color.Black;
            picZoom.SizeMode = PictureBoxSizeMode.Zoom;

            bwUpdateLandblockGrid.DoWork += new DoWorkEventHandler(bwUpdateLandblockGrid_DoWork);
            timerDraw.Enabled = true;
        }

        private void bwUpdateLandblockGrid_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            if (!Diagnostics.LandBlockDiag)
                worker.CancelAsync();

            if (worker.CancellationPending == true)
                e.Cancel = true;
            else
                UpdateLandBlockDiag();
        }

        private void UpdateLandBlockDiag()
        {
            Graphics imgGraphics = Graphics.FromImage(bitmapdiag);
            imgGraphics.CompositingMode = CompositingMode.SourceCopy;

            // 0,255  ******** 255,255
            //        ********
            // 0,0    ******** 255,0

            //  rows decrease / col increase.

            // Draw to bitmap in memory
            using (imgGraphics)
            {
                int coloffset = 0;
                int rowoffset = 0;

                for (int row = 0; row < 255; row++)
                {
                 for    (int col = 0; col < 255; col++)
                    {
                        if (!Diagnostics.LandBlockDiag)
                            return;

                        LandBlockStatusFlag key = new LandBlockStatusFlag();
                        key = Diagnostics.GetLandBlockKeyFlag(col, row);

                        // has it changed from last known state ?
                        if (initdraw)
                        {
                            TempLandBlockKeys[col, row] = key;
                        }
                        else
                        {
                            LandBlockStatusFlag prevkey = new LandBlockStatusFlag();
                            prevkey = TempLandBlockKeys[col, row];
                            // if no change then no need to write.
                            if (prevkey == key)
                                break;
                        }

                        switch (key)
                        {
                            case LandBlockStatusFlag.IdleLoading:
                                imgGraphics.FillRectangle(brushIdleLoading, coloffset, Reverse(rowoffset), blocksize, blocksize);
                                break;
                            case LandBlockStatusFlag.IdleLoaded:
                                imgGraphics.FillRectangle(brushIdleLoaded, coloffset, Reverse(rowoffset), blocksize, blocksize);
                                break;
                            case LandBlockStatusFlag.InUseLow:
                                imgGraphics.FillRectangle(brushInUseLow, coloffset, Reverse(rowoffset), blocksize, blocksize);
                                break;
                            case LandBlockStatusFlag.InUseMed:
                                imgGraphics.FillRectangle(brushInUseMed, coloffset, Reverse(rowoffset), blocksize, blocksize);
                                break;
                            case LandBlockStatusFlag.InuseHigh:
                                imgGraphics.FillRectangle(brushInuseHigh, coloffset, Reverse(rowoffset), blocksize, blocksize);
                                break;
                            case LandBlockStatusFlag.IdleUnloading:
                                imgGraphics.FillRectangle(brushIdleUnloading, coloffset, Reverse(rowoffset), blocksize, blocksize);
                                break;
                            case LandBlockStatusFlag.IdleUnloaded:
                                // todo: grab pixel from original map image
                                imgGraphics.FillRectangle(brushIdleUnloaded, coloffset, Reverse(rowoffset), blocksize, blocksize);
                                break;
                            case LandBlockStatusFlag.GenericError:
                                imgGraphics.FillRectangle(brushGenericError, coloffset, Reverse(rowoffset), blocksize, blocksize);
                                break;
                        }
                        coloffset = coloffset + blocksize;
                    }
                    rowoffset = rowoffset + blocksize;
                    coloffset = 0;
                }

                // over lap ac map
                Image canvas = new Bitmap(Properties.Resources.map);
                Graphics gra = Graphics.FromImage(canvas);

                gra.DrawImage(bitmapdiag, 0, 0);

                picImage.Image = canvas;

            }
            // on first draw this is true.
            if (initdraw)
            {
                // always true for now.
                initdraw = true;
            }
        }

        private void bwGraphics_DoWork(object sender, DoWorkEventArgs e)
        {
            UpdateLandBlockDiag();
        }

        private void timersimulate_Tick(object sender, EventArgs e)
        {
            if (Diagnostics.LandBlockDiag)
                if (!bwUpdateLandblockGrid.IsBusy)
                    bwUpdateLandblockGrid.RunWorkerAsync();
        }

        private void FrmMonitor_Move(object sender, EventArgs e)
        {
            initdraw = true;
        }

        private void UpdateZoomedImage(MouseEventArgs e)
        {
            // Zooming Functions Credit too 
            // JohnWillemse, 30 Oct 2007 
            // https://www.codeproject.com/Articles/21097/PictureBox-Zoom

            // Calculate the width and height of the portion of the image we want

            // to show in the picZoom picturebox. This value changes when the zoom

            // factor is changed.

            int zoomWidth = picZoom.Width / zoomFactor;
            int zoomHeight = picZoom.Height / zoomFactor;

            // Calculate the horizontal and vertical midpoints for the crosshair

            // cursor and correct centering of the new image

            int halfWidth = zoomWidth / 2;
            int halfHeight = zoomHeight / 2;

            // Create a new temporary bitmap to fit inside the picZoom picturebox

            Bitmap tempBitmap = new Bitmap(zoomWidth, zoomHeight,
                                           PixelFormat.Format24bppRgb);

            // Create a temporary Graphics object to work on the bitmap

            Graphics bmGraphics = Graphics.FromImage(tempBitmap);

            // Clear the bitmap with the selected backcolor

            bmGraphics.Clear(Color.Black);

            // Set the interpolation mode

            bmGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw the portion of the main image onto the bitmap

            // The target rectangle is already known now.

            // Here the mouse position of the cursor on the main image is used to

            // cut out a portion of the main image.

            bmGraphics.DrawImage(picImage.Image,
                                 new Rectangle(0, 0, zoomWidth, zoomHeight),
                                 new Rectangle(e.X - halfWidth, e.Y - halfHeight,
                                 zoomWidth, zoomHeight), GraphicsUnit.Pixel);

            // Draw the bitmap on the picZoom picturebox

            picZoom.Image = tempBitmap;

            // Draw a crosshair on the bitmap to simulate the cursor position
            bmGraphics.DrawLine(Pens.Gold, halfWidth + 0, halfHeight - 2, halfWidth + 0, halfHeight - 0);
            bmGraphics.DrawLine(Pens.Gold, halfWidth + 0, halfHeight + 2, halfWidth + 0, halfHeight + 0);
            bmGraphics.DrawLine(Pens.Gold, halfWidth - 2, halfHeight + 0, halfWidth - 0, halfHeight + 0);
            bmGraphics.DrawLine(Pens.Gold, halfWidth + 2, halfHeight + 0, halfWidth + 0, halfHeight + 0);

            // Dispose of the Graphics object

            bmGraphics.Dispose();

            // Refresh the picZoom picturebox to reflect the changes

            picZoom.Refresh();

        }

        private void picImage_MouseMove(object sender, MouseEventArgs e)
        {

            // The AC landblocks are laid out in a grid of 0,0 by 255,255 but as you would expect
            // 0,255  ******** 255,255
            //        ********
            // 0,0    ******** 255,0

            if (picImage.Image == null)
                return;

            selcol = Reverse(e.Y);
            selrow = e.X;

            UpdateZoomedImage(e);
            lblDetail.Text = string.Format("Landblock Detail: {0}, {1} ", selrow, selcol);
        }

        private int Reverse(int x)
        {
            x = (Math.Abs(255 - x));
            return x;
        }

        private void picImage_Click(object sender, EventArgs e)
        {
            txtDetail.Clear();
   
            // this is used for unloaded and loaded landblocks
            LandblockId landblockid = new LandblockId((byte)selrow, (byte)selcol);
            txtDetail.Text = string.Format("Landblock: {0} ", landblockid.Raw.ToString("X"));

            // this only works if the landblock is loaded..
            LandBlockStatus status = new LandBlockStatus();
            status = Diagnostics.GetLandBlockKey(selrow, selcol);
            if (status != null)
            {
                txtDetail.Text += Environment.NewLine + string.Format("Status:  {0} ", status.LandBlockStatusFlag.ToString());
                txtDetail.Text += Environment.NewLine + string.Format("Players:  {0} ", status.PlayerCount);
            }
        }

    }
}
