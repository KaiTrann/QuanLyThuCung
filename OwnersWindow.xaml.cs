using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Nhóm_7
{
    public partial class OwnersWindow : Window
    {
        private int _selectedOwnerId = 0;
        private readonly OwnersRepository repo = new OwnersRepository();

        public OwnersWindow(string keyword = null)
        {
            InitializeComponent();
            txtSearch.Text = keyword ?? "";
            LoadOwners(keyword);
        }

        private void LoadOwners(string keyword = null)
        {
            try
            {
                List<OwnersRepository.OwnerGridRow> list = repo.GetOwners(keyword);
                dgOwners.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi LoadOwners:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

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

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            LoadOwners(txtSearch.Text);
        }

        private void DgOwners_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = dgOwners.SelectedItem as OwnersRepository.OwnerGridRow;
            if (row == null) return;

            _selectedOwnerId = row.Id;
            txtFullName.Text = row.FullName;
            txtPhone.Text = row.Phone;
            txtAddress.Text = row.Address;
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            ClearForm();
            LoadOwners();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string fullName;
            if (!ValidateInput(out fullName)) return;

            var phone = (txtPhone.Text ?? "").Trim();
            var address = (txtAddress.Text ?? "").Trim();

            try
            {
                repo.Insert(fullName, phone, address);

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

            string fullName;
            if (!ValidateInput(out fullName)) return;

            var phone = (txtPhone.Text ?? "").Trim();
            var address = (txtAddress.Text ?? "").Trim();

            try
            {
                var n = repo.Update(_selectedOwnerId, fullName, phone, address);
                if (n == 0)
                {
                    MessageBox.Show("Không tìm thấy bản ghi để cập nhật.", "Warning",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
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

            var ok = MessageBox.Show("Bạn chắc chắn muốn xóa Owner ID = " + _selectedOwnerId + " ?",
                                     "Xác nhận xóa",
                                     MessageBoxButton.YesNo,
                                     MessageBoxImage.Warning);

            if (ok != MessageBoxResult.Yes) return;

            try
            {
                repo.Delete(_selectedOwnerId);

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