namespace XToDicom.Lib
{
    public interface IDicomImageBuilder
    {
        FoDicomImageBuilder AddImage(byte[] data);
        void Build(string outputPath);
        FoDicomImageBuilder WithDefaultPatientData();
    }
}