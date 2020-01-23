using System.IO;
using System.Text;
using System.Xml;

namespace DupFinderUI.Services
{
    public class FileSystemService
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
