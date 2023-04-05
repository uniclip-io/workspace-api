using MongoDB.Driver;
using WorkspaceApi.Repositories.Contracts;

namespace WorkspaceApi.Repositories;

public class WorkspaceRepository
{
    private readonly IMongoCollection<WorkspaceContract> _workspaces;

    public WorkspaceRepository(string connectionString)
    {
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase("workspace-api");
        _workspaces = database.GetCollection<WorkspaceContract>("workspaces");
    }

    public async Task<WorkspaceContract> CreateWorkspaceForUser(Guid userId)
    {
        var found = await GetWorkspaceByUserId(userId);

        if (found != null) throw new ArgumentException("User already has a workspace.");

        var workspaceContract = new WorkspaceContract(Guid.NewGuid(), userId);
        await _workspaces.InsertOneAsync(workspaceContract);
        return workspaceContract;
    }

    public async Task<WorkspaceContract> GetWorkspaceByUserId(Guid userId)
    {
        return await _workspaces.Find(c => c.UserId == userId).FirstOrDefaultAsync();
    }
}