using WorkspaceApi.Models;
using WorkspaceApi.Services;

namespace WorkspaceApi.Test;

public class FakeWorkspaceService : IWorkspaceService
{
    private readonly List<Workspace> _workspaces = new();

    public Task<Workspace?> Get(Guid userId)
    {
        return new Task<Workspace?>(() => _workspaces.Find(w => w.UserId == userId));
    }

    public Task<Workspace> Create(Guid userId)
    {
        var workspace = new Workspace
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Devices = new List<Device>()
        };
        
        _workspaces.Add(workspace);
        return new Task<Workspace>(() => workspace);
    }

    public Task<Workspace> Update(Workspace workspace)
    {
        Delete(workspace);
        _workspaces.Add(workspace);
        return new Task<Workspace>(() => workspace);
    }

    public Task<Workspace> Delete(Workspace workspace)
    {
        var found = _workspaces.Find(w => w.UserId == workspace.UserId);
        
        if (found == null)
        {
            throw new ArgumentException($"Workspace for user {workspace.UserId} not found.");
        }
        
        _workspaces.Remove(found);
        return new Task<Workspace>(() => workspace);
    }
}
