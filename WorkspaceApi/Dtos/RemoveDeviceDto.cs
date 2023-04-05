namespace WorkspaceApi.Dtos;

public record RemoveDeviceDto
(
    Guid UserId,
    Guid DeviceId
);