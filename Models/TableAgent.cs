using FireTestingApp_net8.Models.Shema;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireTestingApp_net8.Models
{
    public class TableAgent
    {
        public static ObservableCollection<Result> GetResults()
        {
            using (var context = new AppDbContext())
            {
                var ResultsList = context.Results
                    .Include(r => r.User)
                    .Include(r => r.Status)
                    .ToList();
                return new ObservableCollection<Result>(ResultsList);      
            }
        }
        public static ObservableCollection<Useranswer> GetUserAnswers()
        {
            using (var context = new AppDbContext())
            {
                var UserAnswerList = context.Useranswers
                    .Include(r => r.User)
                    .Include(r => r.Question)
                    .Include(r => r.Answer)
                    .ToList();
                return new ObservableCollection<Useranswer>(UserAnswerList);
            }
        }
        public static ObservableCollection<Ticket> GetTickets()
        {
            using (var context = new AppDbContext())
            {
                var TicketList = context.Tickets
                    .Include(r => r.Fromuser)
                    .ToList();
                return new ObservableCollection<Ticket>(TicketList);

            }
        }
        public static ObservableCollection<User> GetUsers()
        {
            using (var context = new AppDbContext())
            {
                var userList = context.Users
                    .Include(r => r.Role)
                    .ToList();
                return new ObservableCollection<User>(userList);
            }
        }
        public static ObservableCollection<Question> GetQuestions()
        {
            using (var context = new AppDbContext())
            {
                var questionList = context.Questions
                    .Include(q => q.Answers)
                    .ToList();
                return new ObservableCollection<Question>(questionList);
            }
        }
    }
}
