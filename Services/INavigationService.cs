using FireTestingApp_net8.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FireTestingApp_net8.Services
{
    public interface INavigationService
    {
        void NavigateTo<TViewModel>() where TViewModel : BaseViewModel;
        bool CanGoBack();
        void GoBack();
    }
}
