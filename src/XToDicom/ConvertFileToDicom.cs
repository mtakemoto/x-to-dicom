using ImageMagick;
using System;

namespace XToDicom
{
    class ConvertFileToDicom
    {
        //TODO: test paths for error handling
        public static void SingleFile(string input, string output)
        {
            var inPath = input;
            var outPath = output;

            Console.WriteLine($"Processing file {input}");
            var imageFactory = new ImageCollectionFactory(inPath);

            using (IImageCollection collection = imageFactory.Create())
            {
                var imageBuilder = new DicomImageBuilder(outPath, collection.Width, collection.Height)
                    .WithDefaultPatientData();

                for (int i = 0; i < collection.Data.Count; i++)
                {
                    Console.WriteLine($"Adding image {i + 1} of {collection.Data.Count}");
                    var byteData = collection.Data[i].GetPixels().ToByteArray(PixelMapping.RGB);
                    
                    imageBuilder.AddImage(byteData);
                }

                imageBuilder.Build();
            }

            Console.WriteLine($"File successfully written to {outPath}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
