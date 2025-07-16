using FireTestingApp_net8.Models.Shema;
using FireTestingApp_net8.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FireTestingApp_net8.ViewModels
{
    public class ResultsEditorViewModel : BaseViewModel
    {
        public Result EditedResult { get; set; }

        public ObservableCollection<Teststatus> StatusList { get; set; }

        public RelayCommand SaveEvent{ get; }
        public RelayCommand CancelEvent { get; }

        private readonly INavigationService _nav;

        public ResultsEditorViewModel(INavigationService nav)
        {
            _nav = nav;

            // Получение переданного Result
            EditedResult = NavigationParameterService.Get<Result>("SelectedResult");

            // Получаем список статусов
            using (var context = new AppDbContext())
            {
                StatusList = new ObservableCollection<Teststatus>(context.Teststatuses.ToList());
            }

            SaveEvent = new RelayCommand(Save);
            CancelEvent = new RelayCommand(Cancel);
        }

        private void Save()
        {
            try
            {
                using (var context = new AppDbContext())
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

                MessageBox.Show("данные успешно сохранены");
                _nav.NavigateTo<InstructorViewModel>();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }

        private void Cancel()
        {
            _nav.NavigateTo<InstructorViewModel>();
        }
    }
}
