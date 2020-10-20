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
    public class DepartmentSqlDAO : IDepartmentDAO
    {
        private string connectionString;

        // Single Parameter Constructor
        public DepartmentSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the departments.
        /// </summary>
        /// <returns></returns>
        public IList<Department> GetDepartments()
        {
            List<Department> list = new List<Department>();
            try
            {
                using(SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT department_id, name FROM department", conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Department d = ConvertReaderToDepartment(reader);
                        list.Add(d);
                    }
                }
            }
            catch(SqlException e)
            {
                Console.WriteLine("An error occurred reading departments.");
                Console.WriteLine(e.Message);
                throw;
            }
            return list;
        }
        /// <summary>
        /// Creates a new department.
        /// </summary>
        /// <param name="newDepartment">The department object.</param>
        /// <returns>The id of the new department (if successful).</returns>
        public int CreateDepartment(Department newDepartment)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO department VALUES(@name)", conn);
                    cmd.Parameters.AddWithValue("@name", newDepartment.Name);

                    int numRowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine("Number of rows affected: " + numRowsAffected);

                    cmd = new SqlCommand("SELECT MAX(department_id) FROM department;", conn);
                    int id = Convert.ToInt32(cmd.ExecuteScalar());

                    Console.WriteLine("The new department id is: " + id);

                    return numRowsAffected;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("An error occurred creating departments.");
                Console.WriteLine(e.Message);
                throw;
            }
        }
        /// <summary>
        /// Updates an existing department.
        /// </summary>
        /// <param name="updatedDepartment">The department object.</param>
        /// <returns>True, if successful.</returns>
        public bool UpdateDepartment(Department updatedDepartment)
        {
            bool success = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE department SET name = @name WHERE department_id = @department_id", conn);
                    cmd.Parameters.AddWithValue("@name", updatedDepartment.Name);
                    cmd.Parameters.AddWithValue("@department_id", updatedDepartment.Id);

                    int numRowsAffected = cmd.ExecuteNonQuery();

                    if (numRowsAffected == 1)
                    {
                        success = true;
                    }
                    else
                    {
                        Console.WriteLine("Oh no! You changed " + numRowsAffected + " rows!");
                    }
                                        
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("An error occurred updating departments.");
                Console.WriteLine(e.Message);
                throw;
            }
            return success;
        }
        public Department ConvertReaderToDepartment(SqlDataReader reader)
        {
            Department d = new Department();
            d.Id = Convert.ToInt32(reader["department_id"]);
            d.Name = Convert.ToString(reader["name"]);

            return d;
        }

    }
}
