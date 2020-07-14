using Dicom;
using Dicom.Imaging;
using Dicom.IO.Buffer;
using System;
using System.Text;

namespace XToDicom.Lib
{
    public class DicomImageBuilder
    {
        private DicomImage Image { get; set; }
        private DicomDataset DataSet { get; set; }
        private DicomPixelData PixelData { get; set; }
        public string OutputPath { get; }

        public DicomImageBuilder(string outputPath, int width, int height)
        {
            this.OutputPath = outputPath;
            this.DataSet = this.ConfigureDataSet(width, height);
            this.PixelData = this.CreatePixelData(width, height);
        }

        // From file path
        public DicomImageBuilder AddImage(byte[] data)
        {
            var byteBuffer = new MemoryByteBuffer(data);
            this.PixelData.AddFrame(byteBuffer);

            return this;
        }

        // TODO: copypasta'd from GitHub to work initially - send help
        // Source: https://gist.github.com/mdubey82/4030263
        public DicomImageBuilder WithDefaultPatientData()
        {
            //type 1 attributes.
            this.DataSet.Add(DicomTag.SOPClassUID, DicomUID.MultiFrameTrueColorSecondaryCaptureImageStorage);
            this.DataSet.Add(DicomTag.StudyInstanceUID, this.GenerateUid());
            this.DataSet.Add(DicomTag.SeriesInstanceUID, this.GenerateUid());
            this.DataSet.Add(DicomTag.SOPInstanceUID, this.GenerateUid());

            //type 2 attributes
            this.DataSet.Add(DicomTag.PatientID, "12345");
            this.DataSet.Add(DicomTag.PatientName, string.Empty);
            this.DataSet.Add(DicomTag.PatientBirthDate, "00000000");
            this.DataSet.Add(DicomTag.PatientSex, "M");
            this.DataSet.Add(DicomTag.StudyDate, DateTime.Now);
            this.DataSet.Add(DicomTag.StudyTime, DateTime.Now);
            this.DataSet.Add(DicomTag.AccessionNumber, string.Empty);
            this.DataSet.Add(DicomTag.ReferringPhysicianName, string.Empty);
            this.DataSet.Add(DicomTag.StudyID, "1");
            this.DataSet.Add(DicomTag.SeriesNumber, "1");
            this.DataSet.Add(DicomTag.ModalitiesInStudy, "CR");
            this.DataSet.Add(DicomTag.Modality, "CR");
            this.DataSet.Add(DicomTag.NumberOfStudyRelatedInstances, "1");
            this.DataSet.Add(DicomTag.NumberOfStudyRelatedSeries, "1");
            this.DataSet.Add(DicomTag.NumberOfSeriesRelatedInstances, "1");
            this.DataSet.Add(DicomTag.PatientOrientation, "F/A");
            this.DataSet.Add(DicomTag.ImageLaterality, "U");

            return this;
        }

        //TODO: Hit up MedicalConnections for that free UID namespace?
        //Will fix issue of leading zero NEMA compliance: https://gist.github.com/mdubey82/4030263#gistcomment-2752378
        private DicomUID GenerateUid()
        {
            StringBuilder uid = new StringBuilder();
            uid.Append("1.08.1982.10121984.2.0.07").Append('.').Append(DateTime.UtcNow.Ticks);
            return new DicomUID(uid.ToString(), "SOP Instance UID", DicomUidType.SOPInstance);
        }

        private DicomPixelData CreatePixelData(int width, int height)
        {
            var pd = DicomPixelData.Create(this.DataSet, true);
            pd.BitsStored = 8;
            pd.SamplesPerPixel = 3;
            pd.HighBit = 7;
            pd.PixelRepresentation = 0;
            pd.PlanarConfiguration = 0;
            pd.Height = (ushort)height;
            pd.Width = (ushort)width;
            pd.PhotometricInterpretation = PhotometricInterpretation.Rgb;

            return pd;
        }

        private DicomDataset ConfigureDataSet(int width, int height)
        {
            var ds = new DicomDataset(DicomTransferSyntax.ExplicitVRLittleEndian);
            ds.AddOrUpdate(DicomTag.PhotometricInterpretation, PhotometricInterpretation.Rgb.Value);
            ds.AddOrUpdate(DicomTag.Rows, (ushort)width);
            ds.AddOrUpdate(DicomTag.Columns, (ushort)height);
            ds.AddOrUpdate(DicomTag.BitsAllocated, (ushort)8);

            return ds;
        }


        //TODO: use async
        public void Build()
        {
            var image = new DicomFile(this.DataSet);
            image.Save(this.OutputPath);
        }
    }
}
