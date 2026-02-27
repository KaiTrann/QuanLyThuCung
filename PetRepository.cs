using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Nhóm_7
{
    public class PetRepository
    {
        private static string _ownersNameColumn;

        private static string GetOwnersNameColumn()
        {
            if (!string.IsNullOrWhiteSpace(_ownersNameColumn)) return _ownersNameColumn;

            var dt = Db.Query(@"
            SELECT COLUMN_NAME
            FROM information_schema.COLUMNS
            WHERE TABLE_SCHEMA = DATABASE()
              AND TABLE_NAME = 'owners';");

            var cols = new List<string>();
            foreach (DataRow r in dt.Rows)
                cols.Add((r["COLUMN_NAME"]?.ToString() ?? "").ToLower());

            string[] priority =
            {
                "owner_name","full_name","fullname","name",
                "username","user_name","ten","hoten"
            };

            foreach (var p in priority)
            {
                if (cols.Contains(p))
                {
                    _ownersNameColumn = p;
                    return _ownersNameColumn;
                }
            }

            _ownersNameColumn = "";
            return _ownersNameColumn;
        }

        public List<Pet> GetAll()
        {
            string nameCol = GetOwnersNameColumn();

            string ownerSelect = string.IsNullOrWhiteSpace(nameCol)
                ? "CAST(p.owner_id AS CHAR) AS owner_name"
                : $"o.{nameCol} AS owner_name";

            string sql = $@"
            SELECT p.pet_id, p.owner_id, p.pet_name, p.species, p.breed, p.sex,
                   {ownerSelect}
            FROM pets p
            LEFT JOIN owners o ON o.owner_id = p.owner_id
            ORDER BY p.pet_id DESC;";

            return Map(Db.Query(sql));
        }

        public List<Pet> SearchByName(string keyword)
        {
            string key = (keyword ?? "").Trim();
            string nameCol = GetOwnersNameColumn();

            string ownerSelect = string.IsNullOrWhiteSpace(nameCol)
                ? "CAST(p.owner_id AS CHAR) AS owner_name"
                : $"o.{nameCol} AS owner_name";

            string sql = $@"
            SELECT p.pet_id, p.owner_id, p.pet_name, p.species, p.breed, p.sex,
                   {ownerSelect}
            FROM pets p
            LEFT JOIN owners o ON o.owner_id = p.owner_id
            WHERE p.pet_name LIKE CONCAT('%', @key, '%')
            ORDER BY p.pet_id DESC;";

            return Map(Db.Query(sql, new MySqlParameter("@key", key)));
        }

        public int Insert(Pet p)
        {
            const string sql = @"
            INSERT INTO pets (owner_id, pet_name, species, breed, sex)
            VALUES (@owner_id, @pet_name, @species, @breed, @sex);
            SELECT LAST_INSERT_ID();";

            object idObj = Db.Scalar(sql,
                new MySqlParameter("@owner_id", p.OwnerId),
                new MySqlParameter("@pet_name", p.Name),
                new MySqlParameter("@species", string.IsNullOrWhiteSpace(p.Species) ? (object)DBNull.Value : p.Species),
                new MySqlParameter("@breed", string.IsNullOrWhiteSpace(p.Breed) ? (object)DBNull.Value : p.Breed),
                new MySqlParameter("@sex", string.IsNullOrWhiteSpace(p.Sex) ? (object)DBNull.Value : p.Sex)
            );

            return Convert.ToInt32(idObj);
        }

        public int Update(Pet p)
        {
            const string sql = @"
            UPDATE pets
            SET owner_id = @owner_id,
                pet_name = @pet_name,
                species = @species,
                breed = @breed,
                sex = @sex
            WHERE pet_id = @pet_id;";

            return Db.Execute(sql,
                new MySqlParameter("@pet_id", p.PetId),
                new MySqlParameter("@owner_id", p.OwnerId),
                new MySqlParameter("@pet_name", p.Name),
                new MySqlParameter("@species", string.IsNullOrWhiteSpace(p.Species) ? (object)DBNull.Value : p.Species),
                new MySqlParameter("@breed", string.IsNullOrWhiteSpace(p.Breed) ? (object)DBNull.Value : p.Breed),
                new MySqlParameter("@sex", string.IsNullOrWhiteSpace(p.Sex) ? (object)DBNull.Value : p.Sex)
            );
        }

        public int Delete(int petId)
        {
            const string sql = "DELETE FROM pets WHERE pet_id = @pet_id;";
            return Db.Execute(sql, new MySqlParameter("@pet_id", petId));
        }

        private List<Pet> Map(DataTable dt)
        {
            var list = new List<Pet>();
            if (dt == null) return list;

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Pet
                {
                    PetId = Convert.ToInt32(row["pet_id"]),
                    OwnerId = Convert.ToInt32(row["owner_id"]),
                    Name = row["pet_name"]?.ToString() ?? "",
                    Species = row["species"] == DBNull.Value ? "" : row["species"]?.ToString() ?? "",
                    Breed = row["breed"] == DBNull.Value ? "" : row["breed"]?.ToString() ?? "",
                    Sex = row["sex"] == DBNull.Value ? "" : row["sex"]?.ToString() ?? "",
                    OwnerName = row.Table.Columns.Contains("owner_name") && row["owner_name"] != DBNull.Value
                                ? row["owner_name"].ToString()
                                : Convert.ToInt32(row["owner_id"]).ToString()
                });
            }

            return list;
        }
    }
}