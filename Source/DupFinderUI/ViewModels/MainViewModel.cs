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
using System.Windows.Input;
using DevExpress.Mvvm;
using DupFinderUI.Interfaces;
using DupFinderUI.Models;

namespace DupFinderUI.ViewModels
{
    /// <summary>
    /// </summary>
    /// <seealso cref="DevExpress.Mvvm.ViewModelBase" />
    public class MainViewModel : ViewModelBase
    {
        private readonly IDupFinderService _dupFinderModel;
        private readonly ISettingsService _settingsModel;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        /// <param name="settingsModel">The settings model.</param>
        /// <param name="dupFinderModel">The dup finder model.</param>
        /// <exception cref="ArgumentNullException">
        ///     settingsModel
        ///     or
        ///     dupFinderModel
        /// </exception>
        public MainViewModel(ISettingsService settingsModel, IDupFinderService dupFinderModel)
        {
            _settingsModel               =  settingsModel ?? throw new ArgumentNullException(nameof(settingsModel));
            _dupFinderModel              =  dupFinderModel ?? throw new ArgumentNullException(nameof(dupFinderModel));
            _dupFinderModel.DataReceived += (sender, s) => DupFinderOutput += s + Environment.NewLine;
        }

        /// <summary>
        ///     Gets or sets the dup finder path.
        /// </summary>
        /// <value>
        ///     The dup finder path.
        /// </value>
        public string DupFinderPath
        {
            get => GetProperty(() => DupFinderPath);
            set => SetProperty(() => DupFinderPath, value);
        }

        /// <summary>
        ///     Gets or sets the transform file.
        /// </summary>
        /// <value>
        ///     The transform file.
        /// </value>
        public string TransformFile
        {
            get => GetProperty(() => TransformFile);
            set => SetProperty(() => TransformFile, value);
        }

        /// <summary>
        ///     Gets or sets the source folder.
        /// </summary>
        /// <value>
        ///     The source folder.
        /// </value>
        public string SourceFolder
        {
            get => GetProperty(() => SourceFolder);
            set => SetProperty(() => SourceFolder, value);
        }

        /// <summary>
        ///     Gets or sets the output file.
        /// </summary>
        /// <value>
        ///     The output file.
        /// </value>
        public string OutputFile
        {
            get => GetProperty(() => OutputFile);
            set => SetProperty(() => OutputFile, value);
        }

        /// <summary>
        ///     Gets or sets the dup finder output.
        /// </summary>
        /// <value>
        ///     The dup finder output.
        /// </value>
        public string DupFinderOutput
        {
            get => GetProperty(() => DupFinderOutput);
            set => SetProperty(() => DupFinderOutput, value);
        }

        /// <summary>
        ///     Gets the folder browser dialog service.
        /// </summary>
        /// <value>
        ///     The folder browser dialog service.
        /// </value>
        private IFolderBrowserDialogService FolderBrowserDialogService => GetService<IFolderBrowserDialogService>();

        /// <summary>
        ///     Gets the open file dialog.
        /// </summary>
        /// <value>
        ///     The open file dialog.
        /// </value>
        private IOpenFileDialogService OpenFileDialog => GetService<IOpenFileDialogService>();

        /// <summary>
        ///     Gets the initialization.
        /// </summary>
        /// <value>
        ///     The initialization.
        /// </value>
        public ICommand Initialization => new DelegateCommand(ExecuteInitialization);

        /// <summary>
        ///     Gets the open folder.
        /// </summary>
        /// <value>
        ///     The open folder.
        /// </value>
        public ICommand OpenFolder => new DelegateCommand<int>(ExecuteOpenFolderDialog);

        /// <summary>
        ///     Gets the open file.
        /// </summary>
        /// <value>
        ///     The open file.
        /// </value>
        public ICommand OpenFile => new DelegateCommand<int>(ExecuteOpenFileDialog);

        /// <summary>
        ///     Gets the transform.
        /// </summary>
        /// <value>
        ///     The transform.
        /// </value>
        public ICommand Transform => new DelegateCommand(ExecuteTransform);

        /// <summary>
        ///     Executes the initialization.
        /// </summary>
        private void ExecuteInitialization()
        {
            var data = _settingsModel.LoadSettings();

            DupFinderPath = data.DupFinderPath;
            TransformFile = data.TransformFile;
            SourceFolder  = data.SourceFolder;
            OutputFile    = data.OutputFile;
        }

        /// <summary>
        ///     Executes the open file dialog.
        /// </summary>
        /// <param name="input">The input.</param>
        private void ExecuteOpenFileDialog(int input)
        {
            if (OpenFileDialog != null && OpenFileDialog.ShowDialog())
            {
                switch (input)
                {
                    case 1:
                        TransformFile = OpenFileDialog.File.GetFullName();
                        break;

                    case 2:
                        OutputFile = OpenFileDialog.File.GetFullName();
                        break;
                }
            }
        }

        /// <summary>
        ///     Executes the open folder dialog.
        /// </summary>
        /// <param name="input">The input.</param>
        private void ExecuteOpenFolderDialog(int input)
        {
            if (FolderBrowserDialogService != null && FolderBrowserDialogService.ShowDialog())
            {
                switch (input)
                {
                    case 1:
                        DupFinderPath = FolderBrowserDialogService.ResultPath;
                        break;

                    case 2:
                        SourceFolder = FolderBrowserDialogService.ResultPath;
                        break;
                }
            }
        }

        /// <summary>
        ///     Executes the transform.
        /// </summary>
        private void ExecuteTransform()
        {
            var data = new SettingsData
                       {
                           DupFinderPath = DupFinderPath.Trim(),
                           TransformFile = TransformFile,
                           SourceFolder  = SourceFolder.Trim(),
                           OutputFile    = OutputFile
                       };
            _settingsModel.SaveSettings(data);
            _dupFinderModel.Run(data);
        }
    }
}
