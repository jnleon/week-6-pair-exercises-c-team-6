﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Data.SqlClient;

namespace ProjectOrganizerTests
{
    [TestClass]
    public class EmployeeTest : EmployeeDB_DAOTests
    {
        //GetAllEmployees
        //Search
        //GetEmployeesWithoutProjects

        [TestMethod]
        public void TestGetAllEmployees()
        {
            // Arrange
            EmployeeSqlDAO dao = new EmployeeSqlDAO(ConnectionString);

            //ACT
            IList<Employee> result = dao.GetAllEmployees();

            //ASERT
            Assert.AreEqual(GetRowCount("employee"), result.Count);
        }

        [TestMethod]
        public void TestSearch()
        {
            EmployeeSqlDAO dao = new EmployeeSqlDAO(ConnectionString);

            IList<Employee> result = dao.Search("pe", "pe");

            Assert.AreEqual(NewEmployeeId, result[0].EmployeeId);
        }

        [TestMethod]
        public void TestGetEmployeesWithoutProjects()
        {
            EmployeeSqlDAO dao = new EmployeeSqlDAO(ConnectionString);

            IList<Employee> result = dao.GetEmployeesWithoutProjects();

            Assert.AreEqual(NewEmployeeId2, result[0].EmployeeId);
        }        
    }
}
