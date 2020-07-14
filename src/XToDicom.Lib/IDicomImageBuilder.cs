namespace XToDicom.Lib
{
    public interface IDicomImageBuilder
    {
        string OutputPath { get; }

        FoDicomImageBuilder AddImage(byte[] data);
        void Build();
        FoDicomImageBuilder WithDefaultPatientData();
    }
}