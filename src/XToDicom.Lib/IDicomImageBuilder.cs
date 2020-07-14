using Dicom;

namespace XToDicom.Lib
{
    public interface IDicomImageBuilder
    {
        FoDicomImageBuilder AddImage(byte[] data);
        DicomFile Build();
        FoDicomImageBuilder FromImageCollection(IImageCollection imageCollection);
        FoDicomImageBuilder WithDefaultPatientData();
    }
}