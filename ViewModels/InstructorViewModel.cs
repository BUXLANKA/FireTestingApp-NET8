using FireTestingApp_net8.Models.Shema;
using FireTestingApp_net8.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;

namespace FireTestingApp_net8.ViewModels
{
    public class InstructorViewModel : BaseViewModel
    {
        // private
        private string? _welcomeMessage;
        private readonly INavigationService _navigation;

        // constructor
        public InstructorViewModel(INavigationService navigation)
        {
            WelcomeMessage = $"Добро пожаловать!";

            ExitEvent = new RelayCommand(Exit);
            EditResultEvent = new RelayCommand<Result>(OnEdit);
            DeleteTicketEvent = new RelayCommand<Ticket>(DeleteTicket);

            _navigation = navigation;

            using (var Context = new AppDbContext())
            {
                var ResultsList = Context.Results
                    .Include(r => r.User)
                    .Include(r => r.Status)
                    .ToList();
                ResultsTable = new ObservableCollection<Result>(ResultsList);

                var UserAnswerList = Context.Useranswers
                    .Include(r => r.User)
                    .Include(r => r.Question)
                    .Include(r => r.Answer)
                    .ToList();
                UserAnswerTable = new ObservableCollection<Useranswer>(UserAnswerList);

                var TicketList = Context.Tickets
                    .Include(r => r.Fromuser)
                    .ToList();
                TicketTable = new ObservableCollection<Ticket>(TicketList);
            }
        }

        // public
        public string? WelcomeMessage
        {
            get => _welcomeMessage;
            set
            {
                _welcomeMessage = value;
                OnPropertyChanged(nameof(WelcomeMessage));
            }
        }

        // collection
        public ObservableCollection<Result> ResultsTable { get; set; }
        public ObservableCollection<Useranswer> UserAnswerTable { get; set; }
        public ObservableCollection<Ticket> TicketTable { get; set; }

        // command
        public RelayCommand<Result> EditResultEvent { get; }
        public RelayCommand ExitEvent { get; }
        public RelayCommand<Ticket> DeleteTicketEvent { get; }

        // logic
        private void Exit()
        {
            _navigation.NavigateTo<LoginViewModel>();
        }
        private void OnEdit(Result result)
        {
            if (result == null) return;

            NavigationParameterService.Set("SelectedResult", result);
            _navigation.NavigateTo<ResultsEditorViewModel>();
        }
        private void DeleteTicket(Ticket ticket)
        {
            if (ticket == null) return;

            try
            {
                using (var context = new AppDbContext())
                {
                    context.Tickets.Remove(ticket);
                    context.SaveChanges();
                    TicketTable.Remove(ticket);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при обработке команды\n{ex.Message}");
                throw;
            }
        }
    }
}
