using MongoDB.Driver;
using WorkspaceApi.Models;

namespace WorkspaceApi.Services;

public class WorkspaceService : IWorkspaceService
{
    private readonly IMongoCollection<Workspace> _workspaces;

    public WorkspaceService()
    {
        var mongoClient = new MongoClient(Environment.GetEnvironmentVariable("DB_CONNECTION"));
        var database = mongoClient.GetDatabase("workspaces");
        _workspaces = database.GetCollection<Workspace>("workspaces");
    }

    public async Task<Workspace?> Get(Guid userId)
    {
        return await _workspaces.Find(w => w.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task<Workspace> Create(Guid userId)
    {
        var workspace = new Workspace
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Devices = new List<Device>()
        };
        
        await _workspaces.InsertOneAsync(workspace);
        
        return workspace;
    }

    public async Task<Workspace> Update(Workspace workspace)
    {
        var replaceResult = await _workspaces.ReplaceOneAsync(w => w.UserId == workspace.UserId, workspace);

        if (replaceResult.ModifiedCount == 0)
        {
            throw new ArgumentException($"Workspace for user {workspace.UserId} not found.");
        }
        return workspace;
    }

    public async Task<Workspace> Delete(Workspace workspace)
    {
        var deleteResult = await _workspaces.DeleteOneAsync(w => w.UserId == workspace.UserId);

        if (deleteResult.DeletedCount == 0)
        {
            throw new ArgumentException($"Workspace for user {workspace.UserId} not found.");
        }
        return workspace;
    }
}
