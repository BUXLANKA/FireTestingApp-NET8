using FireTestingApp.Models;
using FireTestingApp_net8.Models.Shema;
using FireTestingApp_net8.Services;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FireTestingApp_net8.ViewModels
{
    public class FeedBackViewModel : BaseViewModel
    {
        private string? _feedBackMessage;
        public string FeedBackMessage
        {
            get => _feedBackMessage;
            set
            {
                _feedBackMessage = value;
                OnPropertyChanged(nameof(FeedBackMessage));
            }
        }

        private readonly INavigationService _nav;
        public FeedBackViewModel(INavigationService nav)
        {
            SendMessageEvent = new RelayCommand(SendMessage);
            GoBackEvent = new RelayCommand(GoBack);
            _nav = nav;
        }

        public RelayCommand SendMessageEvent { get; }
        private void SendMessage()
        {
            if (!string.IsNullOrEmpty(FeedBackMessage))
            {
                Ticket TicketTable = new Ticket();

                TicketTable.Fromuserid = Session.UserID;
                TicketTable.Ticketdate = DateTime.Now;
                TicketTable.Tickettext = FeedBackMessage;

                try
                {
                    using (var Context = new AppDbContext())
                    {
                        Context.Tickets.Add(TicketTable);
                        Context.SaveChanges();

                        MessageBox.Show(
                        "Отзыв успешно отправлен!",
                        "Спасибо",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                        //go back
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
                    "А что исправлять то?",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
        }

        public RelayCommand GoBackEvent { get; }
        private void GoBack()
        {
            //_nav.NavigateTo<UserResultsView>();
        }
    }
}
