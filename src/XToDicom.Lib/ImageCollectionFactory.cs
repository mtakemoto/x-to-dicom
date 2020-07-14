using ImageMagick;
using System;
using System.IO;
using System.Linq;
using Xabe.FFmpeg.Enums;

namespace XToDicom.Lib
{
    class ImageCollectionFactory
    {
        public string FilePath { get; }
        public string Extension { get; }

        public ImageCollectionFactory(string filePath)
        {
            this.FilePath = filePath;
            this.Extension = this.GetFileExtension(filePath);
        }

        public IImageCollection Create()
        {
            IImageCollection imageCollection = null;
            switch (this.Extension)
            {
                case ".gif":
                    imageCollection = this.CreateGif(this.FilePath);
                    break;
                case ".mp4":
                case ".avi":
                    imageCollection = this.CreateFromVideo(this.FilePath);
                    break;
                
            }

            return imageCollection;
        }

        //TODO: handle null case
        //TODO: ensure all frames are same size
        //TODO: consider making each of these their own factory if it gets too big
        private IImageCollection CreateGif(string path)
        {
            var data = new MagickImageCollection(path);
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

        //TODO: this is messy use DI to get a frameextractor
        //TODO: see if we can do this without needing to convert to GIF first
        private IImageCollection CreateFromVideo(string path)
        {
            var frameExtractor = new FrameExtractor(path);
            var gifName = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + FileExtensions.Gif);
            var gif = frameExtractor.ToGif();

            gif.Wait();

            return this.CreateGif(gif.Result);
        }

        public string GetFileExtension(string fileName)
            => Path.GetExtension(fileName).ToLower();
    }
}
