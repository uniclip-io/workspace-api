namespace WorkspaceApi.Models;

public record User(
    Guid Id,
    int MaxConnectedDevices
);