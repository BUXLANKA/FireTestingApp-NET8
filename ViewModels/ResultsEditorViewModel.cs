using CommunityToolkit.Mvvm.Messaging;
using FireTestingApp_net8.Models.Shema;
using FireTestingApp_net8.Services;
using System.Collections.ObjectModel;
using System.Windows;

namespace FireTestingApp_net8.ViewModels
{
    public class ResultsEditorViewModel : BaseViewModel
    {
        // private
        private readonly INavigationService _navigation;
        private readonly IMessageService _messageService;

        // constructor
        public ResultsEditorViewModel(INavigationService navigation, IMessageService messageService)
        {
            _navigation = navigation;

            // Получение переданного Result
            EditedResult = NavigationParameterService.Get<Result>("SelectedResult");

            // Получаем список статусов
            using (var context = new AppDbContext())
            {
                StatusList = new ObservableCollection<Teststatus>(context.Teststatuses.ToList());
            }

            SaveEvent = new RelayCommand(Save);
            CancelEvent = new RelayCommand(Cancel);

            _messageService = messageService;
        }

        // public
        public Result? EditedResult { get; set; }

        // collection
        public ObservableCollection<Teststatus> StatusList { get; set; }

        // command
        public RelayCommand SaveEvent { get; }
        public RelayCommand CancelEvent { get; }

        // logic
        private void Save()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    if (EditedResult != null)
                    {
                        var result = context.Results.FirstOrDefault(r => r.Resultid == EditedResult.Resultid);

                        if (result != null)
                        {
                            result.Userscore = EditedResult.Userscore;
                            result.Statusid = EditedResult.Statusid;
                            result.Testdate = EditedResult.Testdate;
                            context.SaveChanges();

                        }
                    }
                    else
                    {
                        //MessageBox.Show(
                        //    "EditResult оказался Null",
                        //    "Ошибка данных",
                        //    MessageBoxButton.OK,
                        //    MessageBoxImage.Error);

                        _messageService.Error();
                    }
                }

                //MessageBox.Show("данные успешно сохранены");
                _messageService.SaveComplite();

                WeakReferenceMessenger.Default.Send(new UpdateMessage());

                //MessageBox.Show("MSG SEND!");
                _navigation.GoBack();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Ошибка при сохранении: " + ex.Message);

                _messageService.ErrorExMessage(ex);
            }
        }
        private void Cancel()
        {
            //_navigation.NavigateTo<InstructorViewModel>();
            _navigation.GoBack();
        }
    }
}
