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
    /// <summary>
    /// </summary>
    /// <seealso cref="DupFinderUI.Interfaces.IFileSystemService" />
    public class FileSystemService : IFileSystemService
    {
        /// <summary>
        ///     Files the exists.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public bool FileExists(string fileName) => File.Exists(fileName);

        /// <summary>
        ///     Reads all text.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public string ReadAllText(string fileName, Encoding encoding) => File.ReadAllText(fileName, encoding);

        /// <summary>
        ///     Gets the file path.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public string GetFilePath(string fileName) => Path.Combine(GetApplicationPath(), fileName);

        /// <summary>
        ///     Writes all text.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="data">The data.</param>
        /// <param name="encoding">The encoding.</param>
        public void WriteAllText(string fileName, string data, Encoding encoding) => File.WriteAllText(fileName, data, encoding);

        /// <summary>
        ///     Directories the exists.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns></returns>
        public bool DirectoryExists(string directoryPath) => Directory.Exists(directoryPath);

        /// <summary>
        ///     Combines the paths.
        /// </summary>
        /// <param name="paths">The paths.</param>
        /// <returns></returns>
        public string CombinePaths(params string[] paths) => Path.Combine(paths);

        /// <summary>
        ///     Changes the extension.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="extension">The extension.</param>
        /// <returns></returns>
        public string ChangeExtension(string path, string extension) => Path.ChangeExtension(path, extension);

        /// <summary>
        ///     Creates the XML reader.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public XmlReader CreateXmlReader(string path) => XmlReader.Create(ReadFile(path));

        /// <summary>
        ///     Creates the XML writer.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public XmlWriter CreateXmlWriter(string path) => XmlWriter.Create(WriteFile(path));

        /// <summary>
        ///     Gets the application path.
        /// </summary>
        /// <returns></returns>
        private string GetApplicationPath() => Path.GetDirectoryName(GetType().Assembly.Location);

        /// <summary>
        ///     Reads the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private Stream ReadFile(string path) => File.OpenRead(path);

        /// <summary>
        ///     Writes the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private Stream WriteFile(string path) => File.OpenWrite(path);
    }
}
