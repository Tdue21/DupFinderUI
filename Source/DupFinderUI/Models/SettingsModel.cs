using System;
using System.Text;
using DupFinderUI.Services;
using Newtonsoft.Json;

namespace DupFinderUI.Models
{
    public class SettingsModel
    {
        private readonly FileSystemService _fileSystemService;
        private string _settingsFile;

        public SettingsModel(FileSystemService fileSystemService)
        {
            _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
        }

        public string SettingsFile => _settingsFile ?? (_settingsFile = _fileSystemService.GetFilePath("Settings.json"));

        public SettingsData LoadSettings()
        {
            if (!_fileSystemService.FileExists(SettingsFile))
            {
                return new SettingsData();
            }

            var file = _fileSystemService.ReadAllText(SettingsFile, Encoding.UTF8);
            var item = JsonConvert.DeserializeObject<SettingsData>(file);
            return item ?? new SettingsData();
        }

        public void SaveSettings(SettingsData data)
        {
            var content = JsonConvert.SerializeObject(data);
            _fileSystemService.WriteAllText(SettingsFile, content, Encoding.UTF8);
        }
    }
}
