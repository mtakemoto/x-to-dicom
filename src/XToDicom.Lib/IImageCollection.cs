using ImageMagick;
using System;

namespace XToDicom.Lib
{
    public interface IImageCollection : IDisposable
    {
        IMagickImageCollection Data { get; set; }
        int Height { get; set; }
        int Width { get; set; }
    }
}