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
using DupFinderUI.Models;

namespace DupFinderUI.Services
{
    /// <summary>
    /// </summary>
    /// <seealso cref="DupFinderUI.Interfaces.IDupFinderService" />
    public class DupFinderService : IDupFinderService
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly IProcessService _processService;
        private SettingsData _settingsData;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DupFinderService" /> class.
        /// </summary>
        /// <param name="fileSystemService">The file system service.</param>
        /// <param name="processService">The process service.</param>
        /// <exception cref="ArgumentNullException">
        ///     fileSystemService
        ///     or
        ///     processService
        /// </exception>
        public DupFinderService(IFileSystemService fileSystemService, IProcessService processService)
        {
            _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
            _processService    = processService ?? throw new ArgumentNullException(nameof(processService));
        }

        /// <summary>
        ///     Occurs when [data received].
        /// </summary>
        public event EventHandler<string> DataReceived;

        /// <summary>
        ///     Runs the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void Run(SettingsData data)
        {
            _settingsData = new SettingsData(data);
            RunDupFinder();
            var htmlOutput = TransformOutput();
            _processService.StartProcess(htmlOutput);
        }

        /// <summary>
        ///     Runs the dup finder.
        /// </summary>
        /// <exception cref="FileNotFoundException">The program dupfinder.exe was not found.</exception>
        /// <exception cref="DirectoryNotFoundException">The DupFinder path '{_settingsData.DupFinderPath}' does not exist.</exception>
        private void RunDupFinder()
        {
            if (_fileSystemService.DirectoryExists(_settingsData.DupFinderPath))
            {
                var dupFinder = _fileSystemService.CombinePaths(_settingsData.DupFinderPath, "dupfinder.exe");
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
                                                   Arguments              = $"--show-text -o={_settingsData.OutputFile} {_settingsData.SourceFolder}"
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
                throw new DirectoryNotFoundException($"The DupFinder path '{_settingsData.DupFinderPath}' does not exist.");
            }
        }

        /// <summary>
        ///     Transforms the output.
        /// </summary>
        /// <returns></returns>
        private string TransformOutput()
        {
            using (var xsltReader = _fileSystemService.CreateXmlReader(_settingsData.TransformFile))
            {
                var transformer = new XslCompiledTransform();
                transformer.Load(xsltReader);

                using (var documentReader = _fileSystemService.CreateXmlReader(_settingsData.OutputFile))
                {
                    var htmlOutput = _fileSystemService.ChangeExtension(_settingsData.OutputFile, ".html");
                    using (var writer = _fileSystemService.CreateXmlWriter(htmlOutput))
                    {
                        transformer.Transform(documentReader, writer);
                        writer.Flush();
                    }

                    return htmlOutput;
                }
            }
        }

        /// <summary>
        ///     Called when [data received].
        /// </summary>
        /// <param name="e">The e.</param>
        protected virtual void OnDataReceived(string e) => DataReceived?.BeginInvoke(this, e, null, null);
    }
}
