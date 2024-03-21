using CampaignManagementTool.Server.Services;
using CampaignManagementTool.Server.Services.Interfaces;
using CampaignManagementTool.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace CampaignManagementTool.Server.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
public class CampaignController : Controller
{
    private readonly ICampaignService _service;
    private readonly IAuditLogService _auditLogService;

    /// <summary>
    /// Constructor for the Campaign Controller
    /// </summary>
    /// <param name="service"></param>
    /// <param name="auditLogService"></param>
    public CampaignController(ICampaignService service, IAuditLogService auditLogService)
    {
        _service = service;
        _auditLogService = auditLogService;
    }

    /// <summary>
    /// Retrieves all campaigns.
    /// </summary>
    /// <returns>Code 200 with an array of all the campaigns</returns>
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var results = await _service.GetAll();
        return Ok(results);
    }

    /// <summary>
    /// Retrieves a campaign by its ID.
    /// </summary>
    /// <param name="id">The ID of the campaign to retrieve.</param>
    /// <returns>Code 200 and Campaign with the specified ID; otherwise, code 404 and null.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById([FromRoute]string id)
    {
        var response = await _service.GetById(id);

        if (response != null)
        {
            return Ok(await _service.GetById(id));
        }
        else 
        {
            return NotFound(null);
        }
    }

    /// <summary>
    /// Retrieves the history of changes for a specific campaign.
    /// </summary>
    /// <param name="id">The ID of the campaign to retrieve history for.</param>
    /// <returns>Code 200 with result of getting the history of the campaign</returns>
    [HttpGet("{id}/History")]
    public async Task<ActionResult> GetHistory([FromRoute]string id)
    {
        return Ok(await _auditLogService.GetForCampaign(id));
    }

    /// <summary>
    /// Searches for campaigns based on the specified filters.
    /// </summary>
    /// <param name="sort">The sorting option.</param>
    /// <param name="filter">The filtering option.</param>
    /// <param name="code">The search code.</param>
    /// <returns>Code 200 with the result of the search.</returns>
    [HttpGet("Search")]
    public async Task<ActionResult> CampaignSearchFilter([FromQuery] int sort, [FromQuery] int filter, [FromQuery] string code = "")
    {
        return Ok(await _service.CampaignSearchFilter(code, filter, sort));
    }

    /// <summary>
    /// Adds a campaign to the database.
    /// </summary>
    /// <param name="campaign">Campaign to add</param>
    /// <returns>Code 200</returns>
    [HttpPost]
    [Authorize(Roles = Roles.Editor)]
    public async Task<ActionResult> Add([FromBody]Campaign campaign)
    {
        await _service.Add(campaign);

        return Ok();
    }

    /// <summary>
    /// Updates an existing campaign.
    /// </summary>
    /// <param name="campaign">Campaign with updated values</param>
    /// <returns>Code 200</returns>
    [HttpPut]
    [Authorize(Roles = Roles.Editor)]
    public async Task<ActionResult> Update([FromBody] Campaign campaign)
    {
        await _service.Update(campaign.CampaignCode ,campaign);

        return Ok();
    }

    /// <summary>
    /// Exports all campaigns to a CSV file.
    /// </summary>
    /// <returns>Result of the export logic</returns>
    [HttpGet("Export")]
    public async Task<ActionResult> ExportToCsv()
    {
        var csvData = await _service.ExportToCsv();
        if (csvData == null)
        {
            return NotFound("Data not found or error occurred during export.");
        }

        var result = new FileContentResult(csvData, "text/csv")
        {
            FileDownloadName = "AllCampaigns.csv"
        };

        Response.Headers.Add("Content-Disposition", "attachment; filename=AllCampaigns.csv; filename*=UTF-8''AllCampaigns.csv");
        Response.Headers.Add("Content-Length", csvData.Length.ToString());
        Response.Headers.Add("Content-Type", "text/csv");

        return result;
    }

    /// <summary>
    /// Exports filtered campaigns to a CSV file.
    /// </summary>
    /// <returns>Result of the export logic</returns>
    [HttpGet("ExportFiltered")]
    public async Task<ActionResult> ExportToCsvFiltered([FromQuery] int sort, [FromQuery] int filter, [FromQuery] string code = "")
    {
        var csvData = await _service.ExportToCsvFiltered(code, filter, sort);
        if (csvData == null)
        {
            return NotFound("Data not found or error occurred during export.");
        }

        var result = new FileContentResult(csvData, "text/csv")
        {
            FileDownloadName = "AllCampaigns.csv"
        };

        Response.Headers.Add("Content-Disposition", "attachment; filename=AllCampaigns.csv; filename*=UTF-8''AllCampaigns.csv");
        Response.Headers.Add("Content-Length", csvData.Length.ToString());
        Response.Headers.Add("Content-Type", "text/csv");

        return result;
    }

    /// <summary>
    /// Exports a single campaign to a CSV file.
    /// </summary>
    /// <returns>Result of the export logic</returns>
    [HttpGet("ExportSingle")]
    public async Task<ActionResult> ExportToCsvSingle([FromQuery] string id) 
    {
        var csvData = await _service.ExportCsvSingle(id);
        if (csvData == null)
        {
            return NotFound("Data not found or error occurred during export.");
        }

        var result = new FileContentResult(csvData, "text/csv")
        {
            FileDownloadName = "AllCampaigns.csv"
        };

        Response.Headers.Add("Content-Disposition", "attachment; filename=AllCampaigns.csv; filename*=UTF-8''AllCampaigns.csv");
        Response.Headers.Add("Content-Length", csvData.Length.ToString());
        Response.Headers.Add("Content-Type", "text/csv");

        return result;
    }

}