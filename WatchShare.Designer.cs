using System;

namespace LibVLCSharp.WinForms.Sample
{
    partial class WatchShare
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
                this.videoView?.Dispose();
                this._libVLC?.Dispose();
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
            SuspendLayout();
            // 
            // WatchShare
            // 
            Name = "WatchShare";
            Size = new System.Drawing.Size(660, 405);
            ResumeLayout(false);
        }

        #endregion
    }
}
