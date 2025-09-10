using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;

namespace excel2bin
{  
    public partial class Form1 : Form
    {
        // ---------------- WinAPI --------------------------
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, int lParam);

        private const int WM_CONVERTER_NOTIFY = 1525; // (WM_USER + 501)
        // ----------------- WinAPI --------------------------

        double layerwidth = 0.0, smalllayerwidth = 0.0, lonlayerwidth = 0.0, lonsmalllayerwidth = 0.0;
        int blockcount = 0,  smalllayerblockcount = 0, lonblockcount = 0, lonsmalllayerblockcount = 0;
        double latstart = 0.0, latend = 0.0, lonstart = 0.0, lonend = 0.0;
        //int totallatlargeblockcount = 0, totallonlargeblockcount = 0, totallatsmallblockcount = 0, totallonsmallblockcount = 0;
        //int totaloffset = 0;
        DataTable dTableOut = new DataTable();
        DataTable dTableOut1 = new DataTable();
        ArrayList[,] points = null;
        ArrayList[,] tmppoints = null;       // for iteratively find out the final combination
        ArrayList[, ,] multilevelpoints = null;   // for multi files, currently used for CHN
        long[] poioflargelayer = null;
        //uint dataversion = 2, mioversion = 0; //dataversion: for AIT, mioversion: for MiTAC
        //20160314, extend verstion: #method support RUS autodoria: 4
        uint dataversion = 2, mioversion = 0; 
        int CHNmapthreshold = 80000;
        int autodoria_s = 7, autodoria_e = 8;
        string[] dirtypes;
        string[] cameratypes;
        DateTime CurrentTime = new DateTime();
        string str_datetime = null, importexcel = null;
        int layerinfo;   // index: layer information (,)        
        bool mode = false;     // GUI: true;    cmd: false;
        bool executionstatus = false;   // true: successful; false: failed

        // for INI file
        string inifile = null;
        string sectionname = "Model", successlogname = "SuccessLogFile", errorlogname = "ErrorLogFile", rawlogname = "RawFile";
        string successlogpath = null, errorlogpath = null, rawlogpath = null;
        string successlogstr = null, errorlogstr = null, rawlogstr = null;
        //string skuid = null;
        ArrayList successloglist = new ArrayList();
        ArrayList errorloglist = new ArrayList();

        //for double or float version
        bool doubleorfloat = true;   //if true: double version; if false: float version;            //DigiLife should use double version
        
        // for dynamic slicing layers
        bool dynamicslicing = true;   // if false: average slicing; if true: dynamic
        bool IsCHNPOI = false;     // if true: for M-CHN POI data; if true: for other POI data        
        bool IsWiCHNPOI = false;   // if true: for Wi-CHN POI data; if true: for other POI data        
        ArrayList[] ignoreslicingrule;
        ArrayList[] divisionblockcount;
        string[] allfilechecksum;
        
        // for showing the info of how many POIs in each small/large block
        bool showPOIperblock = true; 

        // for division overview
        Form2 DivisionOverview = new Form2();                      

        // for autodoria alert
        ArrayList autodoriapair_idx = new ArrayList();

        // For Aus
        bool IsAusPOI = false;
        bool shiftAUS = true; // if GPS coordinate is negative, should shift to positive
        bool shiftUSA = false;   // if GPS coordinate is negative, should shift to positive
       
        //-----------------------------------------------------------------------------------------------------//
        //public Form1()        
        public Form1(string[] args):base()
        {
            string tmp1 = null, tmp2 = null;            
            
            InitializeComponent();
            CurrentTime = System.DateTime.Now;            
            //str_datetime = CurrentTime.ToShortDateString();   // 2013/12/5            
            if (CurrentTime.Month < 10)
                tmp1 = "0" + CurrentTime.Month.ToString();
            else
                tmp1 = CurrentTime.Month.ToString();

            if (CurrentTime.Day < 10)
                tmp2 = "0" + CurrentTime.Day.ToString();
            else
                tmp2 = CurrentTime.Day.ToString();

            str_datetime = CurrentTime.Year.ToString() + tmp1 + tmp2;
            importexcel = string.Empty;
            if (args.Length > 0)
            {
                if (File.Exists(args[0].ToString()))
                {
                    importexcel = args[0].ToString();
                    mode = false;
                    //MessageBox.Show(importexcel);
                }
                else if (args[0].ToString() == "/u")
                {
                    //MessageBox.Show("/u");
                    mode = true;
                }

                DGV_excel.Rows.Clear();
                dTableOut.Clear();
                successloglist.Clear();
                errorloglist.Clear();
                successlogpath = string.Empty;
                errorlogpath = string.Empty;
                rawlogpath = string.Empty;
            }
        }

        //------------------- Begin of Private Function -----------------------------//       
        public void exportdatagridview()
        {
            //header: "UID", "
            string delimiter = ",";
            string exportfilename = "DBG_GridView.csv";
            StreamWriter csvstream = new StreamWriter(exportfilename, false, System.Text.Encoding.Unicode);
            string header = "", tmpstr = "";
            int i, j;
            for (i = 0; i < DGV_excel.Columns.Count; i++)
                header += DGV_excel.Columns[i].HeaderText + delimiter;
            for (i = 0; i < DGV_excel.Rows.Count; i++)
            {
                tmpstr = "";
                for (j = 0; j < DGV_excel.Columns.Count; j++)
                {                    
                    tmpstr += DGV_excel.Rows[i].Cells[j].Value.ToString() + delimiter;
                }
                csvstream.WriteLine(tmpstr);
            }
            csvstream.Close();
        }

        //-------------------------------------------
        public int findautodoriapair(int uid_s)   //for autodoria pair
        {
            int pair_e = -1;
            string tmpuid_s = DGV_excel.Rows[uid_s].Cells[0].Value.ToString();
            string tmpuid_e = null;
            int i;

            if (tmpuid_s.IndexOf("_E", StringComparison.Ordinal) >= 0)
            {
                tmpuid_e = tmpuid_s.Substring(0, tmpuid_s.Length-2);
            }
            else
            {
                tmpuid_e = tmpuid_s + "_E";
            }

            for (i = 0; i < DGV_excel.RowCount; i++)
            {
                if (string.Compare(tmpuid_e, DGV_excel.Rows[i].Cells[0].Value.ToString()) == 0)
                {
                    pair_e = i;
                    break;
                }
                 
            }            

            return pair_e;
        }

        //--------------------------------------------------
        public void LoadExcelFile(string excelfile)
        {
            int i = 0; // counter
            string[] words;
            string tmpstr = null;
            //double tmp_latstart = 180.0, tmp_latend = 0.0;
            double tmp_latstart = double.MaxValue, tmp_latend = double.MinValue;
            double tmp_lonstart = double.MaxValue, tmp_lonend = double.MinValue;
            bool lonsmallzero = false, latsmallzero = false;
            int csvfilelines = 0;
            string tmpexcelfilename = null;
            double tran_value = double.MinValue;

            //tB_excel.Text = OFDlg1.FileName;
            //tB_bin.Text = System.IO.Path.ChangeExtension(OFDlg1.FileName, "bin");
            tB_excel.Text = excelfile;
            //tB_bin.Text = System.IO.Path.ChangeExtension(excelfile, "bin");  //20131216
            //tB_bin.Text = Application.StartupPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(excelfile) + ".bin";
            //tB_bin.Text = Application.StartupPath + "\\Speedcam_Data_Rus.bin";         
            tmpstr = Application.ExecutablePath;
            words = tmpstr.Split('\\');
            tmpstr = string.Empty;
            for (i = 0; i < words.Length - 1; i++)
                tmpstr += words[i] + "\\";
            
            //tB_bin.Text = tmpstr + "Speedcam_Data_CHN.bin";
            tB_bin.Text = tmpstr + "Speedcam_Data.bin";

            string OpenExcelData = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + tB_excel.Text + ";Extended Properties = 'Excel 8.0;HDR=Yes;IMEX=1;'";
            OleDbConnection ExcelConnection = new OleDbConnection(OpenExcelData);

            if (File.Exists(excelfile))
            {
                try
                {
                    // For Aus > 60000 case
                    if (excelfile.IndexOf("AUS", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        IsAusPOI = true;
                    }

					tmpexcelfilename = System.IO.Path.GetFileNameWithoutExtension(excelfile);

                    if (excelfile.IndexOf(".csv", StringComparison.OrdinalIgnoreCase) >= 0)
                    {                        
                        //if (excelfile.IndexOf("CHN", StringComparison.OrdinalIgnoreCase) >= 0)
                        //if (excelfile.IndexOf("CHN", StringComparison.Ordinal) >= 0)
						if ((tmpexcelfilename.IndexOf("CHN", StringComparison.Ordinal) >= 0) ||
                            (cB_subarea.Checked) )
                        {
                            IsCHNPOI = true;
                            IsWiCHNPOI = true;
                            IsAusPOI = true;
                            //if CHN POI, use float datatype; otherwise, use double datatype
                            doubleorfloat = false;
                        }

                        string[] str = File.ReadAllLines(excelfile);
                        string[] temp = str[0].Split(',');
                        csvfilelines = int.Parse(temp[0].ToString());
                        temp = str[1].Split(',');
                        mioversion = uint.Parse(temp[0].ToString());
                        temp = str[2].Split(',');
                        foreach (string t in temp)
                        {
                            dTableOut1.Columns.Add(t.Trim(), typeof(string));
                        }

                        /*
                        if ((IsAusPOI) && (csvfilelines > 65000)) //for Mxx new AUS POI
                        {
                            if (IsCHNPOI == false)
                                IsCHNPOI = true;
                        }
                        */                       

                        for (i = 1; i <= csvfilelines; i++)
                        {
                            string[] tt = str[2+i].Split(','); 
                            //dTableOut1.Rows.Add(tt); //UID, Y, X, BEARING_ANGLE, SPEED_LIMIT, DIRECTION_TYPE, CAMERA_TYPE
                                                       
                            //if (IsAusPOI)
                            {                                
                                if (shiftAUS)
                                    tt[1] = (double.Parse(tt[1]) + 90.0).ToString();    // for AUS
                                if (shiftUSA)
                                    tt[2] = (double.Parse(tt[2]) + 180.0).ToString();     // for USA
                            }

                            if (tt.Length <= 7)
                            {
                                for (int k = 1; k < 3; k++)  // 20140221, for X, Y
                                {                                 
                                    if (((double.Parse(tt[k])) < 10.0) && ((double.Parse(tt[k])) > 0.0))
                                        tt[k] = "00" + tt[k];
                                    else if (((double.Parse(tt[k])) < 100.0) && ((double.Parse(tt[k])) > 10.0))
                                        tt[k] = '0' + tt[k];
                                }

                                // comment for 20140221
                                //dTableOut1.Rows.Add(tt[0], double.Parse(tt[1]), double.Parse(tt[2]), double.Parse(tt[3]), int.Parse(tt[4]), int.Parse(tt[5]), int.Parse(tt[6]));
                                dTableOut1.Rows.Add(tt[0], tt[1], tt[2], tt[3], tt[4], tt[5], tt[6]);
                            }
                            else if (tt.Length == 8)
                            {
                                for (int k = 1; k < 3; k++) //20140221, for X, Y
                                {
                                    if (((double.Parse(tt[k])) < 10.0) && ((double.Parse(tt[k])) > 0.0))
                                        tt[k] = "00" + tt[k];
                                    else if (((double.Parse(tt[k])) < 100.0) && ((double.Parse(tt[k])) > 10.0))
                                        tt[k] = '0' + tt[k];
                                }                                

                                dTableOut1.Rows.Add(tt[0], double.Parse(tt[1]), double.Parse(tt[2]), double.Parse(tt[3]), int.Parse(tt[4]), int.Parse(tt[5]), int.Parse(tt[6]), int.Parse(tt[7]));
                            }
                            else if (tt.Length > 8)
                            {
                                for (int k = 1; k < 3; k++) //20140221, for X, Y
                                {
                                    if (((double.Parse(tt[k])) < 10.0) && ((double.Parse(tt[k])) > 0.0))
                                        tt[k] = "00" + tt[k];
                                    else if (((double.Parse(tt[k])) < 100.0) && ((double.Parse(tt[k])) > 10.0))
                                        tt[k] = '0' + tt[k];
                                }                                

                                dTableOut1.Rows.Add(tt[0], double.Parse(tt[1]), double.Parse(tt[2]), double.Parse(tt[3]), int.Parse(tt[4]), int.Parse(tt[5]), int.Parse(tt[6]), int.Parse(tt[7]), int.Parse(tt[8]));
                            }
                           
                        }

                        if (dirtypes == null)
                            dirtypes = "0,1,2".Split(',');
                        if (cameratypes == null) 
						{
							if (dataversion == 4)
							{
								cameratypes = "1,2,3,4,5,6,7,8,9".Split(',');
							}
							else {
                            	cameratypes = "1,2,3,4,5".Split(',');
							}
						}

                        dTableOut1.DefaultView.Sort = dTableOut1.Columns[2].ColumnName + ", " + dTableOut1.Columns[1].ColumnName;   //sort by X                 

                        DGV_excel.DataSource = dTableOut1.DefaultView;
                        DGV_excel.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

                        //dTableOut1.DefaultView.Sort = dTableOut1.Columns[2].ColumnName + ", " + dTableOut1.Columns[1].ColumnName;   //sort by X                 
                       // DGV_excel.DataSource = dTableOut1.DefaultView;
                        //DGV_excel.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    }
                    else if (excelfile.IndexOf(".xls", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        //if (excelfile.IndexOf("CHN", StringComparison.OrdinalIgnoreCase) >= 0)
                        //if (excelfile.IndexOf("CHN", StringComparison.Ordinal) >= 0)
                        if ((tmpexcelfilename.IndexOf("CHN", StringComparison.Ordinal) >= 0) ||
                            (cB_subarea.Checked) )
                        {
                            IsCHNPOI = true;
                            IsWiCHNPOI = true;
                            doubleorfloat = false;
                        }

                        ExcelConnection.Open();

                        // if sheet name is not data, use the following codes:
                        //DataTable sheet = ExcelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new Object[] { null, null, null, "TABLE" });
                        //string sheetname = sheet.Rows[2].ItemArray[2].ToString();      // note$
                        //sheet.Dispose();
                        //string OutputSheet = "Select * From [" + sheetname + "]";
                        string OutputSheet = "Select * From [note$]";
                        OleDbCommand ExCommandOut = new OleDbCommand(OutputSheet, ExcelConnection);
                        OleDbDataAdapter dataAdapterOut = new OleDbDataAdapter(ExCommandOut);
                        dataAdapterOut.Fill(dTableOut);
                        DGV_excel.DataSource = dTableOut.DefaultView;
                        mioversion = uint.Parse(DGV_excel.Rows[998].Cells[5].Value.ToString());  // note$, [1000, F]: MIO version                 
                        //dirtypes = DGV_excel.Rows[999].Cells[5].Value.ToString().Split(',');             // note$, [1001, F]: DIR_TYPE, cancelled on 20140317
                        cameratypes = DGV_excel.Rows[999].Cells[5].Value.ToString().Split(',');       // note$, [1001, F]: CAMERA_TYPE

                        if (dirtypes == null)
                            dirtypes = "0,1,2".Split(',');

                        DGV_excel.DataSource = null;
                        DGV_excel.Rows.Clear();
                        DGV_excel.Columns.Clear();
                        dTableOut.Clear();
                        dTableOut.Dispose();
                        dataAdapterOut.Dispose();
                        ExCommandOut.Dispose();
                        //ExcelConnection.Close();
                        //ExcelConnection.Dispose();

                        //----------------------------------------------------------//
                        //string OutputSheet = "Select * From [data$]";
                        //OleDbCommand ExCommandOut = new OleDbCommand(OutputSheet, ExcelConnection);
                        //OleDbDataAdapter dataAdapterOut = new OleDbDataAdapter(ExCommandOut);

                        //ExcelConnection = new OleDbConnection(OpenExcelData);            
                        //ExcelConnection.Open();
                        OutputSheet = "Select * From [data$]";
                        OleDbCommand ExCommandOut1 = new OleDbCommand(OutputSheet, ExcelConnection);
                        OleDbDataAdapter dataAdapterOut1 = new OleDbDataAdapter(ExCommandOut1);
                        //DataTable dTableOut1 = new DataTable();

                        dataAdapterOut1.Fill(dTableOut1);
                        dTableOut1.DefaultView.Sort = dTableOut1.Columns[2].ColumnName + ", " + dTableOut1.Columns[1].ColumnName;   //sort by X                 
                        DGV_excel.DataSource = dTableOut1.DefaultView;
                        DGV_excel.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                        //dTableOut.Dispose();
                        dataAdapterOut1.Dispose();
                        ExCommandOut1.Dispose();
                        ExcelConnection.Close();
                        ExcelConnection.Dispose();
                    }

                    if (!IsCHNPOI)
                    {
                        doubleorfloat = true;
                    }

                    //------------------------------------------------------------------------------------------//
                    // Verify DGV_excel to filter out empty rows
                    if (DGV_excel.RowCount > 0)
                    {
                        i = 0;
                        while (i < DGV_excel.RowCount)
                        {
                            //if (DGV_excel.Rows[i].Cells[1].Value == null)
                            if ((DGV_excel.Rows[i].Cells[1].Value.ToString() == string.Empty) &&
                                (DGV_excel.Rows[i].Cells[2].Value.ToString() == string.Empty))
                            {
                                DGV_excel.Rows.RemoveAt(i);
                                i = -1;
                            }
                            else
                                break;
                            i++;
                        }
                    }

                    if (DGV_excel.RowCount > CHNmapthreshold)
                    {
                        //IsCHNPOI = true;
                        IsWiCHNPOI = true;
                        //if CHN POI, use float datatype; otherwise, use double datatype
                        doubleorfloat = false;
                        dataversion = 3;
                    }

                    if (cB_autodoria.Checked)
                        dataversion = 4;

                    // check if there is any cell.x or cell.y < 0                    
                    latsmallzero = false; lonsmallzero = false;
                    for (i = 0; i < DGV_excel.RowCount; i++)
                    {
                        if (DGV_excel.Rows[i].Cells[1].Value != null)
                            if (double.Parse(DGV_excel.Rows[i].Cells[1].Value.ToString()) < 0)
                                latsmallzero = true;

                        if (DGV_excel.Rows[i].Cells[2].Value != null)
                            if (double.Parse(DGV_excel.Rows[i].Cells[2].Value.ToString()) < 0)
                                lonsmallzero = true;

                        //valuechecking(i);
                    }
                    
                    // if there is a cell.x or cell.y < 0, shift to positive value
                    // stop shift to positive value                    
                    /*if (latsmallzero || lonsmallzero)                    
                    {
                        for (i = 0; i < DGV_excel.RowCount; i++)
                        {
                            DGV_excel.Rows[i].Cells[1].Value = (double.Parse(DGV_excel.Rows[i].Cells[1].Value.ToString()) + 90.0).ToString();
                            DGV_excel.Rows[i].Cells[2].Value = (double.Parse(DGV_excel.Rows[i].Cells[2].Value.ToString()) + 180.0).ToString();
                        }
                    }
                    */
                    // sort cell                
                    DGV_excel.Sort(DGV_excel.Columns[2], ListSortDirection.Ascending);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("File read fail!\n" + ex.Message, "Data handle fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (ExcelConnection != null)
                        ExcelConnection.Close();
                }

                //transform GPS lat coordinate
                // if (shiftAUS)
                if ((shiftAUS) && (!(excelfile.IndexOf(".xls", StringComparison.OrdinalIgnoreCase) >= 0)))
                {
                    for (i = 0; i < DGV_excel.RowCount; i++)
                    {
                        // simultaneously handle two celss would cause memory usage burst
                        // workaround: handle one cell a time
                        // for AUS
                        tran_value = Math.Round(double.Parse(DGV_excel.Rows[i].Cells[1].Value.ToString()) - 90.0, 6);
                        DGV_excel.Rows[i].Cells[1].Value = tran_value.ToString();

                        //tran_value = Math.Round(double.Parse(DGV_excel.Rows[i].Cells[2].Value.ToString()) - 180.0, 6);
                        //DGV_excel.Rows[i].Cells[2].Value = tran_value.ToString();        
                    }
                }

                //transform GPS lon coordinate
                //if (shiftUSA)
                if ((shiftUSA) && (!(excelfile.IndexOf(".xls", StringComparison.OrdinalIgnoreCase) >= 0)))
                {
                    for (i = 0; i < DGV_excel.RowCount; i++)
                    {
                        // simultaneously handle two celss would cause memory usage burst
                        // workaround: handle one cell a time
                        // for AUS
                        tran_value = Math.Round(double.Parse(DGV_excel.Rows[i].Cells[2].Value.ToString()) - 180.0, 6);
                        DGV_excel.Rows[i].Cells[2].Value = tran_value.ToString();

                        /*
                        DGV_excel.Rows[i].Cells[1].Value = (double.Parse(DGV_excel.Rows[i].Cells[1].Value.ToString()) - 90.0).ToString();
                        DGV_excel.Rows[i].Cells[2].Value = (double.Parse(DGV_excel.Rows[i].Cells[2].Value.ToString()) - 180.0).ToString();
                        */
                    }
                }

                /*  debug
                double ttcamy, ttcamx;               
                for (i = 0; i < DGV_excel.RowCount; i++)
                {
                    ttcamy = double.Parse(DGV_excel.Rows[i].Cells[1].Value.ToString());
                    ttcamx = double.Parse(DGV_excel.Rows[i].Cells[2].Value.ToString());
                    if ((ttcamy == 31.988021) && (ttcamx == 120.24699))
                    {
                        ttcamx = ttcamx;
                        ttcamy = ttcamy;
                    }
                }
                */
                //-----------------------------------------------------------------------------
                if (dTableOut1.Rows.Count > 0)
                {
                    for (i = 0; i < DGV_excel.RowCount; i++)
                    {
                        if (double.Parse(DGV_excel.Rows[i].Cells[1].Value.ToString()) < tmp_latstart)
                            tmp_latstart = double.Parse(DGV_excel.Rows[i].Cells[1].Value.ToString());
                        if (double.Parse(DGV_excel.Rows[i].Cells[1].Value.ToString()) > tmp_latend)
                            tmp_latend = double.Parse(DGV_excel.Rows[i].Cells[1].Value.ToString());

                        if (double.Parse(DGV_excel.Rows[i].Cells[2].Value.ToString()) < tmp_lonstart)
                            tmp_lonstart = double.Parse(DGV_excel.Rows[i].Cells[2].Value.ToString());
                        if (double.Parse(DGV_excel.Rows[i].Cells[2].Value.ToString()) > tmp_lonend)
                            tmp_lonend = double.Parse(DGV_excel.Rows[i].Cells[2].Value.ToString());
                    }

                    btn_start.Enabled = true;
                    tB_LayerWidth.Enabled = true;
                    tB_LonLayerWidth.Enabled = true;

                    tB_BlockCount.Enabled = true;
                    tB_BlockCount.Text = "15";  //10
                    blockcount = 15;                 // Lat: Y
                    tB_LonBlockCount.Enabled = true;
                    tB_LonBlockCount.Text = "20";    //10
                    lonblockcount = 20;              // Lon: X

                    tB_SmallLayerWidth.Enabled = true;
                    tB_LonSmallLayerWidth.Enabled = true;

                    tB_SmallBlockCount.Enabled = true;
                    tB_SmallBlockCount.Text = "30";           // 3
                    smalllayerblockcount = 30;
                    tB_LonSmallBlockCount.Enabled = true;
                    tB_LonSmallBlockCount.Text = "30";    // 3
                    lonsmalllayerblockcount = 30;

                    tB_lonstart.Enabled = true;
                    //tB_lonstart.Text = DGV_excel.Rows[0].Cells[2].Value.ToString();
                    //lonstart = double.Parse(tB_lonstart.Text);
                    //lonstart = double.Parse(DGV_excel.Rows[0].Cells[2].Value.ToString());
                    lonstart = tmp_lonstart;
                    lonstart = Math.Floor(lonstart);
                    tB_lonstart.Text = lonstart.ToString();

                    tB_lonend.Enabled = true;
                    //tB_lonend.Text = DGV_excel.Rows[DGV_excel.RowCount - 1].Cells[2].Value.ToString();
                    //lonend = double.Parse(tB_lonend.Text);
                    //lonend = double.Parse(DGV_excel.Rows[DGV_excel.RowCount - 1].Cells[2].Value.ToString());
                    lonend = tmp_lonend;
                    lonend = Math.Ceiling(lonend);
                    tB_lonend.Text = lonend.ToString();

                    tB_latstart.Enabled = true;
                    //tB_latstart.Text = tmp_latstart.ToString();                    
                    tmp_latstart = Math.Floor(tmp_latstart);
                    latstart = tmp_latstart;
                    tB_latstart.Text = tmp_latstart.ToString();

                    tB_latend.Enabled = true;
                    //tB_latend.Text = tmp_latend.ToString();                                        
                    tmp_latend = Math.Ceiling(tmp_latend);
                    latend = tmp_latend;
                    tB_latend.Text = tmp_latend.ToString();

                    tB_version.Enabled = true;
                    //tB_version.Text = "1.0";                    
                    //dataversion = double.Parse(tB_version.Text);
                    tB_version.Text = mioversion.ToString();
                    btn_clear.Enabled = true;

                    if ((dynamicslicing) && (groupBox1.Enabled))
                    {
                        //cbB_lonlat.Enabled = true;
                        tB_lonslicingstart.Enabled = true;
                        tB_lonslicingend.Enabled = true;
                        tB_lonslicingnum.Enabled = true;
                        tB_slicingstart.Enabled = true;
                        tB_slicingend.Enabled = true;
                        tB_slicingnum.Enabled = true;
                    }
                }
				                
                int tmpuid = -1;
                int pairidx = -1;
                int idx;                

                if (dataversion == 4)   //support autodoria alert
                {
                    exportdatagridview();
                    for (idx = 0; idx < DGV_excel.RowCount; idx++)
                    {
                        pairidx = int.MaxValue;
                        tmpuid = int.Parse(DGV_excel.Rows[idx].Cells[5].Value.ToString());
                        if ((tmpuid == autodoria_s) || (tmpuid == autodoria_e))
                        {
                            pairidx = findautodoriapair(idx);
                            if (pairidx == -1)
                                pairidx = idx;
                        }
                        else
                            pairidx = idx;

                        autodoriapair_idx.Add(pairidx);
                    }

                    //exportdatagridview();
                }                
            }
            else
            {
                MessageBox.Show("File no exists");
            }
        }

        //--------------------------------------------------------
        public void valuechecking(int rownum)
        {
            bool wrongvalue = false, dirtypenomatch = false, cameratypenomatch = false;
            int i, j;  // counter
            string tmpstr = null;

            for (i = 0; i < DGV_excel.Columns.Count; i++)
            {
                if (DGV_excel.Columns[i].Name == "Y")
                {
                    if ((double.Parse(DGV_excel.Rows[rownum].Cells[i].Value.ToString()) > 89.9) ||
                          (double.Parse(DGV_excel.Rows[rownum].Cells[i].Value.ToString()) < -89.9))
                        wrongvalue = true;
                }
                else if (DGV_excel.Columns[i].Name == "X")
                {
                    if ((double.Parse(DGV_excel.Rows[rownum].Cells[i].Value.ToString()) > 180) ||
                          (double.Parse(DGV_excel.Rows[rownum].Cells[i].Value.ToString()) < -180))
                        wrongvalue = true;
                }
                else if (DGV_excel.Columns[i].Name == "BEARING_ANGLE")
                {
                    if ( ( double.Parse(DGV_excel.Rows[rownum].Cells[i].Value.ToString()) > 359) ||
                          ( double.Parse(DGV_excel.Rows[rownum].Cells[i].Value.ToString()) < 0 ) )
                        wrongvalue = true;
                }
                else if (DGV_excel.Columns[i].Name == "SPEED_RESTRICTION")
                {
                    if ((double.Parse(DGV_excel.Rows[rownum].Cells[i].Value.ToString()) > 180) ||
                          (double.Parse(DGV_excel.Rows[rownum].Cells[i].Value.ToString()) < 0))
                        wrongvalue = true;
                }
                else if (DGV_excel.Columns[i].Name == "CAMERA_TYPE")
                {
                    // CAMERA_TYPE: 1 ~ 7   
                    /*
                    if ((int.Parse(DGV_excel.Rows[rownum].Cells[i].Value.ToString()) > 7) ||
                          (int.Parse(DGV_excel.Rows[rownum].Cells[i].Value.ToString()) < 1))
                        wrongvalue = true;
                    */
                    
                    cameratypenomatch = true;
                    j = 0;
                    while (j < (cameratypes.Length - 1))                    
                    {
                        if (int.Parse(DGV_excel.Rows[rownum].Cells[i].Value.ToString()) == int.Parse(cameratypes[j].ToString()))
                        {
                            cameratypenomatch = false;
                            break;
                        }
                        j++;
                    }
                    if (cameratypenomatch)
                        wrongvalue = true;
                }                    
                else if (DGV_excel.Columns[i].Name == "DIR_TYPE")
                {                   
                    dirtypenomatch = true;
                    j = 0;
                    while ( j < (dirtypes.Length-1) )
                    {
                        if (int.Parse(DGV_excel.Rows[rownum].Cells[i].Value.ToString()) == int.Parse(dirtypes[j].ToString()))
                        {
                            dirtypenomatch = false;
                            break;
                        }
                        j++;
                    }
                    if (dirtypenomatch)
                        wrongvalue = true;
                }
                     
            }

            tmpstr = null;
            for (j = 0; j < DGV_excel.Rows[rownum].Cells.Count; j++)
            {
                if (j < DGV_excel.Rows[rownum].Cells.Count - 2)
                { 
                    //tmpstr += DGV_excel.Rows[rownum].Cells[j].Value.ToString() + "    ";
                    if ( (j == 1) || (j == 2) )
                        tmpstr += double.Parse(DGV_excel.Rows[rownum].Cells[j].Value.ToString()).ToString() + "    ";
                    else
                        tmpstr += DGV_excel.Rows[rownum].Cells[j].Value.ToString() + "    ";
                }
                else
                {
                    tmpstr += DGV_excel.Rows[rownum].Cells[j].Value.ToString();
                    break;
                }
            }

            if (wrongvalue)
            {                
                errorloglist.Add(tmpstr);
            }
            else
            {               
                successloglist.Add(tmpstr);
            }

            wrongvalue = false;
        }

        //----------------------------------------------------------------------------
        public void speedcamconvert()
        {
            // cell[0]: offset; cell[1]: Y(Lat); cell[2]: X(Lon)
            int i = 0, j = 0; // temp counter     
            points = new ArrayList[blockcount * lonblockcount, smalllayerblockcount * lonsmalllayerblockcount]; 
            poioflargelayer = new long[blockcount * lonblockcount];

            int latlargeindex = 0, lonlargeindex = 0;
            int rel_latsmallindex = 0, rel_lonsmallindex = 0;
            int largeindex = 0, smallindex = 0;   // the first one number: 0
            double tmp_Y = 0.0, tmp_X = 0.0;
            double tmp_Ys = 0.0, tmp_Ye = 0.0;
            double tmp_Xs = 0.0, tmp_Xe = 0.0;
            double rel_latstart = 0.0, rel_lonstart = 0.0;

            string[] tmpindex = new string[2];
            string tmppointinfo = null;
            int tmplargelayerindex = 0, tmpsmalllayerindex = 0;

            //-------------- new division version variable ---------------------------------------
            bool IsPOIsuminsmallblocklessthanthreshold = true;  //false: the original division; true: new division based on 20140305
            int largetestroundnum = 20, smalltestroundnum = 27; // large: from 10*10 to 20*20, step over 1; small: from 3*3 to 30*30, step over 1;                                    
            int largeteststart = 4, smallteststart = 4;//3, 3
            int testroundnum = 100;
            int POIsuminsmallblockthreshold = 5000;       //3000
            int tmplatblockcount = 0, tmplonblockcount = 0; 
            int tmplatsmallblockcount = 3, tmplonsmallblockcount = 3;
            double tmplatblockwidth = 0.0, tmplonblockwidth = 0.0, tmplatsmallblockwidth = 0.0, tmplonsmallblockwidth = 0.0;
            bool failedcombination = false;
            bool fallinotherarea = false;
            Fm_evaluatedivision DivisionEvaluation;
            int r = 0, s = 0, t = 0;
            int tmprowindex = 0, tmpcolumnindex = 0;
            long tmpPOIsum = 0, tmpnineblockPOI = 0;

            string[] parseslicingrule;            
            double CHNlatstart = 0.0, CHNlatend = 0.0, CHNlonstart = 0.0, CHNlonend = 0.0;
            int rangestartindex = 0, rangeendindex = 0, rangePOInum = 0, ignorePOInum = 0;
            double latareaoverlap = 0.0, lonareaoverlap = 0.0;
            //string[] perdivisionblock;
            long tmp_POIcontained = 0;
            bool stopupdatepoints = false;

            //--------------------- for 20140421
            //long[,] tmppoicount = new long[12 * 22, 12 * 22];    //20140421
            //ArrayList POIlargerthan3000 = new ArrayList();
            //---------------------------------------------------------------------

            //dTableOut.Dispose();
            dTableOut1.Dispose();
            DGV_excel.Columns.Add("Layers", "Layers");
            layerinfo = DGV_excel.Columns.Count - 1;

            if (IsCHNPOI)
            {
                POIsuminsmallblockthreshold = 5800;                
                //multilevelpoints = new ArrayList[lB_slicingrules.Items.Count, blockcount * lonblockcount, smalllayerblockcount * lonsmalllayerblockcount];                

                DivisionEvaluation = new Fm_evaluatedivision();    //20140324
                DivisionEvaluation.Hide();
                DivisionEvaluation.DGV_verifydivision.RowCount = largetestroundnum * (smalltestroundnum+3);              // 20*30
                DivisionEvaluation.DGV_verifydivision.ColumnCount = largetestroundnum * (smalltestroundnum + 3);      // 20*30
                //DivisionEvaluation.DGV_verifydivision.RowCount = 20 * 30;              
                //DivisionEvaluation.DGV_verifydivision.ColumnCount = 20 * 30;      

                
                if (lB_slicingrules.Items.Count > 0)
                {
                    multilevelpoints = new ArrayList[lB_slicingrules.Items.Count, blockcount * lonblockcount, smalllayerblockcount * lonsmalllayerblockcount];

                    for (r = 0; r < lB_slicingrules.Items.Count; r++)
                    {
                        parseslicingrule = lB_slicingrules.Items[r].ToString().Split(',');
                        CHNlatstart = double.Parse(parseslicingrule[0]);
                        CHNlatend = double.Parse(parseslicingrule[1]);
                        CHNlonstart = double.Parse(parseslicingrule[2]);
                        CHNlonend = double.Parse(parseslicingrule[3]);

                        layerwidth = (double)(CHNlatend - CHNlatstart) / blockcount;
                        lonlayerwidth = (double)(CHNlonend - CHNlonstart) / lonblockcount;
                        smalllayerwidth = (double)layerwidth / smalllayerblockcount;
                        lonsmalllayerwidth = (double)lonlayerwidth / lonsmalllayerblockcount;

                        for (i = 0; i < DGV_excel.RowCount; i++)   // find out range start_index and range end_index
                        {
                            //tmp_Y = double.Parse(DGV_excel.Rows[i].Cells[1].Value.ToString());
                            tmp_X = double.Parse(DGV_excel.Rows[i].Cells[2].Value.ToString());

                            if (tmp_X <= CHNlonstart)
                            {
                                rangestartindex = i + 1;
                            }

                            if (tmp_X <= CHNlonend)
                            {
                                rangeendindex = i;
                            }
                        }

                        rangePOInum = 0;
                        for (i = rangestartindex; i <= rangeendindex; i++)
                        {
                            tmp_Y = double.Parse(DGV_excel.Rows[i].Cells[1].Value.ToString());
                            tmp_X = double.Parse(DGV_excel.Rows[i].Cells[2].Value.ToString());

                            if ((tmp_Y > CHNlatend) || (tmp_Y < CHNlatstart))
                                continue;

                            rangePOInum++;
                            latlargeindex = (int)Math.Floor((tmp_Y - CHNlatstart) / layerwidth);
                            if (latlargeindex == blockcount)
                                latlargeindex = blockcount - 1;
                            lonlargeindex = (int)Math.Floor((tmp_X - CHNlonstart) / lonlayerwidth);
                            if (lonlargeindex == lonblockcount)
                                lonlargeindex = lonblockcount - 1;
                            largeindex = latlargeindex * lonblockcount + lonlargeindex;

                            rel_latstart = CHNlatstart + latlargeindex * layerwidth;
                            rel_lonstart = CHNlonstart + lonlargeindex * lonlayerwidth;
                            rel_latsmallindex = (int)Math.Floor((tmp_Y - rel_latstart) / smalllayerwidth);
                            if (rel_latsmallindex == smalllayerblockcount)
                                rel_latsmallindex = smalllayerblockcount - 1;
                            rel_lonsmallindex = (int)Math.Floor((tmp_X - rel_lonstart) / lonsmalllayerwidth);
                            if (rel_lonsmallindex == lonsmalllayerblockcount)
                                rel_lonsmallindex = lonsmalllayerblockcount - 1;
                            smallindex = rel_latsmallindex * lonsmalllayerblockcount + rel_lonsmallindex;

                            //DGV_excel.Rows[i].Cells[0].Value = largeindex.ToString() + "," + smallindex.ToString(); 20131213
                            DGV_excel.Rows[i].Cells[layerinfo].Value = largeindex.ToString() + "," + smallindex.ToString();
                        }

                        // save each datagridview item into points[]
                        //MessageBox.Show("Total points: " + DGV_excel.RowCount.ToString());

                        for (i = 0; i < blockcount * lonblockcount; i++)
                        {
                            for (j = 0; j < smalllayerblockcount * lonsmalllayerblockcount; j++)
                                //points[i, j] = new ArrayList();
                                multilevelpoints[r, i, j] = new ArrayList();
                        }

                        for (i = rangestartindex; i <= rangeendindex; i++)
                        {
                            //tmpindex = DGV_excel.Rows[i].Cells[0].Value.ToString().Split(','); //layer: (large, small), 20131213

                            if ((DGV_excel.Rows[i].Cells[layerinfo].Value == null) || (DGV_excel.Rows[i].Cells[layerinfo].Value.ToString().Length == 0))
                                continue;

                            tmpindex = DGV_excel.Rows[i].Cells[layerinfo].Value.ToString().Split(','); //layer: (large, small)

                            tmplargelayerindex = int.Parse(tmpindex[0]);    // large layer index
                            tmpsmalllayerindex = int.Parse(tmpindex[1]);    // small layer index
                            tmppointinfo = i.ToString();
                            if ((tmplargelayerindex > -1) && (tmpsmalllayerindex > -1))
                            {
                                //points[tmplargelayerindex, tmpsmalllayerindex].Add(tmppointinfo);
                                multilevelpoints[r, tmplargelayerindex, tmpsmalllayerindex].Add(tmppointinfo);
                            }
                        }   // end of for                     

                        for (i = 0; i < DGV_excel.RowCount; i++)
                        {
                            if (DGV_excel.Rows[i].Cells[layerinfo].Value != null)
                                DGV_excel.Rows[i].Cells[layerinfo].Value = "";
                        }

                        rangestartindex = 0;
                        rangeendindex = 0;

                    }  // end of for (r = 0; r < lB_slicingrules.Items.Count; r++)
                }                

                if (lB_nestedslicingrules.Items.Count > 0)                
                {
                    multilevelpoints = new ArrayList[(lB_nestedslicingrules.Items.Count+1), blockcount * lonblockcount, smalllayerblockcount * lonsmalllayerblockcount];
                    ignoreslicingrule = new ArrayList[lB_nestedslicingrules.Items.Count];
                    divisionblockcount = new ArrayList[lB_nestedslicingrules.Items.Count+1];
                    //tmppoints = new ArrayList[tmplatblockcount * tmplonblockcount, tmplatsmallblockcount * tmplonsmallblockcount];
                    //DivisionEvaluation = new Fm_evaluatedivision(); //20140324
                    //DivisionEvaluation.Hide();

                    // --------------------------------- Inner Area ---------------------------------------------------//                    
                    for (r = 0; r < lB_nestedslicingrules.Items.Count; r++) 
                    {
                        parseslicingrule = lB_nestedslicingrules.Items[r].ToString().Split(',');
                        ignoreslicingrule[r] = new ArrayList();
                        divisionblockcount[r] = new ArrayList();

                        for (i = 0; i < parseslicingrule.Length; i++)
                            ignoreslicingrule[r].Add(parseslicingrule[i].ToString().Trim());

                        CHNlatstart = double.Parse(parseslicingrule[0]);
                        CHNlatend = double.Parse(parseslicingrule[1]);
                        CHNlonstart = double.Parse(parseslicingrule[2]);
                        CHNlonend = double.Parse(parseslicingrule[3]);

                        failedcombination = false;
                        //DivisionEvaluation = new Fm_evaluatedivision();
                        //DivisionEvaluation.Hide();

                        //for (t = largeteststart; t <= largetestroundnum; t++)
                        //for (t = largeteststart; t <= 9; t++) //DigiLife most 3
                        //for (t = largeteststart; t <= 11; t++)   //20140421
                        for (t = largeteststart; t <= 9; t++)   //20160324
                        {
                            tB_BlockCount.Text = t.ToString();
                            tB_LonBlockCount.Text = t.ToString();
                            tmplatblockcount = t;
                            tmplonblockcount = t;
                            tmplonblockwidth = (double)(CHNlonend - CHNlonstart) / tmplonblockcount;
                            tmplatblockwidth = (double)(CHNlatend - CHNlatstart) / tmplatblockcount;

                            //for (s = smallteststart; s <= smalltestroundnum; s++)
                            //for (s = smallteststart; s <= 9; s++)   //DigiLife < 12
                            for (s = smallteststart; s <= 9; s++)   //MIO AUS
                            {
                                tmplatsmallblockcount = s;
                                tmplonsmallblockcount = s;
                                tB_SmallBlockCount.Text = tmplatsmallblockcount.ToString();
                                tB_LonSmallBlockCount.Text = tmplonsmallblockcount.ToString();
                                tmplonsmallblockwidth = (double)tmplonblockwidth / tmplonsmallblockcount;
                                tmplatsmallblockwidth = (double)tmplatblockwidth / tmplatsmallblockcount;

                                tB_BlockCount.Refresh();
                                tB_LonBlockCount.Refresh();
                                tB_SmallBlockCount.Refresh();
                                tB_LonSmallBlockCount.Refresh();

                                tmppoints = new ArrayList[tmplatblockcount * tmplonblockcount, tmplatsmallblockcount * tmplonsmallblockcount];
                                for ( i = 0; i < tmppoints.GetLength(0); i++)
                                //for (i = 0; i < tmplatblockcount * tmplonblockcount; i++)
                                {
                                    for (j = 0; j < tmppoints.GetLength(1); j++)
                                    //for (j = 0; j < tmplatsmallblockcount * tmplonsmallblockcount; j++)
                                        if (tmppoints[i, j] == null)
                                            tmppoints[i, j] = new ArrayList();
                                        else if ((tmppoints[i, j] != null) && (tmppoints[i, j].Count > 0))
                                            tmppoints[i, j].Clear();
                                }

                                // --------------------------- find out range -----------------------------------------------------------//
                                for (i = 0; i < DGV_excel.RowCount; i++)   // find out range start_index and range end_index
                                {
                                    //tmp_Y = double.Parse(DGV_excel.Rows[i].Cells[1].Value.ToString());
                                    tmp_X = double.Parse(DGV_excel.Rows[i].Cells[2].Value.ToString());

                                    if (tmp_X <= CHNlonstart)
                                    {
                                        rangestartindex = i + 1;
                                    }

                                    if (tmp_X <= CHNlonend)
                                    {
                                        rangeendindex = i;
                                    }
                                }

                                rangePOInum = 0;
                                for (i = rangestartindex; i <= rangeendindex; i++)
                                {
                                    tmp_Y = double.Parse(DGV_excel.Rows[i].Cells[1].Value.ToString());
                                    tmp_X = double.Parse(DGV_excel.Rows[i].Cells[2].Value.ToString());
                                    //if ((tmp_Y > (CHNlatend+areaoverlap)) || (tmp_Y < (CHNlatstart-areaoverlap)))
                                    if ((tmp_Y > CHNlatend) || (tmp_Y < CHNlatstart))
                                        continue;

                                    rangePOInum++;
                                    latlargeindex = (int)Math.Floor((tmp_Y - CHNlatstart) / tmplatblockwidth);
                                    if (latlargeindex == tmplatblockcount)
                                        latlargeindex = tmplatblockcount - 1;
                                    lonlargeindex = (int)Math.Floor((tmp_X - CHNlonstart) / tmplonblockwidth);
                                    if (lonlargeindex == tmplonblockcount)
                                        lonlargeindex = tmplonblockcount - 1;
                                    largeindex = latlargeindex * tmplonblockcount + lonlargeindex;

                                    rel_latstart = CHNlatstart + latlargeindex * tmplatblockwidth;
                                    rel_lonstart = CHNlonstart + lonlargeindex * tmplonblockwidth;
                                    rel_latsmallindex = (int)Math.Floor((tmp_Y - rel_latstart) / tmplatsmallblockwidth);
                                    if (rel_latsmallindex == tmplatsmallblockcount)
                                        rel_latsmallindex = tmplatsmallblockcount - 1;
                                    rel_lonsmallindex = (int)Math.Floor((tmp_X - rel_lonstart) / tmplonsmallblockwidth);
                                    if (rel_lonsmallindex == tmplonsmallblockcount)
                                        rel_lonsmallindex = tmplonsmallblockcount - 1;
                                    smallindex = rel_latsmallindex * tmplonsmallblockcount + rel_lonsmallindex;

                                    //DGV_excel.Rows[i].Cells[0].Value = largeindex.ToString() + "," + smallindex.ToString(); 20131213
                                    DGV_excel.Rows[i].Cells[layerinfo].Value = largeindex.ToString() + "," + smallindex.ToString();                                    
                                }

                                // save each datagridview item into points[]   
                                for (i = rangestartindex; i <= rangeendindex; i++)
                                {
                                    //tmpindex = DGV_excel.Rows[i].Cells[0].Value.ToString().Split(','); //layer: (large, small), 20131213

                                    if ((DGV_excel.Rows[i].Cells[layerinfo].Value == null) || (DGV_excel.Rows[i].Cells[layerinfo].Value.ToString().Length == 0))
                                        continue;

                                    tmpindex = DGV_excel.Rows[i].Cells[layerinfo].Value.ToString().Split(','); //layer: (large, small)

                                    tmplargelayerindex = int.Parse(tmpindex[0]);    // large layer index
                                    tmpsmalllayerindex = int.Parse(tmpindex[1]);    // small layer index                                    

                                    tmppointinfo = i.ToString();
                                    if ((tmplargelayerindex > -1) && (tmpsmalllayerindex > -1))
                                    {
                                        //points[tmplargelayerindex, tmpsmalllayerindex].Add(tmppointinfo);
                                        tmppoints[tmplargelayerindex, tmpsmalllayerindex].Add(tmppointinfo);
                                    }
                                }   // end of for                     

                                //--------------- Verify if this combination is okay -----------------------//
                                /*
                                if (DivisionEvaluation.DGV_verifydivision.RowCount > 1)
                                {
                                    DivisionEvaluation.DGV_verifydivision.RowCount = 1;
                                    DivisionEvaluation.DGV_verifydivision.Columns.Clear();
                                    DivisionEvaluation.DGV_verifydivision.Rows.Clear();
                                }                                

                                DivisionEvaluation.DGV_verifydivision.RowCount = tmplatblockcount * tmplatsmallblockcount;
                                DivisionEvaluation.DGV_verifydivision.ColumnCount = tmplonblockcount * tmplonsmallblockcount;
                                */
                                for (i = 0; i < DivisionEvaluation.DGV_verifydivision.RowCount; i++)
                                    for (j = 0; j < DivisionEvaluation.DGV_verifydivision.ColumnCount; j++)
                                        //if (DivisionEvaluation.DGV_verifydivision.Rows[i].Cells[j].Value.ToString().Length > 0)
                                            DivisionEvaluation.DGV_verifydivision.Rows[i].Cells[j].Value = "";


                                for (i = 0; i < tmppoints.GetLength(0); i++)
                                    for (j = 0; j < tmppoints.GetLength(1); j++)
                                    {
                                        tmprowindex = (i / tmplonblockcount) * tmplatsmallblockcount + (j / tmplonsmallblockcount);
                                        tmpcolumnindex = (i % tmplonblockcount) * tmplonsmallblockcount + (j % tmplonsmallblockcount);

                                        DivisionEvaluation.DGV_verifydivision.Rows[(tmplatblockcount * tmplatsmallblockcount - 1) - tmprowindex].Cells[tmpcolumnindex].Value = tmppoints[i, j].Count.ToString();
                                    }

                                failedcombination = false;
                                for (i = 1; i < (tmplatblockcount * tmplatsmallblockcount - 1); i ++) //, 20140421
                                {
                                    for (j = 1; j < (tmplonblockcount * tmplonsmallblockcount - 1); j++)
                                    {
                                        tmpnineblockPOI = (long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i - 1].Cells[j - 1].Value.ToString()) +
                                             long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i].Cells[j - 1].Value.ToString()) +
                                             long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i + 1].Cells[j - 1].Value.ToString()) +

                                            long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i - 1].Cells[j].Value.ToString()) +
                                             long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i].Cells[j].Value.ToString()) +
                                             long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i + 1].Cells[j].Value.ToString()) +

                                            long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i - 1].Cells[j + 1].Value.ToString()) +
                                             long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i].Cells[j + 1].Value.ToString()) +
                                             long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i + 1].Cells[j + 1].Value.ToString()));

                                        if (tmpnineblockPOI >= POIsuminsmallblockthreshold)
                                        {
                                            failedcombination = true;
                                            break;
                                        }
                                    }

                                    if (failedcombination)
                                        break;
                                }

                                if (!failedcombination)
                                {                                    
                                    divisionblockcount[r].Add(t.ToString() + "," + s.ToString() );                                    
                                }

                                tmpPOIsum = 0;
                                /*
                                for (i = 0; i < DivisionEvaluation.DGV_verifydivision.RowCount; i++)
                                    for (j = 0; j < DivisionEvaluation.DGV_verifydivision.ColumnCount; j++)
                                        tmpPOIsum += long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i].Cells[j].Value.ToString());
                                */
                                for (i = 0; i < tmplatblockcount * tmplatsmallblockcount; i++)
                                    for (j = 0; j < tmplonblockcount * tmplonsmallblockcount; j++)
                                        tmpPOIsum += long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i].Cells[j].Value.ToString());


                                if (tmpPOIsum != rangePOInum)
                                    MessageBox.Show("Wrong Division!!!");

                                if ((!failedcombination) && (divisionblockcount[r][0].ToString().Length > 0) && (multilevelpoints[r, 0, 0] == null))
                                {
                                    for (i = 0; i < tmppoints.GetLength(0); i++)
                                        for (j = 0; j < tmppoints.GetLength(1); j++)
                                        {
                                            if (multilevelpoints[r, i, j] == null)
                                                multilevelpoints[r, i, j] = new ArrayList();

                                            for (int d = 0; d < tmppoints[i, j].Count; d++)
                                            {
                                                multilevelpoints[r, i, j].Add(tmppoints[i, j][d].ToString());
                                            }  
                                        }

                                }

                                for (i = 0; i < tmppoints.GetLength(0); i++)
                                    for (j = 0; j < tmppoints.GetLength(1); j++)
                                    {
                                        tmppoints[i, j].Clear();
                                        tmppoints[i, j] = null;
                                    }

                                //-----------------------------------------------------------------------
                                rangestartindex = 0;
                                rangeendindex = 0;

                                for (i = 0; i < DGV_excel.RowCount; i++)
                                {
                                    if (DGV_excel.Rows[i].Cells[layerinfo].Value != null)
                                        DGV_excel.Rows[i].Cells[layerinfo].Value = "";
                                }

                                if (!failedcombination)  // exit the loop of s
                                    break;
                            }
                        }   // end of for (largetestroundnum)

                        tmpPOIsum = 0;
                        for (i = 0; i < multilevelpoints.GetLength(1); i++)
                            for (j = 0; j < multilevelpoints.GetLength(2); j++)
                                if ( (multilevelpoints[r, i, j] != null) && (multilevelpoints[r, i, j].Count > 0) )
                                    tmpPOIsum += multilevelpoints[r, i, j].Count;

                        if (tmpPOIsum != rangePOInum)
                            MessageBox.Show("Wrong Division: Large: " + t.ToString() + "; Small: " + s.ToString() + "   !!!");


                        //divisionblockcount[r][0].ToString().Length > 0, 20140331
                        if ((divisionblockcount[r][0] != null) && (divisionblockcount[r][0].ToString().Length > 0))
                            continue;


                    }  // end of for (r = 0; r < lB_nestedslicingrules.Items.Count; r++)
                    // -------------------------------------- End of  Inner Area -------------------------------------//                    
                                                  
                    // -------------------------------------- Start of Outest Area -----------------------------------//
                    
                    CHNlatstart = latstart;
                    CHNlatend = latend;
                    CHNlonstart = lonstart;
                    CHNlonend = lonend;
                    r = lB_nestedslicingrules.Items.Count;
                    divisionblockcount[r] = new ArrayList();

                    for (i = 0; i < DGV_excel.RowCount; i++)
                    {
                        if (DGV_excel.Rows[i].Cells[layerinfo].Value != null)
                            DGV_excel.Rows[i].Cells[layerinfo].Value = "";
                    }

                    failedcombination = false;
                    //for (t = 3; t <= 12; t++)                    // for other POI
                    //for (t = 7; t <= 12; t++)                    // for DigiLife CHN                     
                    for (t = 10; t <= 19; t++)                    // for CHN, 20140421
                    {
                        tB_BlockCount.Text = t.ToString();
                        tB_LonBlockCount.Text = t.ToString();
                        tmplatblockcount = t;
                        tmplonblockcount = t;
                        tmplonblockwidth = (double)(CHNlonend - CHNlonstart) / tmplonblockcount;
                        tmplatblockwidth = (double)(CHNlatend - CHNlatstart) / tmplatblockcount;

                        //for (s = smallteststart; s <= 30; s++)  // for others
                        //for (s = 7; s <= 13; s++)    // for DigiLife CHN                         
                        for (s = 10; s <= 29; s++)    // for CHN, 20140421
                        {
                            tmplatsmallblockcount = s;
                            tmplonsmallblockcount = s;
                            tB_SmallBlockCount.Text = tmplatsmallblockcount.ToString();
                            tB_LonSmallBlockCount.Text = tmplonsmallblockcount.ToString();
                            tmplonsmallblockwidth = (double)tmplonblockwidth / tmplonsmallblockcount;
                            tmplatsmallblockwidth = (double)tmplatblockwidth / tmplatsmallblockcount;

                            tB_BlockCount.Refresh();
                            tB_LonBlockCount.Refresh();
                            tB_SmallBlockCount.Refresh();
                            tB_LonSmallBlockCount.Refresh();                                                       

                            tmppoints = new ArrayList[tmplatblockcount * tmplonblockcount, tmplatsmallblockcount * tmplonsmallblockcount];
                            for (i = 0; i < tmplatblockcount * tmplonblockcount; i++)
                            {
                                for (j = 0; j < tmplatsmallblockcount * tmplonsmallblockcount; j++)
                                    if (tmppoints[i, j] == null)
                                        tmppoints[i, j] = new ArrayList();
                                    else if ((tmppoints[i, j] != null) && (tmppoints[i, j].Count > 0))
                                        tmppoints[i, j].Clear();
                            }

                            // --------------------------- find out range -----------------------------------------------------------//                           
                            rangestartindex = 0;
                            rangeendindex = DGV_excel.RowCount - 1;

                            rangePOInum = 0;
                            ignorePOInum = 0;
                            for (i = rangestartindex; i <= rangeendindex; i++)
                            {
                                tmp_Y = double.Parse(DGV_excel.Rows[i].Cells[1].Value.ToString());
                                tmp_X = double.Parse(DGV_excel.Rows[i].Cells[2].Value.ToString());

                                // ------------ skip the POIs located inside lB_nestedslicingrules.Items[] --------------------------//
                                fallinotherarea = false;
                                for (int d = 0; d < lB_nestedslicingrules.Items.Count; d++)
                                {      
                                    /*
                                    perdivisionblock = divisionblockcount[d][0].ToString().Split(',');
                                    lonareaoverlap = (double)(double.Parse(ignoreslicingrule[d][1].ToString().Trim()) - double.Parse(ignoreslicingrule[d][0].ToString().Trim())) / 
                                                                   (int.Parse(perdivisionblock[0].ToString()) * int.Parse(perdivisionblock[1].ToString()) );
                                    latareaoverlap = (double)(double.Parse(ignoreslicingrule[d][3].ToString().Trim()) - double.Parse(ignoreslicingrule[d][2].ToString().Trim())) /
                                                                   (int.Parse(perdivisionblock[0].ToString()) * int.Parse(perdivisionblock[1].ToString()));
                                    */
                                    //lonareaoverlap = 0.03;
                                    lonareaoverlap = 0.03; //20140421
                                    //latareaoverlap = 0.06;
                                    latareaoverlap = 0.015;    //20140421

                                    tmp_Ys = double.Parse(ignoreslicingrule[d][0].ToString().Trim());
                                    tmp_Ye = double.Parse(ignoreslicingrule[d][1].ToString().Trim());
                                    tmp_Xs = double.Parse(ignoreslicingrule[d][2].ToString().Trim());
                                    tmp_Xe = double.Parse(ignoreslicingrule[d][3].ToString().Trim());

                                    //if ((double.Parse(ignoreslicingrule[d][0].ToString().Trim()) != latstart) && (double.Parse(ignoreslicingrule[d][1].ToString().Trim()) != latend))
                                    if ((tmp_Ys != latstart) && (tmp_Ye != latend))
                                    {
                                        //if ((tmp_Y >= (double.Parse(ignoreslicingrule[d][0].ToString().Trim()) + latareaoverlap)) && (tmp_Y <= (double.Parse(ignoreslicingrule[d][1].ToString().Trim()) - latareaoverlap)))
                                        if ((tmp_Y >= (tmp_Ys + latareaoverlap)) && (tmp_Y <= (tmp_Ye - latareaoverlap)))
                                            fallinotherarea = true;
                                    }
                                    //else if (double.Parse(ignoreslicingrule[d][0].ToString().Trim()) == latstart)
                                    else if (tmp_Ys == latstart)
                                    {
                                        //if ((tmp_Y >= double.Parse(ignoreslicingrule[d][0].ToString().Trim()) ) && (tmp_Y <= (double.Parse(ignoreslicingrule[d][1].ToString().Trim()) - latareaoverlap)))
                                        if ((tmp_Y >= tmp_Ys) && (tmp_Y <= (tmp_Ye - latareaoverlap)))
                                            fallinotherarea = true;
                                    }
                                    //else if (double.Parse(ignoreslicingrule[d][1].ToString().Trim()) == latend)
                                    else if (tmp_Ye == latend)
                                    {
                                        //if ((tmp_Y >= (double.Parse(ignoreslicingrule[d][0].ToString().Trim()) + latareaoverlap)) && (tmp_Y <= double.Parse(ignoreslicingrule[d][1].ToString().Trim())))
                                        if ((tmp_Y >= (tmp_Ys + latareaoverlap)) && (tmp_Y <= tmp_Ye))
                                            fallinotherarea = true;
                                    }

                                    if (!fallinotherarea)
                                        continue;

                                    //if ((double.Parse(ignoreslicingrule[d][2].ToString().Trim()) != lonstart) && (double.Parse(ignoreslicingrule[d][3].ToString().Trim()) != lonend))
                                    if ((tmp_Xs != lonstart) && (tmp_Xe != lonend))
                                    {
                                        //if (((tmp_X >= (double.Parse(ignoreslicingrule[d][2].ToString().Trim()) + lonareaoverlap)) && (tmp_X <= (double.Parse(ignoreslicingrule[d][3].ToString().Trim()) - lonareaoverlap))) && (fallinotherarea))
                                        if (((tmp_X >= (tmp_Xs + lonareaoverlap)) && (tmp_X <= (tmp_Xe - lonareaoverlap))) && (fallinotherarea))
                                            fallinotherarea = true;
                                        else
                                            fallinotherarea = false;
                                    }
                                    //else if (double.Parse(ignoreslicingrule[d][2].ToString().Trim()) == lonstart)
                                    else if (tmp_Xs == lonstart)
                                    {
                                        //if (((tmp_X >= double.Parse(ignoreslicingrule[d][2].ToString().Trim())) && (tmp_X <= (double.Parse(ignoreslicingrule[d][3].ToString().Trim()) - lonareaoverlap))) && (fallinotherarea))
                                        if (((tmp_X >= tmp_Xs) && (tmp_X <= (tmp_Xe - lonareaoverlap))) && (fallinotherarea))
                                            fallinotherarea = true;
                                        else
                                            fallinotherarea = false;
                                    }
                                    //else if (double.Parse(ignoreslicingrule[d][3].ToString().Trim()) == lonend)
                                    else if (tmp_Xe == lonend)
                                    {
                                        //if (((tmp_X >= (double.Parse(ignoreslicingrule[d][2].ToString().Trim()) + lonareaoverlap)) && (tmp_X <= double.Parse(ignoreslicingrule[d][3].ToString().Trim()) )) && (fallinotherarea))
                                        if (((tmp_X >= (tmp_Xs + lonareaoverlap)) && (tmp_X <= tmp_Xe)) && (fallinotherarea))
                                            fallinotherarea = true;
                                        else
                                            fallinotherarea = false;
                                    }

                                    if (fallinotherarea)
                                        break;
                                }

                                if (fallinotherarea)
                                {
                                    ignorePOInum++;
                                    continue;
                                }

                                rangePOInum++;
                                latlargeindex = (int)Math.Floor((tmp_Y - CHNlatstart) / tmplatblockwidth);
                                if (latlargeindex == tmplatblockcount)
                                    latlargeindex = tmplatblockcount - 1;
                                lonlargeindex = (int)Math.Floor((tmp_X - CHNlonstart) / tmplonblockwidth);
                                if (lonlargeindex == tmplonblockcount)
                                    lonlargeindex = tmplonblockcount - 1;
                                largeindex = latlargeindex * tmplonblockcount + lonlargeindex;

                                rel_latstart = CHNlatstart + latlargeindex * tmplatblockwidth;
                                rel_lonstart = CHNlonstart + lonlargeindex * tmplonblockwidth;
                                rel_latsmallindex = (int)Math.Floor((tmp_Y - rel_latstart) / tmplatsmallblockwidth);
                                if (rel_latsmallindex == tmplatsmallblockcount)
                                    rel_latsmallindex = tmplatsmallblockcount - 1;
                                rel_lonsmallindex = (int)Math.Floor((tmp_X - rel_lonstart) / tmplonsmallblockwidth);
                                if (rel_lonsmallindex == tmplonsmallblockcount)
                                    rel_lonsmallindex = tmplonsmallblockcount - 1;
                                smallindex = rel_latsmallindex * tmplonsmallblockcount + rel_lonsmallindex;

                                //DGV_excel.Rows[i].Cells[0].Value = largeindex.ToString() + "," + smallindex.ToString(); 20131213
                                DGV_excel.Rows[i].Cells[layerinfo].Value = largeindex.ToString() + "," + smallindex.ToString();
                            }

                            // save each datagridview item into points[]   
                            for (i = rangestartindex; i <= rangeendindex; i++)
                            {
                                //tmpindex = DGV_excel.Rows[i].Cells[0].Value.ToString().Split(','); //layer: (large, small), 20131213

                                if ((DGV_excel.Rows[i].Cells[layerinfo].Value == null) || (DGV_excel.Rows[i].Cells[layerinfo].Value.ToString().Length == 0))
                                    continue;

                                tmpindex = DGV_excel.Rows[i].Cells[layerinfo].Value.ToString().Split(','); //layer: (large, small)

                                tmplargelayerindex = int.Parse(tmpindex[0]);    // large layer index
                                tmpsmalllayerindex = int.Parse(tmpindex[1]);    // small layer index
                                tmppointinfo = i.ToString();
                                if ((tmplargelayerindex > -1) && (tmpsmalllayerindex > -1))
                                {
                                    //points[tmplargelayerindex, tmpsmalllayerindex].Add(tmppointinfo);
                                    tmppoints[tmplargelayerindex, tmpsmalllayerindex].Add(tmppointinfo);
                                }
                            }   // end of for                     

                            //--------------- Verify if this combination is okay -----------------------//                            
                            
                            for (i = 0; i < tmplatblockcount * tmplatsmallblockcount; i++)
                                for (j = 0; j < tmplonblockcount * tmplonsmallblockcount; j++)
                                    DivisionEvaluation.DGV_verifydivision.Rows[i].Cells[j].Value = "";


                            for (i = 0; i < tmppoints.GetLength(0); i++)
                                for (j = 0; j < tmppoints.GetLength(1); j++)
                                {
                                    tmprowindex = (i / tmplonblockcount) * tmplatsmallblockcount + (j / tmplonsmallblockcount);
                                    tmpcolumnindex = (i % tmplonblockcount) * tmplonsmallblockcount + (j % tmplonsmallblockcount);
                                    
                                    DivisionEvaluation.DGV_verifydivision.Rows[(tmplatblockcount * tmplatsmallblockcount - 1) - tmprowindex].Cells[tmpcolumnindex].Value = tmppoints[i, j].Count.ToString();                                    
                                }                            

                            failedcombination = false;                            
                            
                            //------------------------------only for debug --------------------------//

                            //for (i = 1; i < (DivisionEvaluation.DGV_verifydivision.RowCount - 1); i += 3)
                            //for (i = 1; i < (tmplatblockcount * tmplatsmallblockcount - 1); i += 3)
                            for (i = 1; i < (tmplatblockcount * tmplatsmallblockcount - 1); i ++) //20140421
                            {
                                //for (j = 1; j < (DivisionEvaluation.DGV_verifydivision.ColumnCount - 1); j++)
                                for (j = 1; j < (tmplonblockcount * tmplonsmallblockcount - 1); j++)
                                {
                                    tmpnineblockPOI = (long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i - 1].Cells[j - 1].Value.ToString()) +
                                         long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i].Cells[j - 1].Value.ToString()) +
                                         long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i + 1].Cells[j - 1].Value.ToString()) +

                                        long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i - 1].Cells[j].Value.ToString()) +
                                         long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i].Cells[j].Value.ToString()) +
                                         long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i + 1].Cells[j].Value.ToString()) +

                                        long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i - 1].Cells[j + 1].Value.ToString()) +
                                         long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i].Cells[j + 1].Value.ToString()) +
                                         long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i + 1].Cells[j + 1].Value.ToString()));
                                    
                                    if (tmpnineblockPOI >= POIsuminsmallblockthreshold)
                                    //if (tmpnineblockPOI >= 3000), 20140421
                                    {                                        
                                        failedcombination = true;
                                        break;
                                    }
                                }

                                if (failedcombination)
                                    break;
                            }

                            if (!failedcombination)
                            {
                                divisionblockcount[r].Add(t.ToString() + "," + s.ToString());
                            }

                            tmpPOIsum = 0;
                            /*
                            for (i = 0; i < DivisionEvaluation.DGV_verifydivision.RowCount; i++)
                                for (j = 0; j < DivisionEvaluation.DGV_verifydivision.ColumnCount; j++)
                                    tmpPOIsum += long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i].Cells[j].Value.ToString());
                            */
                            for (i = 0; i < tmplatblockcount * tmplatsmallblockcount; i++)
                                for (j = 0; j < tmplonblockcount * tmplonsmallblockcount; j++)
                                    tmpPOIsum += long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i].Cells[j].Value.ToString());

                            if (tmpPOIsum != rangePOInum)
                                MessageBox.Show("Outest Division: Wrong Division!!!");

                            if ((!failedcombination) && (divisionblockcount[r][0].ToString().Length > 0) && (multilevelpoints[r, 0, 0] == null))
                            {
                                for (i = 0; i < tmppoints.GetLength(0); i++)
                                    for (j = 0; j < tmppoints.GetLength(1); j++)
                                    {
                                        if (multilevelpoints[r, i, j] == null)
                                            multilevelpoints[r, i, j] = new ArrayList();

                                        for (int d = 0; d < tmppoints[i, j].Count; d++)
                                        {
                                            multilevelpoints[r, i, j].Add(tmppoints[i, j][d].ToString());
                                        }
                                    }
                            }

                            for (i = 0; i < tmppoints.GetLength(0); i++)
                                for (j = 0; j < tmppoints.GetLength(1); j++)
                                {
                                    tmppoints[i, j].Clear();
                                    tmppoints[i, j] = null;
                                }

                            //-----------------------------------------------------------------------
                            rangestartindex = 0;
                            rangeendindex = 0;

                            for (i = 0; i < DGV_excel.RowCount; i++)
                            {
                                if (DGV_excel.Rows[i].Cells[layerinfo].Value != null)
                                    DGV_excel.Rows[i].Cells[layerinfo].Value = "";
                            }

                            if (!failedcombination)  // exit the loop of s
                                break;
                        }

                        if ((!failedcombination) && (divisionblockcount[r][0].ToString().Length > 0))
                            break;

                    }   // end of for (largetestroundnum)                    

                    // --------------------------------------- End of Outest Area ----------------------------------//
                }   // end of if (lB_nestedslicingrules.Items.Count > 0)

            }

            //------------------------- general division ---------------------------//
            if ( ( !(IsPOIsuminsmallblocklessthanthreshold)) && (IsCHNPOI == false) )
            {
                //points = new ArrayList[blockcount * lonblockcount, smalllayerblockcount * lonsmalllayerblockcount];

                //latstart = Math.Floor(latstart); latend = Math.Ceiling(latend); lonstart = Math.Floor(lonstart); lonend = Math.Ceiling(lonend);
                if (tB_LayerWidth.Text.Length == 0)
                    //layerwidth = Math.Round((double)(latend - latstart) / blockcount, 3);
                    layerwidth = (double)(latend - latstart) / blockcount;
                if (tB_LonLayerWidth.Text.Length == 0)
                    //lonlayerwidth = Math.Round((double)(lonend - lonstart) / lonblockcount, 3);
                    lonlayerwidth = (double)(lonend - lonstart) / lonblockcount;
                if (tB_SmallLayerWidth.Text.Length == 0)
                    //smalllayerwidth = Math.Round((double)layerwidth / smalllayerblockcount, 3);
                    smalllayerwidth = (double)layerwidth / smalllayerblockcount;
                if (tB_LonSmallLayerWidth.Text.Length == 0)
                    //lonsmalllayerwidth = Math.Round((double)lonlayerwidth / lonsmalllayerblockcount, 3);
                    lonsmalllayerwidth = (double)lonlayerwidth / lonsmalllayerblockcount;
                
                // mark position:  (X.large-X.small, Y.large-Y.small)            

                //layerinfo = DGV_excel.Columns.Count;
                //DGV_excel.Columns.Add("Layers", "Layers");
                //layerinfo = DGV_excel.Columns.Count - 1;           

                for (i = 0; i < DGV_excel.RowCount; i++)
                {
                    //valuechecking(i);   // output to logfile: SuccessLogFile or ErrorLogFile

                    tmp_Y = double.Parse(DGV_excel.Rows[i].Cells[1].Value.ToString());
                    tmp_X = double.Parse(DGV_excel.Rows[i].Cells[2].Value.ToString());

                    latlargeindex = (int)Math.Floor((tmp_Y - latstart) / layerwidth);
                    if (latlargeindex == blockcount)
                        latlargeindex = blockcount - 1;
                    lonlargeindex = (int)Math.Floor((tmp_X - lonstart) / lonlayerwidth);
                    if (lonlargeindex == lonblockcount)
                        lonlargeindex = lonblockcount - 1;
                    largeindex = latlargeindex * lonblockcount + lonlargeindex;

                    rel_latstart = latstart + latlargeindex * layerwidth;
                    rel_lonstart = lonstart + lonlargeindex * lonlayerwidth;
                    rel_latsmallindex = (int)Math.Floor((tmp_Y - rel_latstart) / smalllayerwidth);
                    if (rel_latsmallindex == smalllayerblockcount)
                        rel_latsmallindex = smalllayerblockcount - 1;
                    rel_lonsmallindex = (int)Math.Floor((tmp_X - rel_lonstart) / lonsmalllayerwidth);
                    if (rel_lonsmallindex == lonsmalllayerblockcount)
                        rel_lonsmallindex = lonsmalllayerblockcount - 1;
                    smallindex = rel_latsmallindex * lonsmalllayerblockcount + rel_lonsmallindex;
                    
                    //DGV_excel.Rows[i].Cells[0].Value = largeindex.ToString() + "," + smallindex.ToString(); 20131213
                    DGV_excel.Rows[i].Cells[layerinfo].Value = largeindex.ToString() + "," + smallindex.ToString();
                }

                // save each datagridview item into points[]
                //MessageBox.Show("Total points: " + DGV_excel.RowCount.ToString());
                
                for (i = 0; i < blockcount * lonblockcount; i++)
                {                     
                    for (j = 0; j < smalllayerblockcount * lonsmalllayerblockcount; j++)
                        points[i, j] = new ArrayList();
                }

                for (i = 0; i < DGV_excel.RowCount; i++)
                {
                    //tmpindex = DGV_excel.Rows[i].Cells[0].Value.ToString().Split(','); //layer: (large, small), 20131213
                    tmpindex = DGV_excel.Rows[i].Cells[layerinfo].Value.ToString().Split(','); //layer: (large, small)

                    tmplargelayerindex = int.Parse(tmpindex[0]);    // large layer index
                    tmpsmalllayerindex = int.Parse(tmpindex[1]);    // small layer index
                    tmppointinfo = i.ToString();
                    if ((tmplargelayerindex > -1) && (tmpsmalllayerindex > -1))
                    {
                        points[tmplargelayerindex, tmpsmalllayerindex].Add(tmppointinfo);
                    }
                }   // end of for                     

            }
            //-------------------------- new version division ------------------------------------------------//
            else if ( ( (IsPOIsuminsmallblocklessthanthreshold) && (POIsuminsmallblockthreshold > 0) ) && (IsCHNPOI == false) ) //DigiLife use this
            {                               
                failedcombination = false;
                DivisionEvaluation = new Fm_evaluatedivision();
                DivisionEvaluation.Hide();

                //20150617
                if (DGV_excel.RowCount > 550000)
                {
                    largeteststart = 18;
                    smallteststart = 15;
                    largetestroundnum = 24;
                    smalltestroundnum = 24;
                    POIsuminsmallblockthreshold = 6000;
                }
                else if (DGV_excel.RowCount > 450000)
                {
                    largeteststart = 20;
                    smallteststart = 15;
                    largetestroundnum = 22;
                    smalltestroundnum = 25;
                }

                if (DGV_excel.RowCount < 6000)
                {
                    largeteststart = 2;//1
                    smallteststart = 2;//1
                    largetestroundnum = 5;
                    smalltestroundnum = 5;
                }

                if (IsAusPOI)
                {
                    largeteststart = 2;//1
                    smallteststart = 2;//1
                    largetestroundnum = 21;
                    smalltestroundnum = 27;
                }

                DivisionEvaluation.DGV_verifydivision.RowCount = largetestroundnum * (smalltestroundnum + 3);              // 20*30
                DivisionEvaluation.DGV_verifydivision.ColumnCount = largetestroundnum * (smalltestroundnum + 3);      // 20*30                

                //if (lB_nestedslicingrules.Items.Count > 0)
                if (IsCHNPOI)
                {
                    divisionblockcount = new ArrayList[lB_nestedslicingrules.Items.Count];
                }
                else
                {
                    divisionblockcount = new ArrayList[1];
                    divisionblockcount[0] = new ArrayList();
                }
           
                points = new ArrayList[largetestroundnum * largetestroundnum, (smalltestroundnum + 3) * (smalltestroundnum + 3)]; 

                //r: large, s: small
                //for (r = largeteststart; r < testroundnum; r++)
                for (r = largeteststart; r < largetestroundnum; r++)
                {
                    tB_BlockCount.Text = r.ToString();
                    tB_LonBlockCount.Text = r.ToString();
                    tmplatblockcount = r;
                    tmplonblockcount = r;
                    tmplonblockwidth = (double)(lonend - lonstart) / tmplonblockcount;
                    tmplatblockwidth = (double)(latend - latstart) / tmplatblockcount;

                    //for (s = smallteststart; s < testroundnum; s++)
                    for (s = smallteststart; s < smalltestroundnum; s++)
                    {
                        tmplatsmallblockcount = s;
                        tmplonsmallblockcount = s;
                        tB_SmallBlockCount.Text = tmplatsmallblockcount.ToString();
                        tB_LonSmallBlockCount.Text = tmplonsmallblockcount.ToString();

                        tmplonsmallblockwidth = (double)tmplonblockwidth / tmplonsmallblockcount;
                        tmplatsmallblockwidth = (double)tmplatblockwidth / tmplatsmallblockcount;
                        //tB_BlockCount.Refresh();
                        //tB_LonBlockCount.Refresh();
                        //tB_SmallBlockCount.Refresh();
                        //tB_LonSmallBlockCount.Refresh();

                        tB_BlockCount.Update();
                        tB_LonBlockCount.Update();
                        tB_SmallBlockCount.Update();
                        tB_LonSmallBlockCount.Update();

                        tmppoints = new ArrayList[tmplatblockcount*tmplonblockcount, tmplatsmallblockcount*tmplonsmallblockcount];

                        for (i = 0; i < tmppoints.GetLength(0); i++)
                        {
                            for (j = 0; j < tmppoints.GetLength(1); j++)
                                if (tmppoints[i, j] == null)
                                    tmppoints[i, j] = new ArrayList();
                                else if ((tmppoints[i, j] != null) && (tmppoints[i, j].Count > 0))
                                    tmppoints[i, j].Clear();
                        }

                        tmp_POIcontained = 0;

                        for (i = 0; i < DGV_excel.RowCount; i++)
                        {                              
                            tmp_Y = double.Parse(DGV_excel.Rows[i].Cells[1].Value.ToString());
                            tmp_X = double.Parse(DGV_excel.Rows[i].Cells[2].Value.ToString());

                            tmp_POIcontained++;
                            latlargeindex = (int)Math.Floor((tmp_Y - latstart) / tmplatblockwidth);
                            if (latlargeindex == tmplatblockcount)
                                latlargeindex = tmplatblockcount - 1;
                            lonlargeindex = (int)Math.Floor((tmp_X - lonstart) / tmplonblockwidth);
                            if (lonlargeindex == tmplonblockcount)
                                lonlargeindex = tmplonblockcount - 1;
                            largeindex = latlargeindex * tmplonblockcount + lonlargeindex;

                            rel_latstart = latstart + latlargeindex * tmplatblockwidth;
                            rel_lonstart = lonstart + lonlargeindex * tmplonblockwidth;
                            rel_latsmallindex = (int)Math.Floor((tmp_Y - rel_latstart) / tmplatsmallblockwidth);
                            if (rel_latsmallindex == tmplatsmallblockcount)
                                rel_latsmallindex = tmplatsmallblockcount - 1;
                            rel_lonsmallindex = (int)Math.Floor((tmp_X - rel_lonstart) / tmplonsmallblockwidth);
                            if (rel_lonsmallindex == tmplonsmallblockcount)
                                rel_lonsmallindex = tmplonsmallblockcount - 1;
                            smallindex = rel_latsmallindex * tmplonsmallblockcount + rel_lonsmallindex;

                            DGV_excel.Rows[i].Cells[layerinfo].Value = largeindex.ToString() + "," + smallindex.ToString();
                        }

                        for (i = 0; i < largetestroundnum * largetestroundnum; i++)
                        {
                            for (j = 0; j < (smalltestroundnum + 3) * (smalltestroundnum + 3); j++)
                                if (points[i, j] == null)
                                    points[i, j] = new ArrayList();
                                else if (!stopupdatepoints)
                                    points[i, j].Clear();
                        }

                        tmp_POIcontained = 0;
                        for (i = 0; i < DGV_excel.RowCount; i++)
                        {
                            if ((DGV_excel.Rows[i].Cells[layerinfo].Value == null) || (DGV_excel.Rows[i].Cells[layerinfo].Value.ToString().Length == 0))
                                continue;

                            tmpindex = DGV_excel.Rows[i].Cells[layerinfo].Value.ToString().Split(','); //layer: (large, small)

                            tmplargelayerindex = int.Parse(tmpindex[0]);    // large layer index
                            tmpsmalllayerindex = int.Parse(tmpindex[1]);    // small layer index
                            tmppointinfo = i.ToString();
                            if ((tmplargelayerindex > -1) && (tmpsmalllayerindex > -1))
                            {
                                tmppoints[tmplargelayerindex, tmpsmalllayerindex].Add(tmppointinfo);                                
                                if (!stopupdatepoints)
                                    points[tmplargelayerindex, tmpsmalllayerindex].Add(tmppointinfo);
                                tmp_POIcontained++;
                            }
                        }   // end of for          

                        /*
                        if (DivisionEvaluation.DGV_verifydivision.RowCount > 1)
                        {
                            //DivisionEvaluation.DGV_verifydivision.RowCount = 1;
                            DivisionEvaluation.DGV_verifydivision.Columns.Clear();
                            DivisionEvaluation.DGV_verifydivision.Rows.Clear();
                        }
                        DivisionEvaluation.DGV_verifydivision.RowCount = tmplatblockcount * tmplatsmallblockcount;
                        DivisionEvaluation.DGV_verifydivision.ColumnCount = tmplonblockcount * tmplonsmallblockcount;
                        */

                        for (i = 0; i < DivisionEvaluation.DGV_verifydivision.RowCount; i++)
                            for (j = 0; j < DivisionEvaluation.DGV_verifydivision.ColumnCount; j++)
                                //if (DivisionEvaluation.DGV_verifydivision.Rows[i].Cells[j].Value.ToString().Length > 0)
                                DivisionEvaluation.DGV_verifydivision.Rows[i].Cells[j].Value = "";

                        //verify if sumofnightsmall <= 2000
                        for (i = 0; i < tmppoints.GetLength(0); i++)
                            for (j = 0; j < tmppoints.GetLength(1); j++)
                            {
                                tmprowindex = (i / tmplonblockcount) * tmplatsmallblockcount + (j / tmplonsmallblockcount);
                                tmpcolumnindex = (i % tmplonblockcount) * tmplonsmallblockcount + (j % tmplonsmallblockcount);
                                //DivisionOverview.DGV_DivisionOverview.Rows[(blockcount * smalllayerblockcount - 1) - rowindex].Cells[columnindex].Value = points[i, j].Count.ToString();
                                DivisionEvaluation.DGV_verifydivision.Rows[(tmplatblockcount * tmplatsmallblockcount - 1) - tmprowindex].Cells[tmpcolumnindex].Value = tmppoints[i, j].Count.ToString();
                            }

                        failedcombination = false;

                        //for (i = 1; i < (DivisionEvaluation.DGV_verifydivision.RowCount - 1); i += 3)
                        //for (i = 1; i < (DivisionEvaluation.DGV_verifydivision.RowCount - 1); i ++)
                        for (i = 1; i < (tmplatblockcount * tmplatsmallblockcount - 1); i ++)
                        {
                            //for (j = 1; j < (DivisionEvaluation.DGV_verifydivision.ColumnCount-1); j++)
                            for (j = 1; j < (tmplonblockcount * tmplonsmallblockcount - 1); j++)
                            {
                                tmpnineblockPOI = (long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i - 1].Cells[j - 1].Value.ToString()) +
                                     long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i].Cells[j - 1].Value.ToString()) +
                                     long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i + 1].Cells[j - 1].Value.ToString()) +

                                    long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i - 1].Cells[j].Value.ToString()) +
                                     long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i].Cells[j].Value.ToString()) +
                                     long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i + 1].Cells[j].Value.ToString()) +

                                    long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i - 1].Cells[j + 1].Value.ToString()) +
                                     long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i].Cells[j + 1].Value.ToString()) +
                                     long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i + 1].Cells[j + 1].Value.ToString()));

                                 if (tmpnineblockPOI >= POIsuminsmallblockthreshold)
                                {
                                    failedcombination = true;
                                    break;
                                }
                            }

                            if (failedcombination)
                                break;
                        }

                        if (!failedcombination)
                        {
                            divisionblockcount[0].Add(r.ToString() + ", " + s.ToString());

                            if (divisionblockcount[0].Count == 1)
                            {
                                stopupdatepoints = true;
                                blockcount = r;
                                lonblockcount = r;
                                smalllayerblockcount = s;
                                lonsmalllayerblockcount = s;

                                //verify POI counts
                                tmp_POIcontained = 0;
                                for (i = 0; i < points.GetLength(0); i++)
                                    for (j = 0; j < points.GetLength(1); j++)
                                        tmp_POIcontained += points[i, j].Count;
                                tmp_POIcontained = 0;
                            }
                        }

                        tmpPOIsum = 0;
                        for (i = 0; i < tmplatblockcount * tmplatsmallblockcount; i++)
                            for (j = 0; j < tmplonblockcount * tmplonsmallblockcount; j++)
                                tmpPOIsum += long.Parse(DivisionEvaluation.DGV_verifydivision.Rows[i].Cells[j].Value.ToString());

                        if (tmpPOIsum != DGV_excel.RowCount)
                            MessageBox.Show("Wrong Division!!!");

                        for (i = 0; i < tmppoints.GetLength(0); i++)
                            for (j = 0; j < tmppoints.GetLength(1); j++)
                            {
                                tmppoints[i, j].Clear();
                                //tmppoints[i, j] = null;
                                if (!stopupdatepoints)
                                    points[i, j].Clear();
                            }
                    } // end of small < testnum

                } // end of large < testnum
            }   // end of else

            //verify POI counts
            if (!IsCHNPOI)
            {
                tmp_POIcontained = 0;
                for (i = 0; i < points.GetLength(0); i++)
                    for (j = 0; j < points.GetLength(1); j++)
                        tmp_POIcontained += points[i, j].Count;
                tmp_POIcontained = 0;
            }
            else
            {
                if (divisionblockcount[lB_nestedslicingrules.Items.Count] == null)
                {
                    MessageBox.Show("Can't get the map division");
                    btn_savebin.Enabled = false;
                }
            }
            /*
            if ( (showPOIperblock) && (!IsCHNPOI) )
            {
                showPOIinfo(0);
                tB_BlockInfoThreshold.Enabled = true;
                tB_BlockInfoCount.Text = lB_BlockInfo.Items.Count.ToString();
            }
            */
        }

        //-------------------------------------------------------------
        public void SaveExcelFile(string excelfile)
        {
            int i = 0, j = 0, k = 0, filecounter = 0; // counter
            int ii = 0, jj = 0;
            long tmp_POIcontained2 = 0;
            int tmpcount = 0, tmplonindex = 0, tmplatindex = 0, tmplonsmallindex = 0, tmplatsmallindex = 0;
            int[] largelayer = new int[blockcount * lonblockcount];
            int[] largelayerbytes = new int[blockcount * lonblockcount];
            int tmppoint = 0, largelayeroffset = 0, smalllayeroffset = 0, tmpoffset = 0, tmpsmalloffset = 0;
            int sizeofeachcamera = 0, sizeofsmallblockheader = 0, sizeoflargeblockheader = 0, sizeofglobalheader = 0;
            double tmpcamx = 0.0, tmpcamy = 0.0;
            FileStream output;
            BinaryWriter outputwriter;

            // --------------------- CHN POI version ----------------------------//
            string seqfilename = null;
            string[] parseslicingrule;
            string[] finallargesmallblockcount;
            int sectorsize = 512;
            int CHNglobalheader = 4096;
            FileStream input;
            BinaryReader inputreader;
            byte[] filechecksum = new byte[4];            
            byte[] filedata;
            int signature = 1;                  // CHN: 1; others: 0
            long skuid = 0;
            byte id = 0;
            int flag = 0;
            int ubtotalpart = 0;
            int uloffset = 0;
            int[] ulfilesize = null;
            uint[] ulchecksum = null;
            long totalbyteoutput = 0;
            double tmpcorr = 0.0;
            short tmpcorrshort = 0;
            double deblat = double.MinValue;
            double deblon = double.MinValue;
            // ----------------------------------------------------------------------//

            
            //for debug
            ArrayList debuglist = new ArrayList();
            string tmpcamfilename = null;
            StreamWriter outputdebuglist = null;
            FileStream outputcamfile = null;
            BinaryWriter outputcamwriter = null;
            double camdegree = 0.0, camdegree_float = 0.0;
            int camdegree_int = 0;

            gB_BlockInfo.Enabled = false;

            //For Aus
            /*
            if (IsAusPOI)            
            {
                for (i = 0; i < DGV_excel.RowCount; i++)
                {
                    tmplat = double.Parse(DGV_excel.Rows[i].Cells[1].Value.ToString());
                    if (tmplat > 0.0)
                    {
                        DGV_excel.Rows[i].Cells[1].Value = (0 - tmplat).ToString();
                    }
                }
            }
            */

            //FileStream output = File.Open(tB_bin.Text.ToString(), FileMode.OpenOrCreate, FileAccess.Write);
            //tB_bin.Text = Application.StartupPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(excelfile) + ".bin";         
            if ((File.Exists(inifile)) && (rawlogpath.Length > 0) && (errorloglist.Count == 0))
            {
                tB_bin.Text = rawlogstr;                
            }

            if (IsCHNPOI)
            {
                //FileStream output = File.Create(tB_bin.Text);            
                if (!System.IO.Directory.Exists(tB_bin.Text.Trim()))
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(tB_bin.Text.Trim()));               

                //FileStream output = File.Create(tB_bin.Text);
                //BinaryWriter outputwriter = new BinaryWriter(output);

                // For debug, 
                bool Isdebug = false;     //20131209, set to false

                if (Isdebug)
                {
                    debuglist = new ArrayList();

                    if (mode)    //GUI
                    {
                        tmpcamfilename = System.IO.Path.ChangeExtension(OFDlg1.FileName, "txt");
                        outputdebuglist = new StreamWriter(tmpcamfilename);

                        tmpcamfilename = System.IO.Path.GetDirectoryName(OFDlg1.FileName);
                        tmpcamfilename = tmpcamfilename + "\\SpeedCam_debug_points.bin";
                    }
                    else
                    {
                        tmpcamfilename = System.IO.Path.ChangeExtension(tB_bin.Text, "txt");
                        outputdebuglist = new StreamWriter(tmpcamfilename);

                        tmpcamfilename = System.IO.Path.GetDirectoryName(tmpcamfilename);
                        tmpcamfilename = tmpcamfilename + "\\SpeedCam_debug_points.bin";
                    }
                    outputcamfile = File.Create(tmpcamfilename);
                    outputcamwriter = new BinaryWriter(outputcamfile);
                }

                //for (filecounter = 0; filecounter < lB_slicingrules.Items.Count; filecounter++)
                //for (filecounter = 0; filecounter < lB_nestedslicingrules.Items.Count; filecounter++)
                for (filecounter = 0; filecounter < multilevelpoints.GetLength(0); filecounter++)
                {
                    //seqfilename = System.IO.Path.GetFileNameWithoutExtension(tB_bin.Text) + "_area" + filecounter.ToString() + ".bin"; //20150910
                    seqfilename = System.IO.Path.GetFileNameWithoutExtension(tB_bin.Text) + filecounter.ToString() + ".bin";
                    output = File.Create(seqfilename);
                    outputwriter = new BinaryWriter(output);

                    //parseslicingrule = lB_slicingrules.Items[filecounter].ToString().Split(',');
                    if (filecounter < lB_nestedslicingrules.Items.Count)
                    {
                        parseslicingrule = lB_nestedslicingrules.Items[filecounter].ToString().Split(',');
                        latstart = double.Parse(parseslicingrule[0]);
                        latend = double.Parse(parseslicingrule[1]);
                        lonstart = double.Parse(parseslicingrule[2]);
                        lonend = double.Parse(parseslicingrule[3]);
                    }
                    else if (filecounter == lB_nestedslicingrules.Items.Count)
                    {
                        latstart = double.Parse(tB_latstart.Text);
                        latend = double.Parse(tB_latend.Text);
                        lonstart = double.Parse(tB_lonstart.Text);
                        lonend = double.Parse(tB_lonend.Text);
                    }

                    finallargesmallblockcount = divisionblockcount[filecounter][0].ToString().Split(',');
                    lonblockcount = int.Parse(finallargesmallblockcount[0].ToString());
                    blockcount = int.Parse(finallargesmallblockcount[0].ToString());
                    lonsmalllayerblockcount = int.Parse(finallargesmallblockcount[1].ToString());
                    smalllayerblockcount = int.Parse(finallargesmallblockcount[1].ToString());

					//continue to verify
                    layerwidth = (double)(latend - latstart) / blockcount;
                    lonlayerwidth = (double)(lonend - lonstart) / lonblockcount;
                    smalllayerwidth = (double)layerwidth / smalllayerblockcount;
                    lonsmalllayerwidth = (double)lonlayerwidth / lonsmalllayerblockcount;

                    // output order: Global, Large layer, Small layer
                    // 1. Global header            
                    outputwriter.Write((short)lonstart); // Start position of Longitude
                    if (Isdebug) debuglist.Add(lonstart);
                    outputwriter.Write((short)latstart);  // Start position of Latitude
                    if (Isdebug) debuglist.Add(latstart);
                    outputwriter.Write((short)lonend);  // End position of Longitude
                    if (Isdebug) debuglist.Add(lonend);
                    outputwriter.Write((short)latend);   // End position of Latitude
                    if (Isdebug) debuglist.Add(latend);

                    outputwriter.Write((ushort)lonsmalllayerblockcount); // Small Layer Block Number of Longitude
                    if (Isdebug) debuglist.Add(lonsmalllayerblockcount);
                    outputwriter.Write((ushort)smalllayerblockcount);   // Small Layer Block Number of Latitude
                    if (Isdebug) debuglist.Add(smalllayerblockcount);
                    outputwriter.Write((ushort)lonblockcount); // Large Layer Block Number of Longitude
                    if (Isdebug) debuglist.Add(lonblockcount);
                    outputwriter.Write((ushort)blockcount);  // Small Layer Block Number of Latitude                               
                    if (Isdebug) debuglist.Add(blockcount);

                    // version of the camera data
                    if (IsCHNPOI)
                        dataversion = 3; 
                    outputwriter.Write((byte)(dataversion));
                    if (Isdebug) debuglist.Add(dataversion);

                    //for 4-byte alignment
                    for (i = 1; i <= 3; i++)
                        outputwriter.Write((byte)' ');

                    // for MiTAC mioversion [4]
                    outputwriter.Write((uint)mioversion);
                    if (Isdebug) debuglist.Add(mioversion);

                    //for systemtime [12]
                    //str_datetime += '\0';
                    if (filecounter == 0)
                        str_datetime += '\0';

                    char[] tmpchar1 = str_datetime.ToCharArray();
                    for (i = 0; i < tmpchar1.Length; i++)
                    {
                        outputwriter.Write((byte)tmpchar1[i]);
                        if (Isdebug) debuglist.Add(tmpchar1[i]);
                    }
                    //for (i = 1; i <= 3; i++)
                    for (i = 1; i <= (12-tmpchar1.Length); i++)
                        outputwriter.Write((byte)' ');


                    //sizeofglobalheader = sizeof(ushort) * 8 + 4;
                    //sizeofglobalheader = sizeof(ushort) * 8 + sizeof(byte) * 4;
                    sizeofglobalheader = sizeof(ushort) * 8 + sizeof(byte) * 4 + sizeof(uint) * 1 + sizeof(byte) * 12; // new add: Version[4], SysteTime[12]
                    sizeoflargeblockheader = sizeof(ushort) * 5;
                    sizeofsmallblockheader = sizeof(ushort) * 5;
                    //sizeofeachcamera = 20;            

                    if (doubleorfloat)   // true: double version
                        sizeofeachcamera = sizeof(double) * 2 + sizeof(byte) * 4;  //20
                    else                         // false: float version
                        sizeofeachcamera = sizeof(float) * 2 + sizeof(byte) * 4;     // 12

                    // largelayer[i]: # of points contained in the i-th large layer                                             
                    for (i = 0; i < blockcount * lonblockcount; i++)
                    {
                        tmpcount = 0;
                        for (j = 0; j < smalllayerblockcount * lonsmalllayerblockcount; j++)
                            //tmpcount += points[i, j].Count;
                            tmpcount += multilevelpoints[filecounter, i, j].Count;
                            
                        largelayer[i] = tmpcount;
                    }

                    int tttt = 0;  // tttt: total number of points
                    for (i = 0; i < blockcount * lonblockcount; i++)
                        tttt += largelayer[i];

                    // largelayerbytes[i]: # of bytes required for the i-th large layer
                    for (i = 0; i < blockcount * lonblockcount; i++)
                        //largelayerbytes[i] = largelayer[i] * sizeofeachcamera + sizeofsmallblockheader * smalllayerblockcount * lonsmalllayerblockcount;
                        largelayerbytes[i] = largelayer[i] * sizeofeachcamera + sizeofsmallblockheader * smalllayerblockcount * lonsmalllayerblockcount +
                                                            sizeof(uint) * smalllayerblockcount * lonsmalllayerblockcount + sizeoflargeblockheader;

                    // for large layer # 0
                    largelayeroffset = sizeofglobalheader + sizeof(uint) * blockcount * lonblockcount;

                    // for 4-byte alignment
                    //largelayeroffset += 3;

                    outputwriter.Write((uint)(largelayeroffset));
                    if (Isdebug) debuglist.Add(largelayeroffset);

                    // for subsequent large layer
                    tmpoffset = largelayeroffset;
                    for (i = 1; i < blockcount * lonblockcount; i++)
                    {
                        tmpoffset = tmpoffset + largelayerbytes[i - 1];
                        outputwriter.Write((uint)(tmpoffset));
                        if (Isdebug) debuglist.Add(tmpoffset);
                    }

                    // 2. Larger layer header                        
                    for (i = 0; i < blockcount * lonblockcount; i++)
                    {
                        outputwriter.Write((ushort)largelayer[i]);                                                             // # of points
                        if (Isdebug) debuglist.Add(largelayer[i]);
                        tmplatindex = (int)Math.Floor((double)(i / lonblockcount));
                        tmplonindex = i - tmplatindex * lonblockcount;

                        outputwriter.Write((short)(latstart + tmplatindex * layerwidth));                    // Start Latitude
                        if (Isdebug)
                        {
                            tmpcorr = latstart + tmplatindex * layerwidth;
                            tmpcorrshort = (short)(latstart + tmplatindex * layerwidth);
                            debuglist.Add(latstart + tmplatindex * layerwidth);
                        }
                        outputwriter.Write((short)(lonstart + tmplonindex * lonlayerwidth));             // Start Lontitude
                        if (Isdebug)
                        {
                            tmpcorr = (lonstart + tmplonindex * lonlayerwidth);
                            tmpcorrshort = (short)(lonstart + tmplonindex * lonlayerwidth);
                            debuglist.Add(lonstart + tmplonindex * lonlayerwidth);
                        }

                        if ((latstart + tmplatindex * layerwidth + layerwidth) < latend)
                        {
                            outputwriter.Write((short)(latstart + tmplatindex * layerwidth + layerwidth)); // End of Latitude
                            if (Isdebug)
                            {
                                tmpcorr = (latstart + tmplatindex * layerwidth + layerwidth);
                                tmpcorrshort = (short)(latstart + tmplatindex * layerwidth + layerwidth);
                                debuglist.Add(latstart + tmplatindex * layerwidth + layerwidth);
                            }
                        }
                        else
                        {
                            outputwriter.Write((short)latend);
                            if (Isdebug) debuglist.Add(latend);
                        }

                        if ((lonstart + tmplonindex * lonlayerwidth + lonlayerwidth) < lonend)
                        {
                            outputwriter.Write((short)(lonstart + tmplonindex * lonlayerwidth + lonlayerwidth));    // End of Longitude
                            if (Isdebug)
                            {
                                tmpcorr = (lonstart + tmplonindex * lonlayerwidth + lonlayerwidth);
                                tmpcorrshort = (short)(lonstart + tmplonindex * lonlayerwidth + lonlayerwidth);
                                debuglist.Add(lonstart + tmplonindex * lonlayerwidth + lonlayerwidth);
                            }
                        }
                        else
                        {
                            outputwriter.Write((short)lonend);
                            if (Isdebug) debuglist.Add(lonend);
                        }

                        // for smaller block # 0
                        if (i == 0)
                            smalllayeroffset = largelayeroffset + sizeoflargeblockheader + sizeof(uint) * smalllayerblockcount * lonsmalllayerblockcount;
                        else
                        {
                            tmpsmalloffset = 0;
                            for (j = i; j > 0; j--)
                            {
                                tmpsmalloffset = tmpsmalloffset + largelayerbytes[j - 1];

                            }
                            smalllayeroffset = largelayeroffset + tmpsmalloffset + sizeoflargeblockheader + sizeof(uint) * smalllayerblockcount * lonsmalllayerblockcount;

                            //smalllayeroffset = largelayeroffset + largelayerbytes[i - 1] + sizeoflargeblockheader + sizeof(uint) * smalllayerblockcount * lonsmalllayerblockcount;                    
                        }

                        outputwriter.Write((uint)(smalllayeroffset));
                        if (Isdebug) debuglist.Add(smalllayeroffset);

                        // for subsequent smaller block
                        tmpsmalloffset = smalllayeroffset;
                        for (j = 1; j < smalllayerblockcount * lonsmalllayerblockcount; j++)
                        {
                            //tmpsmalloffset = tmpsmalloffset + sizeofsmallblockheader + sizeofeachcamera * points[i, j - 1].Count;

                            tmpsmalloffset = tmpsmalloffset + sizeofsmallblockheader + sizeofeachcamera * multilevelpoints[filecounter, i, j-1].Count;                            

                            // a. # of points in each small layer
                            outputwriter.Write((uint)(tmpsmalloffset));
                            if (Isdebug) debuglist.Add(tmpsmalloffset);
                        }

                        // 3. Small Layer header, for each point in each small layer
                        for (j = 0; j < smalllayerblockcount * lonsmalllayerblockcount; j++)
                        {
                            //outputwriter.Write((ushort)points[i, j].Count);                                               // # of points
                            //if (Isdebug) debuglist.Add(points[i, j].Count);
                            outputwriter.Write((ushort)multilevelpoints[filecounter, i, j].Count);            // # of points
                            if (Isdebug) debuglist.Add(multilevelpoints[filecounter, i, j].Count);
                            
                            //tmplatsmallindex = (int)Math.Floor((double)j / smalllayerblockcount);
                            tmplatsmallindex = (int)Math.Floor((double)j / lonsmalllayerblockcount);
                            tmplonsmallindex = j - tmplatsmallindex * lonsmalllayerblockcount;
                            outputwriter.Write((short)(latstart + tmplatindex * layerwidth + tmplatsmallindex * smalllayerwidth));  // Start Latitude
                            if (Isdebug)
                            {
                                tmpcorr = (latstart + tmplatindex * layerwidth + tmplatsmallindex * smalllayerwidth);
                                tmpcorrshort = (short)(latstart + tmplatindex * layerwidth + tmplatsmallindex * smalllayerwidth);
                                debuglist.Add(latstart + tmplatindex * layerwidth + tmplatsmallindex * smalllayerwidth);
                            }
                            outputwriter.Write((short)(lonstart + tmplonindex * lonlayerwidth + tmplonsmallindex * lonsmalllayerwidth));  // Start of Lon
                            if (Isdebug)
                            {
                                tmpcorr = (lonstart + tmplonindex * lonlayerwidth + tmplonsmallindex * lonsmalllayerwidth);
                                tmpcorrshort = (short)(lonstart + tmplonindex * lonlayerwidth + tmplonsmallindex * lonsmalllayerwidth);
                                debuglist.Add(lonstart + tmplonindex * lonlayerwidth + tmplonsmallindex * lonsmalllayerwidth);
                            }

                            if ((latstart + tmplatindex * layerwidth + tmplatsmallindex * smalllayerwidth + smalllayerwidth) > (latstart + tmplatindex * layerwidth + layerwidth))
                            {
                                outputwriter.Write((short)(latstart + tmplatindex * layerwidth + layerwidth));                // End of Lat
                                if (Isdebug)
                                {
                                    tmpcorr = (latstart + tmplatindex * layerwidth + layerwidth);
                                    tmpcorrshort = (short)(latstart + tmplatindex * layerwidth + layerwidth);
                                    debuglist.Add(latstart + tmplatindex * layerwidth + layerwidth);
                                }
                            }
                            else
                            {
                                outputwriter.Write((short)(latstart + tmplatindex * layerwidth + tmplatsmallindex * smalllayerwidth + smalllayerwidth));                // End of Lat
                                if (Isdebug)
                                {
                                    tmpcorr = (latstart + tmplatindex * layerwidth + tmplatsmallindex * smalllayerwidth + smalllayerwidth);
                                    tmpcorrshort = (short)(latstart + tmplatindex * layerwidth + tmplatsmallindex * smalllayerwidth + smalllayerwidth);
                                    debuglist.Add(latstart + tmplatindex * layerwidth + tmplatsmallindex * smalllayerwidth + smalllayerwidth);
                                }
                            }

                            if ((lonstart + tmplonindex * lonlayerwidth + tmplonsmallindex * lonsmalllayerwidth + lonsmalllayerwidth) > (lonstart + tmplonindex * lonlayerwidth + lonlayerwidth))
                            {
                                outputwriter.Write((short)(lonstart + tmplonindex * lonlayerwidth + lonlayerwidth));               // End of Lon
                                if (Isdebug)
                                {
                                    tmpcorr = (lonstart + tmplonindex * lonlayerwidth + lonlayerwidth);
                                    tmpcorrshort = (short)(lonstart + tmplonindex * lonlayerwidth + lonlayerwidth);
                                    debuglist.Add(lonstart + tmplonindex * lonlayerwidth + lonlayerwidth);
                                }
                            }
                            else
                            {
                                outputwriter.Write((short)(lonstart + tmplonindex * lonlayerwidth + tmplonsmallindex * lonsmalllayerwidth + lonsmalllayerwidth));               // End of Lon
                                if (Isdebug)
                                {
                                    tmpcorr = (lonstart + tmplonindex * lonlayerwidth + tmplonsmallindex * lonsmalllayerwidth + lonsmalllayerwidth);
                                    tmpcorrshort = (short)(lonstart + tmplonindex * lonlayerwidth + tmplonsmallindex * lonsmalllayerwidth + lonsmalllayerwidth);
                                    debuglist.Add(lonstart + tmplonindex * lonlayerwidth + tmplonsmallindex * lonsmalllayerwidth + lonsmalllayerwidth);
                                }
                            }

                            tmpcamx = 0.0;
                            tmpcamy = 0.0;
                            //for (k = 0; k < points[i, j].Count; k++)
                            for (k = 0; k < multilevelpoints[filecounter, i, j].Count; k++)
                            {
                                camdegree = 0.0;
                                camdegree_float = 0.0;
                                camdegree_int = 0;

                                //tmppoint = int.Parse(points[i, j][k].ToString());
                                tmppoint = int.Parse(multilevelpoints[filecounter, i, j][k].ToString());

                                if (IsCHNPOI)
                                {
                                    if ((i == 2) && (filecounter == 8) && ((j == 18) || (j == 23)))
                                    {
                                        camdegree_int = 0;
                                    }
                                }

                                // for debug
                                deblat = double.Parse(DGV_excel.Rows[tmppoint].Cells[1].Value.ToString());
                                deblon = double.Parse(DGV_excel.Rows[tmppoint].Cells[2].Value.ToString());
                                if ((deblat == 31.988021) && (deblon == 120.24699))
                                {
                                    deblat = deblat;
                                    deblon = deblon;
                                }
                                //--------------

                                camdegree = double.Parse(DGV_excel.Rows[tmppoint].Cells[1].Value.ToString());
           
                                //camdegree_int = (int)Math.Floor(camdegree);
                                camdegree_int = (int)Math.Truncate(camdegree);
                                camdegree_float = camdegree - camdegree_int;
                                camdegree = camdegree_int * 100 + camdegree_float * 60;
                                if (k == 0)
                                    tmpcamx = camdegree;
                                //outputwriter.Write((double)(double.Parse(DGV_excel.Rows[tmppoint].Cells[1].Value.ToString())));    // dwLat (Y)
                                //if (Isdebug) debuglist.Add(DGV_excel.Rows[tmppoint].Cells[1].Value.ToString());
                                //outputwriter.Write((double)camdegree);   //20131224

                                if (!doubleorfloat)   // if false: float version
                                {
                                    if (k == 0)
                                        outputwriter.Write(float.Parse(camdegree.ToString()));
                                    else
                                        outputwriter.Write(float.Parse((camdegree - tmpcamx).ToString()));
                                }
                                else     // if true: double version
                                {
                                    outputwriter.Write((double)camdegree);
                                }

                                if (Isdebug) debuglist.Add(camdegree.ToString());

                                //---------------------------- X ---------------------------------------------//
                                camdegree = double.Parse(DGV_excel.Rows[tmppoint].Cells[2].Value.ToString());
                                //camdegree_int = (int)Math.Floor(camdegree);
                                camdegree_int = (int)Math.Truncate(camdegree);
                                camdegree_float = camdegree - camdegree_int;
                                camdegree = camdegree_int * 100 + camdegree_float * 60;
                                if (k == 0)
                                    tmpcamy = camdegree;
                                //outputwriter.Write((double)(double.Parse(DGV_excel.Rows[tmppoint].Cells[2].Value.ToString())));     // dwLon (X)
                                //if (Isdebug) debuglist.Add(DGV_excel.Rows[tmppoint].Cells[2].Value.ToString());
                                //outputwriter.Write((double)camdegree);   //20131224

                                if (!doubleorfloat) //if false: float version
                                {
                                    if (k == 0)
                                        outputwriter.Write(float.Parse(camdegree.ToString()));
                                    else
                                        outputwriter.Write(float.Parse((camdegree - tmpcamy).ToString()));
                                }
                                else                       // if true: double version
                                {
                                    outputwriter.Write((double)camdegree);
                                }

                                if (Isdebug) debuglist.Add(camdegree.ToString());

                                outputwriter.Write((byte)(double.Parse(DGV_excel.Rows[tmppoint].Cells[4].Value.ToString()))); //ubSpeed
                                if (Isdebug) debuglist.Add(DGV_excel.Rows[tmppoint].Cells[4].Value.ToString());

                                outputwriter.Write((byte)(double.Parse(DGV_excel.Rows[tmppoint].Cells[3].Value.ToString()) / 2));  //ubDirection
                                if (Isdebug) debuglist.Add(double.Parse(DGV_excel.Rows[tmppoint].Cells[3].Value.ToString()) / 2);

                                outputwriter.Write((byte)(int.Parse(DGV_excel.Rows[tmppoint].Cells[5].Value.ToString()))); // ubCamera type
                                if (Isdebug) debuglist.Add(DGV_excel.Rows[tmppoint].Cells[5].Value.ToString());

                                outputwriter.Write((byte)(int.Parse(DGV_excel.Rows[tmppoint].Cells[6].Value.ToString()))); //ubDir type
                                if (Isdebug) debuglist.Add(DGV_excel.Rows[tmppoint].Cells[6].Value.ToString());
                            }
                        }

                    }   // end of for (i = 0; i < blockcount*lonblockcount; i++)

                    outputwriter.Flush();
                    output.Flush();
                    outputwriter.Close();
                    output.Close();
                }  // end of for (filecounter)


                if (Isdebug)
                {
                    for (i = 0; i < DGV_excel.RowCount; i++)
                    {
                        outputcamwriter.Write((double)(double.Parse(DGV_excel.Rows[i].Cells[1].Value.ToString())));
                        outputcamwriter.Write((double)(double.Parse(DGV_excel.Rows[i].Cells[2].Value.ToString())));
                        outputcamwriter.Write((byte)(double.Parse(DGV_excel.Rows[i].Cells[4].Value.ToString())));
                        outputcamwriter.Write((byte)(double.Parse(DGV_excel.Rows[i].Cells[3].Value.ToString()) / 2));
                        outputcamwriter.Write((byte)(int.Parse(DGV_excel.Rows[i].Cells[5].Value.ToString())));
                        outputcamwriter.Write((byte)(int.Parse(DGV_excel.Rows[i].Cells[6].Value.ToString())));
                    }
                    outputcamwriter.Flush();
                    outputcamfile.Flush();
                    outputcamwriter.Close();
                    outputcamfile.Close();
                }

                // Clear and Dispose
                //outputwriter.Flush();
                //output.Flush();
                //outputwriter.Close();
                //output.Close();

                if (Isdebug)
                {
                    for (i = 0; i < debuglist.Count; i++)
                        outputdebuglist.WriteLine(debuglist[i].ToString());
                    outputdebuglist.Close();
                    debuglist.Clear();
                }

                /*
                for (i = 0; i < blockcount * lonblockcount; i++)
                    for (j = 0; j < smalllayerblockcount * lonsmalllayerblockcount; j++)                        
                        if (multilevelpoints[filecounter, i, j] != null)
                            multilevelpoints[filecounter, i, j].Clear();
                */
                for (filecounter = 0; filecounter < multilevelpoints.GetLength(0); filecounter++)
                    for (i = 0; i < multilevelpoints.GetLength(1); i++)
                        for (j = 0; j < multilevelpoints.GetLength(2); j++)
                            if (multilevelpoints[filecounter, i, j] != null)
                                multilevelpoints[filecounter, i, j].Clear();

                // ----------------- combine this series of files into one file -------------------------//        
                ulfilesize = new int[lB_nestedslicingrules.Items.Count+1];
                ulchecksum = new uint[lB_nestedslicingrules.Items.Count+1];
                allfilechecksum = new string[lB_nestedslicingrules.Items.Count + 1];
                string tmp;

                // -------------- find out filesize && checksum --------------------------//
                for (filecounter = 0; filecounter <= lB_nestedslicingrules.Items.Count; filecounter++)
                {
                    //seqfilename = System.IO.Path.GetFileNameWithoutExtension(tB_bin.Text) + "_area" + filecounter.ToString() + ".bin";
                    seqfilename = System.IO.Path.GetFileNameWithoutExtension(tB_bin.Text) + filecounter.ToString() + ".bin"; //20150910

                    input = File.Open(seqfilename, FileMode.Open, FileAccess.Read);
                    inputreader = new BinaryReader(input);

                    filedata = inputreader.ReadBytes((int)input.Length);
                    ulfilesize[filecounter] = 0;
                    ulfilesize[filecounter] = (int)input.Length;
                    
                    ulchecksum[filecounter] = 0;
                    
                    /*
                    for (i = 0; i < input.Length; i += 4)
                    {
                        filechecksum = inputreader.ReadBytes(4);                        
                        ulchecksum[filecounter] += BitConverter.ToUInt32(filechecksum, 0); 
                    }               
                    */
                    i = 0;                                   
                    while ( i < input.Length)                    
                    {
                        if ((i + 4) <= input.Length)
                        {
                            
                            filechecksum[i % 4] = filedata[i+3];                            
                            filechecksum[i % 4 + 1] = filedata[i + 2];
                            filechecksum[i % 4 + 2] = filedata[i + 1];
                            filechecksum[i % 4 + 3] = filedata[i];                         

                            ulchecksum[filecounter] += BitConverter.ToUInt32(filechecksum, 0);                            

                            i += 4;
                        }                                                
                        else if ((i + 1) == input.Length)
                        {
                            //filechecksum[i % 4] = filedata[i];
                            
                            // ---- 20140402
                            filechecksum[i % 4] = (byte)0;
                            filechecksum[i % 4 + 1] = (byte)0;
                            filechecksum[i % 4 + 2] = (byte)0;
                            filechecksum[i % 4 + 3] = filedata[i];             
                            //-------

                            ulchecksum[filecounter] += BitConverter.ToUInt32(filechecksum, 0);
                            i += 1;
                        }
                        else if ((i + 2) == input.Length)
                        {
                            //filechecksum[i % 4] = filedata[i];
                            //filechecksum[i % 4 + 1] = filedata[i + 1];                        

                            //-------------- 20140402
                            filechecksum[i % 4] = (byte)0;
                            filechecksum[i % 4 + 1] = (byte)0;
                            filechecksum[i % 4 + 2] = filedata[i + 1];
                            filechecksum[i % 4 + 3] = filedata[i];
                            //---------------------

                            ulchecksum[filecounter] += BitConverter.ToUInt32(filechecksum, 0);
                            i += 2;
                        }
                        else if ((i + 3) == input.Length)
                        {
                            //filechecksum[i % 4] = filedata[i];
                            //filechecksum[i % 4 + 1] = filedata[i + 1];
                            //filechecksum[i % 4 + 2] = filedata[i + 2];

                            //-------------- 20140402
                            filechecksum[i % 4] = (byte)0;
                            filechecksum[i % 4 + 1] = filedata[i + 2];
                            filechecksum[i % 4 + 2] = filedata[i + 1];
                            filechecksum[i % 4 + 3] = filedata[i];
                            //---------------------

                            ulchecksum[filecounter] += BitConverter.ToUInt32(filechecksum, 0);
                            i += 3;
                        }

                        for (j = 0; j < 4; j++)
                            filechecksum[j] = 0;
                    }

                    //temp: 20140402                                       
                    //tmp = ulchecksum[filecounter].ToString("X");
                    /*
                    byte[] tmpdata = BitConverter.GetBytes(ulchecksum[filecounter]);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(tmpdata);
                    tmp = BitConverter.ToInt32(tmpdata, 0).ToString("X");
                    allfilechecksum[filecounter] = BitConverter.ToInt32(tmpdata, 0).ToString("X");
                    */
                    //---------------------- 20140402

                    input.Close();
                    inputreader.Close();
                }

                //--------- 20140402 ----------------------------//
                //seqfilename = System.IO.Path.GetFileNameWithoutExtension(tB_bin.Text) + "_area" + filecounter.ToString() + ".bin";
                /*
                tmp = System.IO.Path.GetFileNameWithoutExtension(tB_bin.Text) + "_checksum" + ".txt";
                FileStream output_tmp = File.Create(tmp);
                BinaryWriter outputwriter_tmp = new BinaryWriter(output_tmp);
                for (i = 0; i < allfilechecksum.Length; i++)
                {
                    outputwriter_tmp.Write(allfilechecksum[i]);
                    outputwriter_tmp.Write("\n");
                }
                outputwriter_tmp.Flush();
                outputwriter_tmp.Close();
                output_tmp.Close();
                */
                // ---------------------------------------------------//


                output = File.Create(tB_bin.Text);
                outputwriter = new BinaryWriter(output);

                totalbyteoutput = 0;
                // ------ Header Info: sector 0 -----------------//
                outputwriter.Write((uint)signature);            // UINT32: Signature
                outputwriter.Write((ulong)skuid);               // 2*ULONG SKUID;
                outputwriter.Write((ulong)skuid);           
                for (i = 0; i < 32; i++)                                   // UBYTE ID[32];
                    outputwriter.Write((byte)id);
                outputwriter.Write((uint)flag);                     // UINT32 flag;

                totalbyteoutput = totalbyteoutput + sizeof(uint) + sizeof(ulong) * 2 + sizeof(byte) * 32 + sizeof(uint);
                j = (int)totalbyteoutput;
                for (i = j; i < sectorsize; i++)     // 512-byte alignment
                {
                    outputwriter.Write((byte)j);
                    j++;
                }

                // ------ Header Info: sector 1 ----------------//
                totalbyteoutput = j;
                ubtotalpart = lB_nestedslicingrules.Items.Count + 1;
                outputwriter.Write((uint)ubtotalpart);        // UINT32 ubtotalpart;
                totalbyteoutput += sizeof(uint);

                //filecounter(lB_nestedslicingrules.Items.Count): the outest overall area                                
                for (filecounter = 0; filecounter <= lB_nestedslicingrules.Items.Count; filecounter++)
                {
                    if (filecounter == 0)
                        uloffset = 0;
                    else
                    {
                        uloffset += ulfilesize[filecounter - 1];
                    }

                    outputwriter.Write((uint)uloffset);
                    outputwriter.Write((uint)ulfilesize[filecounter]);
                    outputwriter.Write((uint)ulchecksum[filecounter]);

                    totalbyteoutput = totalbyteoutput + 3*sizeof(uint);
                }

                // ------ Header Info: other ------------------ //
                j = (int)totalbyteoutput;
                for (i = j; i < CHNglobalheader; i++)
                {
                    outputwriter.Write((byte)j);
                    j++;
                }


                // ------ Info: sequence of files ----- //
                for (filecounter = 0; filecounter <= lB_nestedslicingrules.Items.Count; filecounter++)
                {
                    //seqfilename = System.IO.Path.GetFileNameWithoutExtension(tB_bin.Text) + "_area" + filecounter.ToString() + ".bin";
                    seqfilename = System.IO.Path.GetFileNameWithoutExtension(tB_bin.Text) +  filecounter.ToString() + ".bin"; //20150910

                    input = File.Open(seqfilename, FileMode.Open, FileAccess.Read);
                    inputreader = new BinaryReader(input);
                    byte[] binarydata = inputreader.ReadBytes((int)input.Length);                    
                    outputwriter.Write(binarydata, 0, binarydata.Length);

                    input.Close();
                    inputreader.Close();
                }
                

            }
            else     // if not (IsCHNPOI)
            {

                //FileStream output = File.Create(tB_bin.Text);            
                if (!System.IO.Directory.Exists(tB_bin.Text.Trim()))
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(tB_bin.Text.Trim()));

                //FileStream output = File.Create(tB_bin.Text);
                //BinaryWriter outputwriter = new BinaryWriter(output);
                output = File.Create(tB_bin.Text);
                outputwriter = new BinaryWriter(output);

                // For debug, 
                bool Isdebug = false;     //20131209, set to false

                if (Isdebug)
                {
                    debuglist = new ArrayList();

                    if (mode)    //GUI
                    {
                        tmpcamfilename = System.IO.Path.ChangeExtension(OFDlg1.FileName, "txt");
                        outputdebuglist = new StreamWriter(tmpcamfilename);

                        tmpcamfilename = System.IO.Path.GetDirectoryName(OFDlg1.FileName);
                        tmpcamfilename = tmpcamfilename + "\\SpeedCam_Data_debug_points.bin";
                    }
                    else
                    {
                        tmpcamfilename = System.IO.Path.ChangeExtension(tB_bin.Text, "txt");
                        outputdebuglist = new StreamWriter(tmpcamfilename);

                        tmpcamfilename = System.IO.Path.GetDirectoryName(tmpcamfilename);
                        tmpcamfilename = tmpcamfilename + "\\SpeedCam_Data_debug_points.bin";
                    }
                    outputcamfile = File.Create(tmpcamfilename);
                    outputcamwriter = new BinaryWriter(outputcamfile);
                }

                //continue to verify
                layerwidth = (double)(latend - latstart) / blockcount;
                lonlayerwidth = (double)(lonend - lonstart) / lonblockcount;
                smalllayerwidth = (double)layerwidth / smalllayerblockcount;
                lonsmalllayerwidth = (double)lonlayerwidth / lonsmalllayerblockcount;

                // output order: Global, Large layer, Small layer
                // 1. Global header            
                outputwriter.Write((short)lonstart); // Start position of Longitude
                if (Isdebug) debuglist.Add(lonstart);
                outputwriter.Write((short)latstart);  // Start position of Latitude
                if (Isdebug) debuglist.Add(latstart);
                outputwriter.Write((short)lonend);  // End position of Longitude
                if (Isdebug) debuglist.Add(lonend);
                outputwriter.Write((short)latend);   // End position of Latitude
                if (Isdebug) debuglist.Add(latend);

                outputwriter.Write((ushort)lonsmalllayerblockcount); // Small Layer Block Number of Longitude
                if (Isdebug) debuglist.Add(lonsmalllayerblockcount);
                outputwriter.Write((ushort)smalllayerblockcount);   // Small Layer Block Number of Latitude
                if (Isdebug) debuglist.Add(smalllayerblockcount);
                outputwriter.Write((ushort)lonblockcount); // Large Layer Block Number of Longitude
                if (Isdebug) debuglist.Add(lonblockcount);
                outputwriter.Write((ushort)blockcount);  // Small Layer Block Number of Latitude                               
                if (Isdebug) debuglist.Add(blockcount);

                // version of the camera data
                outputwriter.Write((byte)(dataversion));
                if (Isdebug) debuglist.Add(dataversion);

                //for 4-byte alignment
                for (i = 1; i <= 3; i++)
                    outputwriter.Write((byte)' ');

                // for MiTAC mioversion [4]
                outputwriter.Write((uint)mioversion);
                if (Isdebug) debuglist.Add(mioversion);

                //for systemtime [12]
                str_datetime += '\0';
                char[] tmpchar1 = str_datetime.ToCharArray();
                for (i = 0; i < tmpchar1.Length; i++)
                {
                    outputwriter.Write((byte)tmpchar1[i]);
                    if (Isdebug) debuglist.Add(tmpchar1[i]);
                }
                //for (i = 1; i <= 3; i++)
                for (i = 1; i <= (12-tmpchar1.Length); i++)
                    outputwriter.Write((byte)' ');


                //sizeofglobalheader = sizeof(ushort) * 8 + 4;
                //sizeofglobalheader = sizeof(ushort) * 8 + sizeof(byte) * 4;
                sizeofglobalheader = sizeof(ushort) * 8 + sizeof(byte) * 4 + sizeof(uint) * 1 + sizeof(byte) * 12; // new add: Version[4], SysteTime[12]
                sizeoflargeblockheader = sizeof(ushort) * 5;
                sizeofsmallblockheader = sizeof(ushort) * 5;
                //sizeofeachcamera = 20;            

                if (doubleorfloat)   // true: double version
                    sizeofeachcamera = sizeof(double) * 2 + sizeof(byte) * 4;  //20
                else                         // false: float version
                    sizeofeachcamera = sizeof(float) * 2 + sizeof(byte) * 4;     // 12

                //verify POI counts
                tmp_POIcontained2 = 0;
                for (ii = 0; ii < points.GetLength(0); ii++)
                    for (jj = 0; jj < points.GetLength(1); jj++)
                        tmp_POIcontained2 += points[ii, jj].Count;
                tmp_POIcontained2 = 0;

                // largelayer[i]: # of points contained in the i-th large layer                                             
                for (i = 0; i < blockcount * lonblockcount; i++)
                {
                    tmpcount = 0;
                    for (j = 0; j < smalllayerblockcount * lonsmalllayerblockcount; j++)
                        tmpcount += points[i, j].Count;
                    largelayer[i] = tmpcount;
                }

                int tttt = 0;  // tttt: total number of points
                for (i = 0; i < blockcount * lonblockcount; i++)
                    tttt += largelayer[i];

                // largelayerbytes[i]: # of bytes required for the i-th large layer
                for (i = 0; i < blockcount * lonblockcount; i++)
                    //largelayerbytes[i] = largelayer[i] * sizeofeachcamera + sizeofsmallblockheader * smalllayerblockcount * lonsmalllayerblockcount;
                    largelayerbytes[i] = largelayer[i] * sizeofeachcamera + sizeofsmallblockheader * smalllayerblockcount * lonsmalllayerblockcount +
                                                        sizeof(uint) * smalllayerblockcount * lonsmalllayerblockcount + sizeoflargeblockheader;

                // for large layer # 0
                largelayeroffset = sizeofglobalheader + sizeof(uint) * blockcount * lonblockcount;

                // for 4-byte alignment
                //largelayeroffset += 3;

                outputwriter.Write((uint)(largelayeroffset));
                if (Isdebug) debuglist.Add(largelayeroffset);

                // for subsequent large layer
                tmpoffset = largelayeroffset;
                for (i = 1; i < blockcount * lonblockcount; i++)
                {
                    tmpoffset = tmpoffset + largelayerbytes[i - 1];
                    outputwriter.Write((uint)(tmpoffset));
                    if (Isdebug) debuglist.Add(tmpoffset);
                }

                // 2. Larger layer header                        
                for (i = 0; i < blockcount * lonblockcount; i++)
                {
                    outputwriter.Write((ushort)largelayer[i]);                                                             // # of points
                    if (Isdebug) debuglist.Add(largelayer[i]);
                    tmplatindex = (int)Math.Floor((double)(i / lonblockcount));
                    tmplonindex = i - tmplatindex * lonblockcount;

                    outputwriter.Write((short)(latstart + tmplatindex * layerwidth));                    // Start Latitude
                    if (Isdebug) debuglist.Add(latstart + tmplatindex * layerwidth);
                    outputwriter.Write((short)(lonstart + tmplonindex * lonlayerwidth));             // Start Lontitude
                    if (Isdebug) debuglist.Add(lonstart + tmplonindex * lonlayerwidth);

                    if ((latstart + tmplatindex * layerwidth + layerwidth) < latend)
                    {
                        outputwriter.Write((short)(latstart + tmplatindex * layerwidth + layerwidth)); // End of Latitude
                        if (Isdebug) debuglist.Add(latstart + tmplatindex * layerwidth + layerwidth);
                    }
                    else
                    {
                        outputwriter.Write((short)latend);
                        if (Isdebug) debuglist.Add(latend);
                    }

                    if ((lonstart + tmplonindex * lonlayerwidth + lonlayerwidth) < lonend)
                    {
                        outputwriter.Write((short)(lonstart + tmplonindex * lonlayerwidth + lonlayerwidth));    // End of Longitude
                        if (Isdebug) debuglist.Add(lonstart + tmplonindex * lonlayerwidth + lonlayerwidth);
                    }
                    else
                    {
                        outputwriter.Write((short)lonend);
                        if (Isdebug) debuglist.Add(lonend);
                    }

                    // for smaller block # 0
                    if (i == 0)
                        smalllayeroffset = largelayeroffset + sizeoflargeblockheader + sizeof(uint) * smalllayerblockcount * lonsmalllayerblockcount;
                    else
                    {
                        tmpsmalloffset = 0;
                        for (j = i; j > 0; j--)
                        {
                            tmpsmalloffset = tmpsmalloffset + largelayerbytes[j - 1];

                        }
                        smalllayeroffset = largelayeroffset + tmpsmalloffset + sizeoflargeblockheader + sizeof(uint) * smalllayerblockcount * lonsmalllayerblockcount;

                        //smalllayeroffset = largelayeroffset + largelayerbytes[i - 1] + sizeoflargeblockheader + sizeof(uint) * smalllayerblockcount * lonsmalllayerblockcount;                    
                    }

                    outputwriter.Write((uint)(smalllayeroffset));
                    if (Isdebug) debuglist.Add(smalllayeroffset);

                    // for subsequent smaller block
                    tmpsmalloffset = smalllayeroffset;
                    for (j = 1; j < smalllayerblockcount * lonsmalllayerblockcount; j++)
                    {
                        tmpsmalloffset = tmpsmalloffset + sizeofsmallblockheader + sizeofeachcamera * points[i, j - 1].Count;
                        // a. # of points in each small layer
                        outputwriter.Write((uint)(tmpsmalloffset));
                        if (Isdebug) debuglist.Add(tmpsmalloffset);
                    }

                    // 3. Small Layer header, for each point in each small layer
                    for (j = 0; j < smalllayerblockcount * lonsmalllayerblockcount; j++)
                    {
                        outputwriter.Write((ushort)points[i, j].Count);                                               // # of points
                        if (Isdebug) debuglist.Add(points[i, j].Count);
                        //tmplatsmallindex = (int)Math.Floor((double)j / smalllayerblockcount);
                        tmplatsmallindex = (int)Math.Floor((double)j / lonsmalllayerblockcount);
                        tmplonsmallindex = j - tmplatsmallindex * lonsmalllayerblockcount;
                        outputwriter.Write((short)(latstart + tmplatindex * layerwidth + tmplatsmallindex * smalllayerwidth));  // Start Latitude
                        if (Isdebug) debuglist.Add(latstart + tmplatindex * layerwidth + tmplatsmallindex * smalllayerwidth);
                        outputwriter.Write((short)(lonstart + tmplonindex * lonlayerwidth + tmplonsmallindex * lonsmalllayerwidth));  // Start of Lon
                        if (Isdebug) debuglist.Add(lonstart + tmplonindex * lonlayerwidth + tmplonsmallindex * lonsmalllayerwidth);

                        if ((latstart + tmplatindex * layerwidth + tmplatsmallindex * smalllayerwidth + smalllayerwidth) > (latstart + tmplatindex * layerwidth + layerwidth))
                        {
                            outputwriter.Write((short)(latstart + tmplatindex * layerwidth + layerwidth));                // End of Lat
                            if (Isdebug) debuglist.Add(latstart + tmplatindex * layerwidth + layerwidth);
                        }
                        else
                        {
                            outputwriter.Write((short)(latstart + tmplatindex * layerwidth + tmplatsmallindex * smalllayerwidth + smalllayerwidth));                // End of Lat
                            if (Isdebug) debuglist.Add(latstart + tmplatindex * layerwidth + tmplatsmallindex * smalllayerwidth + smalllayerwidth);
                        }

                        if ((lonstart + tmplonindex * lonlayerwidth + tmplonsmallindex * lonsmalllayerwidth + lonsmalllayerwidth) > (lonstart + tmplonindex * lonlayerwidth + lonlayerwidth))
                        {
                            outputwriter.Write((short)(lonstart + tmplonindex * lonlayerwidth + lonlayerwidth));               // End of Lon
                            if (Isdebug) debuglist.Add(lonstart + tmplonindex * lonlayerwidth + lonlayerwidth);
                        }
                        else
                        {
                            outputwriter.Write((short)(lonstart + tmplonindex * lonlayerwidth + tmplonsmallindex * lonsmalllayerwidth + lonsmalllayerwidth));               // End of Lon
                            if (Isdebug) debuglist.Add(lonstart + tmplonindex * lonlayerwidth + tmplonsmallindex * lonsmalllayerwidth + lonsmalllayerwidth);
                        }

                        tmpcamx = 0.0;
                        tmpcamy = 0.0;
                        for (k = 0; k < points[i, j].Count; k++)
                        {
                            camdegree = 0.0;
                            camdegree_float = 0.0;
                            camdegree_int = 0;

                            tmppoint = int.Parse(points[i, j][k].ToString());

                            camdegree = double.Parse(DGV_excel.Rows[tmppoint].Cells[1].Value.ToString());
                            //camdegree_int = (int)Math.Floor(camdegree);
                            camdegree_int = (int)Math.Truncate(camdegree);
                            camdegree_float = camdegree - camdegree_int;
                            camdegree = camdegree_int * 100 + camdegree_float * 60;
                            if (k == 0)
                                tmpcamx = camdegree;
                            //outputwriter.Write((double)(double.Parse(DGV_excel.Rows[tmppoint].Cells[1].Value.ToString())));    // dwLat (Y)
                            //if (Isdebug) debuglist.Add(DGV_excel.Rows[tmppoint].Cells[1].Value.ToString());
                            //outputwriter.Write((double)camdegree);   //20131224

                            if (!doubleorfloat)   // if false: float version
                            {
                                if (k == 0)
                                    outputwriter.Write(float.Parse(camdegree.ToString()));
                                else
                                    outputwriter.Write(float.Parse((camdegree - tmpcamx).ToString()));
                            }
                            else     // if true: double version
                            {
                                outputwriter.Write((double)camdegree);
                            }

                            if (Isdebug) debuglist.Add(camdegree.ToString());

                            //---------------------------- X ---------------------------------------------//
                            camdegree = double.Parse(DGV_excel.Rows[tmppoint].Cells[2].Value.ToString());
                            //camdegree_int = (int)Math.Floor(camdegree);
                            camdegree_int = (int)Math.Truncate(camdegree);
                            camdegree_float = camdegree - camdegree_int;
                            camdegree = camdegree_int * 100 + camdegree_float * 60;
                            if (k == 0)
                                tmpcamy = camdegree;
                            //outputwriter.Write((double)(double.Parse(DGV_excel.Rows[tmppoint].Cells[2].Value.ToString())));     // dwLon (X)
                            //if (Isdebug) debuglist.Add(DGV_excel.Rows[tmppoint].Cells[2].Value.ToString());
                            //outputwriter.Write((double)camdegree);   //20131224

                            if (!doubleorfloat) //if false: float version
                            {
                                if (k == 0)
                                    outputwriter.Write(float.Parse(camdegree.ToString()));
                                else
                                    outputwriter.Write(float.Parse((camdegree - tmpcamy).ToString()));
                            }
                            else                       // if true: double version
                            {
                                outputwriter.Write((double)camdegree);
                            }

                            if (Isdebug) debuglist.Add(camdegree.ToString());

                            outputwriter.Write((byte)(double.Parse(DGV_excel.Rows[tmppoint].Cells[4].Value.ToString()))); //ubSpeed
                            if (Isdebug) debuglist.Add(DGV_excel.Rows[tmppoint].Cells[4].Value.ToString());

                            outputwriter.Write((byte)(double.Parse(DGV_excel.Rows[tmppoint].Cells[3].Value.ToString()) / 2));  //ubDirection
                            if (Isdebug) debuglist.Add(double.Parse(DGV_excel.Rows[tmppoint].Cells[3].Value.ToString()) / 2);

                            outputwriter.Write((byte)(int.Parse(DGV_excel.Rows[tmppoint].Cells[5].Value.ToString()))); // ubCamera type
                            if (Isdebug) debuglist.Add(DGV_excel.Rows[tmppoint].Cells[5].Value.ToString());

                            outputwriter.Write((byte)(int.Parse(DGV_excel.Rows[tmppoint].Cells[6].Value.ToString()))); //ubDir type
                            if (Isdebug) debuglist.Add(DGV_excel.Rows[tmppoint].Cells[6].Value.ToString());

                            if (dataversion == 4)
                            {
                                outputwriter.Write(uint.Parse(autodoriapair_idx[tmppoint].ToString())); //autodoria pair
                                if (Isdebug) debuglist.Add(autodoriapair_idx[tmppoint]);
                            }
                        }
                    }

                }   // end of for (i = 0; i < blockcount*lonblockcount; i++)

                if (Isdebug)
                {
                    for (i = 0; i < DGV_excel.RowCount; i++)
                    {
                        outputcamwriter.Write((double)(double.Parse(DGV_excel.Rows[i].Cells[1].Value.ToString())));
                        outputcamwriter.Write((double)(double.Parse(DGV_excel.Rows[i].Cells[2].Value.ToString())));
                        outputcamwriter.Write((byte)(double.Parse(DGV_excel.Rows[i].Cells[4].Value.ToString())));
                        outputcamwriter.Write((byte)(double.Parse(DGV_excel.Rows[i].Cells[3].Value.ToString()) / 2));
                        outputcamwriter.Write((byte)(int.Parse(DGV_excel.Rows[i].Cells[5].Value.ToString())));
                        outputcamwriter.Write((byte)(int.Parse(DGV_excel.Rows[i].Cells[6].Value.ToString())));
                    }
                    outputcamwriter.Flush();
                    outputcamfile.Flush();
                    outputcamwriter.Close();
                    outputcamfile.Close();
                }
                // Clear and Dispose
                outputwriter.Flush();
                output.Flush();
                outputwriter.Close();
                output.Close();
                if (Isdebug)
                {
                    for (i = 0; i < debuglist.Count; i++)
                        outputdebuglist.WriteLine(debuglist[i].ToString());
                    outputdebuglist.Close();
                    debuglist.Clear();
                }

                for (i = 0; i < blockcount * lonblockcount; i++)
                    for (j = 0; j < smalllayerblockcount * lonsmalllayerblockcount; j++)
                        points[i, j].Clear();            


            }  // end of If (IsCHNPOI)
            
        }

        //-------------------------------------------------------------
        public class LoadIni
        {
            public string path = null;
            [DllImport("kernel32", CharSet = CharSet.Unicode)]
            private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
            [DllImport("kernel32", CharSet = CharSet.Unicode)]
            private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filepath);
            public void IniWriteValue(string section, string key, string Value, string inifilename)
            {
                //WritePrivateProfileString(section, key, Value, Application.StartupPath + "\\" + inifilename);
                WritePrivateProfileString(section, key, Value, inifilename);
            }
            public string IniReadValue(string section, string key, string inifilename)
            {
                StringBuilder tmp = new StringBuilder(255);
                //int i = GetPrivateProfileString(section, key, "", tmp, 255, Application.StartupPath + "\\" + inifilename);
                int i = GetPrivateProfileString(section, key, "", tmp, 255, inifilename);
                return tmp.ToString();
            }
        }

        //-----------------------------------------------------------------
        public void LoadIniFile(string inifile)
        {            
            LoadIni ini = new LoadIni();
            string exepath = null;
            string[] words;
            int i;            
            
            //successloglist.Clear();
            //errorloglist.Clear();

            if (File.Exists(inifile))
            {
                // to check if absolute path or relative path, use Path.IsPathRooted                
                exepath = System.IO.Path.GetFullPath(inifile);

                //skuid = ini.IniReadValue(sectionname, "SKUID", inifile);

                successlogstr = ini.IniReadValue(sectionname, successlogname, inifile);
                //successlogpath = System.IO.Path.GetFullPath(successlogpath);                             
                successlogpath = Application.ExecutablePath;
                words = successlogpath.Split('\\');
                successlogpath = string.Empty;
                for (i = 0; i < words.Length -1; i++)
                    successlogpath += words[i] + "\\";
                successlogstr = successlogpath + successlogstr;

                errorlogstr = ini.IniReadValue(sectionname, errorlogname, inifile);
                //errorlogpath = System.IO.Path.GetFullPath(errorlogpath);
                errorlogpath = Application.ExecutablePath;
                words = errorlogpath.Split('\\');
                errorlogpath = string.Empty;
                for (i = 0; i < words.Length - 1; i++)
                    errorlogpath += words[i] + "\\";
                errorlogstr = errorlogpath + errorlogstr;

                rawlogstr = ini.IniReadValue(sectionname, rawlogname, inifile);
                //rawlogpath = System.IO.Path.GetFullPath(rawlogpath);                
                rawlogpath = Application.ExecutablePath;
                words = rawlogpath.Split('\\');
                rawlogpath = string.Empty;
                for (i = 0; i < words.Length - 1; i++)
                    rawlogpath += words[i] + "\\";
                rawlogstr = rawlogpath + rawlogstr;
                
                //MessageBox.Show("rawlogpath = " + rawlogpath);
            }
        }

        //----------------------------------------------------------------
        public void SaveLogFile(string inifile)
        {
            string outputlogfile = null;
            int i;
            StreamWriter outputsuccesslog = null;
            StreamWriter outputerrorlog = null;
            string tmpstr = null;
            string[] words;

            if (errorloglist.Count > 0)
                executionstatus = false;    // return "failed"
            else
                executionstatus = true;     // return "successful"

            if (File.Exists(inifile))
            {
                //outputlogfile = System.IO.Path.GetDirectoryName(inifile);
                outputlogfile = successlogstr;
                if (!System.IO.Directory.Exists(outputlogfile.ToString().Trim()))
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputlogfile.ToString().Trim()));                
            }
            else
            {
                /*
                outputlogfile = Application.StartupPath;                
                outputlogfile += "\\" + successlogname + ".txt";
                */
                outputlogfile = Application.ExecutablePath; 
                words = outputlogfile.Split('\\');
                outputlogfile = string.Empty;
                for (i = 0; i < words.Length - 1; i++)
                    outputlogfile += words[i] + "\\";
                outputlogfile = outputlogfile + successlogname + ".txt";
            }

            //MessageBox.Show("successlogname = " + successlogname);
            //MessageBox.Show("outputlogfile = " + outputlogfile);
            //Save successloglist
            if (successloglist.Count > 0)
            {
                outputsuccesslog = new StreamWriter(outputlogfile);
                outputsuccesslog.WriteLine("total data: " + successloglist.Count.ToString());

                tmpstr = string.Empty;
                for (i = 0; i < DGV_excel.Columns.Count; i++)
                {
                    if (DGV_excel.Columns[i].Name.ToString() == "Layers")
                        break;
                    else
                        tmpstr += DGV_excel.Columns[i].Name.ToString() + "    ";
                }
                outputsuccesslog.WriteLine(tmpstr);

                for (i = 0; i < 80; i++)
                    outputsuccesslog.Write("=");
                outputsuccesslog.WriteLine(" ");

                for (i = 0; i < successloglist.Count; i++)
                    outputsuccesslog.WriteLine(successloglist[i].ToString());                    
                outputsuccesslog.Flush();
                outputsuccesslog.Close();
                successloglist.Clear();                
            }

            if (errorloglist.Count > 0)
            {
                if (File.Exists(inifile))
                {
                    //outputlogfile = System.IO.Path.GetDirectoryName(inifile);
                    outputlogfile = errorlogstr;
                    if (!System.IO.Directory.Exists(outputlogfile.ToString().Trim()))
                        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputlogfile.ToString().Trim()));
                }
                else
                {
                    /*
                    outputlogfile = Application.StartupPath;
                    outputlogfile += "\\" + errorlogname + ".txt";
                    */

                    outputlogfile = Application.ExecutablePath;
                    words = outputlogfile.Split('\\');
                    outputlogfile = string.Empty;
                    for (i = 0; i < words.Length - 1; i++)
                        outputlogfile += words[i] + "\\";
                    outputlogfile = outputlogfile + errorlogname + ".txt";
                }

                outputerrorlog = new StreamWriter(outputlogfile);
                outputerrorlog.WriteLine("total data: " + errorloglist.Count.ToString());
                tmpstr = string.Empty;

                for (i = 0; i < DGV_excel.Columns.Count; i++)
                {
                    if (DGV_excel.Columns[i].Name.ToString() == "Layers")
                        break;
                    else
                        tmpstr += DGV_excel.Columns[i].Name.ToString() + "    ";
                }
                outputerrorlog.WriteLine(tmpstr);

                for (i = 0; i < 80; i++)
                    outputerrorlog.Write("=");
                outputerrorlog.WriteLine(" ");

                for (i = 0; i < errorloglist.Count; i++)
                    outputerrorlog.WriteLine(errorloglist[i].ToString());
                outputerrorlog.Flush();
                outputerrorlog.Close();
                errorloglist.Clear();

            }
        }

        /*
        public class ReportStatus
        {            
            [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
            private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
            private static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, int lParam);            
        }
        */

        public void ExitReport()
        {
            int returnvalue;

            IntPtr hWnd = FindWindow("#32770", "Mivue Converter");

            if (hWnd != null)
            {
                // Send a status code, "1" is successful, "0" is failed
                if (executionstatus)
                    returnvalue = SendMessage(hWnd, WM_CONVERTER_NOTIFY, 1, 0);
                else
                    returnvalue = SendMessage(hWnd, WM_CONVERTER_NOTIFY, 0, 0);
            }
            else
            {
                throw new Exception("Can't find MivueConverter !!");
            }

            /*
            if (hWnd == null)
                MessageBox.Show("hWnd is null, and SendMessage return: " + returnvalue.ToString());
            else
                MessageBox.Show("hWnd exists, and SendMessage return: " + returnvalue.ToString());
            */
        }

        public void showPOIinfo(int threshold)
        {
            int i, j;
            int largestlargeindex = -1, largestsmallindex = -1, leastlargeindex = -1;
            long largestlargePOIcount = 0, largestsmallPOIcount = 0, leastlargePOIcount = 1000;
            long tmpPOIintotalsmall = 0;

            lB_BlockInfo.Items.Clear();                        
            // extract info of points[i, j] 
            for (i = 0; i < blockcount * lonblockcount; i++)
            {
                tmpPOIintotalsmall = 0;                
                for (j = 0; j < smalllayerblockcount * lonsmalllayerblockcount; j++)
                {
                    /*
                    if (points[i, j].Count == 0)
                        lB_BlockInfo.Items.Add("[" + i.ToString() + ", " + j.ToString() + "] : 0");
                    else if (points[i, j].Count > 0)
                        lB_BlockInfo.Items.Add("[" + i.ToString() + ", " + j.ToString() + "] : " + points[i, j].Count.ToString());
                    */
                    if (points[i, j].Count > 0)
                        tmpPOIintotalsmall += points[i, j].Count;
                }

                if (threshold > 0)
                {
                    if (tmpPOIintotalsmall >= threshold)
                        lB_BlockInfo.Items.Add("[" + i.ToString() + "] : " + tmpPOIintotalsmall.ToString());
                }
                else if (threshold == 0)
                {
                    if (tmpPOIintotalsmall > 0)
                        lB_BlockInfo.Items.Add("[" + i.ToString() + "] : " + tmpPOIintotalsmall.ToString());
                    else
                        lB_BlockInfo.Items.Add("[" + i.ToString() + "] : 0");
                }

                if (tmpPOIintotalsmall > largestlargePOIcount)
                {
                    largestlargeindex = i;
                    largestlargePOIcount = tmpPOIintotalsmall;
                }

                if (tmpPOIintotalsmall < leastlargePOIcount)
                {
                    leastlargeindex = i;
                    leastlargePOIcount = tmpPOIintotalsmall;
                }
            }

            if (largestlargeindex > -1)
            {
                tB_BlockInfoLeastBlock.Text = leastlargeindex.ToString();
                tB_BlockInfoLeastBlockPOICount.Text = leastlargePOIcount.ToString();

                tB_BlockInfoLargestBlock.Text = largestlargeindex.ToString();
                tB_BlockInfoLargestBlockPOICount.Text = largestlargePOIcount.ToString();
                largestsmallindex = -1;
                largestsmallPOIcount = 0;

                for (j = 0; j < smalllayerblockcount * lonsmalllayerblockcount; j++)
                {
                    if (points[largestlargeindex, j].Count > largestsmallPOIcount)
                    {
                        largestsmallindex = j;
                        largestsmallPOIcount = points[largestlargeindex, j].Count;
                    }
                }

                if (largestsmallindex > -1)
                {
                    tB_BlockInfoLargestSmallBlock.Text = largestsmallindex.ToString();
                    tB_BlockInfoLargestSmallBlockPOICount.Text = largestsmallPOIcount.ToString();
                }

                btn_DivisionOverview.Enabled = true;
            }
        }


        //--------------------------------- End of Private Function ------------------------------------//

        private void btn_openexcel_Click(object sender, EventArgs e)
        {
            //DGV_excel.Rows.Clear();
            //dTableOut.Clear();
            OFDlg1.ShowDialog();
        }

        private void OFDlg1_FileOk(object sender, CancelEventArgs e)
        {           
            if (OFDlg1.CheckFileExists)
            {
                //LoadExcelFile(OFDlg1.FileName);
                //inifile = System.IO.Path.GetDirectoryName(OFDlg1.FileName) + "\\Setting.ini";
                inifile = Application.StartupPath + "\\speedcam_settings.ini";
                LoadIniFile(inifile);
                LoadExcelFile(OFDlg1.FileName);
            }
            else
            {
                MessageBox.Show("File no exists");
                btn_openexcel.Focus();
            }
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            try
            {
                cB_subarea.Enabled = false;
                cB_autodoria.Enabled = false;
                speedcamconvert();
                btn_clear.Enabled = false;
                btn_savebin.Enabled = true;
                btn_start.Enabled = false;
            }
            catch
            {
                MessageBox.Show("Insufficient memory");
                Application.Exit();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void tB_LayerWidth_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void tB_LayerWidth_Leave(object sender, EventArgs e)
        {
            if (tB_LayerWidth.Text.Length > 0)
                if (!double.TryParse(tB_LayerWidth.Text, out layerwidth))
                {
                    MessageBox.Show("Wrong Number!");
                    tB_LayerWidth.Clear();
                }
                else
                {
                    tB_BlockCount.Enabled = false;
                    tB_BlockCount.Clear();
                    blockcount = 0;
                }
        }

        private void tB_BlockCount_Leave(object sender, EventArgs e)
        {
            if (tB_BlockCount.Text.Length > 0)
                if (!int.TryParse(tB_BlockCount.Text, out blockcount))
                {
                    MessageBox.Show("Wrong Number!");
                    tB_BlockCount.Clear();
                }
                else
                {
                    tB_LayerWidth.Enabled = false;
                    tB_LayerWidth.Clear();
                    layerwidth = 0.0;
                }
        }

        private void tB_SmallLayerWidth_Leave(object sender, EventArgs e)
        {
            if (tB_SmallLayerWidth.Text.Length > 0)
                if (!double.TryParse(tB_SmallLayerWidth.Text, out smalllayerwidth))
                {
                    MessageBox.Show("Wrong Number!");
                    tB_SmallLayerWidth.Clear();
                }
                else
                {
                    tB_SmallBlockCount.Enabled = false;
                    tB_SmallBlockCount.Clear();
                    smalllayerblockcount = 0;
                }
        }

        private void tB_SmallBlockCount_Leave(object sender, EventArgs e)
        {
            if (tB_SmallBlockCount.Text.Length > 0)
                if (!int.TryParse(tB_SmallBlockCount.Text, out smalllayerblockcount))
                {
                    MessageBox.Show("Wrong Number!");
                    tB_SmallBlockCount.Clear();
                }
                else
                {
                    tB_SmallLayerWidth.Enabled = false;
                    tB_SmallLayerWidth.Clear();
                    smalllayerwidth = 0.0;
                }
        }

        private void tB_latstart_Leave(object sender, EventArgs e)
        {
            if (tB_latstart.Text.Length > 0)
                if (!double.TryParse(tB_latstart.Text, out latstart))
                {
                    MessageBox.Show("Wrong Number!");
                    tB_latstart.Clear();
                }        
        }

        private void tB_latend_Leave(object sender, EventArgs e)
        {
            if (tB_latend.Text.Length > 0)
                if (!double.TryParse(tB_latend.Text, out latend))
                {
                    MessageBox.Show("Wrong Number!");
                    tB_latend.Clear();
                }        
        }

        private void tB_lonstart_Leave(object sender, EventArgs e)
        {
            if (tB_lonstart.Text.Length > 0)
                if (!double.TryParse(tB_lonstart.Text, out lonstart))
                {
                    MessageBox.Show("Wrong Number!");
                    tB_lonstart.Clear();
                }        
        }

        private void tB_lonend_TextChanged(object sender, EventArgs e)
        {
                  
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            tB_LayerWidth.Enabled = true;
            tB_LayerWidth.Clear();
            layerwidth = 0.0;
            tB_LonLayerWidth.Enabled = true;
            tB_LonLayerWidth.Clear();
            lonlayerwidth = 0.0;
            tB_BlockCount.Enabled = true;
            tB_BlockCount.Text = "4";
            blockcount = 4;
            tB_LonBlockCount.Enabled = true;            
            tB_LonBlockCount.Text = "4";
            lonblockcount = 4;
            tB_SmallLayerWidth.Enabled = true;
            tB_SmallLayerWidth.Clear();
            smalllayerwidth = 0.0;
            tB_SmallBlockCount.Enabled = true;
            tB_SmallBlockCount.Text = "3";
            smalllayerblockcount = 3;
            tB_LonSmallLayerWidth.Enabled = true;
            tB_LonSmallLayerWidth.Clear();
            lonsmalllayerwidth = 0.0;
            tB_LonSmallBlockCount.Enabled = true;
            tB_LonSmallBlockCount.Text = "3";
            lonsmalllayerblockcount = 3;

            cB_subarea.Checked = false;
            cB_autodoria.Checked = false;
        }

        private void tB_LonLayerWidth_Leave(object sender, EventArgs e)
        {
            if (tB_LonLayerWidth.Text.Length > 0)
                if (!double.TryParse(tB_LonLayerWidth.Text, out lonlayerwidth))
                {
                    MessageBox.Show("Wrong Number!");
                    tB_LonLayerWidth.Clear();
                }
                else
                {
                    tB_LonBlockCount.Enabled = false;
                    tB_LonBlockCount.Clear();
                    lonblockcount = 0;
                }
        }

        private void tB_LonSmallLayerWidth_Leave(object sender, EventArgs e)
        {
            if (tB_LonSmallLayerWidth.Text.Length > 0)
                if (!double.TryParse(tB_LonSmallLayerWidth.Text, out lonsmalllayerwidth))
                {
                    MessageBox.Show("Wrong Number!");
                    tB_LonSmallLayerWidth.Clear();
                }
                else
                {
                    tB_LonSmallBlockCount.Enabled = false;
                    tB_LonSmallBlockCount.Clear();
                    lonsmalllayerblockcount = 0;
                }
        }

        private void tB_LonBlockCount_TextChanged(object sender, EventArgs e)
        {

        }

        private void tB_LonBlockCount_Leave(object sender, EventArgs e)
        {
            if (tB_LonBlockCount.Text.Length > 0)
                if (!int.TryParse(tB_LonBlockCount.Text, out lonblockcount))
                {
                    MessageBox.Show("Wrong Number!");
                    tB_LonBlockCount.Clear();
                }
                else
                {
                    tB_LonLayerWidth.Enabled = false;
                    tB_LonLayerWidth.Clear();
                    lonlayerwidth = 0.0;
                }
        }

        private void tB_LonSmallBlockCount_Leave(object sender, EventArgs e)
        {
            if (tB_LonSmallBlockCount.Text.Length > 0)
                if (!int.TryParse(tB_LonSmallBlockCount.Text, out lonsmalllayerblockcount))
                {
                    MessageBox.Show("Wrong Number!");
                    tB_LonSmallBlockCount.Clear();
                }
                else
                {
                    tB_LonSmallLayerWidth.Enabled = false;
                    tB_LonSmallLayerWidth.Clear();
                    lonsmalllayerwidth = 0.0;
                }
        }

        private void tB_lonend_Leave(object sender, EventArgs e)
        {
            if (tB_lonend.Text.Length > 0)
                if (!double.TryParse(tB_lonend.Text, out lonend))
                {
                    MessageBox.Show("Wrong Number!");
                    tB_lonend.Clear();
                } 
        }

        private void btn_savebin_Click(object sender, EventArgs e)
        {
            SaveExcelFile(OFDlg1.FileName);
            SaveLogFile(inifile);
            ExitReport();
            //if (mode)  MessageBox.Show("File Saved!!!");
            cB_subarea.Checked = false;
            cB_autodoria.Checked = false;
            Application.Exit();
        }

        private void tB_version_Leave(object sender, EventArgs e)
        {      
            
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            int i;
            string[] words;

            if (!dynamicslicing)
                groupBox1.Enabled = false;

            //for load division
            {
                string p1 = Application.ExecutablePath;
                string[] ws = p1.Split('\\');
                p1 = string.Empty;
                for (i = 0; i < ws.Length - 1; i++)
                    p1 += ws[i] + "\\";
                p1 += "division.ini";

                if (File.Exists(p1))
                {
                    lB_nestedslicingrules.Items.Clear();
                    //List<string> lines = new List<string>();
                    using (StreamReader r = new StreamReader(p1))
                    {
                        string line;
                        while ((line = r.ReadLine()) != null)
                            //lines.Add(line);
                            lB_nestedslicingrules.Items.Add(line);
                    }
                }
            }

            if (mode == false)
            {
                //MessageBox.Show(importexcel);
                this.Hide();
                //LoadExcelFile(importexcel);

                //inifile = Application.StartupPath + "\\speedcam_settings.ini";
                inifile = Application.ExecutablePath;
                words = inifile.Split('\\');
                inifile = string.Empty;
                for (i = 0; i < words.Length - 1; i++)
                    inifile += words[i] + "\\";
                inifile = inifile + "speedcam_settings.ini";
                LoadIniFile(inifile);
                //MessageBox.Show("Load INI finished.");

                LoadExcelFile(importexcel);
                //MessageBox.Show("Load excel finished.");

                if (errorloglist.Count > 0)
                {

                }
                else
                {
                    speedcamconvert();

                    if (showPOIperblock)
                        showPOIinfo(0);

                    SaveExcelFile(importexcel);
                    //MessageBox.Show("Save Excel finished.");
                }
                
                SaveLogFile(inifile);
                //MessageBox.Show("Save Log finished.");
                ExitReport();
                Application.Exit();                
            }
        }

        private void DGV_excel_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            /*
            if (this.DGV_excel.Rows[0].Cells[0].Value.ToString() == "")
            {
                MessageBox.Show("Fist row first column can't be empty");
            }
            */
            if (e.Exception != null && e.Context == DataGridViewDataErrorContexts.Commit)
            {
                MessageBox.Show("DataGridView has wrong data");
            }
        }

        private void DGV_excel_Validating(object sender, CancelEventArgs e)
        {

        }

        private void DGV_excel_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

        }

        private void cbB_lonlat_SelectedIndexChanged(object sender, EventArgs e)
        {
            tB_slicingstart.Enabled = true;
            tB_slicingend.Enabled = true;
            tB_slicingnum.Enabled = true;
            btn_slicingadd.Enabled = true;            
            if (lB_slicingrules.Items.Count > 0)
                btn_slicingdel.Enabled = true;
        }

        private void btn_slicingdel_Click(object sender, EventArgs e)
        {
            if ((lB_slicingrules.Items.Count > 0) && (lB_slicingrules.SelectedItem != null))
                lB_slicingrules.Items.Remove(lB_slicingrules.SelectedItem);

            if (lB_slicingrules.Items.Count == 0)
            {
                lB_slicingrules.Enabled = false;
                btn_slicingdel.Enabled = false;
                btn_slicingadd.Enabled = false;
                tB_slicingstart.Enabled = false;
                tB_slicingend.Enabled = false;
                tB_slicingnum.Enabled = false;
            }
        }

        private void btn_slicingadd_Click(object sender, EventArgs e)
        {
            string tmpstr1 = null, tmpstr2 = null;
                 
            if ( (double.Parse(tB_slicingstart.Text) < latstart) || (double.Parse(tB_slicingend.Text) > latend) || 
                  (double.Parse(tB_lonslicingstart.Text) < lonstart) || (double.Parse(tB_lonslicingend.Text) > lonend) ||
                 (double.Parse(tB_slicingstart.Text) > double.Parse(tB_slicingend.Text)) ||
                (double.Parse(tB_lonslicingstart.Text) > double.Parse(tB_lonslicingend.Text) ) )
            {
                if (mode)    //GUI         
                {
                    MessageBox.Show("Wrong slicing rule!!!");
                    tB_slicingstart.Clear();
                    tB_slicingend.Clear();
                    tB_slicingnum.Clear();
                    tB_lonslicingstart.Clear();
                    tB_lonslicingend.Clear();
                    tB_lonslicingnum.Clear();
                }
            }            
            else
            {
                tmpstr1 = string.Empty;
                tmpstr2 = string.Empty;

                if ((double.Parse(tB_slicingstart.Text) > 0) && (double.Parse(tB_slicingend.Text) > 0))
                {
                    tmpstr1 = tB_slicingstart.Text + ',' + tB_slicingend.Text;
                }

                if ((double.Parse(tB_lonslicingstart.Text) > 0) && (double.Parse(tB_lonslicingend.Text) > 0))
                {
                    tmpstr2 = tB_lonslicingstart.Text + ',' + tB_lonslicingend.Text;
                }

                if ( (tmpstr1.Length > 0) && (tmpstr2.Length > 0) )
                {                    
                    lB_slicingrules.Items.Add(tmpstr1 + ',' + tmpstr2);
                }
                else if (tmpstr1.Length > 0)
                {
                    lB_slicingrules.Items.Add(tmpstr1);
                }
                else if (tmpstr2.Length > 0)
                {
                    lB_slicingrules.Items.Add(tmpstr2);
                }
                
                tB_slicingstart.Clear();
                tB_slicingend.Clear();
                //tB_slicingnum.Clear();   
                tB_lonslicingstart.Clear();
                tB_lonslicingend.Clear();
                //tB_lonslicingnum.Clear();

            }                       

            if (lB_slicingrules.Items.Count > 0)
            {
                lB_slicingrules.Enabled = true;
                btn_slicingdel.Enabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btn_blockinforeset_Click(object sender, EventArgs e)
        {
            tB_BlockInfoThreshold.Clear();
            tB_BlockInfoLargestBlock.Clear();
            tB_BlockInfoLargestBlockPOICount.Clear();
            tB_BlockInfoLargestSmallBlock.Clear();
            tB_BlockInfoLargestSmallBlockPOICount.Clear();
            tB_BlockInfoCount.Clear();
            showPOIinfo(0);
            tB_BlockInfoCount.Text = lB_BlockInfo.Items.Count.ToString();
        }

        private void btn_listrefine_Click(object sender, EventArgs e)
        {
            btn_blockinforeset.Enabled = true;
            tB_BlockInfoLargestBlock.Clear();
            tB_BlockInfoLargestBlockPOICount.Clear();
            tB_BlockInfoLargestSmallBlock.Clear();
            tB_BlockInfoLargestSmallBlockPOICount.Clear();
            showPOIinfo(int.Parse(tB_BlockInfoThreshold.Text));
            tB_BlockInfoCount.Text = lB_BlockInfo.Items.Count.ToString();
        }

        private void tB_BlockInfoThreshold_TextChanged(object sender, EventArgs e)
        {
            btn_listrefine.Enabled = true;
        }

        private void btn_DivisionOverview_Click(object sender, EventArgs e)
        {
            int i = 0, j = 0; 
            long tmpPOIs = 0;
            int colortypes = 8;
            long interval = 0, largestblockPOIcount = 0, leastblockPOIcount = 0;
            int rowindex = 0, columnindex = 0;
            
            //Form2 DivisionOverview = new Form2();                      

            DivisionOverview.latend = latend;
            DivisionOverview.latstart = latstart;
            DivisionOverview.layerwidth = layerwidth;
            DivisionOverview.lonstart = lonstart;
            DivisionOverview.lonend = lonend;
            DivisionOverview.lonlayerwidth = lonlayerwidth;

            DivisionOverview.lonblockcount = lonblockcount;
            DivisionOverview.blockcount = blockcount;
            DivisionOverview.lonsmalllayerblockcount = lonsmalllayerblockcount;
            DivisionOverview.smalllayerblockcount = smalllayerblockcount;
            
            DivisionOverview.DGV_DivisionOverview.ColumnCount = lonblockcount*lonsmalllayerblockcount;
            DivisionOverview.DGV_DivisionOverview.RowCount = blockcount*smalllayerblockcount;

            DivisionOverview.tB_ColorTypes.Text = colortypes.ToString();
            largestblockPOIcount = long.Parse(tB_BlockInfoLargestBlockPOICount.Text);
            leastblockPOIcount = long.Parse(tB_BlockInfoLeastBlockPOICount.Text);
            interval = (long)( (largestblockPOIcount - leastblockPOIcount) / colortypes );
            interval = 100;

            //------------------------- narrow cell width --------------------------------------------------------
            foreach (DataGridViewColumn gCol in DivisionOverview.DGV_DivisionOverview.Columns)
                gCol.Width = 1;            
            DivisionOverview.DGV_DivisionOverview.RowTemplate.Height = 1;

            //--------------------------------------------------------------------------------------------------------------
            // points[layerindex, smalllayerindex].Count : POI counts
            for (i = 0; i < points.GetLength(0); i++)
                for (j = 0; j < points.GetLength(1); j++)
                {                                        
                    rowindex = (i / lonblockcount) * smalllayerblockcount + (j / lonsmalllayerblockcount);
                    columnindex = (i % lonblockcount) * lonsmalllayerblockcount + (j % lonsmalllayerblockcount);                    
                    DivisionOverview.DGV_DivisionOverview.Rows[(blockcount*smalllayerblockcount - 1) - rowindex].Cells[columnindex].Value = points[i, j].Count.ToString();
                    
                    if (points[i, j].Count > (leastblockPOIcount + (colortypes - 1) * interval))
                    {
                        //DivisionOverview.DGV_DivisionOverview.Rows[rowindex].Cells[columnindex].Style.BackColor = Color.Red;
                        DivisionOverview.DGV_DivisionOverview.Rows[(blockcount*smalllayerblockcount - 1) - rowindex].Cells[columnindex].Style.BackColor = Color.Red;
                    }
                    else if (points[i, j].Count > (leastblockPOIcount + (colortypes - 2) * interval))
                    {
                        //DivisionOverview.DGV_DivisionOverview.Rows[rowindex].Cells[columnindex].Style.BackColor = Color.Yellow;
                        DivisionOverview.DGV_DivisionOverview.Rows[(blockcount * smalllayerblockcount - 1) - rowindex].Cells[columnindex].Style.BackColor = Color.OrangeRed;
                    }
                    else if (points[i, j].Count > (leastblockPOIcount + (colortypes - 3) * interval))
                    {
                        //DivisionOverview.DGV_DivisionOverview.Rows[rowindex].Cells[columnindex].Style.BackColor = Color.Yellow;
                        DivisionOverview.DGV_DivisionOverview.Rows[(blockcount*smalllayerblockcount - 1) - rowindex].Cells[columnindex].Style.BackColor = Color.Orange;
                    }
                    else if (points[i, j].Count > (leastblockPOIcount + (colortypes - 4) * interval))
                    {
                        //DivisionOverview.DGV_DivisionOverview.Rows[rowindex].Cells[columnindex].Style.BackColor = Color.Green;
                        DivisionOverview.DGV_DivisionOverview.Rows[(blockcount*smalllayerblockcount - 1) - rowindex].Cells[columnindex].Style.BackColor = Color.Yellow;
                    }
                    else if (points[i, j].Count > (leastblockPOIcount + (colortypes - 5) * interval))
                    {
                        //DivisionOverview.DGV_DivisionOverview.Rows[rowindex].Cells[columnindex].Style.BackColor = Color.Blue;
                        DivisionOverview.DGV_DivisionOverview.Rows[(blockcount*smalllayerblockcount - 1) - rowindex].Cells[columnindex].Style.BackColor = Color.YellowGreen;
                    }
                    else if (points[i, j].Count > (leastblockPOIcount + (colortypes - 6) * interval))
                    {
                        //DivisionOverview.DGV_DivisionOverview.Rows[rowindex].Cells[columnindex].Style.BackColor = Color.Blue;
                        DivisionOverview.DGV_DivisionOverview.Rows[(blockcount * smalllayerblockcount - 1) - rowindex].Cells[columnindex].Style.BackColor = Color.Green;
                    }
                    else if (points[i, j].Count > (leastblockPOIcount + (colortypes - 7) * interval))
                    {
                        //DivisionOverview.DGV_DivisionOverview.Rows[rowindex].Cells[columnindex].Style.BackColor = Color.Blue;
                        DivisionOverview.DGV_DivisionOverview.Rows[(blockcount * smalllayerblockcount - 1) - rowindex].Cells[columnindex].Style.BackColor = Color.Blue;
                    }
                    else if (points[i, j].Count > 0)
                    {
                        //DivisionOverview.DGV_DivisionOverview.Rows[rowindex].Cells[columnindex].Style.BackColor = Color.Blue;
                        DivisionOverview.DGV_DivisionOverview.Rows[(blockcount * smalllayerblockcount - 1) - rowindex].Cells[columnindex].Style.BackColor = Color.SkyBlue;
                    }
                }

            tmpPOIs = 0;
            for (i = 0; i < DivisionOverview.DGV_DivisionOverview.RowCount; i++)
                for (j = 0; j < DivisionOverview.DGV_DivisionOverview.ColumnCount; j++)
                    tmpPOIs += int.Parse(DivisionOverview.DGV_DivisionOverview.Rows[i].Cells[j].Value.ToString());
            
            DivisionOverview.tB_OverviewtotalPOIs.Text = tmpPOIs.ToString();
            DivisionOverview.Show();
            DivisionOverview.Focus();
        }

        private void Form1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            
        }

        private void tB_BlockCount_Validated(object sender, EventArgs e)
        {
            if (tB_BlockCount.Modified)
                tB_BlockCount.Update();
            if (tB_LonBlockCount.Modified)
                tB_LonBlockCount.Update();
            if (tB_SmallBlockCount.Modified)
                tB_SmallBlockCount.Update();
            if (tB_LonSmallBlockCount.Modified)
                tB_LonSmallBlockCount.Update();
        }

        private void tB_slicingstart_TextChanged(object sender, EventArgs e)
        {
            if ((tB_slicingstart.Text.Length) > 0)
            {
                btn_slicingadd.Enabled = true;
                btn_nestedslicingadd.Enabled = true;
            }
        }

        private void tB_lonslicingstart_TextChanged(object sender, EventArgs e)
        {
            if ( (tB_lonslicingstart.Text.Length) > 0)
            {
                btn_slicingadd.Enabled = true;
                btn_nestedslicingadd.Enabled = true;
            }
        }

        private void btn_nestedslicingadd_Click(object sender, EventArgs e)
        {
            string tmpstr1 = null, tmpstr2 = null;

            if ((double.Parse(tB_slicingstart.Text) < latstart) || (double.Parse(tB_slicingend.Text) > latend) ||
                  (double.Parse(tB_lonslicingstart.Text) < lonstart) || (double.Parse(tB_lonslicingend.Text) > lonend) ||
                 (double.Parse(tB_slicingstart.Text) > double.Parse(tB_slicingend.Text)) ||
                (double.Parse(tB_lonslicingstart.Text) > double.Parse(tB_lonslicingend.Text)))
            {
                if (mode)    //GUI         
                {
                    MessageBox.Show("Wrong slicing rule!!!");
                    tB_slicingstart.Clear();
                    tB_slicingend.Clear();
                    tB_slicingnum.Clear();
                    tB_lonslicingstart.Clear();
                    tB_lonslicingend.Clear();
                    tB_lonslicingnum.Clear();
                }
            }
            else
            {
                tmpstr1 = string.Empty;
                tmpstr2 = string.Empty;

                if ((double.Parse(tB_slicingstart.Text) > 0) && (double.Parse(tB_slicingend.Text) > 0))
                {
                    tmpstr1 = tB_slicingstart.Text + ',' + tB_slicingend.Text;
                }

                if ((double.Parse(tB_lonslicingstart.Text) > 0) && (double.Parse(tB_lonslicingend.Text) > 0))
                {
                    tmpstr2 = tB_lonslicingstart.Text + ',' + tB_lonslicingend.Text;
                }

                if ((tmpstr1.Length > 0) && (tmpstr2.Length > 0))
                {
                    //lB_slicingrules.Items.Add(tmpstr1 + ',' + tmpstr2);
                    lB_nestedslicingrules.Items.Add(tmpstr1 + ',' + tmpstr2);
                }
                else if (tmpstr1.Length > 0)
                {
                    //lB_slicingrules.Items.Add(tmpstr1);
                    lB_nestedslicingrules.Items.Add(tmpstr1);
                }
                else if (tmpstr2.Length > 0)
                {
                    //lB_slicingrules.Items.Add(tmpstr2);
                    lB_nestedslicingrules.Items.Add(tmpstr2);
                }

                tB_slicingstart.Clear();
                tB_slicingend.Clear();
                //tB_slicingnum.Clear();   
                tB_lonslicingstart.Clear();
                tB_lonslicingend.Clear();
                //tB_lonslicingnum.Clear();

            }
            
            if (lB_nestedslicingrules.Items.Count > 0)
            {
                lB_nestedslicingrules.Enabled = true;
                btn_nestedslicingdel.Enabled = true;
            }

        }

        private void btn_nestedslicingdel_Click(object sender, EventArgs e)
        {
            lB_nestedslicingrules.Items.Clear();
        }
    }
}
