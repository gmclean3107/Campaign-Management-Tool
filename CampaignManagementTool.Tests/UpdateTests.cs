using CampaignManagementTool.Server.Repositories;
using CampaignManagementTool.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampaignManagementTool.Tests
{
    [TestFixture]
    public class UpdateTests
    {
        private MockCampaignRepository _campaignRepository;

        [SetUp]
        public void Setup()
        {
            _campaignRepository = new MockCampaignRepository();
        }


        [Test]
        public async Task Update_Updates_Existing_Campaign()
        {
            Console.WriteLine("Testing Update Function");
            var existingCampaign = (await _campaignRepository.GetAll()).First();
            var updatedCampaign = new Campaign
            {
                CampaignCode = existingCampaign.CampaignCode,
                AffiliateCode = existingCampaign.AffiliateCode,
                RequiresApproval = !existingCampaign.RequiresApproval,
                Rules = existingCampaign.Rules,
                RulesUrl = existingCampaign.RulesUrl,
                ExpiryDays = existingCampaign.ExpiryDays,
                isDeleted = existingCampaign.isDeleted
            };

            await _campaignRepository.Update(existingCampaign.CampaignCode, updatedCampaign);
            var retrievedCampaign = await _campaignRepository.GetById(existingCampaign.CampaignCode);

            Assert.That(retrievedCampaign != null);
            Assert.That(updatedCampaign.RequiresApproval == retrievedCampaign.RequiresApproval && retrievedCampaign.RequiresApproval != existingCampaign.RequiresApproval);
        }

    }
}
