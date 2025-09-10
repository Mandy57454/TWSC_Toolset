namespace excel2bin
{
    partial class Fm_evaluatedivision
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
            this.DGV_verifydivision = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_verifydivision)).BeginInit();
            this.SuspendLayout();
            // 
            // DGV_verifydivision
            // 
            this.DGV_verifydivision.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_verifydivision.Location = new System.Drawing.Point(12, 12);
            this.DGV_verifydivision.MultiSelect = false;
            this.DGV_verifydivision.Name = "DGV_verifydivision";
            this.DGV_verifydivision.ReadOnly = true;
            this.DGV_verifydivision.RowTemplate.Height = 24;
            this.DGV_verifydivision.Size = new System.Drawing.Size(843, 427);
            this.DGV_verifydivision.TabIndex = 0;
            // 
            // Fm_evaluatedivision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 451);
            this.Controls.Add(this.DGV_verifydivision);
            this.Name = "Fm_evaluatedivision";
            this.Text = "Form3";
            this.Load += new System.EventHandler(this.Fm_evaluatedivision_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_verifydivision)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView DGV_verifydivision;
    }
}