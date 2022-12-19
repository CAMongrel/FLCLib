using FLCLib;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;
using System.Threading;

namespace FLCTestPlayer
{
    class Program
    {
        static FLCFile file;
        static int counter;

        static void Main(string[] args)
        {
            counter = 0;

            file = new FLCFile(File.OpenRead("Test.flc"));
            file.OnFrameUpdated += file_OnFrameUpdated;
            file.OnPlaybackFinished += file_OnPlaybackFinished;
            file.OnPlaybackStarted += file_OnPlaybackStarted;
            file.ShouldLoop = false;

            file.Open();
            file.Play();

            while (file.IsPlaying)
            {
                Thread.Sleep(1);
            }
        }

        static void file_OnPlaybackStarted(FLCFile file)
        {
            Console.WriteLine("Playback started");
        }

        static void file_OnPlaybackFinished(FLCFile file, bool didFinishNormally)
        {
            Console.WriteLine("Playback finished; " + didFinishNormally);
        }

        static unsafe void file_OnFrameUpdated(FLCFile file)
        {
            FLCColor[] colors = file.GetFramebufferCopy();

            Image<Rgba32> image = new Image<Rgba32>(file.Width, file.Height);
            image.ProcessPixelRows((accessor) =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    var span = accessor.GetRowSpan(y);
                    for (int x = 0; x < accessor.Width; x++)
                    {
                        FLCColor col = colors[x + (y * file.Width)];

                        span[x].R = col.R;
                        span[x].G = col.G;
                        span[x].B = col.B;
                        span[x].A = col.A;
                    }
                }
                
            });

            image.SaveAsJpeg("Images/image" + counter++ + ".jpg");
        }
    }
}
