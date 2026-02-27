using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Nhóm_7
{
    public class OwnersRepository
    {
        public class OwnerGridRow
        {
            public int Id { get; set; }
            public string FullName { get; set; } = "";
            public string Phone { get; set; } = "";
            public string Address { get; set; } = "";
            public int PetCount { get; set; }
        }

        public class OwnerPickItem
        {
            public int OwnerId { get; set; }
            public string OwnerName { get; set; } = "";
        }

        public List<OwnerGridRow> GetOwners(string keyword = null)
        {
            string key = (keyword ?? "").Trim();
            string sql;
            SqlParameter[] prms = null;

            if (string.IsNullOrWhiteSpace(key))
            {
                sql = @"
                SELECT o.owner_id, o.full_name, o.phone, o.address,
                       (SELECT COUNT(*) FROM pets p WHERE p.owner_id = o.owner_id) AS pet_count
                FROM owners o
                ORDER BY o.owner_id DESC;";
            }
            else
            {
                sql = @"
                SELECT o.owner_id, o.full_name, o.phone, o.address,
                       (SELECT COUNT(*) FROM pets p WHERE p.owner_id = o.owner_id) AS pet_count
                FROM owners o
                WHERE o.full_name LIKE @kw OR o.phone LIKE @kw
                ORDER BY o.owner_id DESC;";
                prms = new[] { new SqlParameter("@kw", "%" + key + "%") };
            }

            var dt = Db.Query(sql, prms);
            var list = new List<OwnerGridRow>();

            foreach (DataRow r in dt.Rows)
            {
                list.Add(new OwnerGridRow
                {
                    Id = Convert.ToInt32(r["owner_id"]),
                    FullName = r["full_name"]?.ToString() ?? "",
                    Phone = r["phone"] == DBNull.Value ? "" : r["phone"]?.ToString() ?? "",
                    Address = r["address"] == DBNull.Value ? "" : r["address"]?.ToString() ?? "",
                    PetCount = Convert.ToInt32(r["pet_count"])
                });
            }

            return list;
        }

        public int Insert(string fullName, string phone, string address)
        {
            const string sql = @"
            INSERT INTO owners(full_name, phone, address, created_at, updated_at)
            VALUES (@name, @phone, @addr, GETDATE(), GETDATE());";

            return Db.Execute(
                sql,
                new SqlParameter("@name", fullName),
                new SqlParameter("@phone", string.IsNullOrWhiteSpace(phone) ? (object)DBNull.Value : phone),
                new SqlParameter("@addr", string.IsNullOrWhiteSpace(address) ? (object)DBNull.Value : address)
            );
        }

        public int Update(int ownerId, string fullName, string phone, string address)
        {
            const string sql = @"
            UPDATE owners
            SET full_name=@name, phone=@phone, address=@addr, updated_at=GETDATE()
            WHERE owner_id=@id;";

            return Db.Execute(
                sql,
                new SqlParameter("@name", fullName),
                new SqlParameter("@phone", string.IsNullOrWhiteSpace(phone) ? (object)DBNull.Value : phone),
                new SqlParameter("@addr", string.IsNullOrWhiteSpace(address) ? (object)DBNull.Value : address),
                new SqlParameter("@id", ownerId)
            );
        }

        public int Delete(int ownerId)
        {
            const string sql = @"DELETE FROM owners WHERE owner_id=@id;";
            return Db.Execute(sql, new SqlParameter("@id", ownerId));
        }

        public List<OwnerPickItem> GetOwnerPickList()
        {
            const string sql = @"SELECT owner_id, full_name FROM owners ORDER BY full_name ASC;";
            var dt = Db.Query(sql);

            var list = new List<OwnerPickItem>();
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new OwnerPickItem
                {
                    OwnerId = Convert.ToInt32(r["owner_id"]),
                    OwnerName = r["full_name"]?.ToString() ?? ""
                });
            }
            return list;
        }

        public int ResolveOwnerIdByName(string ownerName)
        {
            string name = (ownerName ?? "").Trim();
            if (name.Length == 0) return 0;

            int id;
            if (int.TryParse(name, out id) && id > 0) return id;

            const string sql = @"SELECT TOP 1 owner_id FROM owners WHERE full_name = @n;";
            object obj = Db.Scalar(sql, new SqlParameter("@n", name));
            if (obj == null || obj == DBNull.Value) return 0;
            return Convert.ToInt32(obj);
        }
    }
}