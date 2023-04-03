using Microsoft.AspNetCore.Mvc;
using WorkspaceApi.Controllers;
using WorkspaceApi.Models.DTOs;

namespace WorkspaceApi.Test;

public class WorkspaceControllerTest : IDisposable
{
    private WorkspaceController _controller;
    
    public void Dispose()
    {
        _controller = new WorkspaceController(new FakeWorkspaceService());
    }

    [Fact]
    public async void Get_WorkspaceWithDevice_200()
    {
        var userId = Guid.NewGuid();
        
        await _controller.ConnectDevice(new DeviceDto {
            UserId = userId,
            Device = { Id = Guid.NewGuid() }
        });
        
        var result = await _controller.Get(userId);
        
        var content = Assert.IsType<ContentResult>(result.Result);
        Assert.Equal(200, content.StatusCode);
        Assert.Equal("Workspace not found.", content.Content);
        Assert.Single(result.Value!.Devices);
    }

    [Fact]
    public async void Get_WorkspaceWithoutDevice_404()
    {
        var result = await _controller.Get(Guid.NewGuid());
        
        var content = Assert.IsType<ContentResult>(result.Result);
        Assert.Equal(404, content.StatusCode);
        Assert.Equal("Workspace not found.", content.Content);
    }

    [Fact]
    public async void ConnectDevice_AddMultipleDevices_200()
    {
        var userId = Guid.NewGuid();

        for (var i = 0; i < 3; i++)
        {
            var deviceDto = new DeviceDto
            {
                UserId = userId,
                Device = { Id = Guid.NewGuid() }
            };

            var result = await _controller.ConnectDevice(deviceDto);

            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(i, result.Value!.Devices.Count);
        }
    }

    [Fact]
    public async void ConnectDevice_AddSameDevice_422()
    {
        var deviceDto = new DeviceDto
        {
            UserId = Guid.NewGuid(),
            Device = { Id = Guid.NewGuid() }
        };

        var success = await _controller.ConnectDevice(deviceDto);
        var failure = await _controller.ConnectDevice(deviceDto);
        
        Assert.IsType<OkObjectResult>(success.Result);
        Assert.IsType<UnprocessableEntityObjectResult>(failure.Result);
    }

    [Fact]
    public async void DisconnectDevice_RemoveExistingDevice_200()
    {
        var deviceDto = new DeviceDto
        {
            UserId = Guid.NewGuid(),
            Device = { Id = Guid.NewGuid() }
        };

        await _controller.ConnectDevice(deviceDto);
        var result = await _controller.DisconnectDevice(deviceDto);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async void DisconnectDevice_RemoveUnknownDevice_404()
    {
        var userId = Guid.NewGuid();
        
        await _controller.ConnectDevice(new DeviceDto {
            UserId = userId,
            Device = { Id = Guid.NewGuid() }
        });

        var result = await _controller.DisconnectDevice(new DeviceDto {
            UserId = userId,
            Device = { Id = Guid.NewGuid() }
        });
        
        var content = Assert.IsType<ContentResult>(result.Result);
        Assert.Equal(404, content.StatusCode);
        Assert.Equal("Device not found.", content.Content);
    }

    [Fact]
    public async void DisconnectDevice_CalledByUnknownUser_404()
    {
        var deviceDto = new DeviceDto
        {
            UserId = Guid.NewGuid(),
            Device = { Id = Guid.NewGuid() }
        };

        var result = await _controller.DisconnectDevice(deviceDto);
        
        var content = Assert.IsType<ContentResult>(result.Result);
        Assert.Equal(404, content.StatusCode);
        Assert.Equal("Workspace not found.", content.Content);
    }

    [Fact]
    public async void UpdateDevice_ChangeNameExistingDevice_200()
    {
        var deviceDto = new DeviceDto
        {
            UserId = Guid.NewGuid(),
            Device = { Id = Guid.NewGuid() }
        };
        
        var connectResult = await _controller.ConnectDevice(deviceDto);

        const string deviceName = "example_name";
        Assert.NotEqual(deviceName, connectResult.Value!.Devices[0].Name);

        deviceDto.Device.Name = deviceName;
        
        var result = await _controller.UpdateDevice(deviceDto);
        Assert.Equal(deviceName, result.Value!.Devices[0].Name);
    }

    [Fact]
    public async void UpdateDevice_ChangeNameUnknownDevice_404()
    {
        var deviceDto = new DeviceDto
        {
            UserId = Guid.NewGuid(),
            Device = { Id = Guid.NewGuid() }
        };
        
        await _controller.ConnectDevice(deviceDto);
        var result = await _controller.UpdateDevice(deviceDto);
        
        var content = Assert.IsType<ContentResult>(result.Result);
        Assert.Equal(404, content.StatusCode);
        Assert.Equal("Device not found.", content.Content);
    }

    [Fact]
    public async void UpdateDevice_CalledByUnknownUser_404()
    {
        var deviceDto = new DeviceDto
        {
            UserId = Guid.NewGuid(),
            Device = { Id = Guid.NewGuid() }
        };
        
        await _controller.ConnectDevice(deviceDto);
        var result = await _controller.UpdateDevice(deviceDto);
        
        var content = Assert.IsType<ContentResult>(result.Result);
        Assert.Equal(404, content.StatusCode);
        Assert.Equal("Workspace not found.", content.Content);
    }
}