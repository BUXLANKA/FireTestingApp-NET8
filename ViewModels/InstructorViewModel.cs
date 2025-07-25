using CommunityToolkit.Mvvm.Messaging;
using FireTestingApp_net8.Models;
using FireTestingApp_net8.Models.Shema;
using FireTestingApp_net8.Services;
using FireTestingApp_net8.Viewes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace FireTestingApp_net8.ViewModels
{
    public class InstructorViewModel : BaseViewModel
        //, IRecipient<SelectTabMessage>
    {
        // private
        private string? _welcomeMessage;
        private readonly INavigationService _navigation;

        private string? _resultSearchText;
        private ICollectionView? _resultsView;

        private string? _userAnswerSearchText;
        private ICollectionView? _userAnswersView;

        private string? _questionSearchText;
        private ICollectionView? _questionsView;

        private string? _userSearchText;
        private ICollectionView? _usersView;

        private string? _ticketSearchText;
        private ICollectionView? _ticketsView;

        //private int _selectedTabIndex;

        // constructor
        public InstructorViewModel(INavigationService navigation)
        {
            WeakReferenceMessenger.Default.Register<UpdateMessage>(this, (r, m) =>
            {
                UpdateResultTable();
                UpdateUserTable();
            });
            //UpdateResultTable();

            WelcomeMessage = $"Добро пожаловать!";

            ExitEvent = new RelayCommand(Exit);
            EditResultEvent = new RelayCommand<Result>(OnEdit);
            EditUserEvent = new RelayCommand<User>(UserEdit);
            DeleteTicketEvent = new RelayCommand<Ticket>(DeleteTicket);
            DeleteResultEvent = new RelayCommand<Result>(DeleteResult);
            CreateNewUserEvent = new RelayCommand(CreateUser);
            AddNewQuestionEvent = new RelayCommand(CreateQuestion);
            EditQuestionEvent = new RelayCommand<Question>(EditQuestion);
            DeleteQuestionEvent = new RelayCommand<Question>(DeleteQuestion);

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
            ResultsView = CollectionViewSource.GetDefaultView(ResultsTable);
            ResultsView.Filter = FilterResults;

            UserAnswerTable = TableAgent.GetUserAnswers();
            UserAnswersView = CollectionViewSource.GetDefaultView(UserAnswerTable);
            UserAnswersView.Filter = FilterUserAnswers;

            TicketTable = TableAgent.GetTickets();
            TicketsView = CollectionViewSource.GetDefaultView(TicketTable);
            TicketsView.Filter = FilterTickets;


            UserTable = TableAgent.GetUsers();
            UsersView = CollectionViewSource.GetDefaultView(UserTable);
            UsersView.Filter = FilterUsers;

            QuestionTable = TableAgent.GetQuestions();
            QuestionsView = CollectionViewSource.GetDefaultView(QuestionTable);
            QuestionsView.Filter = FilterQuestions;
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
        public string? ResultSearchText
        {
            get => _resultSearchText;
            set
            {
                _resultSearchText = value;
                OnPropertyChanged(nameof(ResultSearchText));
                ResultsView.Refresh();
            }
        }
        public ICollectionView ResultsView
        {
            get => _resultsView;
            set
            {
                _resultsView = value;
                OnPropertyChanged(nameof(ResultsView));
            }
        }

        public string? UserAnswerSearchText
        {
            get => _userAnswerSearchText;
            set
            {
                _userAnswerSearchText = value;
                OnPropertyChanged(nameof(UserAnswerSearchText));
                UserAnswersView.Refresh();
            }
        }
        public ICollectionView UserAnswersView
        {
            get => _userAnswersView;
            set
            {
                _userAnswersView = value;
                OnPropertyChanged(nameof(UserAnswersView));
            }
        }

        public string? QuestionSearchText
        {
            get => _questionSearchText;
            set
            {
                _questionSearchText = value;
                OnPropertyChanged(nameof(QuestionSearchText));
                QuestionsView.Refresh();
            }
        }
        public ICollectionView QuestionsView
        {
            get => _questionsView;
            set
            {
                _questionsView = value;
                OnPropertyChanged(nameof(QuestionsView));
            }
        }

        public string? UserSearchText
        {
            get => _userSearchText;
            set
            {
                _userSearchText = value;
                OnPropertyChanged(nameof(UserSearchText));
                UsersView.Refresh();
            }
        }
        public ICollectionView UsersView
        {
            get => _usersView;
            set
            {
                _usersView = value;
                OnPropertyChanged(nameof(UsersView));
            }
        }

        public string? TicketSearchText
        {
            get => _ticketSearchText;
            set
            {
                _ticketSearchText = value;
                OnPropertyChanged(nameof(TicketSearchText));
                TicketsView.Refresh();
            }
        }
        public ICollectionView TicketsView
        {
            get => _ticketsView;
            set
            {
                _ticketsView = value;
                OnPropertyChanged(nameof(TicketsView));
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
        public ObservableCollection<Question> QuestionTable { get; set; }

        // command
        public RelayCommand<Result> EditResultEvent { get; }
        public RelayCommand<User> EditUserEvent { get; }
        public RelayCommand<Ticket> DeleteTicketEvent { get; }
        public RelayCommand<Result> DeleteResultEvent { get; }
        public RelayCommand ExitEvent { get; }
        public RelayCommand CreateNewUserEvent { get; }
        public RelayCommand AddNewQuestionEvent { get; }
        public RelayCommand<Question> EditQuestionEvent { get; }
        public RelayCommand<Question> DeleteQuestionEvent { get; }

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
        private void DeleteQuestion(Question question)
        {
            if (question == null) return;

            MessageBoxResult msgUserChoice = MessageBox.Show(
                $"Вы действительно хотите удалить вопрос и все связанные с ним ответы?\nОтменить действие будет невозможно!",
                "Удаление вопроса",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No);

            if (msgUserChoice != MessageBoxResult.Yes)
            {
                MessageBox.Show("Процесс удаления остановлен");
                return;
            }

            try
            {
                using (var context = new AppDbContext())
                {
                    var questionToDelete = context.Questions
                        .Include(q => q.Answers)
                        .FirstOrDefault(q => q.Questionid == question.Questionid);

                    if (questionToDelete != null)
                    {
                        context.Answers.RemoveRange(questionToDelete.Answers);
                        context.Questions.Remove(questionToDelete);
                        context.SaveChanges();

                        MessageBox.Show($"Билет успешно удалён");

                        QuestionTable.Remove(question);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении:\n{ex.Message}");
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

        private void UpdateResultTable()
        {
            ResultsTable = TableAgent.GetResults();
            ResultsView = CollectionViewSource.GetDefaultView(ResultsTable);
            ResultsView.Filter = FilterResults;

            OnPropertyChanged(nameof(ResultsView));
        }
        private void UpdateUserTable()
        {
            UserTable = TableAgent.GetUsers();
            OnPropertyChanged(nameof(UserTable));
        }
        private void CreateQuestion()
        {
            _navigation.NavigateTo<QuestionEditorViewModel>();
        }
        private void EditQuestion(Question question)
        {
            if (question == null) return;

            NavigationParameterService.Set("QuestionObject", question);
            //NavigationParameterService.Set("AnswerObject", answer);
            _navigation.NavigateTo<QuestionEditorViewModel>();
        }
        private bool FilterResults(object obj)
        {
            if (obj is not Result result) return false;
            if (string.IsNullOrWhiteSpace(ResultSearchText)) return true;

            var search = ResultSearchText.Trim();

            if (result.User != null)
            {
                if (!string.IsNullOrEmpty(result.User.Firstname) &&
                    result.User.Firstname.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;

                if (!string.IsNullOrEmpty(result.User.Lastname) &&
                    result.User.Lastname.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;

                if (!string.IsNullOrEmpty(result.User.Surname) &&
                    result.User.Surname.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }

            return false;
        }
        private bool FilterUserAnswers(object obj)
        {
            if (obj is not Useranswer userAnswer) return false;
            if (string.IsNullOrWhiteSpace(UserAnswerSearchText)) return true;

            var search = UserAnswerSearchText.Trim();

            if (userAnswer.User != null)
            {
                if (!string.IsNullOrEmpty(userAnswer.User.Firstname) &&
                    userAnswer.User.Firstname.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;

                if (!string.IsNullOrEmpty(userAnswer.User.Lastname) &&
                    userAnswer.User.Lastname.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;

                if (!string.IsNullOrEmpty(userAnswer.User.Surname) &&
                    userAnswer.User.Surname.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }

            return false;
        }
        private bool FilterQuestions(object obj)
        {
            if (obj is not Question question) return false;
            if (string.IsNullOrWhiteSpace(QuestionSearchText)) return true;

            var search = QuestionSearchText.Trim();

            if (question.Questiontext != null)
            {
                if (!string.IsNullOrEmpty(question.Questiontext) &&
                    question.Questiontext.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }

            return false;
        }
        private bool FilterUsers(object obj)
        {
            if (obj is not User user) return false;
            if (string.IsNullOrWhiteSpace(UserSearchText)) return true;

            var search = UserSearchText.Trim();

            if (user.Userid != 0)
            {
                if (!string.IsNullOrEmpty(user.Firstname) &&
                    user.Firstname.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;

                if (!string.IsNullOrEmpty(user.Lastname) &&
                    user.Lastname.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;

                if (!string.IsNullOrEmpty(user.Surname) &&
                    user.Surname.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }

            return false;
        }
        private bool FilterTickets(object obj)
        {
            if (obj is not Ticket ticket) return false;
            if (string.IsNullOrWhiteSpace(TicketSearchText)) return true;

            var search = TicketSearchText.Trim();

            if (ticket.Fromuser != null)
            {
                if (!string.IsNullOrEmpty(ticket.Fromuser.Firstname) &&
                    ticket.Fromuser.Firstname.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;

                if (!string.IsNullOrEmpty(ticket.Fromuser.Lastname) &&
                    ticket.Fromuser.Lastname.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;

                if (!string.IsNullOrEmpty(ticket.Fromuser.Surname) &&
                    ticket.Fromuser.Surname.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }

            return false;
        }




    }
}
