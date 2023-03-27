namespace WorkspaceApi.Models;

public class User
{
    public Guid Id { get; set; }

    public int MaxConnectedDevices { get; set; }
}
