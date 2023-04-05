namespace WorkspaceApi.Dtos;

public record AddDeviceDto
(
    Guid UserId,
    string Name
);