using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using LibVLCSharp.Shared;


namespace LibVLCSharp.WinForms.Sample
{
    public partial class Form1 : Form
    {
        Menu menu;
        ShareingView shareing;
        WatchShare watchShare;

        public Form1()
        {
            InitializeComponent();

            menu = new Menu();
            Controls.Add(menu);
            menu.ShareBTN_Clicked += SharingEvent;
            menu.WatchBTN_Clicked += WatchShareEvent;
        }

        void SharingEvent(object s, EventArgs e)
        {
            FFMpegScreenShare fFMpegScreenShare = new();
            fFMpegScreenShare.FfmpegParams.ip = menu.SelectedAdapter.Broadcast.ToString();

            shareing = new ShareingView(fFMpegScreenShare);
            Controls.Add(shareing);
            menu.Hide();
            shareing.Show();
            shareing.StartShare();

        }
        void WatchShareEvent(object s, EventArgs e)
        {
            watchShare = new WatchShare();
            Controls.Add(watchShare);
            menu.Hide();

            SizeChange(new object(), new EventArgs());
            SizeChanged += SizeChange;

            watchShare.Show();
            watchShare.Connect();
            
        }

        void SizeChange(object sender, EventArgs e)
        {
            watchShare.Height = ClientRectangle.Height;
            watchShare.Width = ClientRectangle.Width;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            watchShare?.Dispose();
            shareing?.Dispose();
        }
    }
}
