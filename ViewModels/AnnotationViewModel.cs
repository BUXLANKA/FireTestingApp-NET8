using FireTestingApp.Models;
using FireTestingApp_net8.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FireTestingApp_net8.ViewModels
{
    public class AnnotationViewModel : BaseViewModel
    {
        // private
        private bool _isChecked;

        private string? _welcomeMessage;

        private readonly INavigationService _navigation;

        // constructor
        public AnnotationViewModel(INavigationService navigation)
        {
            WelcomeMessage = $"Добро пожаловать, {Session.UserFirstname} {Session.UserLastname}!";

            StartTestEvent = new RelayCommand(StartTest);

            _navigation = navigation;
        }

        // public
        public bool isChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                OnPropertyChanged(nameof(isChecked));
            }
        }

        public string? WelcomeMessage
        {
            get => _welcomeMessage;
            set
            {
                _welcomeMessage = value;
                OnPropertyChanged(nameof(WelcomeMessage));
            }
        }

        // collection

        // command
        public RelayCommand StartTestEvent { get; }

        // logic
        private void StartTest()
        {
            _navigation.NavigateTo<MainTestViewModel>();
        }
    }
}
