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
    public class ProjectSqlDAO : IProjectDAO
    {
        private string connectionString;

        // Single Parameter Constructor
        public ProjectSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns all projects.
        /// </summary>
        /// <returns></returns>
        public IList<Project> GetAllProjects()
        {
            List<Project> list = new List<Project>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM project", conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Project e = ConvertReaderToProject(reader);
                        list.Add(e);
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("An error occurred reading projects.");
                Console.WriteLine(e.Message);
                throw;
            }
            return list;
        }

    /// <summary>
    /// Assigns an employee to a project using their IDs.
    /// </summary>
    /// <param name="projectId">The project's id.</param>
    /// <param name="employeeId">The employee's id.</param>
    /// <returns>If it was successful.</returns>
    public bool AssignEmployeeToProject(int projectId, int employeeId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO project_employee(project_id, employee_id)" +
                                                    " VALUES(@projectId, @employeeId) SELECT project_id, employee.employee_id FROM project_employee " +
                                                    "INNER JOIN employee " +
                                                    "ON employee.employee_id = project_employee.employee_id ", conn);
                  
                    cmd.Parameters.AddWithValue("@projectId", projectId);
                    cmd.Parameters.AddWithValue("@employeeId", employeeId);

                    int numRowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine("Number of rows affected: " + numRowsAffected);
                }
            }
            catch (SqlException e)
            {
                throw;
            }
            return true;
        }

        /// <summary>
        /// Removes an employee from a project.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("DELETE FROM project_employee " +
                                                    "WHERE project_id = @projectId AND  employee_id = @employeeId", conn);

                    cmd.Parameters.AddWithValue("@projectId", projectId);
                    cmd.Parameters.AddWithValue("@employeeId", employeeId);

                    int numRowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine("Number of rows affected: " + numRowsAffected);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("An error occurred removing employees from projects.");
                Console.WriteLine(e.Message);
                throw;
            }
            return true;
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="newProject">The new project object.</param>
        /// <returns>The new id of the project.</returns>
        public int CreateProject(Project newProject)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO project(name,from_date,to_date) VALUES(@name,@from_date,@to_date)", conn);
                    cmd.Parameters.AddWithValue("@name", newProject.Name);
                    cmd.Parameters.AddWithValue("@from_date", newProject.StartDate);
                    cmd.Parameters.AddWithValue("@to_date", newProject.EndDate);


                    int numRowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine("Number of rows affected: " + numRowsAffected);

                    cmd = new SqlCommand("SELECT MAX(project_id) FROM project;", conn);
                    int id = Convert.ToInt32(cmd.ExecuteScalar());

                    Console.WriteLine("The new project id is: " + id);

                    return numRowsAffected;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("An error occurred creating projects.");
                Console.WriteLine(e.Message);
                throw;
            }
        }
        public Project ConvertReaderToProject(SqlDataReader reader)
        {
            Project d = new Project();
            d.ProjectId = Convert.ToInt32(reader["project_id"]);
            d.Name = Convert.ToString(reader["name"]);
            d.StartDate = Convert.ToDateTime(reader["from_date"]);
            d.EndDate = Convert.ToDateTime(reader["to_date"]);

            return d;
        }
    }
}
