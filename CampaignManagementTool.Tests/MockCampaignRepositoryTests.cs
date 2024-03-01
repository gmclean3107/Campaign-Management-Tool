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
            Console.WriteLine("Testing GetAll Function");
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
            Console.WriteLine("Testing Add Function");
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

        [TestCase("camp001")]
        public async Task SearchFilter_Search_Campaigns(string code)
        {
            Console.WriteLine("Testing Search Function");
            var result = await _campaignRepository.CampaignSearchFilter(code, 0, 0);
            Assert.That(result.Count == 1);
            Assert.That(result[0].AffiliateCode == "aff001");
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task SearchFilter_Filters_Campaigns(int filter)
        {
            var result = await _campaignRepository.CampaignSearchFilter("", filter, 0);

            if (filter == 1)
            {
                Console.WriteLine("Testing Approval Required Filter");
                Assert.That(result.Count == 10);
                Assert.That(result[1].CampaignCode == "camp003" && result[0].AffiliateCode == "aff001");
            }

            if (filter == 2) 
            {
                Console.WriteLine("Testing Approval not Required Filter");
                Assert.That(result.Count == 10);
                Assert.That(result[1].CampaignCode == "camp004" && result[0].AffiliateCode == "aff002");
            }

            if (filter == 3) 
            {
                Console.WriteLine("Testing Active Filter");
                Assert.That(result.Count == 17);
                Assert.That(result[16].CampaignCode == "camp020" && result[0].AffiliateCode == "aff001");
            }

            if (filter == 4) 
            {
                Console.WriteLine("Testing Deleted Filter");
                Assert.That(result.Count == 3);
                Assert.That(result[1].CampaignCode == "camp007" && result[0].AffiliateCode == "aff002");
            }

        }
        // Add more test methods as needed to cover other functionalities
    }
}