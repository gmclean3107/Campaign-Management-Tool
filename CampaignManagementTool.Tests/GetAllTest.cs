using CampaignManagementTool.Server.Repositories;

namespace CampaignManagementTool.Tests
{
    /// <summary>
    /// Test class for validating the GetAll method of the MockCampaignRepository.
    /// </summary>
    [TestFixture]
    public class GetAllTest
    { 
        private MockCampaignRepository _campaignRepository;

        [SetUp]
        public void Setup() 
        {
            _campaignRepository = new MockCampaignRepository();
        }

        /// <summary>
        /// Verifies that the GetAll method returns all campaigns.
        /// </summary>
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