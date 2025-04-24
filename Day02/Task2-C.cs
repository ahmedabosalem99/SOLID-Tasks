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

// This saver enforces read‐only: any Save attempt will throw
public class ReadOnlyFileSaver : IFileSaver
{
    public void Save(string path, string content) =>
        throw new IOException($"File '{path}' is read‐only and cannot be saved.");
}

// Our SqlFile no longer has LoadText/SaveText methods—it just holds data and delegates to abstractions
public class SqlFile
{
    public string FilePath  { get; }
    public string FileText  { get; set; }

    private readonly IFileLoader _loader;
    private readonly IFileSaver  _saver;

    public SqlFile(string filePath, IFileLoader loader, IFileSaver saver)
    {
        FilePath = filePath;
        _loader  = loader;
        _saver   = saver;
    }

    public void Load() => FileText = _loader.Load(FilePath);
    public void Save() => _saver.Save(FilePath, FileText);
}

public class SqlFileManager
{
    private readonly IEnumerable<SqlFile> _files;

    public SqlFileManager(IEnumerable<SqlFile> files)
    {
        _files = files;
    }

    public string LoadAll()
    {
        var sb = new StringBuilder();
        foreach (var f in _files)
        {
            f.Load();
            sb.AppendLine(f.FileText);
        }
        return sb.ToString();
    }

    public void SaveAll()
    {
        foreach (var f in _files)
        {
            f.Save();    // if this file was constructed with ReadOnlyFileSaver, an IOException is thrown
        }
    }
}
