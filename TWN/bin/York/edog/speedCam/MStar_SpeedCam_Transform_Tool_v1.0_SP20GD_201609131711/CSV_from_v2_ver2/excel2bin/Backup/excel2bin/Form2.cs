using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace excel2bin
{
    public partial class Form2 : Form
    {
        public double layerwidth = 0.0, smalllayerwidth = 0.0, lonlayerwidth = 0.0, lonsmalllayerwidth = 0.0;
        public int blockcount = 0, smalllayerblockcount = 0, lonblockcount = 0, lonsmalllayerblockcount = 0;
        public double latstart = 0.0, latend = 0.0, lonstart = 0.0, lonend = 0.0;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {            
            DGV_DivisionOverview.ColumnCount = lonblockcount * lonsmalllayerblockcount;
            DGV_DivisionOverview.RowCount = blockcount * smalllayerblockcount;            
        }

        private void btn_overviewrefresh_Click(object sender, EventArgs e)
        {

            tB_ColorTypes.Text = " Good";

        }

        private void DGV_DivisionOverview_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedcellcount = DGV_DivisionOverview.GetCellCount(DataGridViewElementStates.Selected);
            double tmpLon = 0.0, tmpLat = 0.0;

            tmpLat = (double)((latend - latstart) / (blockcount * smalllayerblockcount));
            tmpLon = (double)( (lonend - lonstart) / (lonblockcount*lonsmalllayerblockcount) );
            if (selectedcellcount > 0)
            {
                tB_SelectedCellLon.Text = (lonstart + tmpLon * ( DGV_DivisionOverview.SelectedCells[0].ColumnIndex + 1) ).ToString();
                tB_SelectedCellLat.Text = (latend - tmpLat * (DGV_DivisionOverview.SelectedCells[0].RowIndex + 1)).ToString();
                tB_OverviewcellPOI.Text = DGV_DivisionOverview.Rows[DGV_DivisionOverview.SelectedCells[0].RowIndex].Cells[DGV_DivisionOverview.SelectedCells[0].ColumnIndex].Value.ToString();
            }
        }
    }
}
