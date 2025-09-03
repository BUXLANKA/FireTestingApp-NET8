using CommunityToolkit.Mvvm.Messaging;
using FireTestingApp_net8.Models;
using FireTestingApp_net8.Models.Shema;
using FireTestingApp_net8.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace FireTestingApp_net8.ViewModels
{
    public class InstructorViewModel : BaseViewModel
    {
        // private
        private string? _welcomeMessage;
        private string? _resultSearchText;
        private string? _userAnswerSearchText;
        private string? _questionSearchText;
        private string? _userSearchText;
        private string? _ticketSearchText;

        private ICollectionView? _resultsView;
        private ICollectionView? _userAnswersView;
        private ICollectionView? _questionsView;
        private ICollectionView? _usersView;
        private ICollectionView? _ticketsView;

        private readonly INavigationService _navigation;
        private readonly IMessageService _message;

        // constructor
        public InstructorViewModel(INavigationService navigation, IMessageService messageService)
        {
            WeakReferenceMessenger.Default.Register<UpdateMessage>(this, (r, m) =>
            {
                UpdateQuestionTable();
                UpdateResultTable();
                UpdateUserTable();
            });

            WelcomeMessage = $"Добро пожаловать!";

            ExitEvent = new RelayCommand(Exit);

            EditUserEvent = new RelayCommand<User>(EditUser);
            EditResultEvent = new RelayCommand<Result>(EditResult);
            EditQuestionEvent = new RelayCommand<Question>(EditQuestion);

            CreateNewUserEvent = new RelayCommand(CreateUser);
            AddNewQuestionEvent = new RelayCommand(CreateQuestion);

            DeleteTicketEvent = new RelayCommand<Ticket>(DeleteTicket);
            DeleteResultEvent = new RelayCommand<Result>(DeleteResult);
            DeleteQuestionEvent = new RelayCommand<Question>(DeleteQuestion);

            UpdateQuestionTable();
            UpdateResultTable();
            UpdateAnswerTable();
            UpdateTicketTable();
            UpdateUserTable();


            _navigation = navigation;
            _message = messageService;
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

        public ICollectionView ResultsView
        {
            get => _resultsView;
            set
            {
                _resultsView = value;
                OnPropertyChanged(nameof(ResultsView));
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
        public ICollectionView QuestionsView
        {
            get => _questionsView;
            set
            {
                _questionsView = value;
                OnPropertyChanged(nameof(QuestionsView));
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
        public ICollectionView TicketsView
        {
            get => _ticketsView;
            set
            {
                _ticketsView = value;
                OnPropertyChanged(nameof(TicketsView));
            }
        }

        // collection
        public ObservableCollection<Result>? ResultsTable { get; set; }
        public ObservableCollection<Useranswer>? UserAnswerTable { get; set; }
        public ObservableCollection<Ticket>? TicketTable { get; set; }
        public ObservableCollection<User>? UserTable { get; set; }
        public ObservableCollection<Question>? QuestionTable { get; set; }

        // command
        public RelayCommand ExitEvent { get; }
        public RelayCommand CreateNewUserEvent { get; }
        public RelayCommand AddNewQuestionEvent { get; }

        public RelayCommand<Result> EditResultEvent { get; }
        public RelayCommand<User> EditUserEvent { get; }
        public RelayCommand<Ticket> DeleteTicketEvent { get; }
        public RelayCommand<Result> DeleteResultEvent { get; }
        public RelayCommand<Question> EditQuestionEvent { get; }
        public RelayCommand<Question> DeleteQuestionEvent { get; }

        // logic
        private void Exit()
        {
            _navigation.NavigateTo<LoginViewModel>();
        }

        private void EditUser(User user)
        {
            if (user == null) return;

            NavigationParameterService.Set("UserKeyObject", user);
            _navigation.NavigateTo<UserEditorViewModel>();
        }
        private void EditResult(Result result)
        {
            if (result == null) return;

            NavigationParameterService.Set("SelectedResult", result);
            _navigation.NavigateTo<ResultsEditorViewModel>();
        }
        private void EditQuestion(Question question)
        {
            if (question == null) return;

            NavigationParameterService.Set("QuestionObject", question);
            _navigation.NavigateTo<QuestionEditorViewModel>();
        }

        private void CreateUser()
        {
            NavigationParameterService.Set("UserKeyObject", new User());
            _navigation.NavigateTo<UserEditorViewModel>();
        }
        private void CreateQuestion()
        {
            _navigation.NavigateTo<QuestionEditorViewModel>();
        }

        private void DeleteTicket(Ticket ticket)
        {
            if (ticket == null) return;

            MessageBoxResult msgUserChoice = _message.ConfirmDelete();

            if (msgUserChoice == MessageBoxResult.No) return;

            using var context = new AppDbContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                context.Tickets.Remove(ticket);
                context.SaveChanges();

                transaction.Commit();

                TicketTable.Remove(ticket);
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                _message.ErrorExMessage(ex);
                throw;
            }
        }
        private void DeleteResult(Result result)
        {
            if (result == null) return;

            MessageBoxResult msgUserChoice = _message.ConfirmDelete();

            if (msgUserChoice == MessageBoxResult.No) return;

            using var context = new AppDbContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                context.Results.Remove(result);
                context.SaveChanges();

                transaction.Commit();

                ResultsTable.Remove(result);
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                _message.ErrorExMessage(ex);
                throw;
            }
        }
        private void DeleteQuestion(Question question)
        {
            if (question == null) return;

            MessageBoxResult msgUserChoice = _message.ConfirmDelete();

            if (msgUserChoice == MessageBoxResult.No) return;

            using var context = new AppDbContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                var questionToDelete = context.Questions
                        .Include(q => q.Answers)
                        .FirstOrDefault(q => q.Questionid == question.Questionid);

                if (questionToDelete != null)
                {
                    context.Answers.RemoveRange(questionToDelete.Answers);
                    context.Questions.Remove(questionToDelete);
                    context.SaveChanges();

                    transaction.Commit();

                    _message.DeleteComplite();

                    QuestionTable.Remove(question);
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                _message.ErrorExMessage(ex);
                throw;
            }
        }

        private void UpdateQuestionTable()
        {
            QuestionTable = TableAgent.GetQuestions();
            QuestionsView = CollectionViewSource.GetDefaultView(QuestionTable);
            QuestionsView.Filter = FilterQuestions;

            OnPropertyChanged(nameof(QuestionsView));
        }

        private void UpdateResultTable()
        {
            ResultsTable = TableAgent.GetResults();
            ResultsView = CollectionViewSource.GetDefaultView(ResultsTable);
            ResultsView.Filter = FilterResults;

            OnPropertyChanged(nameof(ResultsView));
        }
        private void UpdateAnswerTable()
        {
            UserAnswerTable = TableAgent.GetUserAnswers();
            UserAnswersView = CollectionViewSource.GetDefaultView(UserAnswerTable);
            UserAnswersView.Filter = FilterUserAnswers;

            OnPropertyChanged(nameof(UserAnswersView));
        }
        private void UpdateTicketTable()
        {
            TicketTable = TableAgent.GetTickets();
            TicketsView = CollectionViewSource.GetDefaultView(TicketTable);
            TicketsView.Filter = FilterTickets;

            OnPropertyChanged(nameof(TicketTable));
        }
        private void UpdateUserTable()
        {
            UserTable = TableAgent.GetUsers();
            UsersView = CollectionViewSource.GetDefaultView(UserTable);
            UsersView.Filter = FilterUsers;

            OnPropertyChanged(nameof(UserTable));
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
