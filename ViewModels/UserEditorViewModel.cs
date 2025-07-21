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
            UserObject = NavigationParameterService.Get<User>("UserKeyObject") ?? new User();

            SaveEvent = new RelayCommand(Save);
            CancelEvent = new RelayCommand(Cancel);

            using (var context = new AppDbContext())
            {
                RoleList = new ObservableCollection<Role>(context.Roles.ToList());
            }

                _navigatoin = navigatoin;
        }

        // public
        public User UserObject { get; set; }

        // collection
        public ObservableCollection<Role> RoleList { get; set; }

        // command
        public RelayCommand SaveEvent { get; }
        public RelayCommand CancelEvent { get; }

        // logic
        private void Save()
        {
            using (var context = new AppDbContext())
            {
                #region Adding new user code
                if (UserObject.Roleid == 0)
                {
                    MessageBox.Show($"role is null");
                    return;
                }

                if (string.IsNullOrWhiteSpace(UserObject.Firstname)
                    || string.IsNullOrWhiteSpace(UserObject.Surname)
                    || string.IsNullOrWhiteSpace(UserObject.Lastname)
                    || string.IsNullOrWhiteSpace(UserObject.Userlogin)
                    || string.IsNullOrWhiteSpace(UserObject.Userpassword))
                {
                    MessageBox.Show($"str is null?");
                }

                if (UserObject.Userid == 0)
                {
                    var existsLogin = context.Users.FirstOrDefault(u => u.Userlogin == UserObject.Userlogin);

                    if (existsLogin != null) return;

                    User newUser = new()
                    {
                        Firstname = UserObject.Firstname,
                        Surname = UserObject.Surname,
                        Lastname = UserObject.Lastname,
                        Roleid = UserObject.Roleid,
                        Userlogin = UserObject.Userlogin,
                        Userpassword = UserObject.Userpassword
                    };

                    try
                    {
                        context.Users.Add(newUser);
                        context.SaveChanges();
                        MessageBox.Show($"DONE!");
                        return;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}");
                        throw;
                    }
                }
                #endregion

                #region User editing
                var user = context.Users.FirstOrDefault(u => u.Userid == UserObject.Userid);

                if (user != null)
                {
                    if (string.IsNullOrWhiteSpace(UserObject.Firstname)
                        ||string.IsNullOrWhiteSpace(UserObject.Surname)
                        || string.IsNullOrWhiteSpace(UserObject.Lastname)
                        || string.IsNullOrWhiteSpace(UserObject.Userlogin)
                        || string.IsNullOrWhiteSpace(UserObject.Userpassword))
                    {
                        MessageBox.Show($"string is null");
                        return;
                    }

                    user.Firstname = UserObject.Firstname;
                    user.Surname = UserObject.Surname;
                    user.Lastname = UserObject.Lastname;
                    user.Roleid = UserObject.Roleid;
                    user.Userlogin = UserObject.Userlogin;
                    user.Userpassword = UserObject.Userpassword;

                    try
                    {
                        context.SaveChanges();
                        MessageBox.Show($"UPDATED");
                        return;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}");
                        throw;
                    }
                }
                #endregion

                //#region Editing exists users code
                //var user = context.Users.FirstOrDefault(u => u.Userid == UserObject.Userid);

                //if (user != null)
                //{

                //}
                //#endregion



                //if (UserObject.Role == null)
                //{
                //    MessageBox.Show("Роль пользователя не выбрана!");
                //    return;
                //}

                //var role = context.Roles.FirstOrDefault(r => r.Roleid == UserObject.Role.Roleid);

                ////добавление
                //if (UserObject.Userid == 0)
                //{
                //    User newUser = new()
                //    {
                //        Firstname = UserObject.Firstname,
                //        Surname = UserObject.Surname,
                //        Lastname = UserObject.Lastname,
                //        Roleid = UserObject.Role.Roleid,
                //        Userpassword = UserObject.Userpassword,
                //        Userlogin = UserObject.Userlogin
                //    };

                //    bool duplicateUser = context.Users.Any(u => u.Userlogin == UserObject.Userlogin);

                //    if (!duplicateUser)
                //    {
                //        try
                //        {
                //            //context.Users.Add(newUser);
                //            //context.SaveChanges();
                //            MessageBox.Show("user added");
                //            return;
                //        }
                //        catch (Exception ex)
                //        {
                //            MessageBox.Show($"{ex.Message}");
                //            throw;
                //        }
                //    }
                //}

                //// изменение
                //var user = context.Users.FirstOrDefault(u => u.Userid == UserObject.Userid);

                //if (user != null)
                //{
                //    user.Firstname = UserObject.Firstname;
                //    user.Surname = UserObject.Surname;
                //    user.Lastname = UserObject.Lastname;
                //    user.Role = role!;
                //    user.Userpassword = UserObject.Userpassword;

                //    bool exitstLogin = context.Users.Any(u => u.Userlogin == UserObject.Userlogin
                //        && u.Userid != UserObject.Userid);

                //    if (!exitstLogin)
                //    {
                //        user.Userlogin = UserObject.Userlogin;
                //    }
                //    else
                //    {
                //        MessageBox.Show("REPEAT");
                //    }

                //        try
                //        {
                //            context.SaveChanges();
                //            MessageBox.Show("DONE!");
                //            _navigatoin.GoBack();
                //        }
                //        catch (Exception ex)
                //        {
                //            MessageBox.Show($"{ex.Message}");
                //            throw;
                //        }
                //}
            }

            //try
            //{
            //    if (UserObject != null)
            //    {
            //        using (var context = new AppDbContext())
            //        {
            //            var user = context.Users.FirstOrDefault(u => u.Userid == UserObject.Userid);
            //            var role = context.Roles.FirstOrDefault(r => r.Roleid == UserObject.Role.Roleid);
            //            bool existsUserLogin = context.Users.Any(u => u.Userlogin == UserObject.Userlogin);

            //            if (user != null)
            //            {
            //                user.Firstname = UserObject.Firstname;
            //                user.Surname = UserObject.Surname;
            //                user.Lastname = UserObject.Lastname;
            //                user.Role = role!;
            //                user.Userpassword = UserObject.Userpassword;

            //                if (!existsUserLogin)
            //                {
            //                    user.Userlogin = UserObject.Userlogin;
            //                }
            //                else
            //                {
            //                    MessageBox.Show("Такой пользователь уже существует. Придумайте другой логин!");
            //                    return;
            //                }
            //            }
            //            else
            //            {
            //                MessageBox.Show("Пользователь не найден");
            //            }

            //            context.SaveChanges();
            //            MessageBox.Show("Данные успешно изменены!");
            //            _navigatoin.GoBack();
            //        }
            //    }
            //    else
            //    {
            //        MessageBoxResult msgUserChoice = MessageBox.Show(
            //            "Пользователь которого вы пытаетесь редактировать не найден\nХотите создать нового пользователя?",
            //            "Пользователь не найден",
            //            MessageBoxButton.YesNo,
            //            MessageBoxImage.Information,
            //            MessageBoxResult.No);

            //        if (msgUserChoice == MessageBoxResult.Yes)
            //        {
            //            using (var context = new AppDbContext())
            //            {
            //                //var role = context.Roles.FirstOrDefault(r => r.Roleid == UserObject!.Role.Roleid);

            //                var role = UserObject.Role != null ? 
            //                    context.Roles.FirstOrDefault(r => r.Roleid == UserObject.Role.Roleid) : null;

            //                bool existsUserLogin = context.Users.Any(u => u.Userlogin == UserObject!.Userlogin && u.Userid != UserObject.Userid);

            //                User user = new()
            //                {
            //                    Firstname = UserObject!.Firstname,
            //                    Lastname = UserObject.Lastname,
            //                    Surname = UserObject.Surname,
            //                    Role = UserObject.Role,
            //                    Userlogin = UserObject.Userlogin,
            //                    Userpassword = UserObject.Userpassword
            //                };

            //                if (UserObject.Firstname != null && UserObject.Surname != null
            //                    && UserObject.Lastname != null && UserObject.Role != null && !existsUserLogin 
            //                    && UserObject.Userpassword != null)
            //                {
            //                    context.Add(user);
            //                    context.SaveChanges();
            //                }
            //                else
            //                {
            //                    MessageBox.Show("error");
            //                    return;
            //                }

            //            }
            //        }
            //        else
            //        {
            //            MessageBox.Show("Операция отменена");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"При выполнении запроса произошла ошибка\n{ex.Message}");
            //    throw;
            //}
        }
        private void Cancel()
        {
            //WeakReferenceMessenger.Default.Send(new SelectTabMessage(2));
            _navigatoin.GoBack();
        }
    }
}
