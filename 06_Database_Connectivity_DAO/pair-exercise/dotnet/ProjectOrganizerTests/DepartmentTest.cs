using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;
using System.Collections.Generic;

namespace ProjectOrganizerTests
{
    [TestClass]
    public class DepartmentTest : EmployeeDB_DAOTests
    {
        //GetDepartments()
        //CreateDepartment(Department, newDepartment)
        //UpdateDepartment(Department, updatedDepartment)

        [TestMethod]
        public void GetDepartmentShouldReturnRightNumberofDepartments()
        {
            // Arrange
            DepartmentSqlDAO dao = new DepartmentSqlDAO(ConnectionString);

            //ACT
            IList<Department> deps = dao.GetDepartments();

            //ASERT
            Assert.AreEqual(GetRowCount("department"), deps.Count);
        }
      
        [TestMethod]
        public void CreateDepartmentShouldIncreaseCountBy1()
        {
            // Arrange
            DepartmentSqlDAO dao = new DepartmentSqlDAO(ConnectionString);
            int startingRowCount = GetRowCount("department");

            Department dept = new Department();
            dept.Name = "slickmahoney";

            //ACT
            dao.CreateDepartment(dept);
            int endingRowCount = GetRowCount("department");

            // Assert
            Assert.AreNotEqual(startingRowCount, endingRowCount);
        }
       
        [TestMethod]
        public void UpdateDepartmentShouldUpdate()
        {
            //Arrange
            DepartmentSqlDAO dao = new DepartmentSqlDAO(ConnectionString);
            IList<Department> depts = dao.GetDepartments();
          
            Department deptchange = depts[0];

            //Act
            deptchange.Name = "GranTurismo";
            dao.UpdateDepartment(deptchange);

            //Assert
            IList<Department> updateddept = dao.GetDepartments();
            Department fromDB = null;
            foreach (Department c in updateddept)
            {
                if (c.Id == deptchange.Id)
                {
                    fromDB = c;
                    break;
                }
            }
            Assert.AreEqual("GranTurismo", fromDB.Name);
        }
    }
}
