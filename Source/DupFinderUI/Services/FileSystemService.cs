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

using System.IO;
using System.Text;
using System.Xml;
using DupFinderUI.Interfaces;

namespace DupFinderUI.Services
{
    public class FileSystemService : IFileSystemService
    {
        public bool FileExists(string fileName) => File.Exists(fileName);

        public string ReadAllText(string fileName, Encoding encoding) => File.ReadAllText(fileName, encoding);

        public string GetApplicationPath() => Path.GetDirectoryName(GetType().Assembly.Location);

        public string GetFilePath(string fileName) => Path.Combine(GetApplicationPath(), fileName);

        public void WriteAllText(string fileName, string data, Encoding encoding) => File.WriteAllText(fileName, data, encoding);

        public bool DirectoryExists(string directoryPath) => Directory.Exists(directoryPath);

        public string CombinePaths(params string[] paths) => Path.Combine(paths);

        public Stream ReadFile(string path) => File.OpenRead(path);

        public string ChangeExtension(string path, string extension) => Path.ChangeExtension(path, extension);

        public Stream WriteFile(string path) => File.OpenWrite(path);

        public XmlReader CreateXmlReader(string path) => XmlReader.Create(ReadFile(path));

        public XmlWriter CreateXmlWriter(string path) => XmlWriter.Create(WriteFile(path));
    }
}
