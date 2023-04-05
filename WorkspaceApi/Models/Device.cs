namespace WorkspaceApi.Models;

public record Device(
    Guid Id,
    Guid WorkspaceId,
    string Name,
    DateTime Date
);