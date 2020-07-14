using System;
using System.Threading.Tasks;
using Xabe.FFmpeg;
using System.Linq;
using System.IO;
using Xabe.FFmpeg.Enums;
using Xabe.FFmpeg.Model;

namespace XToDicom.Lib
{
    public class FrameExtractor : IFrameExtractor
    {
        //Calculate frametime from ticks (10,000 per ms * 1000 ms/s = 10,000,000 ticks/s)
        private const int TicksPerSecond = 10000000;

        public async Task<IMediaInfo> GetInfo(string fileName)
            => await MediaInfo.Get(fileName);

        //TODO: proof-of-concept for getting frames from .mp4 directly
        public async Task<string> GetFrame(string fileName, int frameNumber)
        {
            var info = await this.GetInfo(fileName);
            double fps = info.VideoStreams.FirstOrDefault().FrameRate;
            string outputPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + FileExtensions.Png);

            TimeSpan frameTime = new TimeSpan(Convert.ToInt64(TicksPerSecond / fps) * frameNumber);

            IConversionResult result = await Conversion.Snapshot(fileName, outputPath, frameTime).Start();

            return outputPath;
        }

        public async Task<string> ToGif(string fileName)
        {
            string outputPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + FileExtensions.Gif);
            await Conversion.ToGif(fileName, outputPath, 1).Start();

            return outputPath;
        }
    }
}
