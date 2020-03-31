// ****************************************************************************
// * MIT License
// *
// * Copyright (c) 2020 Thomas Due
// *
// * Permission is hereby granted, free of charge, to any person obtaining a copy
// * of this software and associated documentation files (the "Software"), to deal
// * in the Software without restriction, including without limitation the rights
// * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// * copies of the Software, and to permit persons to whom the Software is
// * furnished to do so, subject to the following conditions:
// *
// * The above copyright notice and this permission notice shall be included in all
// * copies or substantial portions of the Software.
// *
// * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// * SOFTWARE.
// ****************************************************************************

using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Xsl;
using DupFinderUI.Interfaces;

namespace DupFinderUI.Models
{
    public class DupFinderModel : IDupFinderModel
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly IProcessService _processService;

        public DupFinderModel(IFileSystemService fileSystemService, IProcessService processService)
        {
            _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
            _processService    = processService ?? throw new ArgumentNullException(nameof(processService));
        }

        public event EventHandler<string> DataReceived;

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
                                                   CreateNoWindow         = true,
                                                   RedirectStandardOutput = true,
                                                   RedirectStandardError  = true,
                                                   UseShellExecute        = false,
                                                   RedirectStandardInput  = true,
                                                   FileName               = dupFinder,
                                                   Arguments              = $"--show-text -o={OutputFile} {SourceFolder}"
                                               }
                               };
                    proc.ErrorDataReceived  += (sender, args) => OnDataReceived($"[ERROR]: {args?.Data}");
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
