namespace WorkspaceApi.Repositories.Contracts;

public record DeviceContract(
    Guid Id,
    Guid WorkspaceId,
    string Name,
    DateTime Date
);