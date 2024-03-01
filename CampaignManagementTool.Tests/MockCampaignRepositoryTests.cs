using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampaignManagementTool.Server.Repositories;
using CampaignManagementTool.Shared;

namespace CampaignManagementTool.Tests
{
    [TestFixture]
    public class MockCampaignRepositoryTests
    { 
        private MockCampaignRepository _campaignRepository;

        [SetUp]
        public void Setup() 
        {
            _campaignRepository = new MockCampaignRepository();
        }

        [Test]
        public async Task GetAll_Returns_All_Campaigns()
        {
            var campaigns = await _campaignRepository.GetAll();

            Assert.That(campaigns != null);
            Assert.That(20 == campaigns.Count);
            int count = 0;
            string formattedNumber = "";
            foreach (var campaign in campaigns)
            {
                count++;
                formattedNumber = count.ToString("D3");
                Assert.That(campaign.CampaignCode == "camp" + formattedNumber);
            }
        }

        [Test]
        public async Task Add_Adds_New_Campaign()
        {
            var newCampaign = new Campaign
            {
                CampaignCode = "NEWCAMPAIGN",
                AffiliateCode = "NEWAFFILIATE",
                RequiresApproval = false,
                Rules = "New rules",
                RulesUrl = "http://example.com",
                ExpiryDays = DateTime.UtcNow.ToString(),
                isDeleted = false
            };

            await _campaignRepository.Add(newCampaign);
            var retrievedCampaign = await _campaignRepository.GetById("NEWCAMPAIGN");

            Assert.That(retrievedCampaign != null);
            Assert.That(newCampaign.CampaignCode == retrievedCampaign.CampaignCode);
        }

        [Test]
        public async Task Update_Updates_Existing_Campaign()
        {
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
            Assert.That(updatedCampaign.RequiresApproval == retrievedCampaign.RequiresApproval);
        }

        // Add more test methods as needed to cover other functionalities
    }
}