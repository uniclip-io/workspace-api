using Microsoft.AspNetCore.Mvc;
using WorkspaceApi.Models;
using WorkspaceApi.Models.DTOs;
using WorkspaceApi.Services;

namespace WorkspaceApi.Controllers;

[ApiController]
[Route("workspace")]
public class WorkspaceController : ControllerBase
{
    private readonly WorkspaceService _service;

    public WorkspaceController(WorkspaceService workspaceService)
    {
        _service = workspaceService;
    }

    [HttpGet("/device/user/{userId:guid}")]
    public async Task<ActionResult<Workspace>> Get(Guid userId)
    {
        return await _service.Get(userId);
    }

    [HttpPost("/device/add")]
    public async Task<ActionResult<Workspace>> ConnectDevice(DeviceDto data)
    {
        var workspace = await _service.Get(data.UserId);

        if (workspace == null)
        {
            workspace = await _service.Create(data.UserId);
        }
        else if (workspace.Devices.FindIndex(d => d.Id == data.Device.Id) != -1)
        {
            return UnprocessableEntity("Device already connected.");
        }
        
        workspace.Devices.Add(data.Device);
        await _service.Update(workspace);
        return Ok(workspace);
    }
    
    [HttpPost("/device/remove")]
    public async Task<ActionResult<Workspace>> DisconnectDevice(DeviceDto data)
    {
        var workspace = await _service.Get(data.UserId);

        if (workspace == null)
        {
            return NotFound("Workspace not found.");
        }

        var device = workspace.Devices.SingleOrDefault(d => d!.Id == data.Device.Id, null);

        if (device == null)
        {
            return NotFound("Device not found.");
        }

        workspace.Devices.Remove(device);
        await _service.Update(workspace);
        return Ok(workspace);
    }
    
    [HttpPost("/device/update")]
    public async Task<ActionResult<Workspace>> UpdateDevice(DeviceDto data)
    {
        var workspace = await _service.Get(data.UserId);

        if (workspace == null)
        {
            return NotFound("Workspace not found.");
        }

        var device = workspace.Devices.SingleOrDefault(d => d!.Id == data.Device.Id, null);

        if (device == null)
        {
            return NotFound("Device not found.");
        }

        workspace.Devices.Remove(device);
        workspace.Devices.Add(data.Device);
        await _service.Update(workspace);
        return Ok(workspace);
    }
}