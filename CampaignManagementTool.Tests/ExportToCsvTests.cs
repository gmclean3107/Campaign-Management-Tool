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


        [Test]
        public async Task Export_Filtered_Exports_Filtered_Campaigns()
        {
            Console.WriteLine("Testing that exporting filtered data works correctly");
            var result = await _campaignRepository.ExportToCsvFiltered("",1,0);

            string exportFilePath = "CsvExports/FilteredCampaigns.csv";
            bool fileExists = File.Exists(exportFilePath);

            Assert.That(fileExists, Is.True);


            List<Campaign> expectedData = await _campaignRepository.CampaignSearchFilter("",1,0);
            List<Campaign> actualData = ReadExportedData(exportFilePath);
            Assert.That(actualData.Count == 10);

            for (int i = 0; i < 10; i++)
            {
                string expectedString = $"{expectedData[i].CampaignCode},{expectedData[i].AffiliateCode},{expectedData[i].RequiresApproval},{expectedData[i].Rules},{expectedData[i].RulesUrl},{expectedData[i].ProducerCode},{expectedData[i].ExpiryDays},{expectedData[i].isDeleted}";
                string actualString = $"{actualData[i].CampaignCode},{actualData[i].AffiliateCode},{actualData[i].RequiresApproval},{actualData[i].Rules},{actualData[i].RulesUrl},{actualData[i].ProducerCode},{actualData[i].ExpiryDays},{actualData[i].isDeleted}";
                Assert.That(expectedString.Equals(actualString));
            }
        }


        [Test]
        public async Task Export_Single_Exports_Selected_Campaigns()
        {
            Console.WriteLine("Testing that exporting a single campaign works correctly");
            var result = await _campaignRepository.ExportToCsvSingle("camp004");

            string exportFilePath = "CsvExports/SingleCampaign.csv";
            bool fileExists = File.Exists(exportFilePath);

            Assert.That(fileExists, Is.True);


            Campaign expectedData = await _campaignRepository.GetById("camp004");
            List<Campaign> actualData = ReadExportedData(exportFilePath);
            Assert.That(actualData.Count == 1);

            string expectedString = $"{expectedData.CampaignCode},{expectedData.AffiliateCode},{expectedData.RequiresApproval},{expectedData.Rules},{expectedData.RulesUrl},{expectedData.ProducerCode},{expectedData.ExpiryDays},{expectedData.isDeleted}";
            string actualString = $"{actualData[0].CampaignCode},{actualData[0].AffiliateCode},{actualData[0].RequiresApproval},{actualData[0].Rules},{actualData[0].RulesUrl},{actualData[0].ProducerCode},{actualData[0].ExpiryDays},{actualData[0].isDeleted}";
            Assert.That(expectedString.Equals(actualString));
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
