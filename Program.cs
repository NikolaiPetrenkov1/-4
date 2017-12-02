//Домашняя работа номер 4
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace Lab1
{
    class Program
    {
        static void Main()
        {
           
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en");

            List<Airplane> airplans = LoadData("airplans.txt");
            bool repeat = true;
            while (repeat)
            {
                int choice = Menu();
                switch (choice)
                {
                    case 1:
                        airplans.Add(Create());
                        break;
                    case 2:
                        Print(airplans);
                        break;
                    case 3:
                        CompareWithNumber(airplans);
                        break;
                    case 4:
                        CompareWithAirplane(airplans);
                        break;
                    case 5:
                        repeat = false;
                        break;
                }
            }
        }

        
        private static List<Airplane> LoadData(string path)
        {
           
            if (!File.Exists(path)) return new List<Airplane>();
            string[][] data = File.ReadAllLines(path).Select(x => x.Split('|')).ToArray();

            Model model = 0;
            double speed = 0;
            Model[] values = Enum.GetValues(typeof(Model)).Cast<Model>().ToArray();
            return data
                .Where(row => Enum.TryParse(row[0], out model) && values.Contains(model) && double.TryParse(row[1], out speed))
                .Select(x => new Airplane(model, speed)).ToList();
        }

        private static int Menu()
        {
            Console.WriteLine("\n Выберите требуемое действие: ");
            Console.WriteLine("1. Создать новый самолет");
            Console.WriteLine("2. Посмотреть список существующих самолетов");
            Console.WriteLine("3. Выбрать самолет и сравнить его скорость с числом");
            Console.WriteLine("4. Сравнить самолеты между собой");
            Console.WriteLine("5. Выход");
            Console.WriteLine();
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 && choice > 5)
                Console.WriteLine("Ошибка");
            return choice;
        }

        private static Airplane Create()
        {
            Console.WriteLine("\n Выберите модель самолета:");
            string[] models = Enum.GetNames(typeof(Model));
            Model[] values = Enum.GetValues(typeof(Model)).Cast<Model>().ToArray();
            for (int i = 0; i < models.Length; i++)
                Console.WriteLine("{0} - {1}", (int)values[i], models[i]);
            Console.WriteLine();

            Model model;
            while (!Enum.TryParse(Console.ReadLine(), out model) || !values.Contains(model))
                Console.WriteLine("Ошибка!");
            Console.Write("Введите скорость самолета: ");
            double speed;
            while (!double.TryParse(Console.ReadLine(), out speed) || speed <= 0)
                Console.WriteLine("Ошибка!");
            return new Airplane(model, speed);
        }

        private static void Print(List<Airplane> airplans)
        {
            Console.WriteLine("\nСамолеты:");
            if (airplans.Count == 0)
                Console.WriteLine("Нет в наличии");
            else
                for (int i = 0; i < airplans.Count; i++)
                    Console.WriteLine("{0}: {1}", i, airplans[i]);
            Console.WriteLine();
        }

        private static void CompareWithNumber(List<Airplane> airplans)
        {
            Console.Write("\n Номер самолета: ");
            int i = GetNumber(airplans.Count);
            Console.Write("Число: ");
            double speed;
            while (!double.TryParse(Console.ReadLine(), out speed))
                Console.WriteLine("Ошибка");
            if (airplans[i].Compare(speed))
                Console.WriteLine("Скорость самолета больше");
            else
                Console.WriteLine("Скорость самолета меньше");
            Console.WriteLine();
        }

        private static void CompareWithAirplane(List<Airplane> airplans)
        {
            Console.Write("\n Номер первого самолета: ");
            int i = GetNumber(airplans.Count);
            Console.Write("Номер второго самолета: ");
            int j = GetNumber(airplans.Count);
            if (airplans[i].Compare(airplans[j]))
                Console.WriteLine("Скорость первого самолета больше второго");
            else
                Console.WriteLine("Скорость первого самолета меньше второго");
            Console.WriteLine();
        }

        private static int GetNumber(int count)
        {
            int number;
            while (!int.TryParse(Console.ReadLine(), out number) || number < 0 || number >= count)
                Console.WriteLine("Ошибка");
            return number;
        }

        public class Airplane
        {
          
            public Airplane(Model model, double speed)
            {
                Speed = speed;
                Model = model;
            }

            public Model Model { get; set; }
            public double Speed { get; set; }

           
            public bool Compare(double speed)
            {
                return Speed > speed;
            }

            
            public bool Compare(Airplane airplane)
            {
                return Speed > airplane.Speed;
            }

            public override string ToString()
            {
                return string.Format("модель {0}, скорость {1}", Model, Speed);
            }
        }

        public enum Model
        {
            Ту134 = 1,
            Ан2,
            Кошка,
            Як42
        }
    }
}

