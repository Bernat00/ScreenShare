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
        bool debug;


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
                    // Intel QuickSync prefers NV12, but p010 improves text clarity (10-bit precision)
                    var e when e == Encoder.Intel || e == Encoder.IntelH265 => "p010",

                    // NVENC prefers YUV420P; YUV444P if supported for clearer text
                    var e when e == Encoder.Nvidia || e == Encoder.NvidiaH265 => "yuv420p",

                    // Software encoders (x264/x265): use YUV444P for crisp text
                    _ => "yuv444p"
                };

                // Encoder-specific tuning
                string encoderTuning = FfmpegParams.encoder switch
                {
                    // Intel QuickSync — H.264
                    var e when e == Encoder.Intel =>
                        "-async_depth 1 -tune:v 0 -global_quality 22",

                    // Intel QuickSync — H.265 (no -look_ahead support)
                    var e when e == Encoder.IntelH265 =>
                        "-async_depth 1 -low_power 1 -global_quality 22",

                    // NVIDIA NVENC — H.264
                    var e when e == Encoder.Nvidia =>
                        "-rc:v vbr -cq 19 -profile:v high -bf 2 -no-scenecut 1 -spatial_aq 1 -aq-strength 15",

                    // NVIDIA NVENC — H.265
                    var e when e == Encoder.NvidiaH265 =>
                        "-rc:v vbr -cq 19 -profile:v main -bf 2 -no-scenecut 1 -spatial_aq 1 -aq-strength 15",

                    // Software x264 — full color, sharp text
                    var e when e == Encoder.Universal =>
                        "-tune zerolatency -preset fast -crf 18",

                    // Software x265 — 4:4:4, tuned for text clarity
                    var e when e == Encoder.UniversalH265 =>
                        "-preset fast -x265-params \"crf=18:tune=psnr:aq-mode=3:vbv-bufsize=10000:vbv-maxrate=10000\"",

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

        public void StartShare(bool debug = false)
        {
            string filename = Path.Combine(ffmpegPath, ffmpegName);

            ffmpeg = new Process();
            ffmpeg.StartInfo.FileName = filename;
            ffmpeg.StartInfo.Arguments = FfmpegParamsString;
            ffmpeg.StartInfo.CreateNoWindow = !debug;

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
