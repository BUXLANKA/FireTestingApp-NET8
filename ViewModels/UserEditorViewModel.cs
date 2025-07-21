using CommunityToolkit.Mvvm.Messaging;
using FireTestingApp_net8.Models.Shema;
using FireTestingApp_net8.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FireTestingApp_net8.ViewModels
{
    public class UserEditorViewModel : BaseViewModel
    {
        // private
        private readonly INavigationService _navigatoin;

        // constructor
        public UserEditorViewModel(INavigationService navigatoin)
        {
            EditedUser = NavigationParameterService.Get<User>("SelectedUser");

            SaveEvent = new RelayCommand(Save);
            CancelEvent = new RelayCommand(Cancel);

            using (var context = new AppDbContext())
            {
                RoleList = new ObservableCollection<Role>(context.Roles.ToList());
            }

                _navigatoin = navigatoin;
        }

        // public
        public User? EditedUser { get; set; }

        // collection
        public ObservableCollection<Role> RoleList { get; set; }

        // command
        public RelayCommand SaveEvent { get; }
        public RelayCommand CancelEvent { get; }

        // logic
        private void Save()
        {
            try
            {
                if (EditedUser != null)
                {
                    using (var context = new AppDbContext())
                    {
                        var user = context.Users.FirstOrDefault(u => u.Userid == EditedUser.Userid);
                        var role = context.Roles.FirstOrDefault(r => r.Roleid == EditedUser.Role.Roleid);
                        bool existsUserLogin = context.Users.Any(u => u.Userlogin == EditedUser.Userlogin);

                        if (user != null)
                        {
                            user.Firstname = EditedUser.Firstname;
                            user.Surname = EditedUser.Surname;
                            user.Lastname = EditedUser.Lastname;
                            user.Role = role!;
                            user.Userpassword = EditedUser.Userpassword;

                            if (!existsUserLogin)
                            {
                                user.Userlogin = EditedUser.Userlogin;
                            }
                            else
                            {
                                MessageBox.Show("Такой пользователь уже существует. Придумайте другой логин!");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Пользователь не найден");
                        }

                        context.SaveChanges();
                        MessageBox.Show("Данные успешно изменены!");
                        _navigatoin.GoBack();
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь не выбран");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"При выполнении запроса произошла ошибка\n{ex.Message}");
                throw;
            }
        }
        private void Cancel()
        {
            //WeakReferenceMessenger.Default.Send(new SelectTabMessage(2));
            _navigatoin.GoBack();
        }
    }
}
