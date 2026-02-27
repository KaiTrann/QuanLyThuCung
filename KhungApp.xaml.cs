using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

namespace Nhóm_7
{
    public partial class KhungApp : Window
    {
        public KhungApp()
        {
            InitializeComponent();
            DataContext = new KhungAppViewModel();
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            var vm = DataContext as KhungAppViewModel;
            if (vm == null) return;

            var key = (vm.SearchText ?? "").Trim();
            if (key.Length == 0) return;

            if (key.Any(char.IsDigit))
                new OwnersWindow(key) { Owner = this }.Show();
            else
                new PetCareManage(key) { Owner = this }.Show();
        }
    }

    public class KhungAppViewModel : INotifyPropertyChanged
    {
        private object _currentView;
        private string _searchText = "";

        public object CurrentView
        {
            get { return _currentView; }
            set { if (_currentView == value) return; _currentView = value; OnPropertyChanged(nameof(CurrentView)); }
        }

        public string SearchText
        {
            get { return _searchText; }
            set { if (_searchText == value) return; _searchText = value ?? ""; OnPropertyChanged(nameof(SearchText)); }
        }

        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowPetsCommand { get; }
        public ICommand ShowOwnersCommand { get; }
        public ICommand ShowAppointmentsCommand { get; }

        public KhungAppViewModel()
        {
            var dashboard = new DashboardView();

            ShowDashboardCommand = new RelayCommand(_ =>
            {
                dashboard.Refresh();
                CurrentView = dashboard;
            });

            ShowPetsCommand = new RelayCommand(_ =>
            {
                new PetCareManage().Show();
            });

            ShowOwnersCommand = new RelayCommand(_ =>
            {
                new OwnersWindow().Show();
            });

            ShowAppointmentsCommand = new RelayCommand(_ =>
            {
                new Schedule().Show();
            });

            dashboard.Refresh();
            CurrentView = dashboard;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            var h = PropertyChanged;
            if (h != null) h(this, new PropertyChangedEventArgs(name));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) { return _canExecute == null || _canExecute(parameter); }
        public void Execute(object parameter) { _execute(parameter); }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    // ======================= DASHBOARD VIEW (NẰM TRONG KHUNG APP) =======================
    public class DashboardView : UserControl
    {
        private readonly DashboardRepository _repo = new DashboardRepository();

        private TextBlock _txtOwnerValue;
        private TextBlock _txtPetValue;
        private TextBlock _txtApptValue;

        public DashboardView()
        {
            var root = new Grid();
            root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(12) });
            root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var title = new TextBlock
            {
                Text = "Tổng quan",
                FontSize = 18,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(2, 0, 0, 0),
                Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0x11, 0x18, 0x27))
            };
            Grid.SetRow(title, 0);

            var cards = new UniformGrid { Columns = 3 };
            Grid.SetRow(cards, 2);

            cards.Children.Add(MakeCard("Chủ nuôi", out _txtOwnerValue, "Tổng số chủ nuôi"));
            cards.Children.Add(MakeCard("Thú cưng", out _txtPetValue, "Tổng số thú cưng"));
            cards.Children.Add(MakeCard("Lịch hẹn", out _txtApptValue, "Lịch hẹn sắp tới"));

            root.Children.Add(title);
            root.Children.Add(cards);

            Content = root;
        }

        public void Refresh()
        {
            try
            {
                int owners = _repo.CountOwners();
                int pets = _repo.CountPets();
                int upcoming = _repo.CountUpcomingAppointments();

                _txtOwnerValue.Text = owners.ToString();
                _txtPetValue.Text = pets.ToString();
                _txtApptValue.Text = upcoming.ToString();
            }
            catch
            {
                _txtOwnerValue.Text = "—";
                _txtPetValue.Text = "—";
                _txtApptValue.Text = "—";
            }
        }

        private Border MakeCard(string title, out TextBlock valueText, string sub)
        {
            var b = new Border
            {
                CornerRadius = new CornerRadius(18),
                Background = System.Windows.Media.Brushes.White,
                BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0xE6, 0xE8, 0xF0)),
                BorderThickness = new Thickness(1),
                Padding = new Thickness(16),
                Margin = new Thickness(0, 0, 12, 0),
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    BlurRadius = 18,
                    ShadowDepth = 4,
                    Opacity = 0.10,
                    Color = System.Windows.Media.Colors.Black
                }
            };

            var sp = new StackPanel();
            sp.Children.Add(new TextBlock { Text = title, FontSize = 12, Opacity = 0.7 });

            valueText = new TextBlock
            {
                Text = "—",
                FontSize = 26,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 6, 0, 6)
            };
            sp.Children.Add(valueText);

            sp.Children.Add(new TextBlock { Text = sub, FontSize = 12, Opacity = 0.7 });

            b.Child = sp;
            return b;
        }
    }
}