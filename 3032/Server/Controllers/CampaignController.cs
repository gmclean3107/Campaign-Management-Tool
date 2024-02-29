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

    public CampaignController(ICampaignService service)
    {
        _service = service;
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
        return Ok(await _service.GetById(id));
    }

    [HttpGet("Search")]
    public async Task<ActionResult> CampaignSearchFilter([FromQuery] int sort, [FromQuery] int filter, [FromQuery] string code = "")
    {
        return Ok(await _service.CampaignSearchFilter(code, filter, sort));
    }

    [HttpPost]
    public async Task<ActionResult> Add([FromBody]Campaign campaign)
    {
        await _service.Add(campaign);

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] Campaign campaign)
    {
        await _service.Update(campaign.CampaignCode ,campaign);

        return Ok();
    }

    [HttpGet("Export")]
    public async Task<ActionResult> ExportToCsv([FromQuery] bool isSingleCampaign)
    {
        return Ok(await _service.ExportToCsv(isSingleCampaign));
    }
}