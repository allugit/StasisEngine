using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisEditor
{
    class GraphicsDeviceService : IGraphicsDeviceService
    {
        private static readonly GraphicsDeviceService singletonInstance = new GraphicsDeviceService();
        static int referenceCount;

        GraphicsDevice graphicsDevice;
        PresentationParameters parameters;

        public GraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
        }

        GraphicsDeviceService()
        {
        }

        public static GraphicsDeviceService AddRef(IntPtr windowHandle, int width, int height)
        {
            if (Interlocked.Increment(ref referenceCount) == 1)
            {
                singletonInstance.CreateDevice(windowHandle, width, height);
            }

            return singletonInstance;
        }

        public void Release(bool disposing)
        {
            if (Interlocked.Decrement(ref referenceCount) == 0)
            {
                if (disposing)
                {
                    if (DeviceDisposing != null)
                        DeviceDisposing(this, EventArgs.Empty);

                    graphicsDevice.Dispose();
                }

                graphicsDevice = null;
            }
        }

        public void CreateDevice(IntPtr windowHandle, int width, int height)
        {
            PresentationParameters pp = new PresentationParameters();
            pp.BackBufferWidth = Math.Max(width, 1);
            pp.BackBufferWidth = Math.Max(height, 1);
            pp.DepthStencilFormat = DepthFormat.Depth24;
            pp.DeviceWindowHandle = windowHandle;
            pp.IsFullScreen = false;

            graphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.HiDef, pp);

            if (DeviceCreated != null)
                DeviceCreated(this, EventArgs.Empty);
        }

        public void ResetDevice(int width, int height)
        {
        }

        // IGraphicsDeviceService events.
        public event EventHandler<EventArgs> DeviceCreated;
        public event EventHandler<EventArgs> DeviceDisposing;
        public event EventHandler<EventArgs> DeviceReset;
        public event EventHandler<EventArgs> DeviceResetting;
    }
}
