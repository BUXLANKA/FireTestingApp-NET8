using FireTestingApp_net8.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireTestingApp_net8.ViewModels
{
    public class UserResultsViewModel : BaseViewModel
    {
        private readonly INavigationService _nav;

        public UserResultsViewModel(INavigationService nav)
        {
            _nav = nav;

            ContinueAndExitEvent = new RelayCommand(ContinueAndExit);
            OpenFeedBakcPageEvent = new RelayCommand(OpenFeedBackPage);
        }

        private string? _resultText;
        public string ResultText
        {
            get => _resultText;
            set
            {
                _resultText = value;
                OnPropertyChanged(nameof(ResultText));
            }
        }

        public RelayCommand ContinueAndExitEvent { get; }
        private void ContinueAndExit()
        {
            _nav.NavigateTo<LoginViewModel>();
        }

        public RelayCommand OpenFeedBakcPageEvent { get; }
        private void OpenFeedBackPage()
        {
            _nav.NavigateTo<FeedBackViewModel>();
        }
    }
}
