public class Task
{
    public string Title { get; set; }
    public string Description { get; set; }
    public ITaskWorker AssignedWorker { get; private set; }

    public void AssignTo(ITaskWorker worker)
    {
        AssignedWorker = worker;
    }
}

public interface ITaskAssigner
{
    void Assign(Task task, ITaskWorker worker);
}

public interface ISubTaskCreator
{
    Task CreateSubTask(Task parent, string title, string description);
}

public interface ITaskWorker
{
    void WorkOn(Task task);
}

public class TeamLead : ITaskAssigner, ISubTaskCreator, ITaskWorker
{
    public void Assign(Task task, ITaskWorker worker)
    {
        task.AssignTo(worker);
    }

    public Task CreateSubTask(Task parent, string title, string description)
    {
        return new Task
        {
            Title = title,
            Description = description
        };
    }

    public void WorkOn(Task task)
    {
        // implementation
    }
}

public class Manager : ITaskAssigner
{
    public void Assign(Task task, ITaskWorker worker)
    {
        task.AssignTo(worker);
    }
}

public class Developer : ITaskWorker
{
    public string Name { get; set; }

    public void WorkOn(Task task)
    {
        // implementation
    }
}

public class TaskController
{
    private readonly ITaskAssigner _assigner;
    private readonly ISubTaskCreator _creator;

    public TaskController(ITaskAssigner assigner, ISubTaskCreator creator)
    {
        _assigner = assigner;
        _creator = creator;
    }

    public void DelegateFeature()
    {
        var feature = new Task
        {
            Title = "New Feature",
            Description = "..."
        };

        var sub = _creator.CreateSubTask(feature, "Implement API", "Backend API work");
        var dev = new Developer { Name = "Alice" };
        _assigner.Assign(sub, dev);
    }
}
