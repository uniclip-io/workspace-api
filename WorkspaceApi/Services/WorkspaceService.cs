using WorkspaceApi.Models;
using WorkspaceApi.Repositories;

namespace WorkspaceApi.Services;

public class WorkspaceService
{
    private readonly DeviceRepository _deviceRepository;
    private readonly WorkspaceRepository _workspaceRepository;

    public WorkspaceService(WorkspaceRepository workspaceRepository, DeviceRepository deviceRepository)
    {
        _workspaceRepository = workspaceRepository;
        _deviceRepository = deviceRepository;
    }

    public async Task<Workspace> CreateWorkspace(Guid userId)
    {
        var workspaceContract = await _workspaceRepository.CreateWorkspaceForUser(userId);
        return new Workspace(workspaceContract.Id, userId, new List<Device>());
    }

    public async Task<Workspace?> GetWorkspaceByUserId(Guid userId)
    {
        var workspaceContract = await _workspaceRepository.GetWorkspaceByUserId(userId);

        if (workspaceContract == null) return null;

        var deviceContracts = await _deviceRepository.GetDevicesByWorkspaceId(workspaceContract.Id);
        var devices = deviceContracts.Select(d => new Device(d.Id, d.WorkspaceId, d.Name, d.Date)).ToList();

        return new Workspace(workspaceContract.Id, userId, devices);
    }

    public async Task<Device> AddDeviceToWorkspace(Guid workspaceId, string name)
    {
        var deviceContract = await _deviceRepository.AddDeviceToWorkspace(workspaceId, name);
        return new Device(deviceContract.Id, workspaceId, deviceContract.Name, deviceContract.Date);
    }

    public async Task<Device?> RemoveDeviceFromWorkspace(Guid workspaceId, Guid deviceId)
    {
        var deviceContract = await _deviceRepository.RemoveDeviceFromWorkspace(workspaceId, deviceId);
        return deviceContract == null
            ? null
            : new Device(deviceContract.Id, workspaceId, deviceContract.Name, deviceContract.Date);
    }
}