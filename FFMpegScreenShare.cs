using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibVLCSharp.WinForms.Sample
{
    internal class FFMpegScreenShare: IDisposable
    {
        Process ffmpeg;
        string ffmpegParams = "-f gdigrab -framerate 30 -i desktop -c:v libx264 -tune zerolatency -preset ultrafast -f mpegts \"udp://127.0.0.1:35001?pkt_size=1316\"";
        string ffmpegPath = @".\ffmpeg";
        string ffmpegName = "ffmpeg.exe";


        public string FfmpegPath
        {
            set { ffmpegPath = value; }
        }

        public string FFmpegParams
        {
            get {  return ffmpegParams; }
            set { ffmpegParams = value; }
        }



        public FFMpegScreenShare()
        {
            
        }

        public void StartShare()
        {
            string filename = Path.Combine(ffmpegPath, ffmpegName);

            try
            {
                ffmpeg = new Process();
                ffmpeg.StartInfo.FileName = filename;
                ffmpeg.StartInfo.Arguments = ffmpegParams;
                ffmpeg.StartInfo.CreateNoWindow = true;
                ffmpeg.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }


        public void Dispose()
        {
            ffmpeg.Kill(true);
        }

        ~FFMpegScreenShare()
        {
            Dispose();
        }
    }
}
