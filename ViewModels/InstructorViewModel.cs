using FireTestingApp_net8.Models.Shema;
using FireTestingApp_net8.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireTestingApp_net8.ViewModels
{
    public class InstructorViewModel : BaseViewModel
    {
        private string _welcomeMessage;
        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set
            {
                _welcomeMessage = value;
                OnPropertyChanged(nameof(WelcomeMessage));
            }
        }

        public ObservableCollection<Result> ResultsTable { get; set; }
        public ObservableCollection<Useranswer> UserAnswerTable { get; set; }
        public ObservableCollection<Ticket> TicketTable { get; set; }

        public InstructorViewModel(INavigationService nav)
        {
            WelcomeMessage = $"Добро пожаловать!";

            ExitEvent = new RelayCommand(Exit);
            _nav = nav;

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

        public RelayCommand ExitEvent { get; }
        private readonly INavigationService? _nav;
        private void Exit()
        {
            _nav.NavigateTo<LoginViewModel>();
        }
    }
}
