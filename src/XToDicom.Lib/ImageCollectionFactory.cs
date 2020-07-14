using ImageMagick;
using Serilog;
using System;
using System.IO;
using System.Linq;
using Xabe.FFmpeg.Enums;

namespace XToDicom.Lib
{
    public class ImageCollectionFactory : IImageCollectionFactory
    {
        private readonly ILogger logger;
        private readonly IFrameExtractor frameExtractor;

        public ImageCollectionFactory(ILogger logger, IFrameExtractor frameExtractor)
        {
            this.logger = logger;
            this.frameExtractor = frameExtractor;
        }

        public IImageCollection Create(string filePath)
        {
            IImageCollection imageCollection = null;
            string extension = this.GetFileExtension(filePath);
            logger.Information("Detected file with extension {ext}", extension);

            switch (extension)
            {
                case ".gif":
                    imageCollection = this.CreateFromGif(filePath);
                    break;
                case ".mp4":
                case ".avi":
                    imageCollection = this.CreateFromVideo(filePath);
                    break;
            }

            return imageCollection;
        }

        //TODO: handle null case
        //TODO: ensure all frames are same size
        //TODO: consider making each of these their own factory if it gets too big
        private IImageCollection CreateFromGif(string filePath)
        {
            var data = new MagickImageCollection(filePath);
            var firstFrame = data.First();
            var retVal = new ImageCollection()
            {
                Data = data,
                Width = firstFrame.Width,
                Height = firstFrame.Height
            };

            //Required to un-compress the non-first image frames
            retVal.Data.Coalesce();

            return retVal;
        }

        //TODO: see if we can do this without needing to convert to GIF first
        private IImageCollection CreateFromVideo(string filePath)
        {
            var gifName = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + FileExtensions.Gif);
            var gif = frameExtractor.ToGif(filePath);

            gif.Wait();

            return this.CreateFromGif(gif.Result);
        }

        public string GetFileExtension(string fileName)
            => Path.GetExtension(fileName).ToLower();
    }
}
