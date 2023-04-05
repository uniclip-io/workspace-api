using MongoDB.Driver;
using WorkspaceApi.Repositories.Contracts;

namespace WorkspaceApi.Repositories;

public class DeviceRepository
{
    private readonly IMongoCollection<DeviceContract> _devices;

    public DeviceRepository(string connectionString)
    {
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase("workspace-service");
        _devices = database.GetCollection<DeviceContract>("devices");
    }

    public async Task<List<DeviceContract>> GetDevicesByWorkspaceId(Guid workspaceId)
    {
        var devices = await _devices.Find(d => d.WorkspaceId == workspaceId).ToListAsync();
        devices.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
        return devices;
    }

    public async Task<DeviceContract> AddDeviceToWorkspace(Guid workspaceId, string name)
    {
        var deviceContract = new DeviceContract(Guid.NewGuid(), workspaceId, name, DateTime.Now);
        await _devices.InsertOneAsync(deviceContract);
        return deviceContract;
    }

    public async Task<DeviceContract?> RemoveDeviceFromWorkspace(Guid workspaceId, Guid deviceId)
    {
        var devices = await GetDevicesByWorkspaceId(workspaceId);
        var deviceContract = devices.Find(d => d.Id == deviceId);

        if (deviceContract == null)
        {
            return null;
        }

        await _devices.DeleteOneAsync(d => d.Id == deviceContract.Id);
        return deviceContract;
    }
}