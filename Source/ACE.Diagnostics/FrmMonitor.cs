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

        private Bitmap bitmapdiag = new Bitmap(256, 256, PixelFormat.Format32bppArgb);
        private int blocksize = 1;

        private Brush brushIdleUnloaded = Brushes.Black;
        private Brush brushIdleLoaded = Brushes.LightGreen;
        private Brush brushIdleLoading = Brushes.LightYellow;
        private Brush brushIdleUnloading = Brushes.WhiteSmoke;

        private Brush brushInUseLow = Brushes.Green;
        private Brush brushInUseMed = Brushes.Yellow;
        private Brush brushInuseHigh = Brushes.Orange;
        private Brush brushGenericError = Brushes.DarkRed;

        private int zoomFactor = 20;
        private Color backColor;
        private int selrow = 0;
        private int selcol = 0;

        private void FrmMonitor_Load(object sender, EventArgs e)
        {
            // Synchronize some private members with the form's values.
            backColor = Color.Black;

            // Set the sizemode of both pictureboxes. These modes are important
            // to the functionality and should not be changed.
            // picImage.SizeMode = PictureBoxSizeMode.CenterImage;
            picZoom.SizeMode = PictureBoxSizeMode.StretchImage;

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

            // Draw to bitmap in memory
            using (imgGraphics)
            {
                int coloffset = 0;
                int rowoffset = 0;

                for (int row = 0; row < 256; row++)
                {
                    for (int col = 0; col < 256; col++)
                    {
                        if (!Diagnostics.LandBlockDiag)
                            return;

                        LandBlockStatusFlag key = new LandBlockStatusFlag();
                        key = Diagnostics.GetLandBlockKeyFlag(row, col);

                        // has it changed from last known state ?
                        if (initdraw)
                        {
                            TempLandBlockKeys[row, col] = key;
                        }
                        else
                        {
                            LandBlockStatusFlag prevkey = new LandBlockStatusFlag();
                            prevkey = TempLandBlockKeys[row, col];
                            // if no change then no need to write.
                            if (prevkey == key)
                                break;
                        }

                        switch (key)
                        {
                            case LandBlockStatusFlag.IdleLoading:
                                imgGraphics.FillRectangle(brushIdleLoading, rowoffset, coloffset, blocksize, blocksize);
                                break;
                            case LandBlockStatusFlag.IdleLoaded:
                                imgGraphics.FillRectangle(brushIdleLoaded, rowoffset, coloffset, blocksize, blocksize);
                                break;
                            case LandBlockStatusFlag.InUseLow:
                                imgGraphics.FillRectangle(brushInUseLow, rowoffset, coloffset, blocksize, blocksize);
                                break;
                            case LandBlockStatusFlag.InUseMed:
                                imgGraphics.FillRectangle(brushInUseMed, rowoffset, coloffset, blocksize, blocksize);
                                break;
                            case LandBlockStatusFlag.InuseHigh:
                                imgGraphics.FillRectangle(brushInuseHigh, rowoffset, coloffset, blocksize, blocksize);
                                break;
                            case LandBlockStatusFlag.IdleUnloading:
                                imgGraphics.FillRectangle(brushIdleUnloading, rowoffset, coloffset, blocksize, blocksize);
                                break;
                            case LandBlockStatusFlag.IdleUnloaded:
                                imgGraphics.FillRectangle(brushIdleUnloaded, rowoffset, coloffset, blocksize, blocksize);
                                break;
                            case LandBlockStatusFlag.GenericError:
                                imgGraphics.FillRectangle(brushGenericError, rowoffset, coloffset, blocksize, blocksize);
                                break;
                        }
                        coloffset = coloffset + blocksize;
                    }
                    rowoffset = rowoffset + blocksize;
                    coloffset = 0;
                }

                // Draw from memory to image
                picImage.Image = bitmapdiag;

            }
            // on first draw this is true.
            if (initdraw)
            {
                initdraw = true;
                // ResizeAndDisplayImage();
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

        private void picLandblockDiag_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(string.Format("X: {0} Y: {1}", e.X, e.Y));          
        }

        private void FrmMonitor_Move(object sender, EventArgs e)
        {
            initdraw = true;
        }

        private void ResizeAndDisplayImage()
        {
            // Zooming Functions Credit too 
            // JohnWillemse, 30 Oct 2007 
            // https://www.codeproject.com/Articles/21097/PictureBox-Zoom

            // Set the backcolor of the pictureboxes

            picImage.BackColor = backColor;
            picZoom.BackColor = backColor;

            if (picImage == null)
                return;

            int sourceWidth = bitmapdiag.Width;
            int sourceHeight = bitmapdiag.Height;
            int targetWidth;
            int targetHeight;
            double ratio;

            // Calculate targetWidth and targetHeight, so that the image will fit into

            // the picImage picturebox without changing the proportions of the image.

            if (sourceWidth > sourceHeight)
            {
                // Set the new width

                targetWidth = picImage.Width;
                // Calculate the ratio of the new width against the original width

                ratio = (double)targetWidth / sourceWidth;
                // Calculate a new height that is in proportion with the original image

                targetHeight = (int)(ratio * sourceHeight);
            }
            else if (sourceWidth < sourceHeight)
            {
                // Set the new height

                targetHeight = picImage.Height;
                // Calculate the ratio of the new height against the original height

                ratio = (double)targetHeight / sourceHeight;
                // Calculate a new width that is in proportion with the original image

                targetWidth = (int)(ratio * sourceWidth);
            }
            else
            {
                // In this case, the image is square and resizing is easy

                targetHeight = picImage.Height;
                targetWidth = picImage.Width;
            }

            // Calculate the targetTop and targetLeft values, to center the image

            // horizontally or vertically if needed

            int targetTop = (picImage.Height - targetHeight) / 2;
            int targetLeft = (picImage.Width - targetWidth) / 2;

            // Create a new temporary bitmap to resize the original image

            // The size of this bitmap is the size of the picImage picturebox.

            Bitmap tempBitmap = new Bitmap(picImage.Width, picImage.Height,
                                           PixelFormat.Format24bppRgb);

            // Set the resolution of the bitmap to match the original resolution.

            tempBitmap.SetResolution(bitmapdiag.HorizontalResolution,
                                     bitmapdiag.VerticalResolution);

            // Create a Graphics object to further edit the temporary bitmap

            Graphics bmGraphics = Graphics.FromImage(tempBitmap);

            // First clear the image with the current backcolor

            bmGraphics.Clear(backColor);

            // Set the interpolationmode since we are resizing an image here

            bmGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw the original image on the temporary bitmap, resizing it using

            // the calculated values of targetWidth and targetHeight.

            bmGraphics.DrawImage(bitmapdiag,
                                 new Rectangle(targetLeft, targetTop, targetWidth, targetHeight),
                                 new Rectangle(0, 0, sourceWidth, sourceHeight),
                                 GraphicsUnit.Pixel);

            // Dispose of the bmGraphics object

            bmGraphics.Dispose();

            // Set the image of the picImage picturebox to the temporary bitmap

            picImage.Image = tempBitmap;
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

            bmGraphics.Clear(backColor);

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
            bmGraphics.DrawLine(Pens.Blue, halfWidth + 0, halfHeight - 2, halfWidth + 0, halfHeight - 0);
            bmGraphics.DrawLine(Pens.Blue, halfWidth + 0, halfHeight + 2, halfWidth + 0, halfHeight + 0);
            bmGraphics.DrawLine(Pens.Blue, halfWidth - 2, halfHeight + 0, halfWidth - 0, halfHeight + 0);
            bmGraphics.DrawLine(Pens.Blue, halfWidth + 2, halfHeight + 0, halfWidth + 0, halfHeight + 0);

            // Dispose of the Graphics object

            bmGraphics.Dispose();

            // Refresh the picZoom picturebox to reflect the changes

            picZoom.Refresh();
        }

        private void picImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (picImage.Image == null)
                return;

            Tuple<int, int> bla = Reverse(e.X, e.Y);
            selrow = (bla.Item1);
            selcol = (bla.Item2);

            UpdateZoomedImage(e);
            lblDetail.Text = string.Format("Landblock Detail: {0}, {1} ", selrow, selcol);
        }

        private Tuple<int, int> Reverse(int x, int y)
        {
            var bla = new Tuple<int, int>(Math.Abs(255 - y), Math.Abs(255 - x));
            return bla;
        }


        private void picImage_Click(object sender, EventArgs e)
        {
            txtDetail.Clear();
            LandBlockStatus status = new LandBlockStatus();
            status = Diagnostics.GetLandBlockKey(selrow, selcol);
            if (status != null)
            {
                txtDetail.AppendText(string.Format("Landblock: ", status.LandBlockId));
                txtDetail.AppendText(string.Format("Status: ", status.LandBlockStatusFlag.ToString()));
                txtDetail.AppendText(string.Format("Players: ", status.Playercount));
            }
        }

    }
}
