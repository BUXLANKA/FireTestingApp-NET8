using FireTestingApp_net8.ViewModels;
using System.Windows.Controls;

namespace FireTestingApp_net8.Services
{
    public class PageService : INavigationService
    {
        private readonly Frame _mainFrame;
        private readonly Dictionary<Type, Func<Page>> _pageMap = new();

        public PageService(Frame mainFrame)
        {
            _mainFrame = mainFrame;
        }

        public void Register<TViewModel>(Func<Page> createPage)
        {
            _pageMap[typeof(TViewModel)] = createPage;
        }

        public void NavigateTo<TViewModel>() where TViewModel : BaseViewModel
        {
            var page = _pageMap[typeof(TViewModel)]();
            _mainFrame.Navigate(page);
        }
        public void GoBack()
        {
            if (_mainFrame.CanGoBack)
                _mainFrame.GoBack();
        }

        public bool CanGoBack() => _mainFrame.CanGoBack;
    }
}
