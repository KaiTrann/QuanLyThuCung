using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Nhóm_7
{
    public partial class Schedule : Window
    {
        private readonly ScheduleRepository repo = new ScheduleRepository();

        private int _selectedAppointmentId = 0;
        private int _selectedVaccinationId = 0;

        private List<ScheduleRepository.PetPickItem> _pets = new List<ScheduleRepository.PetPickItem>();
        private List<ScheduleRepository.VaccinePickItem> _vaccines = new List<ScheduleRepository.VaccinePickItem>();

        public Schedule()
        {
            InitializeComponent();

            LoadPickLists();
            LoadAppointments();
            LoadVaccinations();

            dgAppointments.SelectionChanged += DgAppointments_SelectionChanged;
            dgVaccines.SelectionChanged += DgVaccines_SelectionChanged;
        }

        // ===================== LOAD PICK LISTS =====================
        private void LoadPickLists()
        {
            try
            {
                // PETS
                _pets = repo.GetPetPickList();

                // FORM: Appointment
                cbPetAppointment.ItemsSource = _pets;
                cbPetAppointment.DisplayMemberPath = "PetName";
                cbPetAppointment.SelectedValuePath = "PetId";
                cbPetAppointment.SelectedIndex = -1;
                cbPetAppointment.Text = "";

                // FORM: Vaccine
                cbPetVaccine.ItemsSource = _pets;
                cbPetVaccine.DisplayMemberPath = "PetName";
                cbPetVaccine.SelectedValuePath = "PetId";
                cbPetVaccine.SelectedIndex = -1;
                cbPetVaccine.Text = "";

                // SEARCH: Appointment
                cbSearchAppointment.ItemsSource = _pets;
                cbSearchAppointment.DisplayMemberPath = "PetName";
                cbSearchAppointment.SelectedValuePath = "PetId";
                cbSearchAppointment.SelectedIndex = -1;
                cbSearchAppointment.Text = "";

                // SEARCH: Vaccine
                cbSearchVaccine.ItemsSource = _pets;
                cbSearchVaccine.DisplayMemberPath = "PetName";
                cbSearchVaccine.SelectedValuePath = "PetId";
                cbSearchVaccine.SelectedIndex = -1;
                cbSearchVaccine.Text = "";

                // VACCINES (from DB)
                _vaccines = repo.GetVaccinePickList();
                cbVaccineName.ItemsSource = _vaccines;
                cbVaccineName.DisplayMemberPath = "VaccineName";
                cbVaccineName.SelectedValuePath = "VaccineId";
                cbVaccineName.SelectedIndex = -1;
                cbVaccineName.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load danh sách:\n" + ex.Message,
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int GetPetIdFromCombo(ComboBox cb)
        {
            if (cb.SelectedItem is ScheduleRepository.PetPickItem si)
                return si.PetId;

            string text = (cb.Text ?? "").Trim();
            if (text.Length == 0) return 0;

            var match = _pets.FirstOrDefault(x =>
                string.Equals((x.PetName ?? "").Trim(), text, StringComparison.CurrentCultureIgnoreCase));

            if (match != null) return match.PetId;

            int id;
            if (int.TryParse(text, out id) && id > 0) return id;

            return 0;
        }

        private int GetVaccineIdFromCombo(ComboBox cb)
        {
            if (cb.SelectedItem is ScheduleRepository.VaccinePickItem si)
                return si.VaccineId;

            string text = (cb.Text ?? "").Trim();
            if (text.Length == 0) return 0;

            var match = _vaccines.FirstOrDefault(x =>
                string.Equals((x.VaccineName ?? "").Trim(), text, StringComparison.CurrentCultureIgnoreCase));

            if (match != null) return match.VaccineId;

            int id;
            if (int.TryParse(text, out id) && id > 0) return id;

            return 0;
        }

        // ===================== LOAD GRID =====================
        private void LoadAppointments(string keyword = null)
        {
            dgAppointments.ItemsSource = repo.GetAppointments(keyword);
        }

        private void LoadVaccinations(string keyword = null)
        {
            dgVaccines.ItemsSource = repo.GetVaccinations(keyword);
        }

        // ====================== LỊCH HẸN ======================
        private void AddAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (dpAppointmentDate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày hẹn");
                return;
            }

            int petId = GetPetIdFromCombo(cbPetAppointment);
            if (petId <= 0)
            {
                MessageBox.Show("Vui lòng chọn Thú cưng hợp lệ (chọn từ danh sách).",
                    "Thiếu dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbPetAppointment.Focus();
                cbPetAppointment.IsDropDownOpen = true;
                return;
            }

            string reason = (txtReason.Text ?? "").Trim();
            string status = (cbStatus.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";

            try
            {
                repo.InsertAppointment(petId, dpAppointmentDate.SelectedDate.Value, reason, status);

                LoadAppointments();
                ClearAppointmentForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAppointmentId <= 0)
            {
                MessageBox.Show("Hãy chọn 1 dòng lịch hẹn để sửa.");
                return;
            }

            if (dpAppointmentDate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày hẹn");
                return;
            }

            int petId = GetPetIdFromCombo(cbPetAppointment);
            if (petId <= 0)
            {
                MessageBox.Show("Vui lòng chọn Thú cưng hợp lệ (chọn từ danh sách).",
                    "Thiếu dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbPetAppointment.Focus();
                cbPetAppointment.IsDropDownOpen = true;
                return;
            }

            string reason = (txtReason.Text ?? "").Trim();
            string status = (cbStatus.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";

            try
            {
                repo.UpdateAppointment(_selectedAppointmentId, petId, dpAppointmentDate.SelectedDate.Value, reason, status);

                LoadAppointments();
                ClearAppointmentForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAppointmentId <= 0)
            {
                MessageBox.Show("Hãy chọn 1 dòng lịch hẹn để xóa.");
                return;
            }

            if (MessageBox.Show("Xóa lịch hẹn này?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            try
            {
                repo.DeleteAppointment(_selectedAppointmentId);

                LoadAppointments();
                ClearAppointmentForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchAppointment_Click(object sender, RoutedEventArgs e)
        {
            string kw = (cbSearchAppointment.Text ?? "").Trim();
            LoadAppointments(kw);
        }

        private void ShowAllAppointment_Click(object sender, RoutedEventArgs e)
        {
            cbSearchAppointment.SelectedIndex = -1;
            cbSearchAppointment.Text = "";
            LoadAppointments();
        }

        private void CountByDate_Click(object sender, RoutedEventArgs e)
        {
            if (dpFilterDate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày");
                return;
            }

            int count = repo.CountAppointmentsByDate(dpFilterDate.SelectedDate.Value);
            txtResultCount.Text = $"Số khách: {count}";
        }

        private void CountToday_Click(object sender, RoutedEventArgs e)
        {
            int count = repo.CountAppointmentsByDate(DateTime.Today);
            txtResultCount.Text = $"Hôm nay: {count}";
        }

        private void DgAppointments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = dgAppointments.SelectedItem as ScheduleRepository.AppointmentRow;
            if (selected == null) return;

            _selectedAppointmentId = selected.AppointmentId;

            cbPetAppointment.SelectedIndex = -1;
            cbPetAppointment.Text = selected.Pet;

            dpAppointmentDate.SelectedDate = selected.Date;
            txtReason.Text = selected.Reason;

            cbStatus.SelectedIndex = -1;
            foreach (ComboBoxItem item in cbStatus.Items)
            {
                if ((item.Content?.ToString() ?? "") == (selected.Status ?? ""))
                {
                    cbStatus.SelectedItem = item;
                    break;
                }
            }
        }

        private void ClearAppointmentForm()
        {
            _selectedAppointmentId = 0;

            cbPetAppointment.SelectedIndex = -1;
            cbPetAppointment.Text = "";

            dpAppointmentDate.SelectedDate = null;
            txtReason.Clear();
            cbStatus.SelectedIndex = -1;

            dgAppointments.SelectedItem = null;
        }

        // ====================== TIÊM PHÒNG ======================
        private void AddVaccine_Click(object sender, RoutedEventArgs e)
        {
            if (dpVaccineDate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày tiêm");
                return;
            }

            int petId = GetPetIdFromCombo(cbPetVaccine);
            if (petId <= 0)
            {
                MessageBox.Show("Vui lòng chọn Thú cưng hợp lệ (chọn từ danh sách).",
                    "Thiếu dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbPetVaccine.Focus();
                cbPetVaccine.IsDropDownOpen = true;
                return;
            }

            int vaccineId = GetVaccineIdFromCombo(cbVaccineName);
            if (vaccineId <= 0)
            {
                MessageBox.Show("Vui lòng chọn Vắc-xin hợp lệ (chọn từ danh sách).",
                    "Thiếu dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbVaccineName.Focus();
                cbVaccineName.IsDropDownOpen = true;
                return;
            }

            try
            {
                repo.InsertVaccination(
                    petId,
                    vaccineId,
                    dpVaccineDate.SelectedDate.Value,
                    dpNextDueDate.SelectedDate.HasValue ? dpNextDueDate.SelectedDate.Value : (DateTime?)null
                );

                LoadVaccinations();
                ClearVaccineForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditVaccine_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedVaccinationId <= 0)
            {
                MessageBox.Show("Hãy chọn 1 dòng tiêm phòng để sửa.");
                return;
            }

            if (dpVaccineDate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày tiêm");
                return;
            }

            int petId = GetPetIdFromCombo(cbPetVaccine);
            if (petId <= 0)
            {
                MessageBox.Show("Vui lòng chọn Thú cưng hợp lệ (chọn từ danh sách).",
                    "Thiếu dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbPetVaccine.Focus();
                cbPetVaccine.IsDropDownOpen = true;
                return;
            }

            int vaccineId = GetVaccineIdFromCombo(cbVaccineName);
            if (vaccineId <= 0)
            {
                MessageBox.Show("Vui lòng chọn Vắc-xin hợp lệ (chọn từ danh sách).",
                    "Thiếu dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbVaccineName.Focus();
                cbVaccineName.IsDropDownOpen = true;
                return;
            }

            try
            {
                repo.UpdateVaccination(
                    _selectedVaccinationId,
                    petId,
                    vaccineId,
                    dpVaccineDate.SelectedDate.Value,
                    dpNextDueDate.SelectedDate.HasValue ? dpNextDueDate.SelectedDate.Value : (DateTime?)null
                );

                LoadVaccinations();
                ClearVaccineForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteVaccine_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedVaccinationId <= 0)
            {
                MessageBox.Show("Hãy chọn 1 dòng tiêm phòng để xóa.");
                return;
            }

            if (MessageBox.Show("Xóa lượt tiêm này?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            try
            {
                repo.DeleteVaccination(_selectedVaccinationId);

                LoadVaccinations();
                ClearVaccineForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchVaccine_Click(object sender, RoutedEventArgs e)
        {
            string kw = (cbSearchVaccine.Text ?? "").Trim();
            LoadVaccinations(kw);
        }

        private void ShowAllVaccine_Click(object sender, RoutedEventArgs e)
        {
            cbSearchVaccine.SelectedIndex = -1;
            cbSearchVaccine.Text = "";
            LoadVaccinations();
        }

        private void CountVaccineByDate_Click(object sender, RoutedEventArgs e)
        {
            if (dpVaccineFilterDate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày");
                return;
            }

            int count = repo.CountVaccinationsByDate(dpVaccineFilterDate.SelectedDate.Value);
            txtVaccineResult.Text = $"Số lượt tiêm: {count}";
        }

        private void CountVaccineToday_Click(object sender, RoutedEventArgs e)
        {
            int count = repo.CountVaccinationsByDate(DateTime.Today);
            txtVaccineResult.Text = $"Hôm nay: {count}";
        }

        private void DgVaccines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = dgVaccines.SelectedItem as ScheduleRepository.VaccinationRow;
            if (selected == null) return;

            _selectedVaccinationId = selected.VaccinationId;

            cbPetVaccine.SelectedIndex = -1;
            cbPetVaccine.Text = selected.Pet;

            cbVaccineName.SelectedIndex = -1;
            cbVaccineName.Text = selected.VaccineName ?? "";

            dpVaccineDate.SelectedDate = selected.Date;
            dpNextDueDate.SelectedDate = selected.NextDueDate;
        }

        private void ClearVaccineForm()
        {
            _selectedVaccinationId = 0;

            cbPetVaccine.SelectedIndex = -1;
            cbPetVaccine.Text = "";

            cbVaccineName.SelectedIndex = -1;
            cbVaccineName.Text = "";

            dpVaccineDate.SelectedDate = null;
            dpNextDueDate.SelectedDate = null;

            dgVaccines.SelectedItem = null;
        }
    }
}