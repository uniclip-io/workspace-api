namespace WorkspaceApi.Models;

public class Workspace
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public List<Device> Devices { get; set; }
}
