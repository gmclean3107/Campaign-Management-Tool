using CampaignManagementTool.Server.Repositories.Interfaces;
using CampaignManagementTool.Server.Services.Interfaces;
using CampaignManagementTool.Shared;

namespace CampaignManagementTool.Server.Services;

public class CampaignService : ICampaignService
{
    private readonly ICampaignRepository _repo;
    private readonly IAuditLogRepository _auditRepo;
    private readonly IUserContext _userContext;

    public CampaignService(ICampaignRepository repo, IAuditLogRepository auditRepo, IUserContext userContext)
    {
        _repo = repo;
        _auditRepo = auditRepo;
        _userContext = userContext;
    }

    public async Task<List<Campaign>> GetAll()
    {
        var results = await _repo.GetAll();
        return results;
    }

    public async Task Add(Campaign campaign)
    {
        await _repo.Add(campaign);
    }

    public async Task<Campaign?> GetById(string id)
    {
        return await _repo.GetById(id);
    }

    public async Task<List<Campaign>> CampaignSearchFilter(string code, int filter, int sort) {
        var results = await _repo.CampaignSearchFilter(code, filter, sort);
        return results;
    }

    public async Task Update(string id, Campaign campaign)
    {
        var currentDomain = await _repo.GetById(id);
        var newRecord = AuditingExtensions.DeepCopyJson(campaign);

        var updatedFields = AuditingExtensions.GetDifferingProperties(currentDomain, newRecord);
        
        //Update the record
        await _repo.Update(id,campaign);

        var userId = _userContext.IdentityId;
        var userName = _userContext.Name;

        var userInfo = new UserInfo()
        {
            Id = userId,
            Name = userName
        };
        
        //Add the Auditing Information
        await _auditRepo.Add(new AuditLog()
        {
            Id = Guid.NewGuid(),
            CampaignCode = id,
            Updates = updatedFields,
            AddedDate = DateTime.UtcNow,
            AddedBy = userInfo
        });
    }

    public async Task<List<Campaign>> ExportToCsv()
    {
        var result = await _repo.ExportToCsv();

        return result;
    }

    public async Task<List<Campaign>> ExportToCsvFiltered(string code, int filter, int sort) 
    {
        var results = await _repo.ExportToCsvFiltered(code, filter, sort);
        return results;
    }

    public async Task<Campaign?> ExportCsvSingle(string id)
    {
        var result = await _repo.ExportToCsvSingle(id);
        return result;
    }
}