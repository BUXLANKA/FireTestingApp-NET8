using FireTestingApp_net8.Services;
using FireTestingApp_net8.Viewes;
using FireTestingApp_net8.ViewModels;
using System.Windows;

namespace FireTestingApp_net8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static PageService? Navigation;

        public MainWindow()
        {
            InitializeComponent();

            Navigation = new PageService(MainFrame);

            Navigation.Register<LoginViewModel>(() =>
            {
                var vm = new LoginViewModel(Navigation);
                var page = new LoginView();
                page.DataContext = vm;
                return page;
            });

            Navigation.Register<InstructorViewModel>(() =>
            {
                var vm = new InstructorViewModel(Navigation);
                var page = new InstructorView();
                page.DataContext = vm;
                return page;
            });

            Navigation.Register<FeedBackViewModel>(() =>
            {
                var vm = new FeedBackViewModel(Navigation);
                var page = new FeedBackView();
                page.DataContext = vm;
                return page;
            });

            Navigation.Register<MainTestViewModel>(() =>
            {
                var vm = new MainTestViewModel(Navigation);
                var page = new MainTestView();
                page.DataContext = vm;
                return page;
            });

            Navigation.Register<ResultsViewModel>(() =>
            {
                var vm = new ResultsViewModel(Navigation);
                var page = new ResultsView();
                page.DataContext = vm;
                return page;
            });

            Navigation.Register<ResultsEditorViewModel>(() =>
            {
                var vm = new ResultsEditorViewModel(Navigation);
                var page = new ResultsEditorView();
                page.DataContext = vm;
                return page;
            });

            Navigation.Register<AnnotationViewModel>(() =>
            {
                var vm = new AnnotationViewModel(Navigation);
                var page = new AnnotationView();
                page.DataContext = vm;
                return page;
            });

            Navigation.Register<UserEditorViewModel>(() =>
            {
                var vm = new UserEditorViewModel(Navigation);
                var page = new UserEditorView();
                page.DataContext = vm;
                return page;
            });

            Navigation.NavigateTo<InstructorViewModel>();

            this.MinHeight = 720;
            this.MinWidth = 1024;
        }
    }
}