using Dicom;
using System;
using System.Collections.Generic;
using System.Text;

namespace XToDicom.Lib
{
    /// <summary>
    /// Abstracts basic Fo-dicom library operations
    /// Decouples library from implementation
    /// </summary>
    public class DicomFileAdapter
    {
        private readonly DicomFile dicomFile;

        public DicomFileAdapter(DicomFile dicomFile)
        {
            this.dicomFile = dicomFile;
        }

        public void Save(string fileName) => this.dicomFile.Save(fileName);
    }
}
