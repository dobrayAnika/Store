using System.Collections.Generic;

namespace StudentApp.Models
{
    public static class InitialSubjects
    {
        public static Dictionary<int, List<string>> Subjects = new()
        {
            { 1, new List<string> { "Математика", "Физика", "История" } },
            { 2, new List<string> { "Программирование", "Логика", "Математическая статистика" } },
            { 3, new List<string> { "Алгоритмы и структуры данных", "Теория вероятностей", "Технический английский" } },
            { 4, new List<string> { "Операционные системы", "Базы данных", "Инженерная графика" } },
            { 5, new List<string> { "Экономика", "Этика", "Социология" } },
            { 6, new List<string> { "Проектирование программного обеспечения", "Психология", "Философия" } }
        };

        public static int GetSemesterBySubject(string subject)
        {
            foreach (var semester in Subjects)
            {
                if (semester.Value.Contains(subject))
                {
                    return semester.Key;
                }
            }

            return -1;
        }
    }
}
