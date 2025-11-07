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

                // Choose pixel format based on encoder
                string pixelFormat = FfmpegParams.encoder switch
                {
                    var e when e == Encoder.Intel => "nv12",       // native for Intel QSV
                    var e when e == Encoder.Nvidia => "yuv420p",    // NVENC / general hw-friendly
                    _ => "yuv444p"     // full color for crisp desktop text
                };

                // Encoder-specific tuning (only valid options per encoder)
                string encoderTuning = FfmpegParams.encoder switch
                {
                    // Intel QSV (h264_qsv): use QSV-friendly options only
                    var e when e == Encoder.Intel => "-look_ahead 0 -async_depth 1 -tune zerolatency",

                    // NVIDIA NVENC (if you actually set Encoder.Nvidia to "h264_nvenc" elsewhere):
                    // Use NVENC-specific rc/cq options (these are ignored by libx264)
                    var e when e == Encoder.Nvidia => "-rc:v vbr -cq 18 -profile:v high",

                    // Default (libx264 / software): bias toward preserving small details (psnr),
                    // which helps text readability. libx264 supports -tune psnr.
                    _ => "-tune psnr"
                };

                // Build FFmpeg argument string (readable)
                string ffmpegArgs = string.Join(" ",
                    // Input (screen capture)
                    "-f gdigrab",
                    "-framerate 30",

                    // Increase capture-side queue / buffer to avoid brief stalls
                    "-thread_queue_size 1024",
                    "-rtbufsize 200M",
                    "-probesize 32M",
                    "-analyzeduration 0",

                    "-i desktop",

                    // Video encoding
                    $"-c:v {(string)FfmpegParams.encoder}",
                    $"-pix_fmt {pixelFormat}",
                    "-preset fast",                     // quality/speed balance
                    "-crf 18",                          // crisp text
                    "-b:v 10M",
                    "-maxrate 10M",
                    "-bufsize 10M",                     // smooth stream
                    "-g 60",                            // keyframe spacing
                    "-r 30",
                    "-fps_mode cfr",                    // replaces deprecated -vsync
                    "-flags low_delay",
                    "-fflags nobuffer",
                    "-use_wallclock_as_timestamps 1",
                    "-threads 4",

                    //tmp_fix
                    "-thread_queue_size 2048",
                    "-rtbufsize 500M",


                    // Add encoder-specific options (only non-empty)
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
