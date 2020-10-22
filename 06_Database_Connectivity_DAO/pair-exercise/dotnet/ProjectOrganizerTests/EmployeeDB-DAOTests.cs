using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;
using ProjectOrganizer.DAL;

namespace ProjectOrganizerTests
{
    [TestClass]
    public class EmployeeDB_DAOTests
    {
        protected string ConnectionString { get; } = "Server =.\\SQLEXPRESS; Database = EmployeeDB; Trusted_Connection = True;";

        private TransactionScope transaction;

        protected int NewDepartmentId { get; private set; }
        protected int NewProjectId { get; private set; }
        protected int NewEmployeeId { get; private set; }



        [TestInitialize]
        public void Setup()
        {
            transaction = new TransactionScope();

            string sql = File.ReadAllText("test-script.sql");


            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    this.NewProjectId = Convert.ToInt32(reader["newProjectId"]);
                    this.NewDepartmentId = Convert.ToInt32(reader["newDepartmentId"]);
                    this.NewEmployeeId = Convert.ToInt32(reader["newEmployeeId"]);

                }
            }
        }
        [TestCleanup]
        public void Cleanup()
        {
            // Roll back the transaction
            transaction.Dispose();
        }

        // NOT SURE
        protected int GetRowCount(string table)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) FROM {table}", conn);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count;
            }
        }

    }
}
