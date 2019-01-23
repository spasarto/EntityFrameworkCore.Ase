using EntityFrameworkCore.Ase.Tests.Infastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EntityFrameworkCore.Ase.Tests
{
    /// <summary>
    /// These tests are simple sanity tests to ensure the ase provider is functioning correctly.
    /// If these don't work, the rest won't either.
    /// </summary>
    [TestClass]
    public class ProviderTests
    {
        private static AseOptions _options;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestConfiguration.Initialize();
            _options = TestConfiguration.GetOptions<AseOptions>().Value;
        }

        [TestMethod]
        public void EstablishConnection()
        {
            var connection = new AdoNetCore.AseClient.AseConnection(_options.ConnectionString);
            connection.Open();

            Assert.AreEqual(System.Data.ConnectionState.Open, connection.State);
        }
    }
}
