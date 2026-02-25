using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Nhóm_7
{
    public partial class PetCareManage : Window
    {
        private ObservableCollection<Pet> pets = new ObservableCollection<Pet>();
        private ObservableCollection<Pet> petsView = new ObservableCollection<Pet>();

        public MainWindow()
        {
            InitializeComponent();
            dgPets.ItemsSource = petsView;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text)) return;

            string sex = "";
            if (cbSex.SelectedItem is ComboBoxItem item)
                sex = item.Content.ToString();

            Pet p = new Pet
            {
                Name = txtName.Text,
                Species = txtSpecies.Text,
                Breed = txtBreed.Text,
                Sex = sex,
                Owner = txtOwner.Text
            };

            pets.Add(p);
            RefreshView();
            ClearInput();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgPets.SelectedItem is Pet p)
            {
                string sex = "";
                if (cbSex.SelectedItem is ComboBoxItem item)
                    sex = item.Content.ToString();

                p.Name = txtName.Text;
                p.Species = txtSpecies.Text;
                p.Breed = txtBreed.Text;
                p.Sex = sex;
                p.Owner = txtOwner.Text;

                dgPets.Items.Refresh();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgPets.SelectedItem is Pet p)
            {
                pets.Remove(p);
                RefreshView();
                ClearInput();
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string key = txtSearch.Text.ToLower();
            petsView.Clear();

            foreach (var p in pets.Where(x => x.Name != null && x.Name.ToLower().Contains(key)))
            {
                petsView.Add(p);
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            RefreshView();
        }

        private void dgPets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPets.SelectedItem is Pet p)
            {
                txtName.Text = p.Name;
                txtSpecies.Text = p.Species;
                txtBreed.Text = p.Breed;
                txtOwner.Text = p.Owner;

                foreach (ComboBoxItem item in cbSex.Items)
                {
                    if (item.Content.ToString() == p.Sex)
                    {
                        cbSex.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void RefreshView()
        {
            petsView.Clear();
            foreach (var p in pets)
                petsView.Add(p);
        }

        private void ClearInput()
        {
            txtName.Clear();
            txtSpecies.Clear();
            txtBreed.Clear();
            txtOwner.Clear();
            cbSex.SelectedIndex = -1;
        }
    }

    public class Pet
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public string Sex { get; set; }
        public string Owner { get; set; }
    }
}