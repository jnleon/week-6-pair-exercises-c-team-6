using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CampgroundReservations.Models;

namespace CampgroundReservations.DAO
{
    public class SiteSqlDAO : ISiteDAO
    {
        private string connectionString;

        public SiteSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Site> GetSitesThatAllowRVs(int parkId)
        {
            IList<Site> Sitelist = new List<Site>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM site " +
                                                     "INNER JOIN campground c ON c.campground_id = site.campground_id " +
                                                     "INNER JOIN park p ON p.park_id = c.park_id  " +
                                                     "WHERE max_rv_length >0 AND @park_id = p.park_Id", conn);
                   
                    cmd.Parameters.AddWithValue("@park_id", parkId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site s = GetSiteFromReader(reader);
                        Sitelist.Add(s);
                    }
                }
                return Sitelist;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        public IList<Site> GetSitesWithoutReservations(int parkId)
        {
            IList<Site> Sitelist = new List<Site>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM site s " +
                        "LEFT JOIN reservation r ON r.site_id = s.site_id " +
                        "LEFT JOIN campground c ON c.campground_id = s.campground_id " +
                        "LEFT JOIN park p ON p.park_id = c.park_id " +
                        "WHERE r.site_id IS NULL AND p.park_id = @parkId ", conn);

                    cmd.Parameters.AddWithValue("@parkId", parkId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site s = GetSiteFromReader(reader);
                        Sitelist.Add(s);
                    }
                }
                return Sitelist;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public IList<Site> GetSitesForFutureReservations(int parkId, DateTime fromdate, DateTime todate)
        {
            IList<Site> Sitelist = new List<Site>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM site s " +
                        "LEFT JOIN reservation r ON r.site_id = s.site_id " +
                        "LEFT JOIN campground c ON c.campground_id = s.campground_id " +
                        "LEFT JOIN park p ON p.park_id = c.park_id " +
                        "WHERE @from_date NOT BETWEEN R.from_date " +
                        "AND r.to_date " +
                        "AND @to_date NOT BETWEEN R.from_date AND r.to_date AND p.park_id =@parkId " +
                        "AND @from_date > r.from_date AND @from_date >r.to_date", conn);

                    cmd.Parameters.AddWithValue("@from_date", fromdate);
                    cmd.Parameters.AddWithValue("@to_date", todate);
                    cmd.Parameters.AddWithValue("@parkId", parkId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site s = GetSiteFromReader(reader);
                        Sitelist.Add(s);
                    }
                }
                return Sitelist;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        private Site GetSiteFromReader(SqlDataReader reader)
        {
            Site site = new Site();
            site.SiteId = Convert.ToInt32(reader["site_id"]);
            site.CampgroundId = Convert.ToInt32(reader["campground_id"]);
            site.SiteNumber = Convert.ToInt32(reader["site_number"]);
            site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
            site.Accessible = Convert.ToBoolean(reader["accessible"]);
            site.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
            site.Utilities = Convert.ToBoolean(reader["utilities"]);

            return site;
        }
    }
}
