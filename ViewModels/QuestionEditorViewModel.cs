using FireTestingApp_net8.Models.Shema;
using FireTestingApp_net8.Services;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows;

namespace FireTestingApp_net8.ViewModels
{
    public class QuestionEditorViewModel : BaseViewModel
    {
        // private
        private readonly INavigationService _navigation;

        private string? _newQuestionText;
        private string? _answerTextVariant1;
        private string? _answerTextVariant2;
        private string? _answerTextVariant3;
        private string? _answerTextVariant4;
        private string? _answerTextVariant5;

        private int? _selectedAnswerIndex;

        // constructor
        public QuestionEditorViewModel(INavigationService navigation)
        {
            _navigation = navigation;

            AddNewQuestionEvent = new RelayCommand(AddNewQuestion);
        }

        // public
        public int? SelectedAnswerIndex
        {
            get => _selectedAnswerIndex;
            set
            {
                _selectedAnswerIndex = value;
                OnPropertyChanged(nameof(SelectedAnswerIndex));
            }
        }
        public string? NewQuestionText
        {
            get => _newQuestionText;
            set
            {
                _newQuestionText = value;
                OnPropertyChanged(nameof(NewQuestionText));
            }
        }
        public string? AnswerTextVariant1
        {
            get => _answerTextVariant1;
            set
            {
                _answerTextVariant1 = value;
                OnPropertyChanged(nameof(AnswerTextVariant1));
            }
        }
        public string? AnswerTextVariant2
        {
            get => _answerTextVariant2;
            set
            {
                _answerTextVariant2 = value;
                OnPropertyChanged(nameof(AnswerTextVariant2));
            }
        }
        public string? AnswerTextVariant3
        {
            get => _answerTextVariant3;
            set
            {
                _answerTextVariant3 = value;
                OnPropertyChanged(nameof(AnswerTextVariant3));
            }
        }
        public string? AnswerTextVariant4
        {
            get => _answerTextVariant4;
            set
            {
                _answerTextVariant4 = value;
                OnPropertyChanged(nameof(AnswerTextVariant4));
            }
        }
        public string? AnswerTextVariant5
        {
            get => _answerTextVariant5;
            set
            {
                _answerTextVariant5 = value;
                OnPropertyChanged(nameof(AnswerTextVariant5));
            }
        }

        // collection


        // command
        public RelayCommand AddNewQuestionEvent { get; }

        // logic
        private void AddNewQuestion()
        {
            if (string.IsNullOrWhiteSpace(NewQuestionText))
            {
                MessageBox.Show($"DEBUG:STR IS NULL1");
                return;
            }

            if (SelectedAnswerIndex == null ||
                SelectedAnswerIndex < 0 ||
                SelectedAnswerIndex > 4)
            {
                MessageBox.Show($"debug: index is null");
                return;
            }

            if (string.IsNullOrWhiteSpace(AnswerTextVariant1) ||
                string.IsNullOrWhiteSpace(AnswerTextVariant2) ||
                string.IsNullOrWhiteSpace(AnswerTextVariant3) ||
                string.IsNullOrWhiteSpace(AnswerTextVariant4) ||
                string.IsNullOrWhiteSpace(AnswerTextVariant5))
            {
                MessageBox.Show($"debug answ str null1");
                return;
            }

            using (var context = new AppDbContext())
            {
                // добавление нового вопроса
                Question newQuestion = new()
                {
                    Questiontext = NewQuestionText
                };

                try
                {
                    context.Questions.Add(newQuestion);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}");
                    return;
                    throw;
                }

                // добавление ответов на вопрос
                var answers = new[]
                {
                    AnswerTextVariant1,
                    AnswerTextVariant2,
                    AnswerTextVariant3,
                    AnswerTextVariant4,
                    AnswerTextVariant5
                };

                for (int i = 0; i < answers.Length; i++)
                {
                    Answer newAnswer = new()
                    {
                        Questionid = newQuestion.Questionid,
                        Answertext = answers[i],
                        Iscorrectanswer = (i == SelectedAnswerIndex)
                    };
                    context.Answers.Add(newAnswer);
                }

                try
                {
                    context.SaveChanges();
                    MessageBox.Show($"debug: done");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}");
                    throw;
                }
            }
        }
    }
}
