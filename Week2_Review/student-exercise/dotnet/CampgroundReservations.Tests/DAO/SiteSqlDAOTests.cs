using CampgroundReservations.DAO;
using CampgroundReservations.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CampgroundReservations.Tests.DAO
{
    [TestClass]
    public class SiteSqlDAOTests : BaseDAOTests
    {
        [TestMethod]
        public void GetSitesThatAllowRVs_Should_ReturnSitesTest()
        {
            // Arrange
            SiteSqlDAO dao = new SiteSqlDAO(ConnectionString);

            // Act
            IList<Site> sites = dao.GetSitesThatAllowRVs(ParkId);

            // Assert
            Assert.AreEqual(2, sites.Count);
        }

        [TestMethod]
        public void GetSitesWithoutReservationsTest()
        {
            // Arrange
            SiteSqlDAO dao = new SiteSqlDAO(ConnectionString);

            // Act
            IList<Site> sites = dao.GetSitesWithoutReservations(ParkId);

            // Assert
            Assert.AreEqual(2, sites.Count);
        }
       
        [TestMethod]
        public void GetSitesForFutureReservationsTest()
        {
            // Arrange
            SiteSqlDAO dao = new SiteSqlDAO(ConnectionString);

            // Act
            IList<Site> sites = dao.GetSitesForFutureReservations(ParkId, DateTime.Now.AddDays(3), DateTime.Now.AddDays(5));

            // Assert
            Assert.AreEqual(2, sites.Count);
        }
    }
}
