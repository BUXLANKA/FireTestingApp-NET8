using CommunityToolkit.Mvvm.Messaging;
using FireTestingApp_net8.Models.Shema;
using FireTestingApp_net8.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FireTestingApp_net8.ViewModels
{
    public class QuestionEditorViewModel : BaseViewModel
    {
        private readonly INavigationService _navigation;

        public QuestionEditorViewModel(INavigationService navigation)
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
                MessageBox.Show($"dbg questiontext nul");
                return;
            }

            if (Answers.Any(a => string.IsNullOrWhiteSpace(a.Answertext)))
            {
                MessageBox.Show($"dbg answer txt is nul");
                return;
            }

            if (SelectedAnswerIndex < 0 || SelectedAnswerIndex >= Answers.Count)
            {
                MessageBox.Show($"dbg answer index is nul");
                return;
            }

            using (var context = new AppDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
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
                                MessageBox.Show($"dbg question not ex");
                                return;
                            }

                            existedQuestion.Questiontext = QuestionObject.Questiontext;

                            //context.SaveChanges();
                                                        
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
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"{ex.Message}");
                        throw;
                    }
                }               
            }
        }
        private void Cancel()
        {
            _navigation.GoBack();
        }
    }
}
