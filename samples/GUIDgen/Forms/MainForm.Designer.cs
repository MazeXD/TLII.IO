namespace GUIDgen.Forms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.inpGUID = new System.Windows.Forms.TextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.lblDeveloper = new System.Windows.Forms.Label();
            this.btnCopy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // inpGUID
            // 
            this.inpGUID.BackColor = System.Drawing.SystemColors.Window;
            this.inpGUID.Location = new System.Drawing.Point(12, 12);
            this.inpGUID.Name = "inpGUID";
            this.inpGUID.ReadOnly = true;
            this.inpGUID.Size = new System.Drawing.Size(168, 20);
            this.inpGUID.TabIndex = 0;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(18, 38);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 1;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.OnClickGenerate);
            // 
            // lblDeveloper
            // 
            this.lblDeveloper.AutoSize = true;
            this.lblDeveloper.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeveloper.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblDeveloper.Location = new System.Drawing.Point(78, 65);
            this.lblDeveloper.Name = "lblDeveloper";
            this.lblDeveloper.Size = new System.Drawing.Size(117, 13);
            this.lblDeveloper.TabIndex = 2;
            this.lblDeveloper.Text = "Developed by MazeXD";
            // 
            // btnCopy
            // 
            this.btnCopy.Enabled = false;
            this.btnCopy.Location = new System.Drawing.Point(99, 38);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 23);
            this.btnCopy.TabIndex = 1;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.OnClickCopy);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(192, 79);
            this.Controls.Add(this.lblDeveloper);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.inpGUID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "GUIDgen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox inpGUID;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label lblDeveloper;
        private System.Windows.Forms.Button btnCopy;
    }
}

