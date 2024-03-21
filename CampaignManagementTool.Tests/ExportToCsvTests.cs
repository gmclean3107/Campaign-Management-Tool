using CampaignManagementTool.Server.Repositories;
using CampaignManagementTool.Shared;
using System.Text;

namespace CampaignManagementTool.Tests
{
    /// <summary>
    /// Test class for validating the export functionality of the MockCampaignRepository.
    /// </summary>
    [TestFixture]
    public class ExportToCsvTests
    {
        private MockCampaignRepository _campaignRepository;

        [SetUp]
        public void Setup()
        {
            _campaignRepository = new MockCampaignRepository();
        }

        /// <summary>
        /// Verifies that exporting all campaigns successfully exports all campaigns.
        /// </summary>
        [Test]
        public async Task Export_All_Exports_All_Campaigns()
        {

            var result = await _campaignRepository.ExportToCsv();

            List<Campaign> expectedData = _campaignRepository.GetAll().Result;
            List<Campaign> actualData = ReadExportedData(result);
            Assert.That(actualData.Count == 20);

            for (int i = 0; i < 20; i++)
            {
                string expectedString = $"{expectedData[i].CampaignCode},{expectedData[i].AffiliateCode},{expectedData[i].RequiresApproval},{expectedData[i].Rules},{expectedData[i].RulesUrl},{expectedData[i].ProducerCode},{expectedData[i].ExpiryDays},{expectedData[i].isDeleted}";
                string actualString = $"{actualData[i].CampaignCode},{actualData[i].AffiliateCode},{actualData[i].RequiresApproval},{actualData[i].Rules},{actualData[i].RulesUrl},{actualData[i].ProducerCode},{actualData[i].ExpiryDays},{actualData[i].isDeleted}";
                Console.WriteLine($"{expectedString}\n{actualString}");
                Assert.That(expectedString.Equals(actualString));
            }

        }

        /// <summary>
        /// Verifies that exporting filtered campaigns successfully exports filtered campaigns.
        /// </summary>
        [Test]
        public async Task Export_Filtered_Exports_Filtered_Campaigns()
        {
            Console.WriteLine("Testing that exporting filtered data works correctly");
            var result = await _campaignRepository.ExportToCsvFiltered("", 1, 0);

            List<Campaign> expectedData = await _campaignRepository.CampaignSearchFilter("", 1, 0);
            List<Campaign> actualData = ReadExportedData(result);
            Assert.That(actualData.Count == 10);

            for (int i = 0; i < 10; i++)
            {
                string expectedString = $"{expectedData[i].CampaignCode},{expectedData[i].AffiliateCode},{expectedData[i].RequiresApproval},{expectedData[i].Rules},{expectedData[i].RulesUrl},{expectedData[i].ProducerCode},{expectedData[i].ExpiryDays},{expectedData[i].isDeleted}";
                string actualString = $"{actualData[i].CampaignCode},{actualData[i].AffiliateCode},{actualData[i].RequiresApproval},{actualData[i].Rules},{actualData[i].RulesUrl},{actualData[i].ProducerCode},{actualData[i].ExpiryDays},{actualData[i].isDeleted}";
                Assert.That(expectedString.Equals(actualString));
            }
        }

        /// <summary>
        /// Verifies that exporting a single campaign successfully exports the selected campaign.
        /// </summary>
        [Test]
        public async Task Export_Single_Exports_Selected_Campaigns()
        {
            Console.WriteLine("Testing that exporting a single campaign works correctly");
            var result = await _campaignRepository.ExportToCsvSingle("camp004");

            Campaign expectedData = await _campaignRepository.GetById("camp004");
            List<Campaign> actualData = ReadExportedData(result);
            Assert.That(actualData.Count == 1);

            string expectedString = $"{expectedData.CampaignCode},{expectedData.AffiliateCode},{expectedData.RequiresApproval},{expectedData.Rules},{expectedData.RulesUrl},{expectedData.ProducerCode},{expectedData.ExpiryDays},{expectedData.isDeleted}";
            string actualString = $"{actualData[0].CampaignCode},{actualData[0].AffiliateCode},{actualData[0].RequiresApproval},{actualData[0].Rules},{actualData[0].RulesUrl},{actualData[0].ProducerCode},{actualData[0].ExpiryDays},{actualData[0].isDeleted}";
            Assert.That(expectedString.Equals(actualString));
        }

        /// <summary>
        /// Helper method to parse the exported CSV data into a list of Campaign objects.
        /// </summary>
        /// <param name="csvBytes">The byte array containing the CSV data.</param>
        /// <returns>A list of Campaign objects parsed from the CSV data.</returns>
        private List<Campaign> ReadExportedData(byte[] csvBytes)
        {
            string csvString = Encoding.UTF8.GetString(csvBytes);
            List<Campaign> campaigns = new List<Campaign>();

            string[] lines = csvString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length < 2)
            {
                return campaigns;
            }

            string[] headers = lines[0].Split(',');

            for (int i = 1; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(',');
                if (fields.Length != headers.Length)
                {
                    continue;
                }

                Campaign campaign = new Campaign();

                for (int j = 0; j < headers.Length; j++)
                {
                    switch (j)
                    {                       
                        case 0:
                            campaign.CampaignCode = fields[j];
                            break;
                        case 1:
                            campaign.AffiliateCode = fields[j];
                            break;
                        case 2:
                            campaign.RequiresApproval = bool.Parse(fields[j]);
                            break;
                        case 3:
                            campaign.Rules = fields[j];
                            break;
                        case 4:
                            campaign.RulesUrl = fields[j];
                            break;
                        case 5:
                            campaign.ProducerCode = fields[j];
                            break;
                        case 6:
                            campaign.ExpiryDays = fields[j];
                            break;
                        case 7:
                            campaign.isDeleted = bool.Parse(fields[j]);
                            break;
                        default:
                            break;
                    }
                }

                campaigns.Add(campaign);
            }

            return campaigns;
        }
    }
}
