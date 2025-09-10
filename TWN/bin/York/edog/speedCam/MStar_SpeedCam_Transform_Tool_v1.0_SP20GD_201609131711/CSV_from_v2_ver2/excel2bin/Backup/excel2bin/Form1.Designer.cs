namespace excel2bin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tB_excel = new System.Windows.Forms.TextBox();
            this.tB_bin = new System.Windows.Forms.TextBox();
            this.btn_openexcel = new System.Windows.Forms.Button();
            this.btn_savebin = new System.Windows.Forms.Button();
            this.btn_start = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.OFDlg1 = new System.Windows.Forms.OpenFileDialog();
            this.SFDlg1 = new System.Windows.Forms.SaveFileDialog();
            this.DGV_excel = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.tB_LayerWidth = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tB_BlockCount = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tB_SmallLayerWidth = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tB_SmallBlockCount = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tB_latstart = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tB_latend = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tB_lonstart = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tB_lonend = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tB_LonLayerWidth = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tB_LonBlockCount = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tB_LonSmallLayerWidth = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tB_LonSmallBlockCount = new System.Windows.Forms.TextBox();
            this.btn_clear = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.tB_version = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_nestedslicingdel = new System.Windows.Forms.Button();
            this.btn_nestedslicingadd = new System.Windows.Forms.Button();
            this.lB_nestedslicingrules = new System.Windows.Forms.ListBox();
            this.tB_lonslicingnum = new System.Windows.Forms.TextBox();
            this.tB_lonslicingend = new System.Windows.Forms.TextBox();
            this.tB_lonslicingstart = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.btn_slicingdel = new System.Windows.Forms.Button();
            this.tB_slicingnum = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.lB_slicingrules = new System.Windows.Forms.ListBox();
            this.btn_slicingadd = new System.Windows.Forms.Button();
            this.tB_slicingend = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.tB_slicingstart = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.gB_BlockInfo = new System.Windows.Forms.GroupBox();
            this.tB_BlockInfoLeastSmallBlockPOICount = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.tB_BlockInfoLeastSmallBlock = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.tB_BlockInfoLeastBlockPOICount = new System.Windows.Forms.TextBox();
            this.tB_BlockInfoLeastBlock = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.btn_DivisionOverview = new System.Windows.Forms.Button();
            this.btn_blockinforeset = new System.Windows.Forms.Button();
            this.btn_listrefine = new System.Windows.Forms.Button();
            this.tB_BlockInfoThreshold = new System.Windows.Forms.TextBox();
            this.tB_BlockInfoCount = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.tB_BlockInfoLargestSmallBlockPOICount = new System.Windows.Forms.TextBox();
            this.tB_BlockInfoLargestBlockPOICount = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.tB_BlockInfoLargestSmallBlock = new System.Windows.Forms.TextBox();
            this.tB_BlockInfoLargestBlock = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.lB_BlockInfo = new System.Windows.Forms.ListBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_excel)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.gB_BlockInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("PMingLiU", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(9, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "POI File :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("PMingLiU", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(15, 637);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "bin file :";
            // 
            // tB_excel
            // 
            this.tB_excel.Location = new System.Drawing.Point(78, 97);
            this.tB_excel.Name = "tB_excel";
            this.tB_excel.ReadOnly = true;
            this.tB_excel.Size = new System.Drawing.Size(182, 22);
            this.tB_excel.TabIndex = 2;
            // 
            // tB_bin
            // 
            this.tB_bin.Location = new System.Drawing.Point(77, 637);
            this.tB_bin.Name = "tB_bin";
            this.tB_bin.ReadOnly = true;
            this.tB_bin.Size = new System.Drawing.Size(226, 22);
            this.tB_bin.TabIndex = 3;
            // 
            // btn_openexcel
            // 
            this.btn_openexcel.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_openexcel.Location = new System.Drawing.Point(266, 97);
            this.btn_openexcel.Name = "btn_openexcel";
            this.btn_openexcel.Size = new System.Drawing.Size(60, 23);
            this.btn_openexcel.TabIndex = 1;
            this.btn_openexcel.Text = "Browse";
            this.btn_openexcel.UseVisualStyleBackColor = true;
            this.btn_openexcel.Click += new System.EventHandler(this.btn_openexcel_Click);
            // 
            // btn_savebin
            // 
            this.btn_savebin.Enabled = false;
            this.btn_savebin.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_savebin.Location = new System.Drawing.Point(309, 637);
            this.btn_savebin.Name = "btn_savebin";
            this.btn_savebin.Size = new System.Drawing.Size(60, 23);
            this.btn_savebin.TabIndex = 13;
            this.btn_savebin.Text = "Save";
            this.btn_savebin.UseVisualStyleBackColor = true;
            this.btn_savebin.Click += new System.EventHandler(this.btn_savebin_Click);
            // 
            // btn_start
            // 
            this.btn_start.Enabled = false;
            this.btn_start.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_start.Location = new System.Drawing.Point(154, 7);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(92, 74);
            this.btn_start.TabIndex = 6;
            this.btn_start.Text = "Convert";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(35, 8);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(75, 73);
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            // 
            // OFDlg1
            // 
            this.OFDlg1.FileName = "SpeedCam_Data";
            this.OFDlg1.Filter = "xls, xlsx, csv|*.xls; *.xlsx; *.csv|所有檔案|*.*";
            this.OFDlg1.FileOk += new System.ComponentModel.CancelEventHandler(this.OFDlg1_FileOk);
            // 
            // SFDlg1
            // 
            this.SFDlg1.DefaultExt = "bin";
            // 
            // DGV_excel
            // 
            this.DGV_excel.AllowUserToAddRows = false;
            this.DGV_excel.AllowUserToDeleteRows = false;
            this.DGV_excel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_excel.Location = new System.Drawing.Point(444, 7);
            this.DGV_excel.Name = "DGV_excel";
            this.DGV_excel.ReadOnly = true;
            this.DGV_excel.RowTemplate.Height = 24;
            this.DGV_excel.Size = new System.Drawing.Size(301, 652);
            this.DGV_excel.TabIndex = 10;
            this.DGV_excel.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.DGV_excel_CellValidating);
            this.DGV_excel.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DGV_excel_DataError);
            this.DGV_excel.Validating += new System.ComponentModel.CancelEventHandler(this.DGV_excel_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("PMingLiU", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(8, 194);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "Large Lat. Width :";
            // 
            // tB_LayerWidth
            // 
            this.tB_LayerWidth.Enabled = false;
            this.tB_LayerWidth.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tB_LayerWidth.Location = new System.Drawing.Point(138, 192);
            this.tB_LayerWidth.Name = "tB_LayerWidth";
            this.tB_LayerWidth.Size = new System.Drawing.Size(54, 23);
            this.tB_LayerWidth.TabIndex = 6;
            this.tB_LayerWidth.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.tB_LayerWidth.Leave += new System.EventHandler(this.tB_LayerWidth_Leave);
            this.tB_LayerWidth.TabIndexChanged += new System.EventHandler(this.tB_LayerWidth_TabIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("PMingLiU", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(206, 194);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(155, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "Large Lat. Block Counts :";
            // 
            // tB_BlockCount
            // 
            this.tB_BlockCount.Enabled = false;
            this.tB_BlockCount.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tB_BlockCount.Location = new System.Drawing.Point(367, 193);
            this.tB_BlockCount.Name = "tB_BlockCount";
            this.tB_BlockCount.Size = new System.Drawing.Size(65, 23);
            this.tB_BlockCount.TabIndex = 2;
            this.tB_BlockCount.Validated += new System.EventHandler(this.tB_BlockCount_Validated);
            this.tB_BlockCount.Leave += new System.EventHandler(this.tB_BlockCount_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("PMingLiU", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(8, 253);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 15);
            this.label5.TabIndex = 14;
            this.label5.Text = "Small Lat. Width :";
            // 
            // tB_SmallLayerWidth
            // 
            this.tB_SmallLayerWidth.Enabled = false;
            this.tB_SmallLayerWidth.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tB_SmallLayerWidth.Location = new System.Drawing.Point(138, 251);
            this.tB_SmallLayerWidth.Name = "tB_SmallLayerWidth";
            this.tB_SmallLayerWidth.Size = new System.Drawing.Size(54, 23);
            this.tB_SmallLayerWidth.TabIndex = 10;
            this.tB_SmallLayerWidth.Leave += new System.EventHandler(this.tB_SmallLayerWidth_Leave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("PMingLiU", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(206, 254);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(155, 15);
            this.label6.TabIndex = 16;
            this.label6.Text = "Small Lat. Block Counts :";
            // 
            // tB_SmallBlockCount
            // 
            this.tB_SmallBlockCount.Enabled = false;
            this.tB_SmallBlockCount.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tB_SmallBlockCount.Location = new System.Drawing.Point(367, 253);
            this.tB_SmallBlockCount.Name = "tB_SmallBlockCount";
            this.tB_SmallBlockCount.Size = new System.Drawing.Size(65, 23);
            this.tB_SmallBlockCount.TabIndex = 4;
            this.tB_SmallBlockCount.Leave += new System.EventHandler(this.tB_SmallBlockCount_Leave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("PMingLiU", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(8, 133);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 15);
            this.label7.TabIndex = 18;
            this.label7.Text = "Latitude Start :";
            // 
            // tB_latstart
            // 
            this.tB_latstart.Enabled = false;
            this.tB_latstart.Location = new System.Drawing.Point(118, 132);
            this.tB_latstart.Name = "tB_latstart";
            this.tB_latstart.ReadOnly = true;
            this.tB_latstart.Size = new System.Drawing.Size(74, 22);
            this.tB_latstart.TabIndex = 2;
            this.tB_latstart.Leave += new System.EventHandler(this.tB_latstart_Leave);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("PMingLiU", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.Location = new System.Drawing.Point(228, 132);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 15);
            this.label8.TabIndex = 20;
            this.label8.Text = "Latitude (Y) End :";
            // 
            // tB_latend
            // 
            this.tB_latend.Enabled = false;
            this.tB_latend.Location = new System.Drawing.Point(357, 132);
            this.tB_latend.Name = "tB_latend";
            this.tB_latend.ReadOnly = true;
            this.tB_latend.Size = new System.Drawing.Size(75, 22);
            this.tB_latend.TabIndex = 3;
            this.tB_latend.Leave += new System.EventHandler(this.tB_latend_Leave);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("PMingLiU", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label9.Location = new System.Drawing.Point(8, 163);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 15);
            this.label9.TabIndex = 22;
            this.label9.Text = "Longitude Start :";
            // 
            // tB_lonstart
            // 
            this.tB_lonstart.Enabled = false;
            this.tB_lonstart.Location = new System.Drawing.Point(118, 162);
            this.tB_lonstart.Name = "tB_lonstart";
            this.tB_lonstart.ReadOnly = true;
            this.tB_lonstart.Size = new System.Drawing.Size(74, 22);
            this.tB_lonstart.TabIndex = 4;
            this.tB_lonstart.Leave += new System.EventHandler(this.tB_lonstart_Leave);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("PMingLiU", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label10.Location = new System.Drawing.Point(227, 163);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(124, 15);
            this.label10.TabIndex = 24;
            this.label10.Text = "Longitude (X) End :";
            // 
            // tB_lonend
            // 
            this.tB_lonend.Enabled = false;
            this.tB_lonend.Location = new System.Drawing.Point(357, 162);
            this.tB_lonend.Name = "tB_lonend";
            this.tB_lonend.ReadOnly = true;
            this.tB_lonend.Size = new System.Drawing.Size(75, 22);
            this.tB_lonend.TabIndex = 5;
            this.tB_lonend.TextChanged += new System.EventHandler(this.tB_lonend_TextChanged);
            this.tB_lonend.Leave += new System.EventHandler(this.tB_lonend_Leave);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("PMingLiU", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label11.Location = new System.Drawing.Point(9, 223);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(118, 15);
            this.label11.TabIndex = 25;
            this.label11.Text = "Large Lon. Width :";
            // 
            // tB_LonLayerWidth
            // 
            this.tB_LonLayerWidth.Enabled = false;
            this.tB_LonLayerWidth.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tB_LonLayerWidth.Location = new System.Drawing.Point(138, 222);
            this.tB_LonLayerWidth.Name = "tB_LonLayerWidth";
            this.tB_LonLayerWidth.Size = new System.Drawing.Size(54, 23);
            this.tB_LonLayerWidth.TabIndex = 8;
            this.tB_LonLayerWidth.Leave += new System.EventHandler(this.tB_LonLayerWidth_Leave);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("PMingLiU", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label12.Location = new System.Drawing.Point(206, 223);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(159, 15);
            this.label12.TabIndex = 27;
            this.label12.Text = "Large Lon. Block Counts :";
            // 
            // tB_LonBlockCount
            // 
            this.tB_LonBlockCount.Enabled = false;
            this.tB_LonBlockCount.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tB_LonBlockCount.Location = new System.Drawing.Point(367, 224);
            this.tB_LonBlockCount.Name = "tB_LonBlockCount";
            this.tB_LonBlockCount.Size = new System.Drawing.Size(65, 23);
            this.tB_LonBlockCount.TabIndex = 3;
            this.tB_LonBlockCount.TextChanged += new System.EventHandler(this.tB_LonBlockCount_TextChanged);
            this.tB_LonBlockCount.Leave += new System.EventHandler(this.tB_LonBlockCount_Leave);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("PMingLiU", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label13.Location = new System.Drawing.Point(9, 284);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(118, 15);
            this.label13.TabIndex = 29;
            this.label13.Text = "Small Lon. Width :";
            // 
            // tB_LonSmallLayerWidth
            // 
            this.tB_LonSmallLayerWidth.Enabled = false;
            this.tB_LonSmallLayerWidth.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tB_LonSmallLayerWidth.Location = new System.Drawing.Point(138, 283);
            this.tB_LonSmallLayerWidth.Name = "tB_LonSmallLayerWidth";
            this.tB_LonSmallLayerWidth.Size = new System.Drawing.Size(54, 23);
            this.tB_LonSmallLayerWidth.TabIndex = 12;
            this.tB_LonSmallLayerWidth.Leave += new System.EventHandler(this.tB_LonSmallLayerWidth_Leave);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("PMingLiU", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label14.Location = new System.Drawing.Point(206, 284);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(159, 15);
            this.label14.TabIndex = 31;
            this.label14.Text = "Small Lon. Block Counts :";
            // 
            // tB_LonSmallBlockCount
            // 
            this.tB_LonSmallBlockCount.Enabled = false;
            this.tB_LonSmallBlockCount.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tB_LonSmallBlockCount.Location = new System.Drawing.Point(367, 284);
            this.tB_LonSmallBlockCount.Name = "tB_LonSmallBlockCount";
            this.tB_LonSmallBlockCount.Size = new System.Drawing.Size(65, 23);
            this.tB_LonSmallBlockCount.TabIndex = 5;
            this.tB_LonSmallBlockCount.Leave += new System.EventHandler(this.tB_LonSmallBlockCount_Leave);
            // 
            // btn_clear
            // 
            this.btn_clear.Enabled = false;
            this.btn_clear.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_clear.Location = new System.Drawing.Point(372, 637);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(60, 23);
            this.btn_clear.TabIndex = 16;
            this.btn_clear.Text = "Reset";
            this.btn_clear.UseVisualStyleBackColor = true;
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("PMingLiU", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label15.Location = new System.Drawing.Point(332, 100);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(59, 15);
            this.label15.TabIndex = 32;
            this.label15.Text = "Version :";
            // 
            // tB_version
            // 
            this.tB_version.Enabled = false;
            this.tB_version.Location = new System.Drawing.Point(394, 98);
            this.tB_version.Name = "tB_version";
            this.tB_version.ReadOnly = true;
            this.tB_version.Size = new System.Drawing.Size(38, 22);
            this.tB_version.TabIndex = 33;
            this.tB_version.Leave += new System.EventHandler(this.tB_version_Leave);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_nestedslicingdel);
            this.groupBox1.Controls.Add(this.btn_nestedslicingadd);
            this.groupBox1.Controls.Add(this.lB_nestedslicingrules);
            this.groupBox1.Controls.Add(this.tB_lonslicingnum);
            this.groupBox1.Controls.Add(this.tB_lonslicingend);
            this.groupBox1.Controls.Add(this.tB_lonslicingstart);
            this.groupBox1.Controls.Add(this.label30);
            this.groupBox1.Controls.Add(this.btn_slicingdel);
            this.groupBox1.Controls.Add(this.tB_slicingnum);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.lB_slicingrules);
            this.groupBox1.Controls.Add(this.btn_slicingadd);
            this.groupBox1.Controls.Add(this.tB_slicingend);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.tB_slicingstart);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Location = new System.Drawing.Point(12, 468);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(420, 162);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dynamic Division";
            // 
            // btn_nestedslicingdel
            // 
            this.btn_nestedslicingdel.Enabled = false;
            this.btn_nestedslicingdel.Location = new System.Drawing.Point(320, 135);
            this.btn_nestedslicingdel.Name = "btn_nestedslicingdel";
            this.btn_nestedslicingdel.Size = new System.Drawing.Size(75, 23);
            this.btn_nestedslicingdel.TabIndex = 17;
            this.btn_nestedslicingdel.Text = "Nested Del";
            this.btn_nestedslicingdel.UseVisualStyleBackColor = true;
            this.btn_nestedslicingdel.Click += new System.EventHandler(this.btn_nestedslicingdel_Click);
            // 
            // btn_nestedslicingadd
            // 
            this.btn_nestedslicingadd.Enabled = false;
            this.btn_nestedslicingadd.Location = new System.Drawing.Point(230, 135);
            this.btn_nestedslicingadd.Name = "btn_nestedslicingadd";
            this.btn_nestedslicingadd.Size = new System.Drawing.Size(75, 23);
            this.btn_nestedslicingadd.TabIndex = 16;
            this.btn_nestedslicingadd.Text = "Nested Add";
            this.btn_nestedslicingadd.UseVisualStyleBackColor = true;
            this.btn_nestedslicingadd.Click += new System.EventHandler(this.btn_nestedslicingadd_Click);
            // 
            // lB_nestedslicingrules
            // 
            this.lB_nestedslicingrules.Enabled = false;
            this.lB_nestedslicingrules.FormattingEnabled = true;
            this.lB_nestedslicingrules.ItemHeight = 12;
            this.lB_nestedslicingrules.Items.AddRange(new object[] {
            "18.0, 54.0, 75.0, 100.0",
            "18.0, 22.0, 107.0, 118.0",
            "22.0, 24.0, 111.0, 123.0",
            "24.0, 28.0, 100.0, 123.0",
            "28.0, 29.0, 111.0, 123.0",
            "29.0, 30.0, 106.0, 123.0",
            "30.0, 32.0, 116.0, 123.0",
            "28.0, 32.0, 100.0, 106.0",
            "32.0, 35.0, 111.0, 123.0",
            "32.0, 35.0, 100.0, 111.0",
            "35.0, 38.0, 113.0, 123.0",
            "38.0, 40.0, 115.0, 121.0",
            "35.0, 40.0, 100.0, 113.0",
            "40.0, 45.0, 110.0, 132.0",
            "45.0, 54.0, 110.0, 132.0"});
            this.lB_nestedslicingrules.Location = new System.Drawing.Point(218, 77);
            this.lB_nestedslicingrules.Name = "lB_nestedslicingrules";
            this.lB_nestedslicingrules.Size = new System.Drawing.Size(190, 52);
            this.lB_nestedslicingrules.TabIndex = 15;
            // 
            // tB_lonslicingnum
            // 
            this.tB_lonslicingnum.Enabled = false;
            this.tB_lonslicingnum.Location = new System.Drawing.Point(356, 49);
            this.tB_lonslicingnum.Name = "tB_lonslicingnum";
            this.tB_lonslicingnum.Size = new System.Drawing.Size(42, 22);
            this.tB_lonslicingnum.TabIndex = 14;
            // 
            // tB_lonslicingend
            // 
            this.tB_lonslicingend.Enabled = false;
            this.tB_lonslicingend.Location = new System.Drawing.Point(230, 49);
            this.tB_lonslicingend.Name = "tB_lonslicingend";
            this.tB_lonslicingend.Size = new System.Drawing.Size(50, 22);
            this.tB_lonslicingend.TabIndex = 11;
            // 
            // tB_lonslicingstart
            // 
            this.tB_lonslicingstart.Enabled = false;
            this.tB_lonslicingstart.Location = new System.Drawing.Point(138, 49);
            this.tB_lonslicingstart.Name = "tB_lonslicingstart";
            this.tB_lonslicingstart.Size = new System.Drawing.Size(50, 22);
            this.tB_lonslicingstart.TabIndex = 10;
            this.tB_lonslicingstart.TextChanged += new System.EventHandler(this.tB_lonslicingstart_TextChanged);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(21, 52);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(56, 12);
            this.label30.TabIndex = 11;
            this.label30.Text = "Longitude:";
            // 
            // btn_slicingdel
            // 
            this.btn_slicingdel.Enabled = false;
            this.btn_slicingdel.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_slicingdel.Location = new System.Drawing.Point(109, 135);
            this.btn_slicingdel.Name = "btn_slicingdel";
            this.btn_slicingdel.Size = new System.Drawing.Size(75, 23);
            this.btn_slicingdel.TabIndex = 13;
            this.btn_slicingdel.Text = "Indep. Del";
            this.btn_slicingdel.UseVisualStyleBackColor = true;
            this.btn_slicingdel.Click += new System.EventHandler(this.btn_slicingdel_Click);
            // 
            // tB_slicingnum
            // 
            this.tB_slicingnum.Enabled = false;
            this.tB_slicingnum.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tB_slicingnum.Location = new System.Drawing.Point(356, 17);
            this.tB_slicingnum.Name = "tB_slicingnum";
            this.tB_slicingnum.Size = new System.Drawing.Size(41, 23);
            this.tB_slicingnum.TabIndex = 6;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label19.Location = new System.Drawing.Point(295, 37);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(51, 13);
            this.label19.TabIndex = 8;
            this.label19.Text = "Division:";
            // 
            // lB_slicingrules
            // 
            this.lB_slicingrules.Enabled = false;
            this.lB_slicingrules.FormattingEnabled = true;
            this.lB_slicingrules.ItemHeight = 12;
            this.lB_slicingrules.Location = new System.Drawing.Point(8, 77);
            this.lB_slicingrules.Name = "lB_slicingrules";
            this.lB_slicingrules.Size = new System.Drawing.Size(190, 52);
            this.lB_slicingrules.TabIndex = 7;
            // 
            // btn_slicingadd
            // 
            this.btn_slicingadd.Enabled = false;
            this.btn_slicingadd.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_slicingadd.Location = new System.Drawing.Point(23, 135);
            this.btn_slicingadd.Name = "btn_slicingadd";
            this.btn_slicingadd.Size = new System.Drawing.Size(75, 23);
            this.btn_slicingadd.TabIndex = 12;
            this.btn_slicingadd.Text = "Indep. Add";
            this.btn_slicingadd.UseVisualStyleBackColor = true;
            this.btn_slicingadd.Click += new System.EventHandler(this.btn_slicingadd_Click);
            // 
            // tB_slicingend
            // 
            this.tB_slicingend.Enabled = false;
            this.tB_slicingend.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tB_slicingend.Location = new System.Drawing.Point(230, 17);
            this.tB_slicingend.Name = "tB_slicingend";
            this.tB_slicingend.Size = new System.Drawing.Size(50, 23);
            this.tB_slicingend.TabIndex = 9;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label18.Location = new System.Drawing.Point(198, 37);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(23, 13);
            this.label18.TabIndex = 4;
            this.label18.Text = "To:";
            // 
            // tB_slicingstart
            // 
            this.tB_slicingstart.Enabled = false;
            this.tB_slicingstart.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tB_slicingstart.Location = new System.Drawing.Point(138, 17);
            this.tB_slicingstart.Name = "tB_slicingstart";
            this.tB_slicingstart.Size = new System.Drawing.Size(50, 23);
            this.tB_slicingstart.TabIndex = 8;
            this.tB_slicingstart.TextChanged += new System.EventHandler(this.tB_slicingstart_TextChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label17.Location = new System.Drawing.Point(95, 37);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(37, 13);
            this.label17.TabIndex = 2;
            this.label17.Text = "From:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label16.Location = new System.Drawing.Point(20, 23);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(48, 13);
            this.label16.TabIndex = 0;
            this.label16.Text = "Latitude:";
            // 
            // gB_BlockInfo
            // 
            this.gB_BlockInfo.Controls.Add(this.tB_BlockInfoLeastSmallBlockPOICount);
            this.gB_BlockInfo.Controls.Add(this.label29);
            this.gB_BlockInfo.Controls.Add(this.tB_BlockInfoLeastSmallBlock);
            this.gB_BlockInfo.Controls.Add(this.label28);
            this.gB_BlockInfo.Controls.Add(this.tB_BlockInfoLeastBlockPOICount);
            this.gB_BlockInfo.Controls.Add(this.tB_BlockInfoLeastBlock);
            this.gB_BlockInfo.Controls.Add(this.label27);
            this.gB_BlockInfo.Controls.Add(this.label26);
            this.gB_BlockInfo.Controls.Add(this.btn_DivisionOverview);
            this.gB_BlockInfo.Controls.Add(this.btn_blockinforeset);
            this.gB_BlockInfo.Controls.Add(this.btn_listrefine);
            this.gB_BlockInfo.Controls.Add(this.tB_BlockInfoThreshold);
            this.gB_BlockInfo.Controls.Add(this.tB_BlockInfoCount);
            this.gB_BlockInfo.Controls.Add(this.label25);
            this.gB_BlockInfo.Controls.Add(this.label24);
            this.gB_BlockInfo.Controls.Add(this.tB_BlockInfoLargestSmallBlockPOICount);
            this.gB_BlockInfo.Controls.Add(this.tB_BlockInfoLargestBlockPOICount);
            this.gB_BlockInfo.Controls.Add(this.label23);
            this.gB_BlockInfo.Controls.Add(this.label22);
            this.gB_BlockInfo.Controls.Add(this.tB_BlockInfoLargestSmallBlock);
            this.gB_BlockInfo.Controls.Add(this.tB_BlockInfoLargestBlock);
            this.gB_BlockInfo.Controls.Add(this.label21);
            this.gB_BlockInfo.Controls.Add(this.label20);
            this.gB_BlockInfo.Controls.Add(this.lB_BlockInfo);
            this.gB_BlockInfo.Location = new System.Drawing.Point(12, 308);
            this.gB_BlockInfo.Name = "gB_BlockInfo";
            this.gB_BlockInfo.Size = new System.Drawing.Size(420, 154);
            this.gB_BlockInfo.TabIndex = 35;
            this.gB_BlockInfo.TabStop = false;
            this.gB_BlockInfo.Text = "Block Info";
            // 
            // tB_BlockInfoLeastSmallBlockPOICount
            // 
            this.tB_BlockInfoLeastSmallBlockPOICount.Location = new System.Drawing.Point(363, 95);
            this.tB_BlockInfoLeastSmallBlockPOICount.Name = "tB_BlockInfoLeastSmallBlockPOICount";
            this.tB_BlockInfoLeastSmallBlockPOICount.ReadOnly = true;
            this.tB_BlockInfoLeastSmallBlockPOICount.Size = new System.Drawing.Size(50, 22);
            this.tB_BlockInfoLeastSmallBlockPOICount.TabIndex = 23;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(298, 98);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(59, 12);
            this.label29.TabIndex = 22;
            this.label29.Text = "POI counts:";
            // 
            // tB_BlockInfoLeastSmallBlock
            // 
            this.tB_BlockInfoLeastSmallBlock.Location = new System.Drawing.Point(249, 95);
            this.tB_BlockInfoLeastSmallBlock.Name = "tB_BlockInfoLeastSmallBlock";
            this.tB_BlockInfoLeastSmallBlock.ReadOnly = true;
            this.tB_BlockInfoLeastSmallBlock.Size = new System.Drawing.Size(42, 22);
            this.tB_BlockInfoLeastSmallBlock.TabIndex = 21;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(140, 98);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(91, 12);
            this.label28.TabIndex = 20;
            this.label28.Text = "Least Small Index:";
            // 
            // tB_BlockInfoLeastBlockPOICount
            // 
            this.tB_BlockInfoLeastBlockPOICount.Location = new System.Drawing.Point(363, 68);
            this.tB_BlockInfoLeastBlockPOICount.Name = "tB_BlockInfoLeastBlockPOICount";
            this.tB_BlockInfoLeastBlockPOICount.ReadOnly = true;
            this.tB_BlockInfoLeastBlockPOICount.Size = new System.Drawing.Size(50, 22);
            this.tB_BlockInfoLeastBlockPOICount.TabIndex = 19;
            // 
            // tB_BlockInfoLeastBlock
            // 
            this.tB_BlockInfoLeastBlock.Location = new System.Drawing.Point(249, 68);
            this.tB_BlockInfoLeastBlock.Name = "tB_BlockInfoLeastBlock";
            this.tB_BlockInfoLeastBlock.ReadOnly = true;
            this.tB_BlockInfoLeastBlock.Size = new System.Drawing.Size(42, 22);
            this.tB_BlockInfoLeastBlock.TabIndex = 18;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(298, 71);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(59, 12);
            this.label27.TabIndex = 17;
            this.label27.Text = "POI counts:";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(141, 71);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(92, 12);
            this.label26.TabIndex = 16;
            this.label26.Text = "Least Large Index:";
            // 
            // btn_DivisionOverview
            // 
            this.btn_DivisionOverview.Enabled = false;
            this.btn_DivisionOverview.Location = new System.Drawing.Point(66, 125);
            this.btn_DivisionOverview.Name = "btn_DivisionOverview";
            this.btn_DivisionOverview.Size = new System.Drawing.Size(66, 22);
            this.btn_DivisionOverview.TabIndex = 7;
            this.btn_DivisionOverview.Text = "Overview";
            this.btn_DivisionOverview.UseVisualStyleBackColor = true;
            this.btn_DivisionOverview.Click += new System.EventHandler(this.btn_DivisionOverview_Click);
            // 
            // btn_blockinforeset
            // 
            this.btn_blockinforeset.Enabled = false;
            this.btn_blockinforeset.Location = new System.Drawing.Point(363, 122);
            this.btn_blockinforeset.Name = "btn_blockinforeset";
            this.btn_blockinforeset.Size = new System.Drawing.Size(50, 23);
            this.btn_blockinforeset.TabIndex = 14;
            this.btn_blockinforeset.Text = "Reset";
            this.btn_blockinforeset.UseVisualStyleBackColor = true;
            this.btn_blockinforeset.Click += new System.EventHandler(this.btn_blockinforeset_Click);
            // 
            // btn_listrefine
            // 
            this.btn_listrefine.Enabled = false;
            this.btn_listrefine.Location = new System.Drawing.Point(307, 122);
            this.btn_listrefine.Name = "btn_listrefine";
            this.btn_listrefine.Size = new System.Drawing.Size(50, 23);
            this.btn_listrefine.TabIndex = 13;
            this.btn_listrefine.Text = "Refine";
            this.btn_listrefine.UseVisualStyleBackColor = true;
            this.btn_listrefine.Click += new System.EventHandler(this.btn_listrefine_Click);
            // 
            // tB_BlockInfoThreshold
            // 
            this.tB_BlockInfoThreshold.Enabled = false;
            this.tB_BlockInfoThreshold.Location = new System.Drawing.Point(226, 124);
            this.tB_BlockInfoThreshold.Name = "tB_BlockInfoThreshold";
            this.tB_BlockInfoThreshold.Size = new System.Drawing.Size(65, 22);
            this.tB_BlockInfoThreshold.TabIndex = 12;
            this.tB_BlockInfoThreshold.TextChanged += new System.EventHandler(this.tB_BlockInfoThreshold_TextChanged);
            // 
            // tB_BlockInfoCount
            // 
            this.tB_BlockInfoCount.Location = new System.Drawing.Point(13, 125);
            this.tB_BlockInfoCount.Name = "tB_BlockInfoCount";
            this.tB_BlockInfoCount.ReadOnly = true;
            this.tB_BlockInfoCount.Size = new System.Drawing.Size(47, 22);
            this.tB_BlockInfoCount.TabIndex = 11;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(8, 111);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(35, 12);
            this.label25.TabIndex = 10;
            this.label25.Text = "Total: ";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(140, 127);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(78, 12);
            this.label24.TabIndex = 9;
            this.label24.Text = "List Threshold: ";
            // 
            // tB_BlockInfoLargestSmallBlockPOICount
            // 
            this.tB_BlockInfoLargestSmallBlockPOICount.Location = new System.Drawing.Point(363, 42);
            this.tB_BlockInfoLargestSmallBlockPOICount.Name = "tB_BlockInfoLargestSmallBlockPOICount";
            this.tB_BlockInfoLargestSmallBlockPOICount.ReadOnly = true;
            this.tB_BlockInfoLargestSmallBlockPOICount.Size = new System.Drawing.Size(50, 22);
            this.tB_BlockInfoLargestSmallBlockPOICount.TabIndex = 8;
            // 
            // tB_BlockInfoLargestBlockPOICount
            // 
            this.tB_BlockInfoLargestBlockPOICount.Location = new System.Drawing.Point(363, 15);
            this.tB_BlockInfoLargestBlockPOICount.Name = "tB_BlockInfoLargestBlockPOICount";
            this.tB_BlockInfoLargestBlockPOICount.ReadOnly = true;
            this.tB_BlockInfoLargestBlockPOICount.Size = new System.Drawing.Size(50, 22);
            this.tB_BlockInfoLargestBlockPOICount.TabIndex = 7;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(298, 47);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(62, 12);
            this.label23.TabIndex = 6;
            this.label23.Text = "POI counts: ";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(298, 20);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(62, 12);
            this.label22.TabIndex = 5;
            this.label22.Text = "POI counts: ";
            // 
            // tB_BlockInfoLargestSmallBlock
            // 
            this.tB_BlockInfoLargestSmallBlock.Location = new System.Drawing.Point(249, 42);
            this.tB_BlockInfoLargestSmallBlock.Name = "tB_BlockInfoLargestSmallBlock";
            this.tB_BlockInfoLargestSmallBlock.ReadOnly = true;
            this.tB_BlockInfoLargestSmallBlock.Size = new System.Drawing.Size(42, 22);
            this.tB_BlockInfoLargestSmallBlock.TabIndex = 4;
            // 
            // tB_BlockInfoLargestBlock
            // 
            this.tB_BlockInfoLargestBlock.Location = new System.Drawing.Point(249, 15);
            this.tB_BlockInfoLargestBlock.Name = "tB_BlockInfoLargestBlock";
            this.tB_BlockInfoLargestBlock.ReadOnly = true;
            this.tB_BlockInfoLargestBlock.Size = new System.Drawing.Size(42, 22);
            this.tB_BlockInfoLargestBlock.TabIndex = 3;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(141, 47);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(102, 12);
            this.label21.TabIndex = 2;
            this.label21.Text = "Inside Largest Small:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label20.Location = new System.Drawing.Point(141, 20);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(105, 12);
            this.label20.TabIndex = 1;
            this.label20.Text = "Largest Large Index: ";
            // 
            // lB_BlockInfo
            // 
            this.lB_BlockInfo.Font = new System.Drawing.Font("PMingLiU", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lB_BlockInfo.FormattingEnabled = true;
            this.lB_BlockInfo.ItemHeight = 11;
            this.lB_BlockInfo.Location = new System.Drawing.Point(10, 17);
            this.lB_BlockInfo.Name = "lB_BlockInfo";
            this.lB_BlockInfo.Size = new System.Drawing.Size(122, 92);
            this.lB_BlockInfo.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(297, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(75, 73);
            this.pictureBox1.TabIndex = 36;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 667);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.gB_BlockInfo);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tB_version);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.btn_clear);
            this.Controls.Add(this.tB_LonSmallBlockCount);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.tB_LonSmallLayerWidth);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.tB_LonBlockCount);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.tB_LonLayerWidth);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tB_lonend);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tB_lonstart);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tB_latend);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tB_latstart);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tB_SmallBlockCount);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tB_SmallLayerWidth);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tB_BlockCount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tB_LayerWidth);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DGV_excel);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.btn_savebin);
            this.Controls.Add(this.btn_openexcel);
            this.Controls.Add(this.tB_bin);
            this.Controls.Add(this.tB_excel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "SpeedCam Transform Tool v2 - Cropped";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.Click += new System.EventHandler(this.Form1_Click);
            this.TextChanged += new System.EventHandler(this.Form1_TextChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_excel)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gB_BlockInfo.ResumeLayout(false);
            this.gB_BlockInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tB_excel;
        private System.Windows.Forms.TextBox tB_bin;
        private System.Windows.Forms.Button btn_openexcel;
        private System.Windows.Forms.Button btn_savebin;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.OpenFileDialog OFDlg1;
        private System.Windows.Forms.SaveFileDialog SFDlg1;
        private System.Windows.Forms.DataGridView DGV_excel;
        private System.Windows.Forms.Label label3;
        //private System.Windows.Forms.TextBox tB_LayerWidth;
        public System.Windows.Forms.TextBox tB_LayerWidth;
        private System.Windows.Forms.Label label4;
        //private System.Windows.Forms.TextBox tB_BlockCount;
        public System.Windows.Forms.TextBox tB_BlockCount;
        private System.Windows.Forms.Label label5;
        //private System.Windows.Forms.TextBox tB_SmallLayerWidth;
        public System.Windows.Forms.TextBox tB_SmallLayerWidth;
        private System.Windows.Forms.Label label6;
        //private System.Windows.Forms.TextBox tB_SmallBlockCount;
        public System.Windows.Forms.TextBox tB_SmallBlockCount;
        private System.Windows.Forms.Label label7;
        //private System.Windows.Forms.TextBox tB_latstart;
        public System.Windows.Forms.TextBox tB_latstart;
        private System.Windows.Forms.Label label8;
        //private System.Windows.Forms.TextBox tB_latend;
        public System.Windows.Forms.TextBox tB_latend;
        private System.Windows.Forms.Label label9;
        //private System.Windows.Forms.TextBox tB_lonstart;
        public System.Windows.Forms.TextBox tB_lonstart;
        private System.Windows.Forms.Label label10;
        //private System.Windows.Forms.TextBox tB_lonend;
        public System.Windows.Forms.TextBox tB_lonend;
        private System.Windows.Forms.Label label11;
        //private System.Windows.Forms.TextBox tB_LonLayerWidth;
        public System.Windows.Forms.TextBox tB_LonLayerWidth;
        private System.Windows.Forms.Label label12;
        //private System.Windows.Forms.TextBox tB_LonBlockCount;
        public System.Windows.Forms.TextBox tB_LonBlockCount;
        private System.Windows.Forms.Label label13;
        //private System.Windows.Forms.TextBox tB_LonSmallLayerWidth;
        public System.Windows.Forms.TextBox tB_LonSmallLayerWidth;
        private System.Windows.Forms.Label label14;
        //private System.Windows.Forms.TextBox tB_LonSmallBlockCount;
        public System.Windows.Forms.TextBox tB_LonSmallBlockCount;
        private System.Windows.Forms.Button btn_clear;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tB_version;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btn_slicingadd;
        private System.Windows.Forms.TextBox tB_slicingend;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox tB_slicingstart;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ListBox lB_slicingrules;
        private System.Windows.Forms.Button btn_slicingdel;
        private System.Windows.Forms.TextBox tB_slicingnum;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.GroupBox gB_BlockInfo;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ListBox lB_BlockInfo;
        private System.Windows.Forms.TextBox tB_BlockInfoLargestBlockPOICount;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox tB_BlockInfoLargestSmallBlock;
        private System.Windows.Forms.TextBox tB_BlockInfoLargestBlock;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox tB_BlockInfoLargestSmallBlockPOICount;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Button btn_blockinforeset;
        private System.Windows.Forms.Button btn_listrefine;
        private System.Windows.Forms.TextBox tB_BlockInfoThreshold;
        private System.Windows.Forms.TextBox tB_BlockInfoCount;
        private System.Windows.Forms.Button btn_DivisionOverview;
        private System.Windows.Forms.TextBox tB_BlockInfoLeastBlockPOICount;
        private System.Windows.Forms.TextBox tB_BlockInfoLeastBlock;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox tB_BlockInfoLeastSmallBlockPOICount;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox tB_BlockInfoLeastSmallBlock;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox tB_lonslicingnum;
        private System.Windows.Forms.TextBox tB_lonslicingend;
        private System.Windows.Forms.TextBox tB_lonslicingstart;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.ListBox lB_nestedslicingrules;
        private System.Windows.Forms.Button btn_nestedslicingdel;
        private System.Windows.Forms.Button btn_nestedslicingadd;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

