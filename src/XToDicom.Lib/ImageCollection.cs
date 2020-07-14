using ImageMagick;
using System;

namespace XToDicom
{
    public class ImageCollection : IImageCollection
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public IMagickImageCollection Data { get; set; }

        void IDisposable.Dispose()
        {
            Data.Dispose();
        }
    }
}
