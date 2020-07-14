using ImageMagick;
using Serilog;
using System;
using System.IO;

namespace XToDicom.Lib
{
    public class Converter : IConverter
    {
        private readonly ILogger logger;
        private readonly IImageCollectionFactory imageFactory;

        public Converter(ILogger logger, IImageCollectionFactory imageFactory)
        {
            this.logger = logger;
            this.imageFactory = imageFactory;
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
            using (IImageCollection collection = imageFactory.Create(inPath))
            {
                var imageBuilder = new FoDicomImageBuilder()
                    .FromImageCollection(collection)
                    .WithDefaultPatientData();

                for (int i = 0; i < collection.Data.Count; i++)
                {
                    logger.Information($"Adding frame {i + 1} of {collection.Data.Count}");
                    var byteData = collection.Data[i].GetPixels().ToByteArray(PixelMapping.RGB);

                    imageBuilder.AddImage(byteData);
                }

                imageBuilder.Build(outPath);
            }

            logger.Information($"File successfully written to {outPath}");
            logger.Information("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
