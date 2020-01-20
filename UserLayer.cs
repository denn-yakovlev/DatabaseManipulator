using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace DatabaseManipulator
{
    static class UserLayer
    {
        static void PrintMenu()
        {
            Console.WriteLine(@"1. Добавить запись в БД
2. Прочитать запись
3. Обновить запись
4. Удалить запись
5. Прочитать все записи
6. Решить задачу
0. Выход");
        }

        public static void Interpret()
        {
            PrintMenu();
            Console.Write('>');
            int choice;
            bool isParseOk;
            do
            {
                isParseOk = int.TryParse(Console.ReadLine(), out choice);
                if (!isParseOk)
                {
                    Console.WriteLine("Введите целое число!");
                    Console.WriteLine();
                }
            } while (!isParseOk);
            while (choice != 0)
            {
                if (choice < 1 || choice > 6)
                {
                    Console.WriteLine("Некорректный выбор, введите снова");
                    continue;
                }
                SwitchOption(choice);
            }
        }

        static void SwitchOption(int choice)
        {
            switch (choice)
            {
                case 1: { CreateRecord(); break; }
                case 2: { ReadRecord(); break; }
                case 3: { UpdateRecord(); break; }
                case 4: { DeleteRecord(); break; }
                case 5: { ReadAll(); break; }
                case 6: { SolveProblem(); break; }
            }
        }

        static void CreateRecord()
        {
            Console.WriteLine();
            Console.WriteLine("Введите ID для записи (целое число)");
            // wtf??
            string line = Console.ReadLine();
            int id = int.Parse(line);
            Console.WriteLine("Введите дату и время (дд.ММ.гггг чч:мм:сс)");
            bool isParseOk;
            DateTime dt;
            do
            {
                line = Console.ReadLine();
                isParseOk = DateTime.TryParse(line, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out dt);
                if (!isParseOk)
                {
                    Console.WriteLine("Неверный ввод, попробуйте ещё раз");
                }
            }
            while (!isParseOk);

            var nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            double val;
            Console.WriteLine("Введите значение (дробное число)");
            do
            {
                line = Console.ReadLine();
                isParseOk = double.TryParse(line, NumberStyles.AllowDecimalPoint, nfi, out val);
                if (!isParseOk)
                {
                    Console.WriteLine("Неверный ввод, попробуйте ещё раз");
                }
            }
            while (!isParseOk);
            ResponseCodes respCode = Controller.CreateRecord(id, dt, val);
            if (respCode == ResponseCodes.OK)
            {
                Console.WriteLine("OK");
            }
            else if (respCode == ResponseCodes.EXISTS_ID)
            {
                Console.WriteLine("Id уже существует");
            }


        }
        static void ReadRecord()
        {
            Console.WriteLine();
            Console.WriteLine("Введите ID для записи (целое число)");
            // wtf??
            string line = Console.ReadLine();
            int id = int.Parse(line);
            DateTime dt;
            double val;
            ResponseCodes respCode = Controller.ReadRecord(id, out dt, out val);
            if (respCode == ResponseCodes.OK)
            {
                Console.WriteLine($"{id}   {dt}   {val}");
            }
            else if (respCode == ResponseCodes.NO_ID)
            {
                Console.WriteLine("Такого ID нет в БД");
            }
        }
        static void UpdateRecord()
        {
            Console.WriteLine();
            Console.WriteLine("Введите ID для записи (целое число)");
            // wtf??
            string line = Console.ReadLine();
            int id = int.Parse(line);
            Console.WriteLine("Введите новые дату и время (дд.ММ.гггг чч:мм:сс)");
            bool isParseOk;
            DateTime dt;

            do
            {
                line = Console.ReadLine();
                isParseOk = DateTime.TryParse(line, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out dt);
                if (!isParseOk)
                {
                    Console.WriteLine("Неверный ввод, попробуйте ещё раз");
                }
            } while (!isParseOk);

            var nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            double val;
            Console.WriteLine("Введите новое значение (дробное число)");
            do
            {
                line = Console.ReadLine();
                isParseOk = double.TryParse(line, NumberStyles.AllowDecimalPoint, nfi, out val);
                if (!isParseOk)
                {
                    Console.WriteLine("Неверный ввод, попробуйте ещё раз");
                }
            }
            while (!isParseOk);
            ResponseCodes respCode = Controller.UpdateRecord(id, dt, val);
            if (respCode == ResponseCodes.OK)
            {
                Console.WriteLine("OK");
            }
            else if (respCode == ResponseCodes.NO_ID)
            {
                Console.WriteLine("Id уже существует");
            }
        }
        static void DeleteRecord()
        {
            Console.WriteLine();
            Console.WriteLine("Введите ID для записи (целое число)");
            string line = Console.ReadLine();
            int id = int.Parse(line);
            ResponseCodes respCode = Controller.DeleteRecord(id);
            if (respCode == ResponseCodes.OK)
            {
                Console.WriteLine($"ОК");
            }
            else if (respCode == ResponseCodes.NO_ID)
            {
                Console.WriteLine("Такого ID нет в БД");
            }
        }
        static void ReadAll()
        {
            var records = Controller.ReadAll();
            foreach (var rec in records)
            {
                Console.WriteLine($"{rec.Id}   {rec.Datetime}   {rec.Value}");
            }
        }
        static void SolveProblem()
        {
            throw new NotImplementedException();
            string problemText = @"Дана последовательность показаний некоего датчика во времени. 
Отобразить на экране время начала n-ого вхождения заданной последовательности 
искомая последовательность может быть длиной до 5 символов). 
В измерениях «вхождения» последовательности могут повторяться не один раз;
";
            Console.WriteLine(problemText);
            Console.WriteLine("Введите номер вхождения (целое число)");
            string line = Console.ReadLine();
            int entryNum = int.Parse(line);
            Console.WriteLine("Введите последовательность значений через пробел(дробное число)");
            line = Console.ReadLine();
            string[] split = line.Split(" ");
            double d;
            var nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            List<double> valuesSeq = new List<double>();
            foreach (string s in split)
            {
                bool isParseOk = double.TryParse(s, NumberStyles.AllowDecimalPoint, nfi, out d);
                if (!isParseOk)
                {
                    Console.WriteLine("Неверный ввод, попробуйте ещё раз");
                }
                else
                {
                    valuesSeq.Add(d);
                }
            }
            DateTime dt = Controller.SolveProblem(entryNum, valuesSeq.ToArray());
            if (dt == DateTime.MinValue)
            {
                Console.WriteLine("Не найдено!");
            }
            else
            {
                Console.WriteLine($"OK: {dt}");
            }
        }
    }
}
