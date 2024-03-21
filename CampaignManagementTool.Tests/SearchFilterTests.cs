using CampaignManagementTool.Server.Repositories;

namespace CampaignManagementTool.Tests
{
    /// <summary>
    /// Test class for validating the search, filtering, and sorting functionality of the MockCampaignRepository.
    /// </summary>
    [TestFixture]
    public class SearchFilterTests
    {
        private MockCampaignRepository _campaignRepository;

        [SetUp]
        public void Setup()
        {
            _campaignRepository = new MockCampaignRepository();
        }

        /// <summary>
        /// Verifies that searching for campaigns by campaign code returns the expected campaign.
        /// </summary>
        /// <param name="code">The campaign code to search for.</param>
        [TestCase("camp001")]
        public async Task SearchFilter_Search_Campaigns(string code)
        {
            Console.WriteLine("Testing Search Function");
            var result = await _campaignRepository.CampaignSearchFilter(code, 0, 0);
            Assert.That(result.Count == 1);
            Assert.That(result[0].AffiliateCode == "aff001");
        }

        /// <summary>
        /// Verifies that filtering campaigns by different criteria returns the expected results.
        /// </summary>
        /// <param name="filter">The filter criteria to apply.</param>
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

        /// <summary>
        /// Verifies that sorting campaigns by different criteria returns the expected results.
        /// </summary>
        /// <param name="sort">The sort criteria to apply.</param>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task SearchFilter_Sort_Campaigns(int sort)
        {
            var result = await _campaignRepository.CampaignSearchFilter("", 0, sort);

            if (sort == 1)
            {
                Console.WriteLine("Testing Alphabetical Order Ascending");
                Assert.That(result.Count == 20);
                Assert.That(result[0].CampaignCode == "camp001" && result[19].CampaignCode == "camp020");
            }

            if (sort == 2)
            {
                Console.WriteLine("Testing Alphabetical Order Descending");
                Assert.That(result.Count == 20);
                Assert.That(result[0].CampaignCode == "camp020" && result[19].CampaignCode == "camp001");
            }

            if (sort == 3)
            {
                Console.WriteLine("Testing Expiry Date Ascending");
                Assert.That(result.Count == 20);
                Assert.That(result[0].CampaignCode == "camp008" && result[19].CampaignCode == "camp020");
            }

            if (sort == 4)
            {
                Console.WriteLine("Testing Expiry Date Descending");
                Assert.That(result.Count == 20);
                Assert.That(result[0].CampaignCode == "camp020" && result[19].CampaignCode == "camp008");
            }
        }

        /// <summary>
        /// Verifies that filtering and sorting campaigns together returns the expected results.
        /// </summary>
        /// <param name="filter">The filter criteria to apply.</param>
        /// <param name="sort">The sort criteria to apply.</param>
        [TestCase(1, 1)]
        [TestCase(1, 2)]
        [TestCase(1, 3)]
        [TestCase(1, 4)]
        [TestCase(2, 1)]
        [TestCase(2, 2)]
        [TestCase(2, 3)]
        [TestCase(2, 4)]
        [TestCase(3, 1)]
        [TestCase(3, 2)]
        [TestCase(3, 3)]
        [TestCase(3, 4)]
        [TestCase(4, 1)]
        [TestCase(4, 2)]
        [TestCase(4, 3)]
        [TestCase(4, 4)]
        public async Task SearchFilter_Sort_And_Filter_Campaigns(int filter, int sort)
        {
            var result = await _campaignRepository.CampaignSearchFilter("", filter, sort);

            if (sort == 1)
            {
                switch (filter)
                {
                    case 1:
                        Assert.That(result.Count == 10);
                        Assert.That(result[1].CampaignCode == "camp003" && result[0].AffiliateCode == "aff001");
                        break;
                    case 2:
                        Assert.That(result.Count == 10);
                        Assert.That(result[1].CampaignCode == "camp004" && result[0].AffiliateCode == "aff002");
                        break;
                    case 3:
                        Assert.That(result.Count == 17);
                        Assert.That(result[16].CampaignCode == "camp020" && result[0].AffiliateCode == "aff001");
                        break;
                    case 4:
                        Assert.That(result.Count == 3);
                        Assert.That(result[1].CampaignCode == "camp007" && result[0].AffiliateCode == "aff002");
                        break;
                    default:
                        break;
                }
            }
            else if (sort == 2)
            {
                switch (filter)
                {
                    case 1:
                        Assert.That(result.Count == 10);
                        Assert.That(result[8].CampaignCode == "camp003" && result[9].AffiliateCode == "aff001");

                        break;
                    case 2:
                        Assert.That(result.Count == 10);
                        Assert.That(result[8].CampaignCode == "camp004" && result[9].AffiliateCode == "aff002");

                        break;
                    case 3:
                        Assert.That(result.Count == 17);
                        Assert.That(result[0].CampaignCode == "camp020" && result[16].AffiliateCode == "aff001");

                        break;
                    case 4:
                        Assert.That(result.Count == 3);
                        Assert.That(result[1].CampaignCode == "camp007" && result[2].AffiliateCode == "aff002");
                        break;
                    default:
                        break;
                }
            }
            else if (sort == 3)
            {
                switch (filter)
                {
                    case 1:
                        Assert.That(result.Count == 10);
                        Assert.That(result[8].CampaignCode == "camp017" && result[9].AffiliateCode == "aff019");

                        break;
                    case 2:
                        Assert.That(result.Count == 10);
                        Assert.That(result[8].CampaignCode == "camp018" && result[9].AffiliateCode == "aff020");

                        break;
                    case 3:
                        Assert.That(result.Count == 17);
                        Assert.That(result[0].CampaignCode == "camp001" && result[16].AffiliateCode == "aff020");

                        break;
                    case 4:
                        Assert.That(result.Count == 3);
                        Assert.That(result[1].CampaignCode == "camp007" && result[2].AffiliateCode == "aff002");
                        break;
                    default:
                        break;
                }
            }
            else if (sort == 4)
            {
                switch (filter)
                {
                    case 1:
                        Assert.That(result.Count == 10);
                        Assert.That(result[8].CampaignCode == "camp001" && result[9].AffiliateCode == "aff007");

                        break;
                    case 2:
                        Assert.That(result.Count == 10);
                        Assert.That(result[8].CampaignCode == "camp002" && result[9].AffiliateCode == "aff008");

                        break;
                    case 3:
                        Assert.That(result.Count == 17);
                        Assert.That(result[0].CampaignCode == "camp020" && result[16].AffiliateCode == "aff001");

                        break;
                    case 4:
                        Assert.That(result.Count == 3);
                        Assert.That(result[1].CampaignCode == "camp007" && result[2].AffiliateCode == "aff008");
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Verifies that searching and filtering campaigns together returns the expected results.
        /// </summary>
        /// <param name="filter">The filter criteria to apply.</param>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public async Task SearchFilter_Search_And_Filter_Campaigns(int filter)
        {
            string searchString = "camp01";
            var result = await _campaignRepository.CampaignSearchFilter(searchString, filter, 0);

            switch (filter)
            {
                case 1:
                    Assert.That(result.Count == 5);
                    Assert.That(result[0].CampaignCode == "camp011" && result[4].AffiliateCode == "aff019");
                    break;
                case 2:
                    Assert.That(result.Count == 5);
                    Assert.That(result[0].CampaignCode == "camp010" && result[4].AffiliateCode == "aff018");
                    break;
                case 3:
                    Assert.That(result.Count == 10);
                    Assert.That(result[0].CampaignCode == "camp010" && result[9].AffiliateCode == "aff019");
                    break;
                case 4:
                    Assert.That(result.Count == 0);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Verifies that searching, filtering, and sorting campaigns together returns the expected results.
        /// </summary>
        /// <param name="filter">The filter criteria to apply.</param>
        /// <param name="sort">The sort criteria to apply.</param>
        [TestCase(1, 1)]
        [TestCase(1, 2)]
        [TestCase(1, 3)]
        [TestCase(1, 4)]
        [TestCase(2, 1)]
        [TestCase(2, 2)]
        [TestCase(2, 3)]
        [TestCase(2, 4)]
        [TestCase(3, 1)]
        [TestCase(3, 2)]
        [TestCase(3, 3)]
        [TestCase(3, 4)]
        [TestCase(4, 1)]
        [TestCase(4, 2)]
        [TestCase(4, 3)]
        [TestCase(4, 4)]
        public async Task SearchFilter_Search_Filter_And_Sort_Campaigns(int filter, int sort)
        {
            string searchString = "camp01";

            var result = await _campaignRepository.CampaignSearchFilter(searchString, filter, sort);

            if (sort == 1)
            {
                switch (filter)
                {
                    case 1:
                        Assert.That(result.Count == 5);
                        Assert.That(result[0].CampaignCode == "camp011" && result[4].AffiliateCode == "aff019");
                        break;
                    case 2:
                        Assert.That(result.Count == 5);
                        Assert.That(result[0].CampaignCode == "camp010" && result[4].AffiliateCode == "aff018");
                        break;
                    case 3:
                        Assert.That(result.Count == 10);
                        Assert.That(result[0].CampaignCode == "camp010" && result[9].AffiliateCode == "aff019");
                        break;
                    case 4:
                        Assert.That(result.Count == 0);
                        break;
                    default:
                        break;
                }
            }
            else if (sort == 2)
            {
                switch (filter)
                {
                    case 1:
                        Assert.That(result.Count == 5);
                        Assert.That(result[0].CampaignCode == "camp019" && result[4].AffiliateCode == "aff011");
                        break;
                    case 2:
                        Assert.That(result.Count == 5);
                        Assert.That(result[0].CampaignCode == "camp018" && result[4].AffiliateCode == "aff010");
                        break;
                    case 3:
                        Assert.That(result.Count == 10);
                        Assert.That(result[0].CampaignCode == "camp019" && result[9].AffiliateCode == "aff010");
                        break;
                    case 4:
                        Assert.That(result.Count == 0);
                        break;
                    default:
                        break;
                }
            }
            else if (sort == 3)
            {
                switch (filter)
                {
                    case 1:
                        Assert.That(result.Count == 5);
                        Assert.That(result[0].CampaignCode == "camp011" && result[4].AffiliateCode == "aff019");
                        break;
                    case 2:
                        Assert.That(result.Count == 5);
                        Assert.That(result[0].CampaignCode == "camp010" && result[4].AffiliateCode == "aff018");
                        break;
                    case 3:
                        Assert.That(result.Count == 10);
                        Assert.That(result[0].CampaignCode == "camp010" && result[9].AffiliateCode == "aff019");
                        break;
                    case 4:
                        Assert.That(result.Count == 0);
                        break;
                    default:
                        break;
                }
            }
            else if (sort == 4)
            {
                switch (filter)
                {
                    case 1:
                        Assert.That(result.Count == 5);
                        Assert.That(result[0].CampaignCode == "camp019" && result[4].AffiliateCode == "aff011");
                        break;
                    case 2:
                        Assert.That(result.Count == 5);
                        Assert.That(result[0].CampaignCode == "camp018" && result[4].AffiliateCode == "aff010");
                        break;
                    case 3:
                        Assert.That(result.Count == 10);
                        Assert.That(result[0].CampaignCode == "camp019" && result[9].AffiliateCode == "aff010");
                        break;
                    case 4:
                        Assert.That(result.Count == 0);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Verifies that searching for campaigns with an invalid search string returns no results.
        /// </summary>
        /// <param name="filter">The filter criteria to apply.</param>
        /// <param name="sort">The sort criteria to apply.</param>
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(1, 2)]
        [TestCase(1, 3)]
        [TestCase(1, 4)]
        [TestCase(2, 1)]
        [TestCase(2, 2)]
        [TestCase(2, 3)]
        [TestCase(2, 4)]
        [TestCase(3, 1)]
        [TestCase(3, 2)]
        [TestCase(3, 3)]
        [TestCase(3, 4)]
        [TestCase(4, 1)]
        [TestCase(4, 2)]
        [TestCase(4, 3)]
        [TestCase(4, 4)]
        public async Task SearchFilter_Invalid_Search(int filter, int sort) 
        {
            string invalidSearch = "invalid search string";

            var result = await _campaignRepository.CampaignSearchFilter(invalidSearch, filter, sort);

            Assert.That(result.Count == 0);
        }
    }
}
