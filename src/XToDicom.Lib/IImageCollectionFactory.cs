namespace XToDicom.Lib
{
    public interface IImageCollectionFactory
    {
        IImageCollection Create(string filePath);
        string GetFileExtension(string fileName);
    }
}