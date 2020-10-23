using CampgroundReservations.DAO;
using CampgroundReservations.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CampgroundReservations.Tests.DAO
{
    [TestClass]
    public class ReservationSqlDAOTests : BaseDAOTests
    {
        [TestMethod]
        public void CreateReservation_Should_ReturnNewReservationId()
        {
            // Arrange
            ReservationSqlDAO dao = new ReservationSqlDAO(ConnectionString);

            // Act
            int returnedId = dao.CreateReservation(SiteId, "Test Name", DateTime.Now.AddDays(1), DateTime.Now.AddDays(3));

            // Assert
            Assert.AreEqual(ReservationId + 1, returnedId);
        }
        
        [TestMethod]
        public void GetReservationsWithin30DaysSelectedPark()
        {
            //ARRANGE
            ReservationSqlDAO dao = new ReservationSqlDAO(ConnectionString);
           
            //ACT
            List<Reservation> result = (List<Reservation>)dao.GetReservationsWithin30DaysSelectedPark(ParkId.ToString());
       
            //ASSERT 
            Assert.AreEqual(2, result.Count);

        }

    }
}
