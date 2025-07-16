using FireTestingApp.Models;
using FireTestingApp_net8.Models.Shema;
using FireTestingApp_net8.Services;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace FireTestingApp_net8.ViewModels
{
    public class MainTestViewModel : BaseViewModel
    {
        // private
        private string? _timeLeft;
        private string? _questionText;
        private string? _questionIndex;

        private int _currentQuestionIndex;
        private int? _selectedAnswerIndex;

        private Result _сurrentResults = new();

        private readonly INavigationService _navigation;

        // constructor
        public MainTestViewModel(INavigationService navigation)
        {
            Timer.SetMinutes(5);
            Timer.TimeUpdated += Timer_TimeUpdated;
            TimeLeft = Timer.GetTimeLeft().ToString(@"mm\:ss");
            Timer.Start();

            using (var Context = new AppDbContext())
            {
                var QuestionFromDB = Context.Questions
                    .Include(q => q.Answers)
                    .ToList();

                Random rnd = new Random();
                Questions = QuestionFromDB
                    .OrderBy(x => rnd.Next())
                    .Take(10)
                    .ToList();
            }
            CurrentQuestionIndex = 0;
            GoToNextQuestionEvent = new RelayCommand(GoNext);

            _navigation = navigation;
        }

        // public
        public string? TimeLeft
        {
            get => _timeLeft;
            set
            {
                _timeLeft = value;
                OnPropertyChanged(nameof(TimeLeft));
            }
        }
        public string? QuestionText
        {
            get => _questionText;
            set
            {
                _questionText = value;
                OnPropertyChanged(nameof(QuestionText));
            }
        }
        public string? QuestionIndex
        {
            get => _questionIndex;
            set
            {
                _questionIndex = value;
                OnPropertyChanged(nameof(QuestionIndex));
            }
        }

        public int CurrentQuestionIndex
        {
            get => _currentQuestionIndex;
            set
            {
                _currentQuestionIndex = value;
                OnPropertyChanged(nameof(CurrentQuestionIndex));
                LoadQuestion();
            }
        }
        public int Score = 0;
        public int? SelectedAnswerIndex
        {
            get => _selectedAnswerIndex;
            set
            {
                _selectedAnswerIndex = value;
                OnPropertyChanged(nameof(SelectedAnswerIndex));
            }
        }

        public TimerService Timer = new();

        // collection
        public List<string> AnswerContents { get; set; } = new() { "", "", "", "", "" };
        public List<Question> Questions { get; set; }

        // command
        public RelayCommand GoToNextQuestionEvent { get; }

        // logic
        private void LoadQuestion()
        {
            SelectedAnswerIndex = null;

            var Question = Questions[CurrentQuestionIndex];
            QuestionText = Question.Questiontext;
            QuestionIndex = $"Вопрос №{CurrentQuestionIndex + 1}";

            for (int i = 0; i < 5; i++)
            {
                AnswerContents[i] = Question.Answers.ElementAtOrDefault(i)?.Answertext ?? "";
            }

            OnPropertyChanged(nameof(AnswerContents));
        }
        private void GoNext()
        {
            if (SelectedAnswerIndex is null)
            {
                MessageBox.Show("Выберите ответ перед продолжением.");
                return;
            }

            CheckIsCorrect();

            if (CurrentQuestionIndex < Questions.Count - 1)
            {
                CurrentQuestionIndex++;
                LoadQuestion();
            }
            else
            {
                Timer.Stop();

                Session.UserScore = Score;

                _сurrentResults.Userid = Session.UserID;
                _сurrentResults.Testdate = DateTime.Now;
                _сurrentResults.Userscore = Session.UserScore;

                if (Score >= 8)
                {
                    _сurrentResults.Statusid = 1;
                }
                else
                {
                    _сurrentResults.Statusid = 2;
                }


                try
                {
                    using (var Context = new AppDbContext())
                    {
                        Context.Results.Add(_сurrentResults);
                        Context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении результата: {ex.Message}");
                    throw;
                }
                _navigation.NavigateTo<ResultsViewModel>();
            }
        }
        private void CheckIsCorrect()
        {
            var Question = Questions[CurrentQuestionIndex];
            var Answers = Question.Answers.OrderBy(a => a.Answerid);
            var CorrectAnswer = Answers.FirstOrDefault(a => a.Iscorrectanswer);



            var AnswersList = Answers.ToList();

            if (SelectedAnswerIndex.HasValue)
            {
                var SelectedAnswer = AnswersList[SelectedAnswerIndex.Value];

                var CurrentUserAnswer = new Useranswer
                {
                    Userid = Session.UserID,
                    Questionid = Question.Questionid,
                    Answerid = SelectedAnswer.Answerid,
                    Answerdate = DateTime.Now,
                    Iscorrect = SelectedAnswer.Answerid == CorrectAnswer?.Answerid
                };

                if (CurrentUserAnswer.Iscorrect)
                {
                    Score++;
                }

                try
                {
                    using (var Context = new AppDbContext())
                    {
                        Context.Useranswers.Add(CurrentUserAnswer);
                        Context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении ответа: {ex.Message}");
                    throw;
                }
            }
            else
            {
                MessageBox.Show(
                    "Выбранный ответ имел неверные значения",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
        }
        private void Timer_TimeUpdated(object? sender, EventArgs e)
        {
            TimeLeft = Timer.GetTimeLeft().ToString(@"mm\:ss");

            if (Timer.GetTimeLeft().TotalSeconds == 0)
            {
                Timer.Stop();
                MessageBox.Show(
                    "Тест закрыт по истечению времени прохождения.",
                    "Кажется, вы не успели...",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                // добавление в базу данных данных о закрытии теста
                _сurrentResults.Userid = Session.UserID;
                _сurrentResults.Testdate = DateTime.Now;
                _сurrentResults.Userscore = 0;
                _сurrentResults.Statusid = 2;

                try
                {
                    using (var Context = new AppDbContext())
                    {
                        Context.Results.Add(_сurrentResults);
                        Context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении результата: {ex.Message}");
                    throw;
                }
                Timer.TimeUpdated -= Timer_TimeUpdated;
                _navigation.NavigateTo<LoginViewModel>();
            }
        }
    }
}
