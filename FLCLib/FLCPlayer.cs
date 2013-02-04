using FLCLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FLCLib.Metro
{
    public delegate void FrameUpdated(Texture2D texture, FLCFile file);

    public class FLCPlayer : IDisposable
    {
        private GraphicsDevice device;

        public event FrameUpdated OnFrameUpdated;
        public event PlaybackStarted OnPlaybackStarted;
        public event PlaybackFinished OnPlaybackFinished;

        public bool IsPlaying 
        {
            get 
            {
                if (flcFile == null)
                    return false;

                return flcFile.IsPlaying; 
            }
        }
        public bool ShouldLoop
        {
            get 
            {
                if (flcFile == null)
                    return false;

                return flcFile.ShouldLoop; 
            }
            set 
            {
                if (flcFile == null)
                    return;

                flcFile.ShouldLoop = value; 
            }
        }

        private StorageFile file;
        private FLCFile flcFile;

        private Texture2D currentFrame;

        public FLCPlayer(GraphicsDevice setDevice)
        {
            device = setDevice;

            file = null;
            flcFile = null;

            currentFrame = null;
        }

        public async Task<bool> Open(StorageFile setFile)
        {
            file = setFile;
            var str = await file.OpenReadAsync();

            if (flcFile != null)
            {
                flcFile.OnFrameUpdated -= flcFile_OnFrameUpdated;
                flcFile.OnPlaybackStarted -= flcFile_OnPlaybackStarted;
                flcFile.OnPlaybackFinished -= flcFile_OnPlaybackFinished;

                flcFile.Dispose();
                flcFile = null;
            }

            flcFile = new FLCFile(str.AsStreamForRead());
            flcFile.OnFrameUpdated += flcFile_OnFrameUpdated;
            flcFile.OnPlaybackStarted += flcFile_OnPlaybackStarted;
            flcFile.OnPlaybackFinished += flcFile_OnPlaybackFinished;
            flcFile.Open();

            return true;
        }

        void flcFile_OnPlaybackFinished(FLCFile file)
        {
            if (OnPlaybackFinished != null)
                OnPlaybackFinished(file);
        }

        void flcFile_OnPlaybackStarted(FLCFile file)
        {
            if (OnPlaybackStarted != null)
                OnPlaybackStarted(file);
        }

        void flcFile_OnFrameUpdated(FLCFile file)
        {
            FLCColor[] colors = flcFile.GetFramebufferCopy();

            if (currentFrame == null)
                currentFrame = new Texture2D(device, flcFile.Width, flcFile.Height, false, SurfaceFormat.Color);

            Color[] colorData = new Color[currentFrame.Width * currentFrame.Height];

            for (int i = 0; i < colors.Length; i++)
            {
                colorData[i] = new Color(colors[i].R, colors[i].G, colors[i].B, colors[i].A);
            }

            lock (currentFrame)
            {
                currentFrame.SetData<Color>(colorData);

                if (OnFrameUpdated != null)
                    OnFrameUpdated(currentFrame, file);
            }
        }

        public void Play()
        {
            if (flcFile == null)
                throw new Exception("File has not been opened successfully. Did you call Open()?");

            flcFile.Play();
        }

        public void Stop()
        {
            if (flcFile == null)
                throw new Exception("File has not been opened successfully. Did you call Open()?");

            flcFile.Stop();
        }

        public void Dispose()
        {
            Stop();

            if (currentFrame != null)
            {
                currentFrame.Dispose();
                currentFrame = null;
            }

            if (flcFile != null)
            {
                flcFile.Dispose();
                flcFile = null;
            }
        }
    }
}
