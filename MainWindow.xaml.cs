using FireTestingApp_net8.Services;
using FireTestingApp_net8.Viewes;
using FireTestingApp_net8.ViewModels;
using System.Windows;

namespace FireTestingApp_net8
{
    public partial class MainWindow : Window
    {
        private readonly NavigationService _navigation;

        public MainWindow(IServiceProvider provider)
        {
            InitializeComponent();

            _navigation = new NavigationService(MainFrame);



            _navigation.Register<LoginView, LoginViewModel>(provider);
            _navigation.Register<InstructorView, InstructorViewModel>(provider);
            _navigation.Register<FeedBackView, FeedBackViewModel>(provider);
            _navigation.Register<MainTestView, MainTestViewModel>(provider);
            _navigation.Register<ResultsView, ResultsViewModel>(provider);
            _navigation.Register<ResultsEditorView, ResultsEditorViewModel>(provider);
            _navigation.Register<AnnotationView, AnnotationViewModel>(provider);
            _navigation.Register<UserEditorView, UserEditorViewModel>(provider);
            _navigation.Register<QuestionEditorView, QuestionEditorViewModel>(provider);

            _navigation.NavigateTo<LoginViewModel>();

            MinHeight = 720;
            MinWidth = 1024;
        }

        //private void RegisterPage<TView, TViewModel>(IServiceProvider provider)
        //    where TView : System.Windows.Controls.Page
        //    where TViewModel : class
        //{
        //    _pageService.Register<TViewModel>(() =>
        //    {
        //        // Вручную создаём ViewModel, передаём навигацию и нужные сервисы
        //        var vm = ActivatorUtilities.CreateInstance<TViewModel>(provider, _pageService);
        //        var view = provider.GetRequiredService<TView>();
        //        view.DataContext = vm;
        //        return view;
        //    });
        //}
    }
}
