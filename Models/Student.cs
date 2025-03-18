using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentApp.Models
{
    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Course { get; set; }
        public int Group { get; set; }

        // {семестр -> {предмет -> оценка}}
        private Dictionary<int, Dictionary<string, int>> grades = new();

        // {семестр -> [список предметов]}
        private Dictionary<int, List<string>> subjectsPerSemester = new();

        public Dictionary<int, List<string>> SubjectsPerSemester => subjectsPerSemester;

        public int Age => DateTime.Now.Year - BirthDate.Year - 
            (DateTime.Now.DayOfYear < BirthDate.DayOfYear ? 1 : 0);

        // Индексатор для оценок
        public int this[int semester, string subject]
        {
            get => grades.ContainsKey(semester) && grades[semester].ContainsKey(subject) 
                ? grades[semester][subject] 
                : 0;
            set
            {
                if (!subjectsPerSemester.ContainsKey(semester) || !subjectsPerSemester[semester].Contains(subject))
                {
                    throw new ArgumentException($"Предмет {subject} отсутствует в семестре {semester}.");
                }
                if (value < 2 || value > 5)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Оценка должна быть от 2 до 5.");
                }
                if (!grades.ContainsKey(semester))
                    grades[semester] = new Dictionary<string, int>();

                grades[semester][subject] = value;
            }
        }

        public Student(string firstName, string lastName, DateTime birthDate, int course, int group,
                    Dictionary<int, List<string>> subjects)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Course = course;
            Group = group;
            subjectsPerSemester = subjects.ToDictionary(entry => entry.Key, entry => new List<string>(entry.Value));
        }

        public double GetAverageGrade()
        {
            var allGrades = grades.Values.SelectMany(s => s.Values).ToList();
            return allGrades.Any() ? allGrades.Average() : 0;
        }

        public double GetAverageBySubject(string subject)
        {
            var subjectGrades = grades.Values
                .Where(s => s.ContainsKey(subject))
                .Select(s => s[subject])
                .ToList();
            return subjectGrades.Any() ? subjectGrades.Average() : 0;
        }

        public void AddGrade(int semester, string subject, int grade)
        {
            if (!grades.ContainsKey(semester))
                grades[semester] = new Dictionary<string, int>();

            grades[semester][subject] = grade;
        }

        public int GetGrade(int semester, string subject)
        {
            return grades.ContainsKey(semester) && grades[semester].ContainsKey(subject) 
                ? grades[semester][subject] 
                : 0;
        }

        public List<(int Semester, string Subject)> GetDebts() =>
            grades
                .SelectMany(g => g.Value.Where(s => s.Value < 3).Select(s => (g.Key, s.Key)))
                .ToList();
    }
}
