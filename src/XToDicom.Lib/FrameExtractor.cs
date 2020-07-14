using System;
using System.Threading.Tasks;
using Xabe.FFmpeg;
using System.Linq;
using System.IO;
using Xabe.FFmpeg.Enums;
using Xabe.FFmpeg.Model;

namespace XToDicom.Lib
{
    class FrameExtractor
    {
        public string FileName { get; }
        public IMediaInfo FileInfo { get; }

        //Calculate frametime from ticks (10,000 per ms * 1000 ms/s = 10,000,000 ticks/s)
        private const int TicksPerSecond = 10000000;

        public FrameExtractor(string fileName)
        {
            this.FileName = fileName;
        }

        public async Task<IMediaInfo> GetInfo(string fileName)
            => await MediaInfo.Get(fileName);

        public async Task<string> GetFrame(int frameNumber)
        {
            var info = await this.GetInfo(FileName);
            double fps = info.VideoStreams.FirstOrDefault().FrameRate;
            string outputPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + FileExtensions.Png);

            TimeSpan frameTime = new TimeSpan(Convert.ToInt64(TicksPerSecond / fps) * frameNumber);

            IConversionResult result = await Conversion.Snapshot(this.FileName, outputPath, frameTime).Start();

            return outputPath;
        }

        public async Task<string> ToGif() {
            string outputPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + FileExtensions.Gif);
            await Conversion.ToGif(this.FileName, outputPath, 1).Start();

            return outputPath;
        } 
    }
}
