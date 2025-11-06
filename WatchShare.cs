using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibVLCSharp.Shared;

namespace LibVLCSharp.WinForms.Sample
{
    public partial class WatchShare : UserControl, IDisposable
    {
        VideoView videoView;
        public string Port { get; set; }
        public LibVLC _libVLC;


        public WatchShare()
        {
            if (!DesignMode)
            {
                Core.Initialize();
            }

            InitializeComponent();

            Port = "35001";

            _libVLC = new LibVLC();
            videoView = new VideoView();

            videoView.MediaPlayer = new MediaPlayer(_libVLC);

            SizeChange(new object(), new EventArgs());
            SizeChanged += SizeChange;

            Controls.Add(videoView);
            videoView.Show();
        }

        public void Connect()
        {
            string sourceAdress = $"udp://@:{Port}";
            videoView.MediaPlayer.Play(new Media(_libVLC, sourceAdress, FromType.FromLocation));
        }

        public void Disconnect()
        {
            videoView.MediaPlayer.Stop();
        }

        void SizeChange(object sender, EventArgs e)
        {
            videoView.Height = ClientRectangle.Height;
            videoView.Width = ClientRectangle.Width;
        }
    }
}
