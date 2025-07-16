using FireTestingApp_net8.ViewModels;

namespace FireTestingApp_net8.Services
{
    public interface INavigationService
    {
        void NavigateTo<TViewModel>() where TViewModel : BaseViewModel;
    }
}
