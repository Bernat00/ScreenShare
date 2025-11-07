using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibVLCSharp.WinForms.Sample
{
    public partial class ShareingView : UserControl, IDisposable
    {
        readonly FFMpegScreenShare share;
        public ShareingView()
        {
            InitializeComponent();
            share = new FFMpegScreenShare();
        }

        public ShareingView(FFMpegScreenShare share)
        {
            InitializeComponent();
            this.share = share;
        }

        public void StartShare(bool debug)
        {
            share.StartShare(debug);
        }
    }
}
