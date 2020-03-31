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

using System.Text;
using System.Xml;

namespace DupFinderUI.Interfaces
{
    /// <summary>
    /// </summary>
    public interface IFileSystemService
    {
        /// <summary>
        ///     Files the exists.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        bool FileExists(string fileName);

        /// <summary>
        ///     Reads all text.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        string ReadAllText(string fileName, Encoding encoding);

        /// <summary>
        ///     Gets the file path.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        string GetFilePath(string fileName);

        /// <summary>
        ///     Writes all text.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="data">The data.</param>
        /// <param name="encoding">The encoding.</param>
        void WriteAllText(string fileName, string data, Encoding encoding);

        /// <summary>
        ///     Directories the exists.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns></returns>
        bool DirectoryExists(string directoryPath);

        /// <summary>
        ///     Combines the paths.
        /// </summary>
        /// <param name="paths">The paths.</param>
        /// <returns></returns>
        string CombinePaths(params string[] paths);

        /// <summary>
        ///     Changes the extension.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="extension">The extension.</param>
        /// <returns></returns>
        string ChangeExtension(string path, string extension);

        /// <summary>
        ///     Creates the XML reader.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        XmlReader CreateXmlReader(string path);

        /// <summary>
        ///     Creates the XML writer.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        XmlWriter CreateXmlWriter(string path);
    }
}
