using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace Nhóm_7
{
    public partial class OwnersWindow : Window
    {
        private int _selectedOwnerId = 0;

        public OwnersWindow()
        {
            InitializeComponent();

            // gắn data ban đầu
            LoadOwners();
        }

        // ================= DB =================
        private static MySqlConnection OpenConn()
        {
            var cs = ConfigurationManager.ConnectionStrings["MySqlConn"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(cs))
                throw new Exception("Thiếu connectionStrings name=\"MySqlConn\" trong App.config");

            return new MySqlConnection(cs);
        }

        // ================= Model for DataGrid =================
        private class OwnerGridRow
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public int PetCount { get; set; }
        }

        // ================= Load list =================
        private void LoadOwners(string keyword = null)
        {
            var list = new List<OwnerGridRow>();

            try
            {
                using (var conn = OpenConn())
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    // Có PetCount (optional)
                    // pets.owner_id FK -> owners.owner_id
                    if (string.IsNullOrWhiteSpace(keyword))
                    {
                        cmd.CommandText =
                            @"SELECT  o.owner_id,
                                      o.full_name,
                                      o.phone,
                                      o.address,
                                      (SELECT COUNT(*) FROM pets p WHERE p.owner_id = o.owner_id) AS pet_count
                              FROM owners o
                              ORDER BY o.owner_id DESC;";
                    }
                    else
                    {
                        cmd.CommandText =
                            @"SELECT  o.owner_id,
                                      o.full_name,
                                      o.phone,
                                      o.address,
                                      (SELECT COUNT(*) FROM pets p WHERE p.owner_id = o.owner_id) AS pet_count
                              FROM owners o
                              WHERE o.full_name LIKE @kw OR o.phone LIKE @kw
                              ORDER BY o.owner_id DESC;";
                        cmd.Parameters.AddWithValue("@kw", "%" + keyword.Trim() + "%");
                    }

                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new OwnerGridRow
                            {
                                Id = r.GetInt32("owner_id"),
                                FullName = r.GetString("full_name"),
                                Phone = r.IsDBNull(r.GetOrdinal("phone")) ? "" : r.GetString("phone"),
                                Address = r.IsDBNull(r.GetOrdinal("address")) ? "" : r.GetString("address"),
                                PetCount = Convert.ToInt32(r["pet_count"])
                            });
                        }
                    }
                }

                dgOwners.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi LoadOwners:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ================= Helpers =================
        private void ClearForm()
        {
            _selectedOwnerId = 0;
            txtFullName.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            dgOwners.SelectedItem = null;
        }

        private bool ValidateInput(out string fullName)
        {
            fullName = (txtFullName.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Vui lòng nhập Họ tên.", "Thiếu dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFullName.Focus();
                return false;
            }

            return true;
        }

        // ================= Events =================
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            LoadOwners(txtSearch.Text);
        }

        private void DgOwners_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgOwners.SelectedItem is OwnerGridRow row)
            {
                _selectedOwnerId = row.Id;
                txtFullName.Text = row.FullName;
                txtPhone.Text = row.Phone;
                txtAddress.Text = row.Address;
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            ClearForm();
            LoadOwners();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput(out var fullName)) return;

            var phone = (txtPhone.Text ?? "").Trim();
            var address = (txtAddress.Text ?? "").Trim();

            try
            {
                using (var conn = OpenConn())
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText =
                        @"INSERT INTO owners(full_name, phone, address, created_at, updated_at)
                          VALUES (@name, @phone, @addr, NOW(), NOW());";

                    cmd.Parameters.AddWithValue("@name", fullName);
                    cmd.Parameters.AddWithValue("@phone", string.IsNullOrWhiteSpace(phone) ? (object)DBNull.Value : phone);
                    cmd.Parameters.AddWithValue("@addr", string.IsNullOrWhiteSpace(address) ? (object)DBNull.Value : address);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Thêm chủ nuôi thành công!", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
                LoadOwners();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Thêm:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedOwnerId <= 0)
            {
                MessageBox.Show("Bạn chưa chọn chủ nuôi để sửa (click 1 dòng trong bảng).",
                                "Thiếu chọn dòng", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!ValidateInput(out var fullName)) return;

            var phone = (txtPhone.Text ?? "").Trim();
            var address = (txtAddress.Text ?? "").Trim();

            try
            {
                using (var conn = OpenConn())
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText =
                        @"UPDATE owners
                          SET full_name=@name,
                              phone=@phone,
                              address=@addr,
                              updated_at=NOW()
                          WHERE owner_id=@id;";

                    cmd.Parameters.AddWithValue("@name", fullName);
                    cmd.Parameters.AddWithValue("@phone", string.IsNullOrWhiteSpace(phone) ? (object)DBNull.Value : phone);
                    cmd.Parameters.AddWithValue("@addr", string.IsNullOrWhiteSpace(address) ? (object)DBNull.Value : address);
                    cmd.Parameters.AddWithValue("@id", _selectedOwnerId);

                    var n = cmd.ExecuteNonQuery();
                    if (n == 0)
                    {
                        MessageBox.Show("Không tìm thấy bản ghi để cập nhật.", "Warning",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                MessageBox.Show("Cập nhật thành công!", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
                LoadOwners();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Sửa:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedOwnerId <= 0)
            {
                MessageBox.Show("Bạn chưa chọn chủ nuôi để xóa (click 1 dòng trong bảng).",
                                "Thiếu chọn dòng", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var ok = MessageBox.Show($"Bạn chắc chắn muốn xóa Owner ID = {_selectedOwnerId} ?",
                                     "Xác nhận xóa",
                                     MessageBoxButton.YesNo,
                                     MessageBoxImage.Warning);

            if (ok != MessageBoxResult.Yes) return;

            try
            {
                using (var conn = OpenConn())
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    cmd.CommandText = @"DELETE FROM owners WHERE owner_id=@id;";
                    cmd.Parameters.AddWithValue("@id", _selectedOwnerId);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Đã xóa!", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
                LoadOwners();
            }
            catch (MySqlException ex) when (ex.Number == 1451)
            {
                MessageBox.Show("Không thể xóa vì Owner đang liên kết với Pets.\n" +
                                "Hãy xóa Pets trước hoặc chỉnh FK ON DELETE SET NULL/CASCADE.",
                                "Ràng buộc khóa ngoại", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Xóa:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}