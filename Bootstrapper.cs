using Microsoft.Extensions.DependencyInjection;
using System;
using FireTestingApp_net8.ViewModels;
using FireTestingApp_net8.Viewes;
using FireTestingApp_net8.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FireTestingApp_net8
{
    public class Bootstrapper
    {
        public IServiceProvider ServiceProvider { get; }

        public Bootstrapper()
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMessageService, MessageService>(); 
            services.AddSingleton<INavigationService, NavigationService>();

            NavigationService.RegisterViewAndViewModel<LoginView, LoginViewModel>(services);
            NavigationService.RegisterViewAndViewModel<InstructorView, InstructorViewModel>(services);
            NavigationService.RegisterViewAndViewModel<FeedBackView, FeedBackViewModel>(services);
            NavigationService.RegisterViewAndViewModel<MainTestView, MainTestViewModel>(services);
            NavigationService.RegisterViewAndViewModel<ResultsView, ResultsViewModel>(services);
            NavigationService.RegisterViewAndViewModel<ResultsEditorView, ResultsEditorViewModel>(services);
            NavigationService.RegisterViewAndViewModel<AnnotationView, AnnotationViewModel>(services);
            NavigationService.RegisterViewAndViewModel<UserEditorView, UserEditorViewModel>(services);
            NavigationService.RegisterViewAndViewModel<QuestionEditorView, QuestionEditorViewModel>(services);

            services.AddSingleton<MainWindow>();
        }

        //private void RegisterViewAndViewModel<TView, TViewModel>(IServiceCollection services)
        //    where TView : class
        //    where TViewModel : class
        //{
        //    services.AddTransient<TView>();
        //    // ViewModel создаём через фабрику, чтобы передать INavigationService из MainWindow
        //    // Регистрация просто для разрешения в DI, реальное создание — вручную в MainWindow
        //    services.AddTransient<TViewModel>();
        //}
    }
}
