﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            EmployeeSqlDAO employeeDAO = new EmployeeSqlDAO(ConnectionString);
            int startingRowCount = GetRowCount("project_employee");

            Project SM64 = new Project();
            SM64.Name = "Super Mario";
            SM64.StartDate = DateTime.Now;
            SM64.EndDate = DateTime.Now;

            Employee Enrique = new Employee();
            Enrique.FirstName = "Enrique";
            Enrique.LastName = "Josh";
            Enrique.Gender = "M";
            Enrique.JobTitle = "CEO";
            Enrique.BirthDate = DateTime.Now;
            Enrique.HireDate = DateTime.Now;
            Enrique.DepartmentId = NewDepartmentId;

            dao.CreateProject(SM64);
            employeeDAO.CreateEmployee(Enrique);
            dao.AssignEmployeeToProject(SM64.ProjectId, Enrique.EmployeeId);

            int endingRowCount = GetRowCount("project_employee");
            //ACT

            //ASSERT
            Assert.AreNotEqual(startingRowCount, endingRowCount);
        }

        [TestMethod]
        public void TestRemoveEmployeeFromProject()
        {
            ProjectSqlDAO dao = new ProjectSqlDAO(ConnectionString);
            int startingRowCount = GetRowCount("project_employee");

            dao.RemoveEmployeeFromProject(NewProjectId, NewEmployeeId);

            int endingRowCount = GetRowCount("project_employee");

            Assert.AreEqual(startingRowCount - 1, endingRowCount);
        }

        [TestMethod]
        public void TestCreateProject()
        {
            ProjectSqlDAO dao = new ProjectSqlDAO(ConnectionString);
            Project SM64 = new Project();
            SM64.Name = "Super Mario";
            SM64.StartDate = DateTime.Now;
            SM64.EndDate = DateTime.Now;

            int result = dao.CreateProject(SM64);

            Assert.AreEqual(1, result);
        }
    }
}
