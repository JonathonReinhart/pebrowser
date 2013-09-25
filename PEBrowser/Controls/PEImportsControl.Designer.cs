namespace PEBrowser.Controls
{
    partial class PEImportsControl
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
            this.dgvLibraries = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvFunctions = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLibraries)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFunctions)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvLibraries
            // 
            this.dgvLibraries.AllowUserToAddRows = false;
            this.dgvLibraries.AllowUserToDeleteRows = false;
            this.dgvLibraries.AllowUserToResizeRows = false;
            this.dgvLibraries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLibraries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLibraries.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvLibraries.Location = new System.Drawing.Point(0, 0);
            this.dgvLibraries.MultiSelect = false;
            this.dgvLibraries.Name = "dgvLibraries";
            this.dgvLibraries.ReadOnly = true;
            this.dgvLibraries.RowHeadersVisible = false;
            this.dgvLibraries.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLibraries.Size = new System.Drawing.Size(227, 443);
            this.dgvLibraries.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvLibraries);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvFunctions);
            this.splitContainer1.Size = new System.Drawing.Size(683, 443);
            this.splitContainer1.SplitterDistance = 227;
            this.splitContainer1.TabIndex = 3;
            // 
            // dgvFunctions
            // 
            this.dgvFunctions.AllowUserToAddRows = false;
            this.dgvFunctions.AllowUserToDeleteRows = false;
            this.dgvFunctions.AllowUserToResizeRows = false;
            this.dgvFunctions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFunctions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFunctions.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvFunctions.Location = new System.Drawing.Point(0, 0);
            this.dgvFunctions.Name = "dgvFunctions";
            this.dgvFunctions.ReadOnly = true;
            this.dgvFunctions.RowHeadersVisible = false;
            this.dgvFunctions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFunctions.Size = new System.Drawing.Size(452, 443);
            this.dgvFunctions.TabIndex = 3;
            // 
            // PEImportsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "PEImportsControl";
            this.Size = new System.Drawing.Size(692, 451);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLibraries)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFunctions)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvLibraries;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvFunctions;
    }
}
