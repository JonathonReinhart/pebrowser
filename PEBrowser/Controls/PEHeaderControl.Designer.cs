namespace PEBrowser.Controls
{
    partial class PEHeaderControl
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
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvFileHeader = new System.Windows.Forms.DataGridView();
            this.lblFileHeader = new System.Windows.Forms.Label();
            this.dgvOptionalHeader = new System.Windows.Forms.DataGridView();
            this.lblOptionalHeader = new System.Windows.Forms.Label();
            this.dgvOptionalHeader2 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFileHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptionalHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptionalHeader2)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvFileHeader
            // 
            this.dgvFileHeader.AllowUserToAddRows = false;
            this.dgvFileHeader.AllowUserToDeleteRows = false;
            this.dgvFileHeader.AllowUserToResizeRows = false;
            this.dgvFileHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFileHeader.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvFileHeader.Location = new System.Drawing.Point(6, 16);
            this.dgvFileHeader.Name = "dgvFileHeader";
            this.dgvFileHeader.ReadOnly = true;
            this.dgvFileHeader.RowHeadersVisible = false;
            this.dgvFileHeader.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFileHeader.Size = new System.Drawing.Size(390, 163);
            this.dgvFileHeader.TabIndex = 0;
            // 
            // lblFileHeader
            // 
            this.lblFileHeader.AutoSize = true;
            this.lblFileHeader.Location = new System.Drawing.Point(3, 0);
            this.lblFileHeader.Name = "lblFileHeader";
            this.lblFileHeader.Size = new System.Drawing.Size(64, 13);
            this.lblFileHeader.TabIndex = 1;
            this.lblFileHeader.Text = "File Header:";
            // 
            // dgvOptionalHeader
            // 
            this.dgvOptionalHeader.AllowUserToAddRows = false;
            this.dgvOptionalHeader.AllowUserToDeleteRows = false;
            this.dgvOptionalHeader.AllowUserToResizeRows = false;
            this.dgvOptionalHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOptionalHeader.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvOptionalHeader.Location = new System.Drawing.Point(6, 198);
            this.dgvOptionalHeader.Name = "dgvOptionalHeader";
            this.dgvOptionalHeader.ReadOnly = true;
            this.dgvOptionalHeader.RowHeadersVisible = false;
            this.dgvOptionalHeader.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOptionalHeader.Size = new System.Drawing.Size(390, 260);
            this.dgvOptionalHeader.TabIndex = 2;
            // 
            // lblOptionalHeader
            // 
            this.lblOptionalHeader.AutoSize = true;
            this.lblOptionalHeader.Location = new System.Drawing.Point(3, 182);
            this.lblOptionalHeader.Name = "lblOptionalHeader";
            this.lblOptionalHeader.Size = new System.Drawing.Size(87, 13);
            this.lblOptionalHeader.TabIndex = 2;
            this.lblOptionalHeader.Text = "Optional Header:";
            // 
            // dgvOptionalHeader2
            // 
            this.dgvOptionalHeader2.AllowUserToAddRows = false;
            this.dgvOptionalHeader2.AllowUserToDeleteRows = false;
            this.dgvOptionalHeader2.AllowUserToResizeRows = false;
            this.dgvOptionalHeader2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOptionalHeader2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvOptionalHeader2.Location = new System.Drawing.Point(402, 16);
            this.dgvOptionalHeader2.Name = "dgvOptionalHeader2";
            this.dgvOptionalHeader2.ReadOnly = true;
            this.dgvOptionalHeader2.RowHeadersVisible = false;
            this.dgvOptionalHeader2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOptionalHeader2.Size = new System.Drawing.Size(390, 442);
            this.dgvOptionalHeader2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(409, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Optional Header (cont):";
            // 
            // PEHeaderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblFileHeader);
            this.Controls.Add(this.dgvFileHeader);
            this.Controls.Add(this.dgvOptionalHeader);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblOptionalHeader);
            this.Controls.Add(this.dgvOptionalHeader2);
            this.MinimumSize = new System.Drawing.Size(660, 460);
            this.Name = "PEHeaderControl";
            this.Size = new System.Drawing.Size(800, 460);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFileHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptionalHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptionalHeader2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvFileHeader;
        private System.Windows.Forms.Label lblFileHeader;
        private System.Windows.Forms.DataGridView dgvOptionalHeader;
        private System.Windows.Forms.Label lblOptionalHeader;
        private System.Windows.Forms.DataGridView dgvOptionalHeader2;
        private System.Windows.Forms.Label label1;
    }
}
