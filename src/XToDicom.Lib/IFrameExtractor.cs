using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace XToDicom.Lib
{
    public interface IFrameExtractor
    {
        IMediaInfo FileInfo { get; }
        string FileName { get; }

        Task<string> GetFrame(int frameNumber);
        Task<IMediaInfo> GetInfo(string fileName);
        Task<string> ToGif();
    }
}