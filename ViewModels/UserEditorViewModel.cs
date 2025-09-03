using CommunityToolkit.Mvvm.Messaging;
using FireTestingApp_net8.Models.Shema;
using FireTestingApp_net8.Services;
using System.Collections.ObjectModel;

namespace FireTestingApp_net8.ViewModels
{
    public class UserEditorViewModel : BaseViewModel
    {
        // private
        private readonly INavigationService _navigatoin;
        private readonly IMessageService _messageService;

        // constructor
        public UserEditorViewModel(INavigationService navigatoin, IMessageService messegeService)
        {
            UserObject = NavigationParameterService.Get<User>("UserKeyObject") ?? new User();

            SaveEvent = new RelayCommand(Save);
            CancelEvent = new RelayCommand(Cancel);

            using (var context = new AppDbContext())
            {
                RoleList = new ObservableCollection<Role>(context.Roles.ToList());
            }

            _navigatoin = navigatoin;
            _messageService = messegeService;
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
            if (UserObject.Roleid == 0)
            {
                _messageService.Error();
                return;
            }

            if (string.IsNullOrWhiteSpace(UserObject.Firstname) ||
                string.IsNullOrWhiteSpace(UserObject.Surname) ||
                string.IsNullOrWhiteSpace(UserObject.Lastname) ||
                string.IsNullOrWhiteSpace(UserObject.Userlogin) ||
                string.IsNullOrWhiteSpace(UserObject.Userpassword))
            {
                _messageService.NullTextField();
                return;
            }

            if (string.IsNullOrWhiteSpace(UserObject.Firstname) ||
                string.IsNullOrWhiteSpace(UserObject.Surname) ||
                string.IsNullOrWhiteSpace(UserObject.Lastname) ||
                string.IsNullOrWhiteSpace(UserObject.Userlogin) ||
                string.IsNullOrWhiteSpace(UserObject.Userpassword))
            {
                _messageService.NullTextField();
                return;
            }

            using var context = new AppDbContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                #region Добавление нового пользователя            

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

                    context.Users.Add(newUser);
                    context.SaveChanges();

                    transaction.Commit();

                    _messageService.SaveComplite();
                    return;
                }
                #endregion

                #region Редактирование пользователя

                var user = context.Users.FirstOrDefault(u => u.Userid == UserObject.Userid);

                if (user != null)
                {
                    user.Firstname = UserObject.Firstname;
                    user.Surname = UserObject.Surname;
                    user.Lastname = UserObject.Lastname;
                    user.Roleid = UserObject.Roleid;
                    user.Userlogin = UserObject.Userlogin;
                    user.Userpassword = UserObject.Userpassword;

                    context.SaveChanges();

                    transaction.Commit();

                    _messageService.SaveComplite();

                    WeakReferenceMessenger.Default.Send(new UpdateMessage());
                    return;
                }
                #endregion
            }
            catch (Exception ex)
            {
                _messageService.ErrorExMessage(ex);
                throw;
            }
        }
        private void Cancel()
        {
            NavigationParameterService.Clear("UserKeyObject");
            _navigatoin.GoBack();
        }
    }
}
