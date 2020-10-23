using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CampgroundReservations.Models;

namespace CampgroundReservations.DAO
{
    public class ReservationSqlDAO : IReservationDAO
    {
        private string connectionString;

        public ReservationSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public int CreateReservation(int siteId, string name, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO reservation(site_id,name, from_date,to_date, create_date) " +
                                                   "VALUES (@site_id ,@name, @from_date,@to_date, @create_date)", conn);

                    cmd.Parameters.AddWithValue("@site_id", siteId);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@from_date", fromDate);
                    cmd.Parameters.AddWithValue("@to_date", toDate);
                    cmd.Parameters.AddWithValue("@create_date", DateTime.Now);

                    cmd.ExecuteNonQuery();
                    
                    cmd = new SqlCommand("SELECT MAX(reservation_id) FROM reservation;", conn);
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    return id;
                }
            }  


            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public IList<Reservation> GetReservationsWithin30DaysSelectedPark(string parkId)
        {
            List<Reservation> reservs = new List<Reservation>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM reservation r " +
                        "INNER JOIN site s ON s.site_id = r.site_id " +
                        "INNER JOIN campground c ON c.campground_id = s.campground_id " +
                        "INNER JOIN park p ON p.park_id = c.park_id " +
                        "WHERE from_date BETWEEN CURRENT_TIMESTAMP " +
                        "AND DATEADD(DAY, 30, CURRENT_TIMESTAMP) " +
                        "AND p.park_id = @parkid", conn);

                    cmd.Parameters.AddWithValue("@parkid", parkId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Reservation reserv = GetReservationFromReader(reader);
                        reservs.Add(reserv);

                    }
                    return reservs;
                }
            }
            catch(SqlException e)
            {
                Console.Write(e.Message);
                throw;
            }

        }

        private Reservation GetReservationFromReader(SqlDataReader reader)
        {
            Reservation reservation = new Reservation();
            reservation.ReservationId = Convert.ToInt32(reader["reservation_id"]);
            reservation.SiteId = Convert.ToInt32(reader["site_id"]);
            reservation.Name = Convert.ToString(reader["name"]);
            reservation.FromDate = Convert.ToDateTime(reader["from_date"]);
            reservation.ToDate = Convert.ToDateTime(reader["to_date"]);
            reservation.CreateDate = Convert.ToDateTime(reader["create_date"]);

            return reservation;
        }
    }
}
