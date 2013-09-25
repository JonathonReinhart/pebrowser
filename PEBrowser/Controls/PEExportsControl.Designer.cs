namespace PEBrowser.Controls
{
    partial class PEExportsControl
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
            this.dgvExports = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExports)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvExports
            // 
            this.dgvExports.AllowUserToAddRows = false;
            this.dgvExports.AllowUserToDeleteRows = false;
            this.dgvExports.AllowUserToResizeRows = false;
            this.dgvExports.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvExports.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvExports.Location = new System.Drawing.Point(0, 0);
            this.dgvExports.Name = "dgvExports";
            this.dgvExports.ReadOnly = true;
            this.dgvExports.RowHeadersVisible = false;
            this.dgvExports.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvExports.Size = new System.Drawing.Size(449, 307);
            this.dgvExports.TabIndex = 4;
            // 
            // PEExportsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvExports);
            this.Name = "PEExportsControl";
            this.Size = new System.Drawing.Size(449, 307);
            ((System.ComponentModel.ISupportInitialize)(this.dgvExports)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvExports;
    }
}
