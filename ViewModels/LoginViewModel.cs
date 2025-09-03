using FireTestingApp.Models;
using FireTestingApp_net8.Models.Shema;
using FireTestingApp_net8.Services;
using Microsoft.EntityFrameworkCore;

namespace FireTestingApp_net8.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        // private
        private string? _login;
        private string? _password;

        private readonly INavigationService _navigation;
        private readonly IMessageService _messageService;

        // constructor
        public LoginViewModel(INavigationService navigation, IMessageService messageService)
        {
            EnterEvent = new RelayCommand(EnterAccount);
            _navigation = navigation;
            _messageService = messageService;
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
                            case 1 or 4:
                                _navigation.NavigateTo<InstructorViewModel>();
                                break;

                            case 3:
                                var ExamDateRestrict = context.Results.AsNoTracking()
                                    .FirstOrDefault(e => e.Userid == Session.UserID);

                                if (ExamDateRestrict?.Testdate != null && (DateTime.Now - ExamDateRestrict.Testdate).TotalDays <= 31)
                                {
                                    _messageService.UserTestRestriction();
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
                        _messageService.LoginError();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _messageService.DbConnectionError(ex);
                    throw;
                }
            }
            else
            {
                _messageService.NullTextField();
                return;
            }
        }
    }
}
