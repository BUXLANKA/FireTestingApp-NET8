using CommunityToolkit.Mvvm.Messaging;
using FireTestingApp_net8.Models;
using FireTestingApp_net8.Models.Shema;
using FireTestingApp_net8.Services;
using FireTestingApp_net8.Viewes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.ObjectModel;
using System.Windows;

namespace FireTestingApp_net8.ViewModels
{
    public class InstructorViewModel : BaseViewModel
        //, IRecipient<SelectTabMessage>
    {
        // private
        private string? _welcomeMessage;
        private readonly INavigationService _navigation;
        //private int _selectedTabIndex;

        // constructor
        public InstructorViewModel(INavigationService navigation)
        {
            WelcomeMessage = $"Добро пожаловать!";

            ExitEvent = new RelayCommand(Exit);
            EditResultEvent = new RelayCommand<Result>(OnEdit);
            EditUserEvent = new RelayCommand<User>(UserEdit);
            DeleteTicketEvent = new RelayCommand<Ticket>(DeleteTicket);
            DeleteResultEvent = new RelayCommand<Result>(DeleteResult);
            CreateNewUserEvent = new RelayCommand(CreateUser);

            _navigation = navigation;

            //WeakReferenceMessenger.Default.Register(this);

            //using (var Context = new AppDbContext())
            //{
            //    var ResultsList = Context.Results
            //        .Include(r => r.User)
            //        .Include(r => r.Status)
            //        .ToList();
            //    ResultsTable = new ObservableCollection<Result>(ResultsList);

            //    var UserAnswerList = Context.Useranswers
            //        .Include(r => r.User)
            //        .Include(r => r.Question)
            //        .Include(r => r.Answer)
            //        .ToList();
            //    UserAnswerTable = new ObservableCollection<Useranswer>(UserAnswerList);

            //    var TicketList = Context.Tickets
            //        .Include(r => r.Fromuser)
            //        .ToList();
            //    TicketTable = new ObservableCollection<Ticket>(TicketList);

            //    var userList = Context.Users
            //        .Include(r => r.Role)
            //        .ToList();
            //    UserTable = new ObservableCollection<User>(userList);
            //}

            ResultsTable = TableAgent.GetResults();
            UserAnswerTable = TableAgent.GetUserAnswers();
            TicketTable = TableAgent.GetTickets();
            UserTable = TableAgent.GetUsers();
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
        //public int SelectedTabIndex
        //{
        //    get => _selectedTabIndex;
        //    set => SetProperty(ref _selectedTabIndex, value);
        //}

        // collection

        // todo сделать эту хуйню через private - public свойства. иначе нихуя не работает

        public ObservableCollection<Result> ResultsTable { get; set; }
        public ObservableCollection<Useranswer> UserAnswerTable { get; set; }
        public ObservableCollection<Ticket> TicketTable { get; set; }
        public ObservableCollection<User> UserTable { get; set; }

        // command
        public RelayCommand<Result> EditResultEvent { get; }
        public RelayCommand<User> EditUserEvent { get; }
        public RelayCommand<Ticket> DeleteTicketEvent { get; }
        public RelayCommand<Result> DeleteResultEvent { get; }
        public RelayCommand ExitEvent { get; }
        public RelayCommand CreateNewUserEvent { get; }

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

        //public void Receive(SelectTabMessage message)
        //{
        //    SelectedTabIndex = message.TabIndex;
        //}

        private void DeleteTicket(Ticket ticket)
        {
            if (ticket == null) return;

            MessageBoxResult msgUserChoice = MessageBox.Show(
                $"Вы действительно хотите удалить строку? Отменить действие будет невозможно!",
                "Удаление строки",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No);

            try
            {
                if (msgUserChoice == MessageBoxResult.Yes)
                {
                    using (var context = new AppDbContext())
                    {
                        context.Tickets.Remove(ticket);
                        context.SaveChanges();
                        TicketTable.Remove(ticket);
                    }
                }
                else
                {
                    MessageBox.Show("Процесс удаления остановлен");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса\n{ex.Message}");
                return;
                throw;
            }
        }
        private void DeleteResult(Result result)
        {
            if (result == null) return;

            MessageBoxResult msgUserChoice = MessageBox.Show(
                        $"Вы действительно хотите удалить строку? Отменить действие будет невозможно!",
                        "Удаление строки",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning,
                        MessageBoxResult.No);

            try
            {
                if (msgUserChoice == MessageBoxResult.Yes)
                {
                    using (var context = new AppDbContext())
                    {
                        context.Results.Remove(result);
                        context.SaveChanges();
                        ResultsTable.Remove(result);
                    }
                }
                else
                {
                    MessageBox.Show("Процесс удаления остановлен");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса\n{ex.Message}");
                return;
                throw;
            }
        }
        private void UserEdit(User user)
        {
            if (user == null) return;

            NavigationParameterService.Set("UserKeyObject", user);
            _navigation.NavigateTo<UserEditorViewModel>();
        }
        private void CreateUser()
        {
            NavigationParameterService.Set("UserKeyObject", new User());
            _navigation.NavigateTo<UserEditorViewModel>();
        }
    }
}
