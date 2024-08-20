using System;
using System.Collections.Generic;
using System.Linq;

namespace PracticTask1
{
    class Program
    {
        static void Main(string[] args)
        {
            var vacationDictionary = new Dictionary<string, List<DateTime>>()
            {
                ["Иванов Иван Иванович"] = new List<DateTime>(),
                ["Петров Петр Петрович"] = new List<DateTime>(),
                ["Юлина Юлия Юлиановна"] = new List<DateTime>(),
                ["Сидоров Сидор Сидорович"] = new List<DateTime>(),
                ["Павлов Павел Павлович"] = new List<DateTime>(),
                ["Георгиев Георг Георгиевич"] = new List<DateTime>()
            };

            var availableWorkingDaysOfWeek = new HashSet<DayOfWeek>
            {
                DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
                DayOfWeek.Thursday, DayOfWeek.Friday
            };

            var vacations = new List<DateTime>();
            var allVacationCount = 0;
            var random = new Random();

            foreach (var kvp in vacationDictionary)
            {
                var dateList = kvp.Value;
                const int initialVacationDays = 28;

                DateTime start = new DateTime(DateTime.Now.Year, 1, 1);
                DateTime end = new DateTime(DateTime.Today.Year, 12, 31);

                int vacationCount = initialVacationDays;
                while (vacationCount > 0)
                {
                    var startDate = start.AddDays(random.Next((end - start).Days));

                    if (availableWorkingDaysOfWeek.Contains(startDate.DayOfWeek))
                    {
                        var vacationSteps = new[] { 7, 14 };
                        var vacationLength = vacationSteps[random.Next(vacationSteps.Length)];
                        var endDate = startDate.AddDays(vacationLength);

                        if (vacationCount < vacationLength)
                        {
                            endDate = startDate.AddDays(vacationCount);
                            vacationLength = vacationCount;
                        }

                        // Check if the vacation period is free from overlapping or gaps
                        bool canCreateVacation = !vacations.Any(d => d >= startDate && d < endDate)
                            && !vacations.Any(d => d.AddDays(3) >= startDate && d.AddDays(3) < endDate)
                            && !dateList.Any(d => d.AddMonths(1) >= startDate && d.AddMonths(1) < endDate)
                            && !dateList.Any(d => d.AddMonths(-1) <= endDate && d.AddMonths(-1) > startDate);

                        if (canCreateVacation)
                        {
                            for (DateTime dt = startDate; dt < endDate; dt = dt.AddDays(1))
                            {
                                vacations.Add(dt);
                                dateList.Add(dt);
                            }
                            allVacationCount += vacationLength;
                            vacationCount -= vacationLength;
                        }
                    }
                }
            }

            foreach (var kvp in vacationDictionary)
            {
                var setDateList = kvp.Value;
                Console.WriteLine("Дни отпуска " + kvp.Key + " : ");
                foreach (var date in setDateList)
                {
                    Console.WriteLine(date.ToShortDateString());
                }
            }

            Console.ReadKey();
        }
    }
}

