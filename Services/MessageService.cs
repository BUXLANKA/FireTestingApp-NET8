using System.Windows;

namespace FireTestingApp_net8.Services
{
    public class MessageService : IMessageService
    {
        // error
        public void DbConnectionError(Exception ex)
        {
            MessageBox.Show($"Не удаётся создать соединение с базой данный\n{ex.Message}", "Ошибка соединения",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public void ErrorExMessage(Exception ex)
        {
            MessageBox.Show($"Возникала внутряняя ошибка:\n{ex.Message}");
        }
        public void NullTextField()
        {
            MessageBox.Show($"Все поля должны быть заполнены!", "Ошибка: пустые поля для ввода",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public void Error()
        {
            MessageBox.Show($"Объект имел неверные значения", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public void LoginError()
        {
            MessageBox.Show($"Неправильный логин или пароль", "Ошибка авторизации",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // complite
        public void TicketCompiteSend()
        {
            MessageBox.Show("Отзыв успешно отправлен!", "Обратная связь",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void SaveComplite()
        {
            MessageBox.Show("данные успешно сохранены");
        }
        public void DeleteComplite()
        {
            MessageBox.Show($"Объект успешно удалён!");
        }

        // info
        public void UserTestRestriction()
        {
            MessageBox.Show($"Повторная сдача будет доступна после 31 дня с момента последней сдачи.\nЗа подробностями обратитесь к инструктору.",
                "Ограничение на прохождение теста",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void TestTimeOut()
        {
            MessageBox.Show($"Тест закрыт по истечению времени прохождения.", "Кажется, вы не успели...",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // comfirm
        public MessageBoxResult ConfirmDelete()
        {
            return MessageBox.Show(
                    $"Вы действительно хотите удалить запись? Отменить действие будет невозможно!",
                    "Удаление строки",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning,
                    MessageBoxResult.No);
        }
    }
}
