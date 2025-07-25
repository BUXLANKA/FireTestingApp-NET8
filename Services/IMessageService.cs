using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FireTestingApp_net8.Services
{
    public interface IMessageService
    {
        void TicketCompiteSend();
        void ErrorExMessage(Exception ex);
        void NullTextField();
        void DbConnectionError(Exception ex);
        void LoginError();
        void UserTestRestriction();
        MessageBoxResult ConfirmDelete();
        void Error();
        void TestTimeOut();
        void SaveComplite();
    }
}
