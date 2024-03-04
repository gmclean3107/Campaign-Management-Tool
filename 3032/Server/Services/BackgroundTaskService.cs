namespace CampaignManagementTool.Server.Services;

using CampaignManagementTool.Server.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

public class BackgroundTaskService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public BackgroundTaskService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var _campaignService = scope.ServiceProvider.GetRequiredService<ICampaignService>();

        while (!stoppingToken.IsCancellationRequested)
        {
            // Perform the task you want to schedule here
            Console.WriteLine("Expiry Date Check Done: " + DateTime.Now);
            //await CheckCampaignExpiry(_campaignService);

            // Wait for some time before executing the task again
            await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
        }
    }

    private async Task CheckCampaignExpiry(ICampaignService campaignService) 
    {
        var campaigns = await campaignService.GetAll();

        foreach (var campaign in campaigns) 
        {
            bool prevState = campaign.isDeleted;
            int dateComp = DateTime.Compare(DateTime.Parse(campaign.ExpiryDays), DateTime.Now.Date);

            if (dateComp <= 0)
            {
                campaign.isDeleted = true;
            }
            else
            {
                campaign.isDeleted = false;
            }
            if (campaign.isDeleted != prevState)
            {
                await campaignService.Update(campaign.CampaignCode, campaign);
            }
        }
    }

}
