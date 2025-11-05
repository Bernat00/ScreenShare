namespace LibVLCSharp.WinForms.Sample
{
    partial class Menu
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
            watchBTN = new System.Windows.Forms.Button();
            shareBTN = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // watchBTN
            // 
            watchBTN.Font = new System.Drawing.Font("Segoe UI", 16F);
            watchBTN.Location = new System.Drawing.Point(204, 211);
            watchBTN.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            watchBTN.Name = "watchBTN";
            watchBTN.Size = new System.Drawing.Size(271, 63);
            watchBTN.TabIndex = 0;
            watchBTN.Text = "Watch Share";
            watchBTN.UseVisualStyleBackColor = true;
            // 
            // shareBTN
            // 
            shareBTN.Font = new System.Drawing.Font("Segoe UI", 16F);
            shareBTN.Location = new System.Drawing.Point(513, 211);
            shareBTN.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            shareBTN.Name = "shareBTN";
            shareBTN.Size = new System.Drawing.Size(245, 63);
            shareBTN.TabIndex = 1;
            shareBTN.Text = "Start Share";
            shareBTN.UseVisualStyleBackColor = true;
            // 
            // Menu
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(15F, 37F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(shareBTN);
            Controls.Add(watchBTN);
            Font = new System.Drawing.Font("Segoe UI", 16F);
            Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            Name = "Menu";
            Size = new System.Drawing.Size(977, 494);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button watchBTN;
        private System.Windows.Forms.Button shareBTN;
    }
}
