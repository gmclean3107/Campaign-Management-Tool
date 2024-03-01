using CampaignManagementTool.Server.Repositories;
using CampaignManagementTool.Shared;
using CsvHelper;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampaignManagementTool.Tests
{
    [TestFixture]
    public class ExportToCsvTests
    {
        private MockCampaignRepository _campaignRepository;

        [SetUp]
        public void Setup()
        {
            _campaignRepository = new MockCampaignRepository();
        }


        [Test]
        public async Task Export_All_Exports_All_Campaigns()
        {
            
            var result = await _campaignRepository.ExportToCsv();
            Assert.That(result.Count == 20);

            string exportFilePath = "CsvExports/AllCampaigns.csv";
            bool fileExists = File.Exists(exportFilePath);

            Assert.That(fileExists, Is.True);
            

            List<Campaign> expectedData = _campaignRepository.GetAll().Result;
            List<Campaign> actualData = ReadExportedData(exportFilePath);
            Assert.That(actualData.Count == 20);
            
            for (int i = 0; i < 20; i++) 
            {
                string expectedString = $"{expectedData[i].CampaignCode},{expectedData[i].AffiliateCode},{expectedData[i].RequiresApproval},{expectedData[i].Rules},{expectedData[i].RulesUrl},{expectedData[i].ProducerCode},{expectedData[i].ExpiryDays},{expectedData[i].isDeleted}";
                string actualString = $"{actualData[i].CampaignCode},{actualData[i].AffiliateCode},{actualData[i].RequiresApproval},{actualData[i].Rules},{actualData[i].RulesUrl},{actualData[i].ProducerCode},{actualData[i].ExpiryDays},{actualData[i].isDeleted}";
                Assert.That(expectedString.Equals(actualString));
            }

        }
        
        
        private List<Campaign> ReadExportedData(string filePath)
        {
            List<Campaign> data = new List<Campaign>();

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                data = csv.GetRecords<Campaign>().ToList();
            }

            return data;
        }
    }
}
