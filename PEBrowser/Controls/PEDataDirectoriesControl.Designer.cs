namespace PEBrowser.Controls
{
    partial class PEDataDirectoriesControl
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
            this.dgvDirs = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDirs)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvDataDirectories
            // 
            this.dgvDirs.AllowUserToAddRows = false;
            this.dgvDirs.AllowUserToDeleteRows = false;
            this.dgvDirs.AllowUserToResizeRows = false;
            this.dgvDirs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDirs.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvDirs.Location = new System.Drawing.Point(3, 3);
            this.dgvDirs.Name = "dgvDirs";
            this.dgvDirs.ReadOnly = true;
            this.dgvDirs.RowHeadersVisible = false;
            this.dgvDirs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDirs.Size = new System.Drawing.Size(367, 403);
            this.dgvDirs.TabIndex = 1;
            // 
            // PEDataDirectoriesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvDirs);
            this.Name = "PEDataDirectoriesControl";
            this.Size = new System.Drawing.Size(373, 409);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDirs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDirs;

    }
}
