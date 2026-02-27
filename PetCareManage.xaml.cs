using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Nhóm_7
{
    public partial class PetCareManage : Window
    {
        private readonly ObservableCollection<Pet> petsView = new ObservableCollection<Pet>();

        private readonly PetRepository petRepo = new PetRepository();
        private readonly OwnersRepository ownersRepo = new OwnersRepository();

        private List<OwnersRepository.OwnerPickItem> owners = new List<OwnersRepository.OwnerPickItem>();
        private ICollectionView ownersView;

        public PetCareManage(string keyword = null)
        {
            InitializeComponent();

            dgPets.ItemsSource = petsView;

            Loaded += (s, e) =>
            {
                LoadOwnersToCombo();
                RefreshPets();

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    txtSearch.Text = keyword;
                    DoSearch(keyword);
                }
            };

            // bắt sự kiện gõ trong ComboBox editable để filter
            cbOwner.AddHandler(System.Windows.Controls.Primitives.TextBoxBase.TextChangedEvent,
                new TextChangedEventHandler(CbOwner_TextChanged));
        }

        // ==================== OWNER COMBO (SEARCHABLE) ====================
        private void LoadOwnersToCombo()
        {
            owners = ownersRepo.GetOwnerPickList();

            cbOwner.ItemsSource = owners;
            cbOwner.DisplayMemberPath = "OwnerName";
            cbOwner.SelectedValuePath = "OwnerId";

            ownersView = CollectionViewSource.GetDefaultView(cbOwner.ItemsSource);
            ownersView.Filter = OwnerFilter;
        }

        private bool OwnerFilter(object obj)
        {
            var o = obj as OwnersRepository.OwnerPickItem;
            if (o == null) return false;

            string text = (cbOwner.Text ?? "").Trim().ToLower();
            if (text.Length == 0) return true;

            return (o.OwnerName ?? "").ToLower().Contains(text)
                || o.OwnerId.ToString().Contains(text);
        }

        private void CbOwner_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ownersView == null) return;

            ownersView.Refresh();
            cbOwner.IsDropDownOpen = true;
        }

        private int GetOwnerIdFromCombo()
        {
            // 1) user chọn item
            if (cbOwner.SelectedItem is OwnersRepository.OwnerPickItem si)
                return si.OwnerId;

            // 2) user gõ đúng tên nhưng chưa chọn
            string text = (cbOwner.Text ?? "").Trim();
            if (text.Length == 0) return 0;

            var match = owners.FirstOrDefault(x =>
                string.Equals((x.OwnerName ?? "").Trim(), text, StringComparison.CurrentCultureIgnoreCase));
            if (match != null) return match.OwnerId;

            // 3) user gõ số -> coi là owner_id
            int id;
            if (int.TryParse(text, out id) && id > 0) return id;

            return 0;
        }

        // ==================== PETS ====================
        private void RefreshPets()
        {
            petsView.Clear();
            foreach (var p in petRepo.GetAll())
                petsView.Add(p);
        }

        private void DoSearch(string keyword)
        {
            petsView.Clear();
            foreach (var p in petRepo.SearchByName(keyword))
                petsView.Add(p);
        }

        private string GetSex()
        {
            var item = cbSex.SelectedItem as ComboBoxItem;
            return item?.Content?.ToString() ?? "";
        }

        // ==================== BUTTONS ====================
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text)) return;

            int ownerId = GetOwnerIdFromCombo();
            if (ownerId <= 0)
            {
                MessageBox.Show("Bạn chưa chọn Chủ nuôi hợp lệ.\nHãy chọn trong danh sách hoặc gõ để tìm rồi chọn.",
                    "Thiếu chủ nuôi", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbOwner.Focus();
                cbOwner.IsDropDownOpen = true;
                return;
            }

            var p = new Pet
            {
                OwnerId = ownerId,
                Name = (txtName.Text ?? "").Trim(),
                Species = (txtSpecies.Text ?? "").Trim(),
                Breed = (txtBreed.Text ?? "").Trim(),
                Sex = GetSex(),
            };

            try
            {
                petRepo.Insert(p);
                RefreshPets();
                ClearInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi Thêm", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgPets.SelectedItem as Pet;
            if (selected == null) return;

            int ownerId = GetOwnerIdFromCombo();
            if (ownerId <= 0)
            {
                MessageBox.Show("Bạn chưa chọn Chủ nuôi hợp lệ.", "Thiếu chủ nuôi",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                cbOwner.Focus();
                cbOwner.IsDropDownOpen = true;
                return;
            }

            selected.OwnerId = ownerId;
            selected.Name = (txtName.Text ?? "").Trim();
            selected.Species = (txtSpecies.Text ?? "").Trim();
            selected.Breed = (txtBreed.Text ?? "").Trim();
            selected.Sex = GetSex();

            try
            {
                petRepo.Update(selected);
                RefreshPets();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi Cập nhật", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgPets.SelectedItem as Pet;
            if (selected == null) return;

            if (MessageBox.Show("Xóa thú cưng này?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            try
            {
                petRepo.Delete(selected.PetId);
                RefreshPets();
                ClearInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi Xóa", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            DoSearch(txtSearch.Text);
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            RefreshPets();
        }

        // ==================== GRID SELECT ====================
        private void dgPets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var p = dgPets.SelectedItem as Pet;
            if (p == null) return;

            txtName.Text = p.Name;
            txtSpecies.Text = p.Species;
            txtBreed.Text = p.Breed;

            // set owner theo OwnerId
            cbOwner.SelectedValue = p.OwnerId;
            if (cbOwner.SelectedValue == null)
                cbOwner.Text = p.OwnerName ?? "";

            // set sex
            cbSex.SelectedIndex = -1;
            foreach (var obj in cbSex.Items)
            {
                var item = obj as ComboBoxItem;
                if (item != null && (item.Content?.ToString() ?? "") == (p.Sex ?? ""))
                {
                    cbSex.SelectedItem = item;
                    break;
                }
            }
        }

        private void ClearInput()
        {
            txtName.Clear();
            txtSpecies.Clear();
            txtBreed.Clear();

            cbSex.SelectedIndex = -1;

            cbOwner.SelectedIndex = -1;
            cbOwner.Text = "";
        }

        private void CbOwner_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}