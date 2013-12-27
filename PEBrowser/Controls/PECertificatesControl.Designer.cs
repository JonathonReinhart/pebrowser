namespace PEBrowser.Controls
{
    partial class PECertificatesControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpSignature = new System.Windows.Forms.GroupBox();
            this.btnViewSig = new System.Windows.Forms.Button();
            this.dgvSignatures = new System.Windows.Forms.DataGridView();
            this.grpSignature.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSignatures)).BeginInit();
            this.SuspendLayout();
            // 
            // grpSignature
            // 
            this.grpSignature.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSignature.Controls.Add(this.btnViewSig);
            this.grpSignature.Controls.Add(this.dgvSignatures);
            this.grpSignature.Location = new System.Drawing.Point(4, 4);
            this.grpSignature.Name = "grpSignature";
            this.grpSignature.Size = new System.Drawing.Size(377, 270);
            this.grpSignature.TabIndex = 0;
            this.grpSignature.TabStop = false;
            this.grpSignature.Text = "Signature List";
            // 
            // btnViewSig
            // 
            this.btnViewSig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewSig.Location = new System.Drawing.Point(233, 176);
            this.btnViewSig.Name = "btnViewSig";
            this.btnViewSig.Size = new System.Drawing.Size(138, 26);
            this.btnViewSig.TabIndex = 1;
            this.btnViewSig.Text = "View Signature...";
            this.btnViewSig.UseVisualStyleBackColor = true;
            this.btnViewSig.Click += new System.EventHandler(this.btnViewSig_Click);
            // 
            // dgvSignatures
            // 
            this.dgvSignatures.AllowUserToAddRows = false;
            this.dgvSignatures.AllowUserToDeleteRows = false;
            this.dgvSignatures.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSignatures.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvSignatures.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSignatures.Location = new System.Drawing.Point(6, 19);
            this.dgvSignatures.MultiSelect = false;
            this.dgvSignatures.Name = "dgvSignatures";
            this.dgvSignatures.ReadOnly = true;
            this.dgvSignatures.RowHeadersVisible = false;
            this.dgvSignatures.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSignatures.ShowEditingIcon = false;
            this.dgvSignatures.Size = new System.Drawing.Size(365, 150);
            this.dgvSignatures.TabIndex = 0;
            // 
            // PECertificatesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpSignature);
            this.Name = "PECertificatesControl";
            this.Size = new System.Drawing.Size(384, 274);
            this.grpSignature.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSignatures)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSignature;
        private System.Windows.Forms.DataGridView dgvSignatures;
        private System.Windows.Forms.Button btnViewSig;
    }
}
