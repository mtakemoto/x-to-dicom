namespace XToDicom.Lib
{
    public interface IImageCollectionFactory
    {
        string Extension { get; }
        string FilePath { get; }

        IImageCollection Create();
        string GetFileExtension(string fileName);
    }
}