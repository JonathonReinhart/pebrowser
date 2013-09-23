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
            this.label1 = new System.Windows.Forms.Label();
            this.dgvOptionalHeader = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFileHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptionalHeader)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvFileHeader
            // 
            this.dgvFileHeader.AllowUserToAddRows = false;
            this.dgvFileHeader.AllowUserToDeleteRows = false;
            this.dgvFileHeader.AllowUserToResizeRows = false;
            this.dgvFileHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFileHeader.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvFileHeader.Location = new System.Drawing.Point(3, 16);
            this.dgvFileHeader.Name = "dgvFileHeader";
            this.dgvFileHeader.ReadOnly = true;
            this.dgvFileHeader.RowHeadersVisible = false;
            this.dgvFileHeader.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFileHeader.Size = new System.Drawing.Size(439, 163);
            this.dgvFileHeader.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "File Header:";
            // 
            // dgvOptionalHeader
            // 
            this.dgvOptionalHeader.AllowUserToAddRows = false;
            this.dgvOptionalHeader.AllowUserToDeleteRows = false;
            this.dgvOptionalHeader.AllowUserToResizeRows = false;
            this.dgvOptionalHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOptionalHeader.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvOptionalHeader.Location = new System.Drawing.Point(3, 197);
            this.dgvOptionalHeader.Name = "dgvOptionalHeader";
            this.dgvOptionalHeader.ReadOnly = true;
            this.dgvOptionalHeader.RowHeadersVisible = false;
            this.dgvOptionalHeader.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOptionalHeader.Size = new System.Drawing.Size(439, 150);
            this.dgvOptionalHeader.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Optional Header:";
            // 
            // PEHeaderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvOptionalHeader);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvFileHeader);
            this.Name = "PEHeaderControl";
            this.Size = new System.Drawing.Size(522, 391);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFileHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptionalHeader)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvFileHeader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvOptionalHeader;
        private System.Windows.Forms.Label label2;
    }
}
