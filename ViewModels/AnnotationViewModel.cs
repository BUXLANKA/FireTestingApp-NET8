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

        private readonly INavigationService _navigation;

        // constructor
        public AnnotationViewModel(INavigationService navigation)
        {
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
