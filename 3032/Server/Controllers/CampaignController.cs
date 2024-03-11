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

    public CampaignController(ICampaignService service, IAuditLogService auditLogService)
    {
        _service = service;
        _auditLogService = auditLogService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var results = await _service.GetAll();
        return Ok(results);
    }

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
    
    [HttpGet("{id}/History")]
    public async Task<ActionResult> GetHistory([FromRoute]string id)
    {
        return Ok(await _auditLogService.GetForCampaign(id));
    }

    [HttpGet("Search")]
    public async Task<ActionResult> CampaignSearchFilter([FromQuery] int sort, [FromQuery] int filter, [FromQuery] string code = "")
    {
        return Ok(await _service.CampaignSearchFilter(code, filter, sort));
    }

    [HttpPost]
    [Authorize(Roles = Roles.Editor)]
    public async Task<ActionResult> Add([FromBody]Campaign campaign)
    {
        await _service.Add(campaign);

        return Ok();
    }

    [HttpPut]
    [Authorize(Roles = Roles.Editor)]
    public async Task<ActionResult> Update([FromBody] Campaign campaign)
    {
        await _service.Update(campaign.CampaignCode ,campaign);

        return Ok();
    }

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