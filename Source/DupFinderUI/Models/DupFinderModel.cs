using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Xsl;
using DupFinderUI.Services;

namespace DupFinderUI.Models
{
    public class DupFinderModel
    {
        private readonly FileSystemService _fileSystemService;
        private readonly ProcessService _processService;
        public event EventHandler<string> DataReceived;

        public DupFinderModel(FileSystemService fileSystemService, ProcessService processService)
        {
            _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
            _processService = processService ?? throw new ArgumentNullException(nameof(processService));
        }

        public string DupFinderPath { get; set; }
        public string SourceFolder { get; set; }
        public string OutputFile { get; set; }
        public string TransformFile { get; set; }

        public void Run()
        {
            RunDupFinder();
            var htmlOutput = TransformOutput();
            _processService.StartProcess(htmlOutput);
        }

        private void RunDupFinder()
        {
            if (_fileSystemService.DirectoryExists(DupFinderPath))
            {
                var dupFinder = _fileSystemService.CombinePaths(DupFinderPath, "dupfinder.exe");
                if (_fileSystemService.FileExists(dupFinder))
                {
                    var proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            CreateNoWindow = true,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            RedirectStandardInput = true,
                            FileName = dupFinder,
                            Arguments = $"--show-text -o={OutputFile} {SourceFolder}",
                        }
                    };
                    proc.ErrorDataReceived += (sender, args) => OnDataReceived($"[ERROR]: {args?.Data}");
                    proc.OutputDataReceived += (sender, args) => OnDataReceived($"[INFO] : {args?.Data}");
                    proc.Start();

                    proc.BeginErrorReadLine();
                    proc.BeginOutputReadLine();

                    proc.WaitForExit();
                }
                else
                {
                    throw new FileNotFoundException("The program dupfinder.exe was not found.", dupFinder);
                }
            }
            else
            {
                throw new DirectoryNotFoundException($"The DupFinder path '{DupFinderPath}' does not exist.");
            }
        }

        private string TransformOutput()
        {
            using (var xsltReader = _fileSystemService.CreateXmlReader(TransformFile))
            {
                var transformer = new XslCompiledTransform();
                transformer.Load(xsltReader);

                using (var documentReader = _fileSystemService.CreateXmlReader(OutputFile))
                {
                    var htmlOutput = _fileSystemService.ChangeExtension(OutputFile, ".html");
                    using (var writer = _fileSystemService.CreateXmlWriter(htmlOutput))
                    {
                        transformer.Transform(documentReader, writer);
                        writer.Flush();
                    }

                    return htmlOutput;
                }
            }
        }

        protected virtual void OnDataReceived(string e) => DataReceived?.BeginInvoke(this, e, null, null);
    }
}


