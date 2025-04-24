// Current Design Smells

// 1.Type‐check for behavior
// SqlFileManager does if (file is ReadOnlySqlFile) to decide behavior. That couples it to every new file subtype you might add.
//2. SRP violation
// Manager both orchestrates and contains business logic about who can be saved.
//3. OCP violation
// Every time you introduce a new file‐type with different save rules, you must change SqlFileManager.
// 4.DIP violation
// High‐level code (manager) depends on concrete class identities rather than abstractions.

// Refactored Solution
// – Using the Null‐Object (or Strategy) pattern for saving.
// – Pushing “can’t save” behavior into a saver implementation.
// – Manager simply calls Save() on every file—no checks required.

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


public class NoOpFileSaver : IFileSaver
{
    public void Save(string path, string content) { /* no-op */ }
}

public class SqlFile
{
    public string FilePath { get; }
    public string FileText { get; set; }

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
            f.Save();
    }
}
