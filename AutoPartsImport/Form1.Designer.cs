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
            this.numericUpDownDeliveryTime = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDeliveryTime)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSelectFile
            // 
            this.buttonSelectFile.Location = new System.Drawing.Point(75, 26);
            this.buttonSelectFile.Name = "buttonSelectFile";
            this.buttonSelectFile.Size = new System.Drawing.Size(195, 23);
            this.buttonSelectFile.TabIndex = 0;
            this.buttonSelectFile.Text = "Выберите файл для импорта";
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
            this.buttonImport.Location = new System.Drawing.Point(75, 103);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(195, 34);
            this.buttonImport.TabIndex = 1;
            this.buttonImport.Text = "Импорт";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // numericUpDownDeliveryTime
            // 
            this.numericUpDownDeliveryTime.Location = new System.Drawing.Point(214, 64);
            this.numericUpDownDeliveryTime.Name = "numericUpDownDeliveryTime";
            this.numericUpDownDeliveryTime.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownDeliveryTime.TabIndex = 2;
            this.numericUpDownDeliveryTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownDeliveryTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(75, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Срок поставки (дней)";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 201);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownDeliveryTime);
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.buttonSelectFile);
            this.Name = "Form1";
            this.Text = "Import ALFA PARTS";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDeliveryTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSelectFile;
        private System.Windows.Forms.OpenFileDialog openImportFileDialog;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.NumericUpDown numericUpDownDeliveryTime;
        private System.Windows.Forms.Label label1;
    }
}

