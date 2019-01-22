using ImageMagick;
using System;

namespace XToDicom
{
    public interface IImageCollection : IDisposable
    {
        IMagickImageCollection Data { get; set; }
        int Height { get; set; }
        int Width { get; set; }
    }
}