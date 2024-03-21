using CampaignManagementTool.Server.Repositories;
using CampaignManagementTool.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampaignManagementTool.Tests
{
    /// <summary>
    /// Test class for validating the update functionality of the MockCampaignRepository.
    /// </summary>
    [TestFixture]
    public class UpdateTests
    {
        private MockCampaignRepository _campaignRepository;

        [SetUp]
        public void Setup()
        {
            _campaignRepository = new MockCampaignRepository();
        }

        /// <summary>
        /// Verifies that updating an existing campaign modifies the campaign as expected.
        /// </summary>
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

        /// <summary>
        /// Verifies that attempting to update a campaign with invalid rules throws an InvalidOperationException.
        /// </summary>
        [Test]
        public async Task Update_InvalidRules_ThrowsInvalidOperationException()
        {
            var existingCampaign = (await _campaignRepository.GetAll()).First();
            var invalidCampaign = new Campaign
            {
                CampaignCode = "INVALIDCODE",
                AffiliateCode = "INVALIDAFF",
                RequiresApproval = false,
                Rules = "New rules",
                RulesUrl = "",
                ProducerCode = "",
                ExpiryDays = DateTime.UtcNow.ToString(),
                isDeleted = false
            };

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _campaignRepository.Update(existingCampaign.CampaignCode, invalidCampaign));
        }

        /// <summary>
        /// Verifies that attempting to update a campaign with an invalid rules URL throws an InvalidOperationException.
        /// </summary>
        [Test]
        public async Task Update_InvalidRulesUrl_ThrowsInvalidOperationException()
        {
            var existingCampaign = (await _campaignRepository.GetAll()).First();
            var invalidCampaign = new Campaign
            {
                CampaignCode = "INVALIDCODE",
                AffiliateCode = "INVALIDAFF",
                RequiresApproval = false,
                Rules = "",
                RulesUrl = "www.google.com",
                ProducerCode = "",
                ExpiryDays = DateTime.UtcNow.ToString(),
                isDeleted = false
            };

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _campaignRepository.Update(existingCampaign.CampaignCode, invalidCampaign));
        }

        /// <summary>
        /// Verifies that attempting to update a campaign with an invalid campaign code throws an InvalidOperationException.
        /// </summary>
        [Test]
        public async Task Update_InvalidCampaignCode_ThrowsInvalidOperationException()
        {
            var existingCampaign = (await _campaignRepository.GetAll()).First();
            var invalidCampaign = new Campaign
            {
                CampaignCode = "",
                AffiliateCode = "INVALIDAFF",
                RequiresApproval = false,
                Rules = "",
                RulesUrl = "",
                ProducerCode = "",
                ExpiryDays = DateTime.UtcNow.ToString(),
                isDeleted = false
            };

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _campaignRepository.Update(existingCampaign.CampaignCode, invalidCampaign));
        }

        /// <summary>
        /// Verifies that attempting to update a campaign with an invalid affiliate code throws an InvalidOperationException.
        /// </summary>
        [Test]
        public async Task Update_InvalidAffiliateCode_ThrowsInvalidOperationException()
        {
            var existingCampaign = (await _campaignRepository.GetAll()).First();
            var invalidCampaign = new Campaign
            {
                CampaignCode = "INVALIDCODE",
                AffiliateCode = "",
                RequiresApproval = false,
                Rules = "",
                RulesUrl = "",
                ProducerCode = "",
                ExpiryDays = DateTime.UtcNow.ToString(),
                isDeleted = false
            };

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _campaignRepository.Update(existingCampaign.CampaignCode, invalidCampaign));
        }

        /// <summary>
        /// Verifies that attempting to update a campaign with an invalid producer code throws an InvalidOperationException.
        /// </summary>
        [Test]
        public async Task Update_InvalidProducerCode_ThrowsInvalidOperationException()
        {
            var existingCampaign = (await _campaignRepository.GetAll()).First();
            var invalidCampaign = new Campaign
            {
                CampaignCode = "INVALIDCODE",
                AffiliateCode = "INVALIDAFF",
                RequiresApproval = false,
                Rules = "",
                RulesUrl = "",
                ProducerCode = GenerateString(1001),
                ExpiryDays = DateTime.UtcNow.ToString(),
                isDeleted = false
            };

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _campaignRepository.Update(existingCampaign.CampaignCode, invalidCampaign));
        }

        /// <summary>
        /// Verifies that attempting to update a campaign with an invalid expiry date throws an InvalidOperationException.
        /// </summary>
        [Test]
        public async Task Update_InvalidExpiryDate_ThrowsInvalidOperationException()
        {
            var existingCampaign = (await _campaignRepository.GetAll()).First();
            var invalidCampaign = new Campaign
            {
                CampaignCode = "INVALIDCODE",
                AffiliateCode = "INVALIDAFF",
                RequiresApproval = false,
                Rules = GenerateString(1000),
                RulesUrl = "www.example.com",
                ProducerCode = GenerateString(1000),
                ExpiryDays = "",
                isDeleted = false
            };

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _campaignRepository.Update(existingCampaign.CampaignCode, invalidCampaign));
        }

        /// <summary>
        /// Generates a string of a specified length.
        /// </summary>
        /// <param name="length">The length of the string to generate.</param>
        /// <returns>A string of the specified length.</returns>
        private string GenerateString(int length)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append('a');
            }
            return sb.ToString();
        }

    }
}
