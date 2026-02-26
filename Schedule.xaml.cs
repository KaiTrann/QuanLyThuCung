using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Nhóm_7
{
    public partial class Schedule : Window
    {
        List<Appointment> appointments = new List<Appointment>();
        List<Vaccine> vaccines = new List<Vaccine>();

        public Schedule()
        {
            InitializeComponent();

            dgAppointments.ItemsSource = appointments;
            dgVaccines.ItemsSource = vaccines;

            dgAppointments.SelectionChanged += DgAppointments_SelectionChanged;
            dgVaccines.SelectionChanged += DgVaccines_SelectionChanged;
        }

        // ====================== LỊCH HẸN ======================

        private void AddAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (dpAppointmentDate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày hẹn");
                return;
            }

            Appointment ap = new Appointment
            {
                Pet = txtPetAppointment.Text,
                Date = dpAppointmentDate.SelectedDate.Value,
                Reason = txtReason.Text,
                Status = (cbStatus.SelectedItem as ComboBoxItem)?.Content.ToString()
            };

            appointments.Add(ap);
            dgAppointments.ItemsSource = null;
            dgAppointments.ItemsSource = appointments;

            ClearAppointmentForm();
        }

        private void EditAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (dgAppointments.SelectedItem is Appointment selected)
            {
                selected.Pet = txtPetAppointment.Text;
                selected.Date = dpAppointmentDate.SelectedDate.Value;
                selected.Reason = txtReason.Text;
                selected.Status = (cbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();

                dgAppointments.ItemsSource = null;
                dgAppointments.ItemsSource = appointments;

                ClearAppointmentForm();
            }
        }

        private void DeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (dgAppointments.SelectedItem is Appointment selected)
            {
                appointments.Remove(selected);

                dgAppointments.ItemsSource = null;
                dgAppointments.ItemsSource = appointments;

                ClearAppointmentForm();
            }
        }

        // 🔍 TÌM KIẾM LỊCH HẸN
        private void SearchAppointment_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearchAppointment.Text.ToLower();

            var result = appointments
                .Where(a => a.Pet.ToLower().Contains(keyword))
                .ToList();

            dgAppointments.ItemsSource = result;
        }

        private void ShowAllAppointment_Click(object sender, RoutedEventArgs e)
        {
            dgAppointments.ItemsSource = appointments;
        }

        // 📊 THỐNG KÊ LỊCH HẸN THEO NGÀY
        private void CountByDate_Click(object sender, RoutedEventArgs e)
        {
            if (dpFilterDate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày");
                return;
            }

            DateTime selectedDate = dpFilterDate.SelectedDate.Value.Date;

            int count = appointments
                .Count(a => a.Date.Date == selectedDate);

            txtResultCount.Text = $"Số khách: {count}";
        }

        private void CountToday_Click(object sender, RoutedEventArgs e)
        {
            DateTime today = DateTime.Today;

            int count = appointments
                .Count(a => a.Date.Date == today);

            txtResultCount.Text = $"Hôm nay: {count}";
        }

        private void DgAppointments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAppointments.SelectedItem is Appointment selected)
            {
                txtPetAppointment.Text = selected.Pet;
                dpAppointmentDate.SelectedDate = selected.Date;
                txtReason.Text = selected.Reason;

                foreach (ComboBoxItem item in cbStatus.Items)
                {
                    if (item.Content.ToString() == selected.Status)
                    {
                        cbStatus.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void ClearAppointmentForm()
        {
            txtPetAppointment.Clear();
            dpAppointmentDate.SelectedDate = null;
            txtReason.Clear();
            cbStatus.SelectedIndex = -1;
        }

        // ====================== TIÊM PHÒNG ======================

        private void AddVaccine_Click(object sender, RoutedEventArgs e)
        {
            if (dpVaccineDate.SelectedDate == null || dpNextDueDate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn đủ ngày");
                return;
            }

            Vaccine vc = new Vaccine
            {
                Pet = txtPetVaccine.Text,
                VaccineName = txtVaccineName.Text,
                Date = dpVaccineDate.SelectedDate.Value,
                NextDueDate = dpNextDueDate.SelectedDate.Value
            };

            vaccines.Add(vc);

            dgVaccines.ItemsSource = null;
            dgVaccines.ItemsSource = vaccines;

            ClearVaccineForm();
        }

        private void EditVaccine_Click(object sender, RoutedEventArgs e)
        {
            if (dgVaccines.SelectedItem is Vaccine selected)
            {
                selected.Pet = txtPetVaccine.Text;
                selected.VaccineName = txtVaccineName.Text;
                selected.Date = dpVaccineDate.SelectedDate.Value;
                selected.NextDueDate = dpNextDueDate.SelectedDate.Value;

                dgVaccines.ItemsSource = null;
                dgVaccines.ItemsSource = vaccines;

                ClearVaccineForm();
            }
        }

        private void DeleteVaccine_Click(object sender, RoutedEventArgs e)
        {
            if (dgVaccines.SelectedItem is Vaccine selected)
            {
                vaccines.Remove(selected);

                dgVaccines.ItemsSource = null;
                dgVaccines.ItemsSource = vaccines;

                ClearVaccineForm();
            }
        }

        // 🔍 TÌM KIẾM TIÊM PHÒNG
        private void SearchVaccine_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearchVaccine.Text.ToLower();

            var result = vaccines
                .Where(v => v.Pet.ToLower().Contains(keyword))
                .ToList();

            dgVaccines.ItemsSource = result;
        }

        private void ShowAllVaccine_Click(object sender, RoutedEventArgs e)
        {
            dgVaccines.ItemsSource = vaccines;
        }

        // 📊 THỐNG KÊ TIÊM PHÒNG THEO NGÀY
        private void CountVaccineByDate_Click(object sender, RoutedEventArgs e)
        {
            if (dpVaccineFilterDate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày");
                return;
            }

            DateTime selectedDate = dpVaccineFilterDate.SelectedDate.Value.Date;

            int count = vaccines
                .Count(v => v.Date.Date == selectedDate);

            txtVaccineResult.Text = $"Số lượt tiêm: {count}";
        }

        private void CountVaccineToday_Click(object sender, RoutedEventArgs e)
        {
            DateTime today = DateTime.Today;

            int count = vaccines
                .Count(v => v.Date.Date == today);

            txtVaccineResult.Text = $"Hôm nay: {count}";
        }

        private void DgVaccines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgVaccines.SelectedItem is Vaccine selected)
            {
                txtPetVaccine.Text = selected.Pet;
                txtVaccineName.Text = selected.VaccineName;
                dpVaccineDate.SelectedDate = selected.Date;
                dpNextDueDate.SelectedDate = selected.NextDueDate;
            }
        }

        private void ClearVaccineForm()
        {
            txtPetVaccine.Clear();
            txtVaccineName.Clear();
            dpVaccineDate.SelectedDate = null;
            dpNextDueDate.SelectedDate = null;
        }
    }
    public class Appointment
    {
        public string Pet { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    }

    public class Vaccine
    {
        public string Pet { get; set; }
        public string VaccineName { get; set; }
        public DateTime Date { get; set; }
        public DateTime NextDueDate { get; set; }
    }
}