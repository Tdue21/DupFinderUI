// ****************************************************************************
// * MIT License
// *
// * Copyright (c)  Thomas Due
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

namespace DupFinderUI.Interfaces
{
    public interface IFileSystemService
    {
        bool FileExists(string fileName);
        string ReadAllText(string fileName, Encoding encoding);
        string GetApplicationPath();
        string GetFilePath(string fileName);
        void WriteAllText(string fileName, string data, Encoding encoding);
        bool DirectoryExists(string directoryPath);
        string CombinePaths(params string[] paths);
        Stream ReadFile(string path);
        string ChangeExtension(string path, string extension);
        Stream WriteFile(string path);
        XmlReader CreateXmlReader(string path);
        XmlWriter CreateXmlWriter(string path);
    }
}
