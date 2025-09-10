namespace excel2bin
{
    partial class Form2
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
            this.DGV_DivisionOverview = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.tB_ColorTypes = new System.Windows.Forms.TextBox();
            this.btn_overviewrefresh = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tB_SelectedCellLon = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tB_SelectedCellLat = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tB_OverviewtotalPOIs = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tB_OverviewcellPOI = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_DivisionOverview)).BeginInit();
            this.SuspendLayout();
            // 
            // DGV_DivisionOverview
            // 
            this.DGV_DivisionOverview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_DivisionOverview.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DGV_DivisionOverview.Location = new System.Drawing.Point(0, 41);
            this.DGV_DivisionOverview.MultiSelect = false;
            this.DGV_DivisionOverview.Name = "DGV_DivisionOverview";
            this.DGV_DivisionOverview.ReadOnly = true;
            this.DGV_DivisionOverview.RowTemplate.Height = 24;
            this.DGV_DivisionOverview.Size = new System.Drawing.Size(1317, 650);
            this.DGV_DivisionOverview.TabIndex = 0;
            this.DGV_DivisionOverview.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_DivisionOverview_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("PMingLiU", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(220, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Color Types: ";
            // 
            // tB_ColorTypes
            // 
            this.tB_ColorTypes.Location = new System.Drawing.Point(298, 12);
            this.tB_ColorTypes.Name = "tB_ColorTypes";
            this.tB_ColorTypes.Size = new System.Drawing.Size(100, 22);
            this.tB_ColorTypes.TabIndex = 2;
            // 
            // btn_overviewrefresh
            // 
            this.btn_overviewrefresh.Enabled = false;
            this.btn_overviewrefresh.Location = new System.Drawing.Point(417, 12);
            this.btn_overviewrefresh.Name = "btn_overviewrefresh";
            this.btn_overviewrefresh.Size = new System.Drawing.Size(75, 23);
            this.btn_overviewrefresh.TabIndex = 3;
            this.btn_overviewrefresh.Text = "Refresh";
            this.btn_overviewrefresh.UseVisualStyleBackColor = true;
            this.btn_overviewrefresh.Click += new System.EventHandler(this.btn_overviewrefresh_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(598, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Lon:";
            // 
            // tB_SelectedCellLon
            // 
            this.tB_SelectedCellLon.Location = new System.Drawing.Point(631, 12);
            this.tB_SelectedCellLon.Name = "tB_SelectedCellLon";
            this.tB_SelectedCellLon.ReadOnly = true;
            this.tB_SelectedCellLon.Size = new System.Drawing.Size(100, 22);
            this.tB_SelectedCellLon.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(738, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Lat:";
            // 
            // tB_SelectedCellLat
            // 
            this.tB_SelectedCellLat.Location = new System.Drawing.Point(767, 12);
            this.tB_SelectedCellLat.Name = "tB_SelectedCellLat";
            this.tB_SelectedCellLat.ReadOnly = true;
            this.tB_SelectedCellLat.Size = new System.Drawing.Size(100, 22);
            this.tB_SelectedCellLat.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "Total POIs:";
            // 
            // tB_OverviewtotalPOIs
            // 
            this.tB_OverviewtotalPOIs.Location = new System.Drawing.Point(75, 12);
            this.tB_OverviewtotalPOIs.Name = "tB_OverviewtotalPOIs";
            this.tB_OverviewtotalPOIs.ReadOnly = true;
            this.tB_OverviewtotalPOIs.Size = new System.Drawing.Size(72, 22);
            this.tB_OverviewtotalPOIs.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(930, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "POIs:";
            // 
            // tB_OverviewcellPOI
            // 
            this.tB_OverviewcellPOI.Location = new System.Drawing.Point(966, 12);
            this.tB_OverviewcellPOI.Name = "tB_OverviewcellPOI";
            this.tB_OverviewcellPOI.ReadOnly = true;
            this.tB_OverviewcellPOI.Size = new System.Drawing.Size(81, 22);
            this.tB_OverviewcellPOI.TabIndex = 11;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1317, 691);
            this.Controls.Add(this.tB_OverviewcellPOI);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tB_OverviewtotalPOIs);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tB_SelectedCellLat);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tB_SelectedCellLon);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_overviewrefresh);
            this.Controls.Add(this.tB_ColorTypes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DGV_DivisionOverview);
            this.Name = "Form2";
            this.Text = "Layer Division Overview";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_DivisionOverview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private System.Windows.Forms.DataGridView DGV_DivisionOverview;
        public System.Windows.Forms.DataGridView DGV_DivisionOverview;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox tB_ColorTypes;
        private System.Windows.Forms.Button btn_overviewrefresh;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tB_SelectedCellLon;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tB_SelectedCellLat;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox tB_OverviewtotalPOIs;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tB_OverviewcellPOI;
    }
}