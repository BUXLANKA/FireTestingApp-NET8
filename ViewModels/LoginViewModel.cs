using FireTestingApp.Models;
using FireTestingApp_net8.Models.Shema;
using FireTestingApp_net8.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace FireTestingApp_net8.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string? _login;
        private string? _password;

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        public string Password
        {
            get => _password!;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public RelayCommand EnterEvent { get; }

        private readonly INavigationService _nav;

        public LoginViewModel(INavigationService nav)
        {
            EnterEvent = new RelayCommand(EnterAccount);
            _nav = nav;
        }

        private void EnterAccount()
        {
            if(!string.IsNullOrEmpty(Login) || !string.IsNullOrEmpty(Password))
            {
                try
                {
                    using var Context = new AppDbContext();

                    var User = Context.Users.AsNoTracking()
                        .FirstOrDefault(u => u.Userlogin == Login);

                    if(User != null && User.Userpassword == Password)
                    {
                        Session.UserID = User.Userid;
                        Session.RoleID = User.Roleid;
                        Session.UserFirstname = User.Firstname;
                        Session.UserLastname = User.Lastname;

                        switch (Session.RoleID)
                        {
                            case 1:
                                _nav.NavigateTo<InstructorViewModel>();
                                //NavigationService.Navigate(new InstructorPage());
                                break;

                            case 2:
                                //NavigationService.Navigate(new RevisorPage());
                                break;

                            case 3:
                                var ExamDateRestrict = Context.Results.AsNoTracking()
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
                                    //NavigationService.Navigate(new UserPage());
                                }
                                break;

                            case 4:
                                //NavigationService.Navigate(new InstructorPage());
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
                    return;
                    throw;
                }
            }
            else
            {
                MessageBox.Show(
                    "Введите логин и пароль",
                    "Пусто? Пусто!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
