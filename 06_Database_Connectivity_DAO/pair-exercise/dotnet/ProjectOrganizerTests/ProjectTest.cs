using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;
using System.Collections.Generic;
using System;

namespace ProjectOrganizerTests
{
    [TestClass]
    public class ProjectTest : EmployeeDB_DAOTests
    {
        //GetAllprojects()
        //assignemployeetoproject(int projectId, int employeeId)
        //removeemployeefromproject(int projectId, int employeeId)
        //create project(Project, NewProject)

        [TestMethod]
        public void GetProjectsShouldReturnRightNumberofProjects()
        {
            // Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(ConnectionString);

            //ACT
            IList<Project> project = dao.GetAllProjects();

            //ASERT
            Assert.AreEqual(GetRowCount("project"), project.Count);
        }

        [TestMethod]
        public void AssignEmployeeToProjectShouldAssignEmployeeToProject()
        {
            // Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(ConnectionString);
            int startingRowCount = GetRowCount("project_employee");

            Project SM64 = new Project();
            SM64.Name = "Super Mario";
            SM64.ProjectId = NewProjectId + 1;
            SM64.StartDate = DateTime.Now;
            SM64.EndDate = DateTime.Now;

            Employee Ricardo = new Employee();
            Ricardo.FirstName = "enrique";
            Ricardo.LastName = "josh";
            Ricardo.Gender = "M";
            Ricardo.JobTitle = "CEO";
            Ricardo.BirthDate = DateTime.Now;
            Ricardo.HireDate = DateTime.Now;
            Ricardo.DepartmentId = NewDepartmentId;
            Ricardo.EmployeeId = NewEmployeeId +1;

            dao.AssignEmployeeToProject(SM64.ProjectId, Ricardo.EmployeeId);

            int endingRowCount = GetRowCount("project_employee");
            //ACT

            //ASERT
            Assert.AreNotEqual(startingRowCount, endingRowCount);


            ////Arrange
            //DepartmentSqlDAO dao = new DepartmentSqlDAO(ConnectionString);
            //IList<Department> depts = dao.GetDepartments();

            //Department deptchange = depts[0];

            ////Act
            //deptchange.Name = "GranTurismo";
            //dao.UpdateDepartment(deptchange);

            ////Assert
            //IList<Department> updateddept = dao.GetDepartments();
            //Department fromDB = null;
            //foreach (Department c in updateddept)
            //{
            //    if (c.Id == deptchange.Id)
            //    {
            //        fromDB = c;
            //        break;
            //    }
            //}
            //Assert.AreEqual("GranTurismo", fromDB.Name);
        }
    }
}
