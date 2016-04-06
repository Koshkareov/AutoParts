namespace AutoPartsImport
{
    partial class Form1
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
            this.buttonSelectFile = new System.Windows.Forms.Button();
            this.openImportFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.buttonImport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonSelectFile
            // 
            this.buttonSelectFile.Location = new System.Drawing.Point(327, 12);
            this.buttonSelectFile.Name = "buttonSelectFile";
            this.buttonSelectFile.Size = new System.Drawing.Size(124, 23);
            this.buttonSelectFile.TabIndex = 0;
            this.buttonSelectFile.Text = "SelectFile 4 Import";
            this.buttonSelectFile.UseVisualStyleBackColor = true;
            this.buttonSelectFile.Click += new System.EventHandler(this.button1_Click);
            // 
            // openImportFileDialog
            // 
            this.openImportFileDialog.InitialDirectory = "C:\\Import";
            // 
            // buttonImport
            // 
            this.buttonImport.Enabled = false;
            this.buttonImport.Location = new System.Drawing.Point(327, 60);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(124, 23);
            this.buttonImport.TabIndex = 1;
            this.buttonImport.Text = "Import";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 268);
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.buttonSelectFile);
            this.Name = "Form1";
            this.Text = "Import AutoParts";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSelectFile;
        private System.Windows.Forms.OpenFileDialog openImportFileDialog;
        private System.Windows.Forms.Button buttonImport;
    }
}

