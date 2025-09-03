using FireTestingApp_net8.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

///////////////////////////////////////////////////////////////////////////
//                              ШАБЛОННЫЙ КOД                            //
///////////////////////////////////////////////////////////////////////////

namespace FireTestingApp_net8.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Frame _mainFrame;
        private readonly Dictionary<Type, Func<Page>> _pageMap = [];

        public NavigationService(Frame mainFrame)
        {
            _mainFrame = mainFrame;
        }

        public void Register<TView, TViewModel>(IServiceProvider provider)
            where TView : Page
            where TViewModel : class
        {
            _pageMap[typeof(TViewModel)] = () =>
            {
                var navigationService = this as INavigationService;

                // Вручную передаём INavigationService, остальное из DI
                var viewModel = ActivatorUtilities.CreateInstance<TViewModel>(provider, navigationService!);

                var view = provider.GetRequiredService<TView>();
                view.DataContext = viewModel;

                return view;
            };
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
        public static void RegisterViewAndViewModel<TView, TViewModel>(IServiceCollection services)
            where TView : class
            where TViewModel : class
        {
            services.AddTransient<TView>();
            services.AddTransient<TViewModel>();
        }
    }
}
