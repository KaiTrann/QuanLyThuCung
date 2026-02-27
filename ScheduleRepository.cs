using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Nhóm_7
{
    public class ScheduleRepository
    {
        public class PetPickItem
        {
            public int PetId { get; set; }
            public string PetName { get; set; } = "";
        }

        public class VaccinePickItem
        {
            public int VaccineId { get; set; }
            public string VaccineName { get; set; } = "";
        }

        public class AppointmentRow
        {
            public int AppointmentId { get; set; }
            public string Pet { get; set; } = "";
            public DateTime Date { get; set; }
            public string Reason { get; set; } = "";
            public string Status { get; set; } = "";
        }

        public class VaccinationRow
        {
            public int VaccinationId { get; set; }
            public string Pet { get; set; } = "";
            public string VaccineName { get; set; } = "";
            public DateTime Date { get; set; }
            public DateTime? NextDueDate { get; set; }
        }

        public List<PetPickItem> GetPetPickList()
        {
            const string sql = @"SELECT pet_id, pet_name FROM pets ORDER BY pet_name ASC;";
            var dt = Db.Query(sql);

            var list = new List<PetPickItem>();
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new PetPickItem
                {
                    PetId = Convert.ToInt32(r["pet_id"]),
                    PetName = r["pet_name"]?.ToString() ?? ""
                });
            }
            return list;
        }

        public List<VaccinePickItem> GetVaccinePickList()
        {
            const string sql = @"SELECT vaccine_id, vaccine_name FROM vaccines ORDER BY vaccine_name ASC;";
            var dt = Db.Query(sql);

            var list = new List<VaccinePickItem>();
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new VaccinePickItem
                {
                    VaccineId = Convert.ToInt32(r["vaccine_id"]),
                    VaccineName = r["vaccine_name"]?.ToString() ?? ""
                });
            }
            return list;
        }

        public List<AppointmentRow> GetAppointments(string keyword = null)
        {
            string key = (keyword ?? "").Trim();
            string sql;
            SqlParameter[] prms = null;

            if (string.IsNullOrWhiteSpace(key))
            {
                sql = @"SELECT appointment_id, pet_name, appointment_date, reason, status FROM vw_appointments ORDER BY appointment_id DESC;";
            }
            else
            {
                sql = @"SELECT appointment_id, pet_name, appointment_date, reason, status FROM vw_appointments WHERE pet_name LIKE @kw ORDER BY appointment_id DESC;";
                prms = new[] { new SqlParameter("@kw", "%" + key + "%") };
            }

            var dt = Db.Query(sql, prms);
            var list = new List<AppointmentRow>();

            foreach (DataRow r in dt.Rows)
            {
                list.Add(new AppointmentRow
                {
                    AppointmentId = Convert.ToInt32(r["appointment_id"]),
                    Pet = r["pet_name"]?.ToString() ?? "",
                    Date = Convert.ToDateTime(r["appointment_date"]),
                    Reason = r["reason"] == DBNull.Value ? "" : r["reason"]?.ToString() ?? "",
                    Status = r["status"] == DBNull.Value ? "" : r["status"]?.ToString() ?? ""
                });
            }

            return list;
        }

        public int InsertAppointment(int petId, DateTime date, string reason, string status)
        {
            const string sql = @"
            INSERT INTO appointments(pet_id, appointment_date, reason, status)
            VALUES (@pet, @d, @r, @s);";

            return Db.Execute(
                sql,
                new SqlParameter("@pet", petId),
                new SqlParameter("@d", date.Date),
                new SqlParameter("@r", string.IsNullOrWhiteSpace(reason) ? (object)DBNull.Value : reason),
                new SqlParameter("@s", string.IsNullOrWhiteSpace(status) ? (object)DBNull.Value : status)
            );
        }

        public int UpdateAppointment(int appointmentId, int petId, DateTime date, string reason, string status)
        {
            const string sql = @"
            UPDATE appointments
            SET pet_id=@pet, appointment_date=@d, reason=@r, status=@s
            WHERE appointment_id=@id;";

            return Db.Execute(
                sql,
                new SqlParameter("@pet", petId),
                new SqlParameter("@d", date.Date),
                new SqlParameter("@r", string.IsNullOrWhiteSpace(reason) ? (object)DBNull.Value : reason),
                new SqlParameter("@s", string.IsNullOrWhiteSpace(status) ? (object)DBNull.Value : status),
                new SqlParameter("@id", appointmentId)
            );
        }

        public int DeleteAppointment(int appointmentId)
        {
            const string sql = @"DELETE FROM appointments WHERE appointment_id=@id;";
            return Db.Execute(sql, new SqlParameter("@id", appointmentId));
        }

        public int CountAppointmentsByDate(DateTime date)
        {
            const string sql = @"SELECT COUNT(*) FROM appointments WHERE appointment_date=@d;";
            object obj = Db.Scalar(sql, new SqlParameter("@d", date.Date));
            return Convert.ToInt32(obj);
        }

        public List<VaccinationRow> GetVaccinations(string keyword = null)
        {
            string key = (keyword ?? "").Trim();
            string sql;
            SqlParameter[] prms = null;

            if (string.IsNullOrWhiteSpace(key))
            {
                sql = @"SELECT vaccination_id, pet_name, vaccine_name, vaccination_date, next_vaccination_date FROM vw_vaccinations ORDER BY vaccination_id DESC;";
            }
            else
            {
                sql = @"SELECT vaccination_id, pet_name, vaccine_name, vaccination_date, next_vaccination_date FROM vw_vaccinations WHERE pet_name LIKE @kw ORDER BY vaccination_id DESC;";
                prms = new[] { new SqlParameter("@kw", "%" + key + "%") };
            }

            var dt = Db.Query(sql, prms);
            var list = new List<VaccinationRow>();

            foreach (DataRow r in dt.Rows)
            {
                list.Add(new VaccinationRow
                {
                    VaccinationId = Convert.ToInt32(r["vaccination_id"]),
                    Pet = r["pet_name"]?.ToString() ?? "",
                    VaccineName = r["vaccine_name"]?.ToString() ?? "",
                    Date = Convert.ToDateTime(r["vaccination_date"]),
                    NextDueDate = r["next_vaccination_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["next_vaccination_date"])
                });
            }

            return list;
        }

        public int InsertVaccination(int petId, int vaccineId, DateTime vaccinationDate, DateTime? nextVaccinationDate)
        {
            const string sql = @"
            INSERT INTO vaccinations(pet_id, vaccine_id, vaccination_date, next_vaccination_date)
            VALUES (@pet, @vax, @d, @next);";

            return Db.Execute(
                sql,
                new SqlParameter("@pet", petId),
                new SqlParameter("@vax", vaccineId),
                new SqlParameter("@d", vaccinationDate.Date),
                new SqlParameter("@next", nextVaccinationDate.HasValue ? (object)nextVaccinationDate.Value.Date : DBNull.Value)
            );
        }

        public int UpdateVaccination(int vaccinationId, int petId, int vaccineId, DateTime vaccinationDate, DateTime? nextVaccinationDate)
        {
            const string sql = @"
            UPDATE vaccinations
            SET pet_id=@pet, vaccine_id=@vax, vaccination_date=@d, next_vaccination_date=@next
            WHERE vaccination_id=@id;";

            return Db.Execute(
                sql,
                new SqlParameter("@pet", petId),
                new SqlParameter("@vax", vaccineId),
                new SqlParameter("@d", vaccinationDate.Date),
                new SqlParameter("@next", nextVaccinationDate.HasValue ? (object)nextVaccinationDate.Value.Date : DBNull.Value),
                new SqlParameter("@id", vaccinationId)
            );
        }

        public int DeleteVaccination(int vaccinationId)
        {
            const string sql = @"DELETE FROM vaccinations WHERE vaccination_id=@id;";
            return Db.Execute(sql, new SqlParameter("@id", vaccinationId));
        }

        public int CountVaccinationsByDate(DateTime date)
        {
            const string sql = @"SELECT COUNT(*) FROM vaccinations WHERE vaccination_date=@d;";
            object obj = Db.Scalar(sql, new SqlParameter("@d", date.Date));
            return Convert.ToInt32(obj);
        }
    }
}