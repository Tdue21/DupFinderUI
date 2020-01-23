using System;
using System.Windows.Input;
using DevExpress.Mvvm;
using DupFinderUI.Models;

namespace DupFinderUI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly SettingsModel _settingsModel;
        private readonly DupFinderModel _dupFinderModel;

        public MainViewModel(SettingsModel settingsModel, DupFinderModel dupFinderModel)
        {
            _settingsModel = settingsModel ?? throw new ArgumentNullException(nameof(settingsModel));
            _dupFinderModel = dupFinderModel ?? throw new ArgumentNullException(nameof(dupFinderModel));
            _dupFinderModel.DataReceived += (sender, s) => DupFinderOutput += s + Environment.NewLine;
        }

        public string DupFinderPath
        {
            get => GetProperty(() => DupFinderPath);
            set => SetProperty(() => DupFinderPath, value);
        }

        public string TransformFile
        {
            get => GetProperty(() => TransformFile);
            set => SetProperty(() => TransformFile, value);
        }

        public string SourceFolder
        {
            get => GetProperty(() => SourceFolder);
            set => SetProperty(() => SourceFolder, value);
        }

        public string OutputFile
        {
            get => GetProperty(() => OutputFile);
            set => SetProperty(() => OutputFile, value);
        }

        public string DupFinderOutput
        {
            get => GetProperty(() => DupFinderOutput);
            set => SetProperty(() => DupFinderOutput, value);
        }
         
        public IFolderBrowserDialogService FolderBrowserDialogService => GetService<IFolderBrowserDialogService>();

        public IOpenFileDialogService OpenFileDialog => GetService<IOpenFileDialogService>();

        public ICommand Initialization => new DelegateCommand(ExecuteInitialization);

        public ICommand OpenFolder => new DelegateCommand<int>(ExecuteOpenFolderDialog);

        public ICommand OpenFile => new DelegateCommand<int>(ExecuteOpenFileDialog);

        public ICommand Transform => new DelegateCommand(ExecuteTransform);


        private void ExecuteInitialization()
        {
            var data = _settingsModel.LoadSettings();

            DupFinderPath = data.DupFinderPath;
            TransformFile = data.TransformFile;
            SourceFolder  = data.SourceFolder;
            OutputFile    = data.OutputFile;
        }

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

        private void ExecuteTransform()
        {
            var data = new SettingsData
                       {
                           DupFinderPath = DupFinderPath,
                           TransformFile = TransformFile,
                           SourceFolder = SourceFolder,
                           OutputFile = OutputFile
                       };
            _settingsModel.SaveSettings(data);

            _dupFinderModel.DupFinderPath = DupFinderPath.Trim();
            _dupFinderModel.SourceFolder = SourceFolder.Trim();
            _dupFinderModel.OutputFile = OutputFile;
            _dupFinderModel.TransformFile = TransformFile;
            _dupFinderModel.Run();
        }
    }
}
