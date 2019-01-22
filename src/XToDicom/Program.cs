using CommandLine;

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
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    ConvertFileToDicom.SingleFile(o.InputFilePath, o.OutputFilePath);
                });
        }
    }
}
