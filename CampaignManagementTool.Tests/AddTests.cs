﻿using CampaignManagementTool.Server.Repositories;
using CampaignManagementTool.Shared;
using System.Text;

namespace CampaignManagementTool.Tests
{

    /// <summary>
    /// Test class for validating the Add method of MockCampaignRepository.
    /// </summary>
    [TestFixture]
    public class AddTests
    {
        private MockCampaignRepository _campaignRepository;

        [SetUp]
        public void Setup()
        {
            _campaignRepository = new MockCampaignRepository();
        }

        /// <summary>
        /// Verifies that the Add method successfully adds a new campaign to the repository.
        /// </summary>
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
                ProducerCode = "",
                ExpiryDays = DateTime.UtcNow.ToString(),
                isDeleted = false
            };

            await _campaignRepository.Add(newCampaign);
            var retrievedCampaign = await _campaignRepository.GetById("NEWCAMPAIGN");

            Assert.That(retrievedCampaign != null);
            Assert.That(newCampaign.CampaignCode == retrievedCampaign.CampaignCode);
        }

        /// <summary>
        /// Verifies that adding a campaign with invalid rules throws an InvalidOperationException.
        /// </summary>
        [Test]
        public void Add_InvalidRules_ThrowsInvalidOperationException()
        {
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

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _campaignRepository.Add(invalidCampaign));
        }

        /// <summary>
        /// Verifies that adding a campaign with invalid rules URL throws an InvalidOperationException.
        /// </summary>
        [Test]
        public void Add_InvalidRulesUrl_ThrowsInvalidOperationException()
        {
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

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _campaignRepository.Add(invalidCampaign));
        }

        /// <summary>
        /// Verifies that adding a campaign with an invalid campaign code throws an InvalidOperationException.
        /// </summary>
        [Test]
        public void Add_InvalidCampaignCode_ThrowsInvalidOperationException()
        {
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

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _campaignRepository.Add(invalidCampaign));
        }

        /// <summary>
        /// Verifies that adding a campaign with an invalid affiliate code throws an InvalidOperationException.
        /// </summary>
        [Test]
        public void Add_InvalidAffiliateCode_ThrowsInvalidOperationException()
        {
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

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _campaignRepository.Add(invalidCampaign));
        }

        /// <summary>
        /// Verifies that adding a campaign with an invalid producer code throws an InvalidOperationException.
        /// </summary>
        [Test]
        public void Add_InvalidProducerCode_ThrowsInvalidOperationException()
        {
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

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _campaignRepository.Add(invalidCampaign));
        }

        /// <summary>
        /// Verifies that adding a campaign with an invalid expiry date throws an InvalidOperationException.
        /// </summary>
        [Test]
        public void Add_InvalidExpiryDate_ThrowsInvalidOperationException()
        {
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

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _campaignRepository.Add(invalidCampaign));
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
