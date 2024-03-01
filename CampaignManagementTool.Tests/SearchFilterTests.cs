using CampaignManagementTool.Server.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampaignManagementTool.Tests
{
    [TestFixture]
    public class SearchFilterTests
    {
        private MockCampaignRepository _campaignRepository;

        [SetUp]
        public void Setup()
        {
            _campaignRepository = new MockCampaignRepository();
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
    }
}
