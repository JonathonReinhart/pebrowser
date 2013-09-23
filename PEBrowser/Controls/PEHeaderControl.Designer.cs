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
            this.field = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFileHeader)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvFileHeader
            // 
            this.dgvFileHeader.AllowUserToAddRows = false;
            this.dgvFileHeader.AllowUserToDeleteRows = false;
            this.dgvFileHeader.AllowUserToResizeRows = false;
            this.dgvFileHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFileHeader.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.field,
            this.value,
            this.description});
            this.dgvFileHeader.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvFileHeader.Location = new System.Drawing.Point(3, 16);
            this.dgvFileHeader.Name = "dgvFileHeader";
            this.dgvFileHeader.ReadOnly = true;
            this.dgvFileHeader.RowHeadersVisible = false;
            this.dgvFileHeader.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFileHeader.Size = new System.Drawing.Size(439, 150);
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
            // field
            // 
            this.field.HeaderText = "Field Name";
            this.field.Name = "field";
            this.field.ReadOnly = true;
            this.field.Width = 120;
            // 
            // value
            // 
            this.value.HeaderText = "Data Value";
            this.value.Name = "value";
            this.value.ReadOnly = true;
            // 
            // description
            // 
            this.description.HeaderText = "Description";
            this.description.Name = "description";
            this.description.ReadOnly = true;
            this.description.Width = 200;
            // 
            // PEHeaderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvFileHeader);
            this.Name = "PEHeaderControl";
            this.Size = new System.Drawing.Size(522, 291);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFileHeader)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvFileHeader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn field;
        private System.Windows.Forms.DataGridViewTextBoxColumn value;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
    }
}
