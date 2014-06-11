using FLCLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

            Bitmap bm = new Bitmap(file.Width, file.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            BitmapData bd = bm.LockBits(new Rectangle(0, 0, file.Width, file.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            byte* ptr = (byte*)bd.Scan0.ToPointer();
            for (int y = 0; y < file.Height; y++)
            {
                for (int x = 0; x < file.Width; x++)
                {
                    FLCColor col = colors[x + (y * file.Width)];

                    *ptr++ = col.B;
                    *ptr++ = col.G;
                    *ptr++ = col.R;
                    *ptr++ = col.A;
                }
            }
            bm.UnlockBits(bd);

            bm.Save("Images/image" + counter++ + ".jpg", ImageFormat.Jpeg);
        }
    }
}
