using WorkspaceApi.Models;

namespace WorkspaceApi.Services;

public interface IWorkspaceService
{
    Task<Workspace> Get(Guid userId);
    Task<Workspace> Create(Guid userId);
    Task<Workspace> Update(Workspace workspace);
    Task<Workspace> Delete(Workspace workspace);
}