using FireTestingApp.Models;
using FireTestingApp_net8.Models.Shema;
using FireTestingApp_net8.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Navigation;
using System.Windows.Threading;
using static System.Formats.Asn1.AsnWriter;

namespace FireTestingApp_net8.ViewModels
{
    public class MainTestViewModel : BaseViewModel
    {
        ITimerService Timer = new ITimerService();
        private string? _timeLeft;
        public string? TimeLeft
        {
            get => _timeLeft;
            set
            {
                _timeLeft = value;
                OnPropertyChanged(nameof(TimeLeft));
            }
        }

        private readonly INavigationService _nav;

        private int _currentQuestionIndex { get; set; }
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

        private string? _questionText;
        public string? QuestionText
        {
            get => _questionText;
            set
            {
                _questionText = value;
                OnPropertyChanged(nameof(QuestionText));
            }
        }

        private string? _questionIndex;
        public string? QuestionIndex
        {
            get => _questionIndex;
            set
            {
                _questionIndex = value;
                OnPropertyChanged(nameof(QuestionIndex));
            }
        }

        public int Score = 0;

        public List<string> AnswerContents { get; set; } = new() { "", "", "", "", "" };
        public List<Question> Questions { get; set; }

        public MainTestViewModel(INavigationService nav)
        {
            _nav = nav;

            Timer.SetMinutes(1);
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
        }

        private void LoadQuestion()
        {
            SelectedAnswerIndex = null;

            var Question = Questions[CurrentQuestionIndex];
            QuestionText = Question.Questiontext;
            QuestionIndex = $"Вопрос №{CurrentQuestionIndex + 1}";

            for(int i =0;  i < 5; i++)
            {
                AnswerContents[i] = Question.Answers.ElementAtOrDefault(i)?.Answertext ?? "";
            }

            OnPropertyChanged(nameof(AnswerContents));
        }

        private int? _selectedAnswerIndex { get; set; }
        public int? SelectedAnswerIndex
        {
            get => _selectedAnswerIndex;
            set
            {
                _selectedAnswerIndex = value;
                OnPropertyChanged(nameof(SelectedAnswerIndex));
            }
        }

        public RelayCommand GoToNextQuestionEvent { get; }
        private Result CurrentResults = new Result();
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

                CurrentResults.Userid = Session.UserID;
                CurrentResults.Testdate = DateTime.Now;
                CurrentResults.Userscore = Session.UserScore;

                if (Score >= 8)
                {
                    CurrentResults.Statusid = 1;
                }
                else
                {
                    CurrentResults.Statusid = 2;
                }


                try
                {
                    using(var Context = new AppDbContext())
                    {
                        Context.Results.Add(CurrentResults);
                        Context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении результата: {ex.Message}");
                    throw;
                }
                _nav.NavigateTo<ResultsViewModel>();
            }
        }

        private void CheckIsCorrect()
        {
            var Question = Questions[CurrentQuestionIndex];
            var Answers = Question.Answers.OrderBy(a => a.Answerid);
            var CorrectAnswer = Answers.FirstOrDefault(a => a.Iscorrectanswer);

            

                var AnswersList = Answers.ToList();
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
                Score++;

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
                CurrentResults.Userid = Session.UserID;
                CurrentResults.Testdate = DateTime.Now;
                CurrentResults.Userscore = 0;
                CurrentResults.Statusid = 2;

                try
                {
                    using(var Context = new AppDbContext())
                    {
                        Context.Results.Add(CurrentResults);
                        Context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении результата: {ex.Message}");
                    throw;
                }
                Timer.TimeUpdated -= Timer_TimeUpdated;
                _nav.NavigateTo<LoginViewModel>();
            }
        }

    }
}
