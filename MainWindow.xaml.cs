using FireTestingApp_net8.Services;
using FireTestingApp_net8.Viewes;
using FireTestingApp_net8.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            Navigation.Register<UserResultsViewModel>(() =>
            {
                var vm = new UserResultsViewModel(Navigation);
                var page = new UserResultsView();
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

            Navigation.NavigateTo<LoginViewModel>();

            this.MinHeight = 640;
            this.MinWidth = 960;
        }
    }
}