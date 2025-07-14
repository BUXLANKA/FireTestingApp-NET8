using FireTestingApp.Models;
using FireTestingApp_net8.Models.Shema;
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
        public FeedBackViewModel()
        {
            SendMessageEvent = new RelayCommand(SendMessage);
            GoBackEvent = new RelayCommand(GoBack);
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
                    using(var Context = new AppDbContext())
                    {
                        Context.Tickets.Add(TicketTable);
                        Context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Возникала внутряняя ошибка:\n{ex.Message}");
                    throw;
                }
            }
        }

        public RelayCommand GoBackEvent { get; }
        private void GoBack()
        {

        }
    }
}
