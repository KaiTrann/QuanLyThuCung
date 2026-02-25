using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace QLTHUCUNG_NGUOI4
{
    public partial class MainWindow : Window
    {
        List<Appointment> appointments = new List<Appointment>();
        List<Vaccine> vaccines = new List<Vaccine>();

        public MainWindow()
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
            dgAppointments.Items.Refresh();
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

                dgAppointments.Items.Refresh();
                ClearAppointmentForm();
            }
        }

        private void DeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (dgAppointments.SelectedItem is Appointment selected)
            {
                appointments.Remove(selected);
                dgAppointments.Items.Refresh();
                ClearAppointmentForm();
            }
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
            dgVaccines.Items.Refresh();
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

                dgVaccines.Items.Refresh();
                ClearVaccineForm();
            }
        }

        private void DeleteVaccine_Click(object sender, RoutedEventArgs e)
        {
            if (dgVaccines.SelectedItem is Vaccine selected)
            {
                vaccines.Remove(selected);
                dgVaccines.Items.Refresh();
                ClearVaccineForm();
            }
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

        private void FilterDue_Click(object sender, RoutedEventArgs e)
        {
            DateTime today = DateTime.Today;
            var dueSoon = vaccines
                .Where(v => v.NextDueDate <= today.AddDays(7))
                .ToList();

            dgVaccines.ItemsSource = dueSoon;
        }
    }
}