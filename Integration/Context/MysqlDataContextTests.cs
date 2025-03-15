using FmsWebScrapingApi.Data.Context;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Context
{
    [TestClass]
    public class MysqlDataContextTests
    {
        private MysqlDataContext _context;

        [TestInitialize]
        public void Setup()
        {
            _context = new MysqlDataContext();
        }

        [TestMethod]
        public void Should_Open_And_Close_Connection_Successfully()
        {
            // Arrange & Act
            MySqlConnection connection = _context.Connection;

            // Assert
            Assert.IsNotNull(connection, "A conexão não deve ser nula.");
            Assert.AreEqual(System.Data.ConnectionState.Open, connection.State, "A conexão deve estar aberta.");
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Dispose();
        }
    }
}
