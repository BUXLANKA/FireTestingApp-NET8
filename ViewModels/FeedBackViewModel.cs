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

        // constructor
        public FeedBackViewModel(INavigationService navigation)
        {
            SendMessageEvent = new RelayCommand(SendMessage);
            GoBackEvent = new RelayCommand(GoBack);

            _navigation = navigation;
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
            if (!string.IsNullOrEmpty(FeedBackMessage))
            {
                Ticket TicketTable = new()
                {
                    Fromuserid = Session.UserID,
                    Ticketdate = DateTime.Now,
                    Tickettext = FeedBackMessage
                };

                try
                {
                    using (var context = new AppDbContext())
                    {
                        context.Tickets.Add(TicketTable);
                        context.SaveChanges();

                        MessageBox.Show(
                        "Отзыв успешно отправлен!",
                        "Спасибо",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                        _navigation.NavigateTo<ResultsViewModel>();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Возникала внутряняя ошибка:\n{ex.Message}");
                    throw;
                }
            }
            else
            {
                MessageBox.Show(
                    "Нельзя отправить пустое сообщение",
                    "А что исправлять?",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
        }
        private void GoBack()
        {
            _navigation.NavigateTo<ResultsViewModel>();
        }
    }
}
