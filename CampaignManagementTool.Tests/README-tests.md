Tests

This project is in place to run all tests for the solution.
The Unit tests are done using the NUnit library.

Implementing Test Functions.
To keep the cost reduced for making calls to the database, we have setup a file in the Server project "Repositories/MockCampaignRepository". Once the function has been implemented on the production/dev environment and you wish to test it. The same logic must be implemented here, with the difference being that there are no calls to the database. All of the functions will operate on a set of test data using the same logic as the data from the database.
