using FireTestingApp.Models;
using FireTestingApp_net8.Models.Shema;
using FireTestingApp_net8.Services;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace FireTestingApp_net8.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        // private
        private string? _login;
        private string? _password;

        private readonly INavigationService _navigation;

        // constructor
        public LoginViewModel(INavigationService navigation)
        {
            EnterEvent = new RelayCommand(EnterAccount);
            _navigation = navigation;
        }

        // public
        public string? Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        public string? Password
        {
            get => _password!;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        // collection

        // command
        public RelayCommand EnterEvent { get; }

        // logic
        private void EnterAccount()
        {
            if (!string.IsNullOrEmpty(Login) || !string.IsNullOrEmpty(Password))
            {
                try
                {
                    using var context = new AppDbContext();

                    var User = context.Users.AsNoTracking()
                        .FirstOrDefault(u => u.Userlogin == Login);

                    if (User != null && User.Userpassword == Password)
                    {
                        Session.UserID = User.Userid;
                        Session.RoleID = User.Roleid;
                        Session.UserFirstname = User.Firstname;
                        Session.UserLastname = User.Lastname;

                        switch (Session.RoleID)
                        {
                            case 1:
                                _navigation.NavigateTo<InstructorViewModel>();
                                break;

                            case 3:
                                var ExamDateRestrict = context.Results.AsNoTracking()
                                    .FirstOrDefault(e => e.Userid == Session.UserID);

                                if (ExamDateRestrict?.Testdate != null && (DateTime.Now - ExamDateRestrict.Testdate).TotalDays <= 31)
                                {
                                    MessageBox.Show(
                                        "Повторная сдача будет доступна после 31 дня с момента последней сдачи.\nЗа подробностями обратитесь к инструктору.",
                                        "Информация",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information);
                                    return;
                                }
                                else
                                {
                                    _navigation.NavigateTo<AnnotationViewModel>();
                                }
                                break;
                        }
                    }
                    else
                    {
                        MessageBox.Show(
                            "Неправильный логин или пароль",
                            "Ошибка авторизации",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);

                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Не удаётся создать соединение с базой данный. Обратитесь к администратору.",
                        "Ошибка сервера",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    MessageBox.Show(ex.Message);
                    throw;
                }
            }
            else
            {
                MessageBox.Show(
                    "Введите логин или пароль",
                    "Пусто? Пусто!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }
        }
    }
}
