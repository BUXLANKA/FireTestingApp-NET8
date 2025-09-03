using CommunityToolkit.Mvvm.Messaging;
using FireTestingApp_net8.Models.Shema;
using FireTestingApp_net8.Services;
using System.Collections.ObjectModel;

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
            using var context = new AppDbContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                if (EditedResult != null)
                {
                    var result = context.Results.FirstOrDefault(r => r.Resultid == EditedResult.Resultid);

                    if (result == null)
                    {
                        _messageService.Error();
                        return;
                    }

                    result.Userscore = EditedResult.Userscore;
                    result.Statusid = EditedResult.Statusid;
                    result.Testdate = EditedResult.Testdate;

                    context.SaveChanges();

                    transaction.Commit();

                    WeakReferenceMessenger.Default.Send(new UpdateMessage());

                    _messageService.SaveComplite();
                    NavigationParameterService.Clear("SelectedResult");
                    _navigation.GoBack();
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                _messageService.ErrorExMessage(ex);
                throw;
            }
        }
        private void Cancel()
        {
            NavigationParameterService.Clear("SelectedResult");
            _navigation.GoBack();
        }
    }
}
