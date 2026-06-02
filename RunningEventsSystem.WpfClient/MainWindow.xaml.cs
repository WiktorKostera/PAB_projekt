using RunningEventsSystem.SharedKernel.Dto;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace RunningEventsSystem.WpfClient
{
    public partial class MainWindow : Window
    {
        private readonly ApiClient _api = new();
        private List<EventDto> _activeEvents = new();
        private List<EventDto> _resultEvents = new();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadEvents();
            await LoadUsers();
        }

        // ── EVENTY ──────────────────────────────────────────
        private async Task LoadEvents()
        {
            try
            {
                var events = await _api.GetEventsAsync();
                var allEvents = events ?? new List<EventDto>();

                _activeEvents = allEvents
                    .Where(x => x.IsActive && x.RegistrationDeadline >= DateTime.Now)
                    .OrderBy(x => x.EventDate)
                    .ToList();

                _resultEvents = allEvents
                    .Where(x => !x.IsActive || x.EventDate < DateTime.Now)
                    .OrderByDescending(x => x.EventDate)
                    .ToList();

                EventsGrid.ItemsSource = _activeEvents;
                CmbEvents.ItemsSource = _activeEvents;
                CmbResultsEvent.ItemsSource = _resultEvents;
                TxtEventStatus.Text = $"Dostępne biegi: {_activeEvents.Count}";
            }
            catch (Exception ex)
            {
                TxtEventStatus.Text = $"Błąd: {ex.Message}";
            }
        }

        private async void BtnRefreshEvents_Click(object sender, RoutedEventArgs e)
            => await LoadEvents();

        private void EventsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EventsGrid.SelectedItem is EventDto ev)
                CmbEvents.SelectedItem = CmbEvents.Items.OfType<EventDto>().FirstOrDefault(x => x.Id == ev.Id);
        }

        // ── UCZESTNICY ──────────────────────────────────────
        private async Task LoadUsers()
        {
            try
            {
                var users = await _api.GetUsersAsync();
                UsersGrid.ItemsSource = users;
                CmbUsers.ItemsSource = users;
            }
            catch (Exception ex)
            {
                TxtUserStatus.Text = $"Błąd: {ex.Message}";
                TxtUserStatus.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private async void BtnRefreshUsers_Click(object sender, RoutedEventArgs e)
            => await LoadUsers();

        private async void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtFirstName.Text)
                || string.IsNullOrWhiteSpace(TxtLastName.Text)
                || string.IsNullOrWhiteSpace(TxtEmail.Text)
                || string.IsNullOrWhiteSpace(TxtPassword.Password))
            {
                TxtUserStatus.Text = "❌ Wypełnij imię, nazwisko, email i hasło";
                TxtUserStatus.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            if (TxtPassword.Password.Length < 6)
            {
                TxtUserStatus.Text = "❌ Hasło musi mieć minimum 6 znaków";
                TxtUserStatus.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            var dto = new CreateUserDto
            {
                FirstName = TxtFirstName.Text,
                LastName = TxtLastName.Text,
                Email = TxtEmail.Text,
                Password = TxtPassword.Password
            };

            try
            {
                var resp = await _api.CreateUserAsync(dto);
                if (resp.IsSuccessStatusCode)
                {
                    TxtUserStatus.Text = "✅ Dodano uczestnika!";
                    TxtUserStatus.Foreground = System.Windows.Media.Brushes.Green;
                    TxtFirstName.Clear(); TxtLastName.Clear(); TxtEmail.Clear(); TxtPassword.Clear();
                    await LoadUsers();
                }
                else
                {
                    var msg = await resp.Content.ReadAsStringAsync();
                    TxtUserStatus.Text = $"❌ {FormatApiError(msg)}";
                    TxtUserStatus.Foreground = System.Windows.Media.Brushes.Red;
                }
            }
            catch (Exception ex)
            {
                TxtUserStatus.Text = $"❌ {ex.Message}";
                TxtUserStatus.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        // ── REJESTRACJE ─────────────────────────────────────
        private async void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (CmbEvents.SelectedItem is not EventDto selectedEvent)
            {
                TxtRegStatus.Text = "❌ Wybierz event"; TxtRegStatus.Foreground = System.Windows.Media.Brushes.Red; return;
            }

            if (!selectedEvent.IsActive || selectedEvent.RegistrationDeadline < DateTime.Now)
            {
                TxtRegStatus.Text = "❌ Zapisy na ten bieg są już zamknięte";
                TxtRegStatus.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            if (CmbUsers.SelectedItem == null)
            {
                TxtRegStatus.Text = "❌ Wybierz uczestnika"; TxtRegStatus.Foreground = System.Windows.Media.Brushes.Red; return;
            }

            if (CmbUsers.SelectedItem is not UserDto selectedUser)
            {
                TxtRegStatus.Text = "❌ Wybierz uczestnika";
                TxtRegStatus.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            var dto = new CreateRegistrationDto { EventId = selectedEvent.Id, UserId = selectedUser.Id };

            try
            {
                var resp = await _api.CreateRegistrationAsync(dto);
                if (resp.IsSuccessStatusCode)
                {
                    TxtRegStatus.Text = "✅ Zapisano na bieg!";
                    TxtRegStatus.Foreground = System.Windows.Media.Brushes.Green;
                }
                else
                {
                    var msg = await resp.Content.ReadAsStringAsync();
                    TxtRegStatus.Text = $"❌ {msg}";
                    TxtRegStatus.Foreground = System.Windows.Media.Brushes.Red;
                }
            }
            catch (Exception ex)
            {
                TxtRegStatus.Text = $"❌ {ex.Message}";
                TxtRegStatus.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private async void BtnShowRegistrations_Click(object sender, RoutedEventArgs e)
        {
            if (CmbEvents.SelectedItem is not EventDto selectedEvent) return;
            try
            {
                var regs = await _api.GetRegistrationsByEventAsync(selectedEvent.Id);
                RegistrationsGrid.ItemsSource = regs;
            }
            catch (Exception ex)
            {
                TxtRegStatus.Text = $"❌ {ex.Message}";
            }
        }

        private async void BtnCancelReg_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var result = MessageBox.Show("Anulować rejestrację?", "Potwierdzenie", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    await _api.CancelRegistrationAsync(id);
                    if (CmbEvents.SelectedItem is EventDto ev)
                    {
                        var regs = await _api.GetRegistrationsByEventAsync(ev.Id);
                        RegistrationsGrid.ItemsSource = regs;
                    }
                }
            }
        }

        // ── WYNIKI ───────────────────────────────────────────
        private async void BtnShowResults_Click(object sender, RoutedEventArgs e)
        {
            if (CmbResultsEvent.SelectedItem is not EventDto selectedEvent) return;
            try
            {
                var results = await _api.GetResultsByEventAsync(selectedEvent.Id);
                ResultsGrid.ItemsSource = results;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}");
            }
        }

        private static string FormatApiError(string responseBody)
        {
            if (string.IsNullOrWhiteSpace(responseBody))
                return "Nie udało się wykonać operacji";

            try
            {
                using var document = JsonDocument.Parse(responseBody);
                var root = document.RootElement;

                if (root.TryGetProperty("errors", out var errors))
                {
                    var messages = new List<string>();
                    foreach (var property in errors.EnumerateObject())
                    {
                        foreach (var message in property.Value.EnumerateArray())
                            messages.Add(message.GetString() ?? string.Empty);
                    }

                    return string.Join(Environment.NewLine, messages.Where(x => !string.IsNullOrWhiteSpace(x)));
                }

                if (root.TryGetProperty("title", out var title))
                    return title.GetString() ?? responseBody;
            }
            catch (JsonException)
            {
            }

            return responseBody;
        }
    }
}
