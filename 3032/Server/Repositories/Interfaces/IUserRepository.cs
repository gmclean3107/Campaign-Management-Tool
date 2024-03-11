namespace CampaignManagementTool.Server.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAll();
    Task Add(User user);
    Task<User?> GetById(string id);

}