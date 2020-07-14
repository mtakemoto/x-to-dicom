using Dicom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using XToDicom.Lib;

namespace Tests
{
    [TestClass]
    public class DicomImageBuilderTests
    {
        [TestMethod]
        public void DicomImageBuilder_StudyInstanceUID_IsValid()
        {
            //Arrange
            IDicomImageBuilder builder = new FoDicomImageBuilder();
            IImageCollection imageCollection = new ImageCollection();
            DicomFile image = builder.FromImageCollection(imageCollection)
                .WithDefaultPatientData()
                .Build();

            //Act
            string uid = image.Dataset.GetValues<string>(DicomTag.StudyInstanceUID).FirstOrDefault();

            //Assert
            Assert.IsTrue(DicomUID.IsValid(uid));
        }
    }
}
