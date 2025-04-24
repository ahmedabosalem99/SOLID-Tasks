public interface IFileLoader
{
    string Load(string path);
}

public interface IFileSaver
{
    void Save(string path, string content);
}

public class FileSystemLoader : IFileLoader
{
    public string Load(string path) => File.ReadAllText(path);
}

public class FileSystemSaver : IFileSaver
{
    public void Save(string path, string content) => File.WriteAllText(path, content);
}

public class SqlFile
{
    public string FilePath  { get; }
    public string FileText  { get; set; }

    public SqlFile(string filePath)
    {
        FilePath = filePath;
    }
}

public class SqlFileManager
{
    private readonly IEnumerable<SqlFile> _files;
    private readonly IFileLoader         _loader;
    private readonly IFileSaver          _saver;

    public SqlFileManager(
        IEnumerable<SqlFile> files,
        IFileLoader loader,
        IFileSaver saver)
    {
        _files  = files;
        _loader = loader;
        _saver  = saver;
    }

    public string LoadAll()
    {
        var sb = new StringBuilder();
        foreach (var file in _files)
        {
            file.FileText = _loader.Load(file.FilePath);
            sb.AppendLine(file.FileText);
        }
        return sb.ToString();
    }

    public void SaveAll()
    {
        foreach (var file in _files)
        {
            _saver.Save(file.FilePath, file.FileText);
        }
    }
}
