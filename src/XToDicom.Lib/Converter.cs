using ImageMagick;
using Serilog;
using System;
using System.IO;

namespace XToDicom.Lib
{
    public class Converter : IConverter
    {
        private readonly ILogger logger;

        public Converter(ILogger logger)
        {
            this.logger = logger;
        }

        //TODO: test paths for error handling
        public void SingleFile(string input, string output)
        {
            if(!File.Exists(input))
            {
                logger.Error("File {inputFile} doesn't exist.  Exiting...", input);
            }
            else
            {
                InnerSingleFile(input, output);
            }
        }

        private void InnerSingleFile(string input, string output)
        {
            var inPath = input;
            var outPath = output;

            logger.Information($"Processing file {input}");
            var imageFactory = new ImageCollectionFactory(inPath);

            using (IImageCollection collection = imageFactory.Create())
            {
                var imageBuilder = new FoDicomImageBuilder(outPath, collection.Width, collection.Height)
                    .WithDefaultPatientData();

                for (int i = 0; i < collection.Data.Count; i++)
                {
                    logger.Information($"Adding image {i + 1} of {collection.Data.Count}");
                    var byteData = collection.Data[i].GetPixels().ToByteArray(PixelMapping.RGB);

                    imageBuilder.AddImage(byteData);
                }

                imageBuilder.Build();
            }

            logger.Information($"File successfully written to {outPath}");
            logger.Information("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
