using FireTestingApp_net8.Models.Shema;
using FireTestingApp_net8.Services;
using System.Collections.ObjectModel;

namespace FireTestingApp_net8.ViewModels
{
    public class QuestionEditorViewModel : BaseViewModel
    {
        private readonly INavigationService _navigation;
        private readonly IMessageService _messageService;

        public QuestionEditorViewModel(INavigationService navigation, IMessageService messageService)
        {
            _navigation = navigation;

            QuestionObject = NavigationParameterService.Get<Question>("QuestionObject") ?? new Question();
            Answers = new ObservableCollection<Answer>();

            SaveEvent = new RelayCommand(Save);
            CancelEvent = new RelayCommand(Cancel);

            if (QuestionObject.Questionid != 0)
            {
                using var context = new AppDbContext();
                var loadedAnswers = context.Answers
                    .Where(a => a.Questionid == QuestionObject.Questionid)
                    .OrderBy(a => a.Answerid)
                    .ToList();

                foreach (var answer in loadedAnswers)
                    Answers.Add(answer);

                SelectedAnswerIndex = loadedAnswers.FindIndex(a => a.Iscorrectanswer);
            }
            else
            {
                for (int i = 0; i < 5; i++)
                    Answers.Add(new Answer());
            }

            _messageService = messageService;
        }

        public Question QuestionObject { get; set; }
        public ObservableCollection<Answer> Answers { get; set; }
        public int SelectedAnswerIndex { get; set; } = -1;

        public RelayCommand SaveEvent { get; }
        public RelayCommand CancelEvent { get; }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(QuestionObject.Questiontext))
            {
                _messageService.Error();
                return;
            }

            if (Answers.Any(a => string.IsNullOrWhiteSpace(a.Answertext)))
            {
                _messageService.Error();
                return;
            }

            if (SelectedAnswerIndex < 0 || SelectedAnswerIndex >= Answers.Count)
            {
                _messageService.Error();
                return;
            }



            using var context = new AppDbContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                #region Добавление билета
                if (QuestionObject.Questionid == 0)
                {
                    context.Questions.Add(QuestionObject);
                    context.SaveChanges();

                    for (int i = 0; i < Answers.Count; i++)
                    {
                        Answers[i].Questionid = QuestionObject.Questionid;
                        Answers[i].Iscorrectanswer = (i == SelectedAnswerIndex);
                        context.Answers.Add(Answers[i]);
                    }
                }
                #endregion

                #region Редактирование билета
                if (QuestionObject.Questionid != 0)
                {
                    var existedQuestion = context.Questions
                        .FirstOrDefault(q => q.Questionid == QuestionObject.Questionid);

                    if (existedQuestion == null)
                    {
                        _messageService.Error();
                        return;
                    }

                    existedQuestion.Questiontext = QuestionObject.Questiontext;

                    var existingAnswers = context.Answers
                        .Where(a => a.Questionid == QuestionObject.Questionid).ToList();

                    for (int i = 0; i < existingAnswers.Count; i++)
                    {
                        existingAnswers[i].Answertext = Answers[i].Answertext;
                        existingAnswers[i].Iscorrectanswer = (i == SelectedAnswerIndex);
                    }

                    context.SaveChanges();
                }
                #endregion

                transaction.Commit();

                _messageService.SaveComplite();
            }
            catch (Exception ex)
            {
                transaction.RollbackAsync();

                _messageService.ErrorExMessage(ex);
                throw;
            }
        }
        private void Cancel()
        {
            NavigationParameterService.Clear("QuestionObject");
            _navigation.GoBack();
        }
    }
}
