using CommandLine;
using Serilog;
using Unity;
using XToDicom.Lib;

namespace XToDicom
{
    //TODO: Need a catchier name
    //TODO: Actual error handling
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            var container = new UnityContainer();
            container.RegisterInstance<ILogger>(Log.Logger);
            container.RegisterType<IDicomImageBuilder, FoDicomImageBuilder>();
            container.RegisterType<IImageCollectionFactory, ImageCollectionFactory>();
            container.RegisterType<IFrameExtractor, FrameExtractor>();
            container.RegisterType<IConverter, Converter>();

            var converter = container.Resolve<IConverter>();

            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(o =>
                {
                    converter.SingleFile(o.InputFilePath, o.OutputFilePath);
                });
        }
    }
}
