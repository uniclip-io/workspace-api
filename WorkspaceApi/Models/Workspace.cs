namespace WorkspaceApi.Models;

public record Workspace
(
    Guid Id,
    Guid UserId,
    List<Device> Devices
);