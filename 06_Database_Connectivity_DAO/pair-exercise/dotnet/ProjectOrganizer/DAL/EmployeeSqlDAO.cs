using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOrganizer.DAL
{
    public class EmployeeSqlDAO : IEmployeeDAO
    {
        private string connectionString;

        // Single Parameter Constructor
        public EmployeeSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the employees.
        /// </summary>
        /// <returns>A list of all employees.</returns>
        public IList<Employee> GetAllEmployees()
        {
            List<Employee> list = new List<Employee>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM employee", conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Employee e = ConvertReaderToEmployee(reader);
                        list.Add(e);
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("An error occurred reading employees.");
                Console.WriteLine(e.Message);
                throw;
            }
            return list;
        }

        

        /// <summary>
        /// Searches the system for an employee by first name or last name.
        /// </summary>
        /// <remarks>The search performed is a wildcard search.</remarks>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <returns>A list of employees that match the search.</returns>
        public IList<Employee> Search(string firstname, string lastname)
        {
            List<Employee> list = new List<Employee>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM employee WHERE first_name LIKE @firstname OR last_name LIKE @lastname", conn);
                    cmd.Parameters.AddWithValue("@firstname", "%" + firstname + "%");
                    cmd.Parameters.AddWithValue("@lastname", "%" + lastname + "%");

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Employee e = ConvertReaderToEmployee(reader);
                        list.Add(e);
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("An error occurred searching for employees.");
                Console.WriteLine(e.Message);
                throw;
            }
            return list;
        }

        /// <summary>
        /// Gets a list of employees who are not assigned to any active projects.
        /// </summary>
        /// <returns></returns>
        public IList<Employee> GetEmployeesWithoutProjects()
        {
            List<Employee> list = new List<Employee>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM employee " +
                                                    "WHERE employee_id NOT IN(SELECT DISTINCT employee_id " +
                                                    "FROM project_employee)", conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Employee e = ConvertReaderToEmployee(reader);
                        list.Add(e);
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("An error occurred reading employees without projects.");
                Console.WriteLine(e.Message);
                throw;
            }
            return list;
        }
     
        
        private Employee ConvertReaderToEmployee(SqlDataReader reader)
        {
            Employee e = new Employee();
            e.EmployeeId = Convert.ToInt32(reader["employee_id"]);
            e.DepartmentId = Convert.ToInt32(reader["department_id"]);
            e.FirstName = Convert.ToString(reader["first_name"]);
            e.LastName = Convert.ToString(reader["last_name"]);
            e.JobTitle = Convert.ToString(reader["job_title"]);
            e.BirthDate = Convert.ToDateTime(reader["birth_date"]);
            e.Gender = Convert.ToString(reader["gender"]);
            e.HireDate = Convert.ToDateTime(reader["hire_date"]);

            return e;
        }
    }
 }

