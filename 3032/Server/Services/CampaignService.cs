using CampaignManagementTool.Server.Repositories.Interfaces;
using CampaignManagementTool.Server.Services.Interfaces;
using CampaignManagementTool.Shared;

namespace CampaignManagementTool.Server.Services;

/// <summary>
/// Service for managing campaigns.
/// </summary>
public class CampaignService : ICampaignService
{
    private readonly ICampaignRepository _repo;
    private readonly IAuditLogRepository _auditRepo;
    private readonly IUserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="CampaignService"/> class.
    /// </summary>
    /// <param name="repo">The campaign repository.</param>
    /// <param name="auditRepo">The audit log repository.</param>
    /// <param name="userContext">The user context.</param>
    public CampaignService(ICampaignRepository repo, IAuditLogRepository auditRepo, IUserContext userContext)
    {
        _repo = repo;
        _auditRepo = auditRepo;
        _userContext = userContext;
    }

    /// <summary>
    /// Retrieves all campaigns.
    /// </summary>
    /// <returns>The list of campaigns.</returns>
    public async Task<List<Campaign>> GetAll()
    {
        var results = await _repo.GetAll();
        return results;
    }

    /// <summary>
    /// Adds a new campaign.
    /// </summary>
    /// <param name="campaign">The campaign to add.</param>
    public async Task Add(Campaign campaign)
    {
        await _repo.Add(campaign);
    }

    /// <summary>
    /// Retrieves a campaign by its ID.
    /// </summary>
    /// <param name="id">The ID of the campaign to retrieve.</param>
    /// <returns>The campaign with the specified ID, or null if not found.</returns>
    public async Task<Campaign?> GetById(string id)
    {
        return await _repo.GetById(id);
    }

    /// <summary>
    /// Filters campaigns based on search criteria.
    /// </summary>
    /// <param name="code">The search criteria.</param>
    /// <param name="filter">The filter criteria.</param>
    /// <param name="sort">The sort criteria.</param>
    /// <returns>The filtered list of campaigns.</returns>
    public async Task<List<Campaign>> CampaignSearchFilter(string code, int filter, int sort) {
        var results = await _repo.CampaignSearchFilter(code, filter, sort);
        return results;
    }

    /// <summary>
    /// Updates a campaign.
    /// </summary>
    /// <param name="id">The ID of the campaign to update.</param>
    /// <param name="campaign">The updated campaign data.</param>
    public async Task Update(string id, Campaign campaign)
    {
        var currentDomain = await _repo.GetById(id);
        var newRecord = AuditingExtensions.DeepCopyJson(campaign);

        var updatedFields = AuditingExtensions.GetDifferingProperties(currentDomain, newRecord);
        
        //Update the record
        await _repo.Update(id,campaign);

        try
        {
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
        }catch(Exception ex) 
        {
            Console.WriteLine("No UserContext");
        }
    }

    /// <summary>
    /// Exports all campaigns to a CSV file.
    /// </summary>
    /// <returns>The CSV file data.</returns>
    public async Task<byte[]> ExportToCsv()
    {
        var result = await _repo.ExportToCsv();

        return result;
    }

    /// <summary>
    /// Exports filtered campaigns to a CSV file.
    /// </summary>
    /// <param name="code">The search criteria.</param>
    /// <param name="filter">The filter criteria.</param>
    /// <param name="sort">The sort criteria.</param>
    /// <returns>The CSV file data of filtered campaigns.</returns>
    public async Task<byte[]> ExportToCsvFiltered(string code, int filter, int sort) 
    {
        var results = await _repo.ExportToCsvFiltered(code, filter, sort);
        return results;
    }

    /// <summary>
    /// Exports a single campaign to a CSV file.
    /// </summary>
    /// <param name="id">The ID of the campaign to export.</param>
    /// <returns>The CSV file data of the specified campaign.</returns>
    public async Task<byte[]> ExportCsvSingle(string id)
    {
        var result = await _repo.ExportToCsvSingle(id);
        return result;
    }
}