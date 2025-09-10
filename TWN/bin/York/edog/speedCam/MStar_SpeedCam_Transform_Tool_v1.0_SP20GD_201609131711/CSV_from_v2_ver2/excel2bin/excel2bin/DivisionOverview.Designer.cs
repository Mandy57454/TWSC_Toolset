namespace excel2bin
{
    partial class fm_DivisionOverview
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
            this.DGV_divisionoverview = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_divisionoverview)).BeginInit();
            this.SuspendLayout();
            // 
            // DGV_divisionoverview
            // 
            this.DGV_divisionoverview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_divisionoverview.Location = new System.Drawing.Point(12, 12);
            this.DGV_divisionoverview.Name = "DGV_divisionoverview";
            this.DGV_divisionoverview.RowTemplate.Height = 24;
            this.DGV_divisionoverview.Size = new System.Drawing.Size(567, 368);
            this.DGV_divisionoverview.TabIndex = 0;
            // 
            // fm_DivisionOverview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 392);
            this.Controls.Add(this.DGV_divisionoverview);
            this.Name = "fm_DivisionOverview";
            this.Text = "Layer Division Overview";
            ((System.ComponentModel.ISupportInitialize)(this.DGV_divisionoverview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV_divisionoverview;
    }
}