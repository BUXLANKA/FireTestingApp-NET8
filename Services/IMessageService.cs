using System.Windows;

namespace FireTestingApp_net8.Services
{
    public interface IMessageService
    {
        // error
        void DbConnectionError(Exception ex);
        void ErrorExMessage(Exception ex);
        void NullTextField();
        void Error();
        void LoginError();

        // complite
        void TicketCompiteSend();
        void SaveComplite();
        void DeleteComplite();

        // info
        void UserTestRestriction();
        void TestTimeOut();

        // confirm
        MessageBoxResult ConfirmDelete();
    }
}
