using FireTestingApp.Models;
using FireTestingApp_net8.Services;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireTestingApp_net8.ViewModels
{
    public class ResultsViewModel : BaseViewModel
    {
        private readonly INavigationService _nav;

        private string? _resultMessage;
        public string? ResultMessage
        {
            get => _resultMessage;
            set
            {
                _resultMessage = value;
                OnPropertyChanged(nameof(ResultMessage));
            }
        }

        public RelayCommand ContinueAndExitEvent { get; }
        public RelayCommand OpenFeedbackPageEvent { get; }

        public ResultsViewModel(INavigationService nav)
        {
            _nav = nav;

            ResultMessage = Session.UserScore.ToString();

            ContinueAndExitEvent = new RelayCommand(ContinueAndExit);
            OpenFeedbackPageEvent = new RelayCommand(OpenFeedbackPage);
        }

        private void ContinueAndExit()
        {
            _nav.NavigateTo<LoginViewModel>();
        }

        private void OpenFeedbackPage()
        {
            _nav.NavigateTo<FeedBackViewModel>();
        }
    }
}
