using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using StudentApp.Models;
using System.Reactive;

namespace StudentApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private int _course;
        private int _group;
        private int _semester;
        private int _grade;
        private string _averageGrade = string.Empty;
        private string _debts = string.Empty;
        private string? _selectedSubject;
        private string _selectedSubjectAverageGrade = string.Empty;
        public string? SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedSubject, value);
                CalculateSubjectAverageGrade();
            }
        }

        public string SelectedSubjectAverageGrade
        {
            get => _selectedSubjectAverageGrade;
            set => this.RaiseAndSetIfChanged(ref _selectedSubjectAverageGrade, value);
        }

        public string FirstName { get => _firstName; set => this.RaiseAndSetIfChanged(ref _firstName, value); }
        public string LastName { get => _lastName; set => this.RaiseAndSetIfChanged(ref _lastName, value); }
        public int Group { get => _group; set => this.RaiseAndSetIfChanged(ref _group, value); }
        public int Course { get => _course; set => this.RaiseAndSetIfChanged(ref _course, value); }
        public int Semester { get => _semester; set => this.RaiseAndSetIfChanged(ref _semester, value); }
        public int Grade { get => _grade; set => this.RaiseAndSetIfChanged(ref _grade, value); }
        public string AverageGrade { get => _averageGrade; set => this.RaiseAndSetIfChanged(ref _averageGrade, value); }
        public string Debts { get => _debts; set => this.RaiseAndSetIfChanged(ref _debts, value); }

        public ObservableCollection<Student> Students { get; } = new();
        public ObservableCollection<string> Subjects { get; } = new();

        public ReactiveCommand<Unit, Unit> AddStudent { get; }
        public ReactiveCommand<Unit, Unit> AddGrade { get; }
        public ReactiveCommand<Unit, Unit> CalculateStatistics { get; }

        private Student? _selectedStudent;
        public Student? SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedStudent, value);
                UpdateSubjects();
            }
        }

        public MainWindowViewModel()
        {
            AddStudent = ReactiveCommand.Create(AddStudentAction);
            AddGrade = ReactiveCommand.Create(AddGradeAction);
            CalculateStatistics = ReactiveCommand.Create(CalculateStatisticsAction);
        }

        private void AddStudentAction()
        {
            var student = new Student(FirstName, LastName, DateTime.Now, Course, Group, InitialSubjects.Subjects);
            Students.Add(student);
            SelectedStudent = student;
            UpdateSubjects();
        }

        private void AddGradeAction()
        {
            if (SelectedStudent != null && !string.IsNullOrWhiteSpace(SelectedSubject))
            {
                SelectedStudent.AddGrade(InitialSubjects.GetSemesterBySubject(SelectedSubject), SelectedSubject, Grade);
                UpdateSubjects();
            }
        }

        private void CalculateStatisticsAction()
        {
            if (SelectedStudent != null)
            {
                AverageGrade = $"Средний балл: {SelectedStudent.GetAverageGrade():0.00}";
                var debts = SelectedStudent.GetDebts();
                Debts = debts.Any()
                    ? $"Задолженности: {string.Join(", ", debts.Select(d => $"Семестр {d.Semester}: {d.Subject}"))}"
                    : "Нет задолженностей";
            }
        }

        private void UpdateSubjects()
        {
            Subjects.Clear();
            if (SelectedStudent != null)
            {
                foreach (var subjectList in SelectedStudent.SubjectsPerSemester.Values)
                {
                    foreach (var subject in subjectList)
                    {
                        if (!Subjects.Contains(subject))
                            Subjects.Add(subject);
                    }
                }
            }
        }

        private void CalculateSubjectAverageGrade()
        {
            if (SelectedStudent != null && !string.IsNullOrWhiteSpace(SelectedSubject))
            {
                double avgGrade = SelectedStudent.GetAverageBySubject(SelectedSubject);
                SelectedSubjectAverageGrade = $"Балл по {SelectedSubject}: {avgGrade:0.00}";
            }
            else
            {
                SelectedSubjectAverageGrade = "Выберите предмет";
            }
        }

    }
}
