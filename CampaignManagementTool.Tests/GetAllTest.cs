using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampaignManagementTool.Server.Repositories;
using CampaignManagementTool.Shared;

namespace CampaignManagementTool.Tests
{
    [TestFixture]
    public class GetAllTest
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

    }
}