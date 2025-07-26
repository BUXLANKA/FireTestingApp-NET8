using FireTestingApp.Models;
using FireTestingApp_net8.Models.Shema;
using FireTestingApp_net8.Services;
using System.Windows;

namespace FireTestingApp_net8.ViewModels
{
    public class FeedBackViewModel : BaseViewModel
    {
        // private
        private string? _feedBackMessage;

        private readonly INavigationService _navigation;
        private readonly IMessageService _message;

        // constructor
        public FeedBackViewModel(INavigationService navigation, IMessageService message)
        {
            SendMessageEvent = new RelayCommand(SendMessage);
            GoBackEvent = new RelayCommand(GoBack);

            _navigation = navigation;
            _message = message;
        }

        // public
        public string? FeedBackMessage
        {
            get => _feedBackMessage;
            set
            {
                _feedBackMessage = value;
                OnPropertyChanged(nameof(FeedBackMessage));
            }
        }

        // collection


        // command
        public RelayCommand SendMessageEvent { get; }
        public RelayCommand GoBackEvent { get; }

        // logic
        private void SendMessage()
        {
            if (string.IsNullOrEmpty(FeedBackMessage))
            {
                _message.NullTextField();
                return;
            }

            using var context = new AppDbContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                Ticket ticket = new()
                {
                    Fromuserid = Session.UserID,
                    Ticketdate = DateTime.Now,
                    Tickettext = FeedBackMessage
                };

                context.Tickets.Add(ticket);
                context.SaveChanges();

                transaction.Commit();

                _message.TicketCompiteSend();
                _navigation.GoBack();
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                _message.ErrorExMessage(ex);
                throw;
            }
        }
        private void GoBack()
        {
            _navigation.NavigateTo<ResultsViewModel>();
        }
    }
}
