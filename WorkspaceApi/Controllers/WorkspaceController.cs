using Microsoft.AspNetCore.Mvc;
using WorkspaceApi.Dtos;
using WorkspaceApi.Models;
using WorkspaceApi.Services;

namespace WorkspaceApi.Controllers;

[ApiController]
[Route("workspace")]
public class WorkspaceController : ControllerBase
{
    private readonly WorkspaceService _workspaceService;

    public WorkspaceController(WorkspaceService workspaceService)
    {
        _workspaceService = workspaceService;
    }

    [HttpGet("/device/user/{userId:guid}")]
    public async Task<ActionResult<Workspace>> Get(Guid userId)
    {
        var workspace = await _workspaceService.GetWorkspaceByUserId(userId);

        if (workspace == null)
        {
            return NotFound("Workspace not found.");
        }

        return Ok(workspace);
    }

    [HttpPost("/device/add")]
    public async Task<ActionResult<Device>> ConnectDevice(AddDeviceDto addDeviceDto)
    {
        var userId = addDeviceDto.UserId;

        var workspace = await _workspaceService.GetWorkspaceByUserId(userId) ??
                        await _workspaceService.CreateWorkspace(userId);

        if (workspace.Devices.Find(d => d.Name == addDeviceDto.Name) != null)
        {
            return UnprocessableEntity("Device with name already exists.");
        }

        var device = await _workspaceService.AddDeviceToWorkspace(workspace.Id, addDeviceDto.Name);
        return Ok(device);
    }

    [HttpDelete("/device/remove")]
    public async Task<ActionResult<Device>> DisconnectDevice(RemoveDeviceDto removeDeviceDto)
    {
        var workspace = await _workspaceService.GetWorkspaceByUserId(removeDeviceDto.UserId);

        if (workspace == null)
        {
            return NotFound("Workspace not found.");
        }

        if (workspace.Devices.Find(d => d.Id == removeDeviceDto.DeviceId) == null)
        {
            return NotFound("Device not found.");
        }

        var device = await _workspaceService.RemoveDeviceFromWorkspace(workspace.Id, removeDeviceDto.DeviceId);
        return Ok(device);
    }
}