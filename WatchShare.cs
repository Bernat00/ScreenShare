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
    public partial class WatchShare : VideoView, IDisposable
    {
        public LibVLC _libVLC;
        public MediaPlayer _mp;

        public WatchShare()
        {
            if (!DesignMode)
            {
                Core.Initialize();
            }

            InitializeComponent();
            _libVLC = new LibVLC();
            _mp = new MediaPlayer(_libVLC);
            MediaPlayer = _mp;
        }

        public void Connect(string sourceAdress = "udp://@:35001")
        {
            _mp.Play(new Media(_libVLC, sourceAdress, FromType.FromLocation));
        }

        public void Disconnect()
        {
            _mp.Stop();
        }

    }
}
