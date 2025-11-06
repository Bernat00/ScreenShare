using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using StrEnum;

using System.Windows.Forms;

namespace LibVLCSharp.WinForms.Sample
{
    public struct FFmpegParams
    {
        public string ip;
        public string port;
        public Encoder encoder;
    }

    public class Encoder : StringEnum<Encoder>
    {
        public static readonly Encoder Universal = Define("libx264");
    }

    public class FFMpegScreenShare: IDisposable
    {
        Process ffmpeg;
        string ffmpegPath = @".\ffmpeg";
        string ffmpegName = "ffmpeg.exe";
        public FFmpegParams FfmpegParams;


        public string FfmpegPath
        {
            set { ffmpegPath = value; }
        }

        string FfmpegParamsString
        {
            get 
            { 
                return $"-f gdigrab -framerate 30 -i desktop -c:v {(string)FfmpegParams.encoder} -tune " +
                    $"zerolatency -preset ultrafast -f mpegts" +
                    $" \"udp://{FfmpegParams.ip}:{FfmpegParams.port}?pkt_size=1316\"";
            }
        }



        public FFMpegScreenShare()
        {
            FfmpegParams = new FFmpegParams()
            {
                ip="127.0.0.0",
                port= "35001",
                encoder=Encoder.Universal
            };
        }

        public void StartShare()
        {
            string filename = Path.Combine(ffmpegPath, ffmpegName);

            ffmpeg = new Process();
            ffmpeg.StartInfo.FileName = filename;
            ffmpeg.StartInfo.Arguments = FfmpegParamsString;
            //ffmpeg.StartInfo.CreateNoWindow = true;

            ffmpeg.Start();


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
