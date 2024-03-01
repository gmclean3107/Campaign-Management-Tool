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
    public class AddTests
    {
        private MockCampaignRepository _campaignRepository;

        [SetUp]
        public void Setup()
        {
            _campaignRepository = new MockCampaignRepository();
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

    }
}
