using CommandLine;

namespace XToDicom
{
    class CommandLineOptions
    {
        [Option('i', "in", Required = true, HelpText = "Input file path")]
        public string InputFilePath { get; set; }

        [Option('o', "out", Required = true, HelpText = "Output file path")]
        public string OutputFilePath { get; set; }
    }
}
