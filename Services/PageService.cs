using FireTestingApp_net8.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
