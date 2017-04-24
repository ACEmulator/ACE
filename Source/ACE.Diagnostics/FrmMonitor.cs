using ACE.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACE.Diagnostics
{
    public partial class FrmMonitor : Form
    {
        public FrmMonitor()
        {
            InitializeComponent();
            ACE.Common.Diagnostics.LandBlockDiag = true;           
        }

        BackgroundWorker bwUpdateLandblockGrid = new BackgroundWorker();
        int blocksize = 3;

        Brush brushIdleUnloaded = Brushes.Black;
        Brush brushIdleLoaded = Brushes.LightGreen;
        Brush brushIdleLoading = Brushes.LightYellow;
        Brush brushIdleUnloading = Brushes.WhiteSmoke;

        Brush brushInUseLow = Brushes.Green;
        Brush brushInUseMed = Brushes.Yellow;
        Brush brushInuseHigh = Brushes.Orange;
        Brush brushGenericError = Brushes.DarkRed;

        private void FrmMonitor_Load(object sender, EventArgs e)
        {
            bwUpdateLandblockGrid.DoWork += new DoWorkEventHandler(bwUpdateLandblockGrid_DoWork);
            timer.Enabled = true;
        }

        private void bwUpdateLandblockGrid_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            UpdateLandBlockDiag();
        }

        private void UpdateLandBlockDiag()
        {
            Graphics imgGraphics = picLandblockDiag.CreateGraphics();

            using (imgGraphics)
            {
                int rowoffset = 0;
                int coloffset = 0;

                for (int row = 0; row < 256; row++)
                {
                    for (int col = 0; col < 256; col++)
                    {
                        LandBlockStatusFlag key = new LandBlockStatusFlag();
                        key = ACE.Common.Diagnostics.GetLandBlockKey(row, col);

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
            }
        }

        private void bwGraphics_DoWork(object sender, DoWorkEventArgs e)
        {
            UpdateLandBlockDiag();
        }

        private void timersimulate_Tick(object sender, EventArgs e)
        {

            if (!bwUpdateLandblockGrid.IsBusy)
            {
                bwUpdateLandblockGrid.RunWorkerAsync();
            }
        }
    }
}
