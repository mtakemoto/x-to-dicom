using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace XToDicom.Lib
{
    public interface IFrameExtractor
    {
        Task<string> GetFrame(string fileName, int frameNumber);
        Task<IMediaInfo> GetInfo(string fileName);
        Task<string> ToGif(string fileName);
    }
}