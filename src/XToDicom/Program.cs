using CommandLine;
using XToDicom.Lib;

namespace XToDicom
{
    //TODO: Need a catchier name
    //TODO: DI
    //TODO: Actual logging
    //TODO: Actual error handling
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(o =>
                {
                    ConvertFileToDicom.SingleFile(o.InputFilePath, o.OutputFilePath);
                });
        }
    }
}
