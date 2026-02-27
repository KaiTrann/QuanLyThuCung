using System;
using System.Data.SqlClient;

namespace Nhóm_7
{
    public class DashboardRepository
    {
        public int CountOwners()
        {
            const string sql = "SELECT COUNT(*) FROM owners;";
            object obj = Db.Scalar(sql);
            return (obj == null || obj == DBNull.Value) ? 0 : Convert.ToInt32(obj);
        }

        public int CountPets()
        {
            const string sql = "SELECT COUNT(*) FROM pets;";
            object obj = Db.Scalar(sql);
            return (obj == null || obj == DBNull.Value) ? 0 : Convert.ToInt32(obj);
        }

        public int CountUpcomingAppointments()
        {
            const string sql = @"
            SELECT COUNT(*)
            FROM appointments
            WHERE appointment_date >= CAST(GETDATE() AS date);";

            object obj = Db.Scalar(sql);
            return (obj == null || obj == DBNull.Value) ? 0 : Convert.ToInt32(obj);
        }
    }
}