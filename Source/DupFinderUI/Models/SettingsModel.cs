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
using System.Text;
using DupFinderUI.Interfaces;
using Newtonsoft.Json;

namespace DupFinderUI.Models
{
    public class SettingsModel : ISettingsModel
    {
        private readonly IFileSystemService _fileSystemService;
        private string _settingsFile;

        public SettingsModel(IFileSystemService fileSystemService)
        {
            _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
        }

        private string SettingsFile => _settingsFile ?? (_settingsFile = _fileSystemService.GetFilePath("Settings.json"));

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
