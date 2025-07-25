using FireTestingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FireTestingApp_net8.Common
{
    public class AppMessage
    {
        public string Welcome = $"Добро пожаловать {Session.UserFirstname} {Session.UserLastname}";

        public void TicketSendComplite()
        {
            MessageBox.Show(
                "Отзыв успешно отправлен!",
                "Спасибо",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
