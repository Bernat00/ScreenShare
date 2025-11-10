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

            videoView.MediaPlayer = new MediaPlayer(_libVLC)
            {
                FileCaching = 0,
                NetworkCaching = 0,
                EnableHardwareDecoding = true,
            };

            SizeChange(new object(), new EventArgs());
            SizeChanged += SizeChange;

            Controls.Add(videoView);
            videoView.Show();
        }

        public void Connect()
        {
            string sourceAdress = $"udp://@:{Port}";
            var media = new Media(_libVLC, sourceAdress, FromType.FromLocation);
            media.AddOption(":vout=direct3d11"); // ensures GPU scaling
            media.AddOption(":scale-factor=0"); // auto fit
            media.AddOption(":video-filter=scale"); // enable scaling filter
            media.AddOption(":sout-transcode-scale=1");
            media.AddOption(":scale-mode=best");      
            media.AddOption("--video-filter=adjust");     // enable adjust filter
            media.AddOption("--adjust-saturation=0.0");


            videoView.MediaPlayer.Play(media);
            videoView.Size = new Size(1920, 1080);
            SizeChange(new object(), new EventArgs());
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
