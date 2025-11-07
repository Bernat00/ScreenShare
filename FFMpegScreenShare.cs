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

    //todo preset enum (fast, veryfast, stb)
    
    public class Encoder : StringEnum<Encoder>
    {
        public static readonly Encoder Universal = Define("libx264");
        public static readonly Encoder Nvidia = Define("libx264");
        public static readonly Encoder Intel = Define("h264_qsv");
        public static readonly Encoder UniversalH265 = Define("libx264");
        public static readonly Encoder NvidiaH265 = Define("hevc_nvenc");
        public static readonly Encoder IntelH265 = Define("hevc_qsv");
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
            get //todo megnezni mi ez a swich sintax
            {

                // Pixel format selection
                string pixelFormat = FfmpegParams.encoder switch
                {
                    var e when e == Encoder.Intel || e == Encoder.IntelH265 => "nv12",        // QSV prefers NV12
                    var e when e == Encoder.Nvidia || e == Encoder.NvidiaH265 => "yuv420p",   // NVENC prefers YUV420P
                    _ => "yuv444p"                                                            // Software (x264/x265): crisp desktop text
                };

                // Encoder-specific tuning
                string encoderTuning = FfmpegParams.encoder switch
                {
                    // Intel QuickSync — H.264
                    var e when e == Encoder.Intel => "-look_ahead 0 -async_depth 1 -tune zerolatency",

                    // Intel QuickSync — H.265
                    var e when e == Encoder.IntelH265 => "-look_ahead 0 -async_depth 1 -tune:v 0 -low_power 1 -global_quality 20",

                    // NVIDIA NVENC — H.264
                    var e when e == Encoder.Nvidia => "-rc:v vbr -cq 19 -profile:v high -bf 2 -no-scenecut 1 -spatial_aq 1",

                    // NVIDIA NVENC — H.265
                    var e when e == Encoder.NvidiaH265 => "-rc:v vbr -cq 19 -profile:v main -bf 2 -no-scenecut 1 -spatial_aq 1",

                    // Software x264
                    var e when e == Encoder.Universal => "-tune psnr -preset fast",

                    // Software x265
                    var e when e == Encoder.UniversalH265 => "-preset fast -x265-params \"tune=psnr:aq-mode=3:vbv-bufsize=10000:vbv-maxrate=10000\"",

                    _ => ""
                };

                // Shared quality settings
                string crf = "18";
                string bitrate = "10M";
                string preset = "fast";

                // Build the final FFmpeg argument string
                string ffmpegArgs = string.Join(" ",
                    // Input (screen capture)
                    "-f gdigrab",
                    "-framerate 30",
                    "-thread_queue_size 1024",
                    "-rtbufsize 200M",
                    "-probesize 32M",
                    "-analyzeduration 0",
                    "-i desktop",

                    // Video encoding
                    $"-c:v {(string)FfmpegParams.encoder}",
                    $"-pix_fmt {pixelFormat}",
                    $"-preset {preset}",
                    $"-crf {crf}",
                    $"-b:v {bitrate}",
                    $"-maxrate {bitrate}",
                    $"-bufsize {bitrate}",
                    "-g 60",
                    "-r 30",
                    "-fps_mode cfr",
                    "-flags low_delay",
                    "-fflags nobuffer",
                    "-use_wallclock_as_timestamps 1",
                    "-threads 4",

                    // Extra buffering
                    "-thread_queue_size 2048",
                    "-rtbufsize 500M",

                    encoderTuning,

                    // Output (UDP)
                    "-f mpegts",
                    $"\"udp://{FfmpegParams.ip}:{FfmpegParams.port}?pkt_size=1316&ttl=1&overrun_nonfatal=1\""
                );






                return ffmpegArgs;
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
            ffmpeg.StartInfo.CreateNoWindow = true;

            ffmpeg.Start();

            ffmpeg.PriorityClass = ProcessPriorityClass.AboveNormal;

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
