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
        // private
        private string? _resultMessage;

        private readonly INavigationService _navigation;

        // constructor
        public ResultsViewModel(INavigationService navigation)
        {
            ResultMessage = Session.UserScore.ToString();

            ContinueAndExitEvent = new RelayCommand(ContinueAndExit);
            OpenFeedbackPageEvent = new RelayCommand(OpenFeedbackPage);

            _navigation = navigation;
        }

        // public
        public string? ResultMessage
        {
            get => _resultMessage;
            set
            {
                _resultMessage = value;
                OnPropertyChanged(nameof(ResultMessage));
            }
        }

        // collection


        // command

        public RelayCommand ContinueAndExitEvent { get; }
        public RelayCommand OpenFeedbackPageEvent { get; }

        // logic
        private void ContinueAndExit()
        {
            _navigation.NavigateTo<LoginViewModel>();
        }
        private void OpenFeedbackPage()
        {
            _navigation.NavigateTo<FeedBackViewModel>();
        }
    }
}
