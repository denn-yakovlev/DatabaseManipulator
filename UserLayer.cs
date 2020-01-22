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
            Console.WriteLine();
            Console.WriteLine(@"1. Добавить запись в БД
2. Прочитать запись
3. Обновить запись
4. Удалить запись
5. Прочитать все записи
6. Решить задачу
0. Выход");
        }

        static T ParseUntilOk<T>(string promptText = "", string FailureText = "") 
            where T: struct
        {
           
            T target = default(T);
            IFormatProvider provider;
            if (target is int)
            {
                provider = NumberFormatInfo.CurrentInfo;
            }
            else if (target is DateTime)
            {
                provider = DateTimeFormatInfo.CurrentInfo;
            }
            else if (target is double)
            {
                provider = new NumberFormatInfo() { NumberDecimalSeparator = "." };
            }
            else
            {
                throw new Exception(@"T должно быть double/DateTime/int");
            }
            string line;
            Console.WriteLine(promptText);
            while (true)
            {
                line = Console.ReadLine();
                try
                {
                    target = (T)((IConvertible)line).ToType(typeof(T), provider);
                    return target;
                }
                catch(FormatException)
                {
                    Console.WriteLine(FailureText);
                    Console.WriteLine();
                }          
                
            }
            
        }

        public static void Interpret()
        {
          
            int choice;
            do
            {
                PrintMenu();
                choice = ParseUntilOk<int>(FailureText: "Введите целое число!");
                if (choice < 0 || choice > 6)
                {
                    Console.WriteLine("Некорректный выбор, введите снова");
                    continue;
                }
                SwitchOption(choice);
            } while (choice != 0);
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
            int id = ParseUntilOk<int>(
                "Введите ID для записи (целое число)", 
                "Неверный ввод, попробуйте ещё раз"
                );
            DateTime dt = ParseUntilOk<DateTime>(
                "Введите дату и время (дд.ММ.гггг чч:мм:сс)", 
                "Неверный ввод, попробуйте ещё раз"
                );
            double val = ParseUntilOk<double>(
                "Введите значение (дробное число, десятич. разделитель - точка)", 
                "Неверный ввод, попробуйте ещё раз"
                );
            
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
            int id = ParseUntilOk<int>(
                "Введите ID для записи (целое число)",
                "Неверный ввод, попробуйте ещё раз"
                );
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
            int id = ParseUntilOk<int>(
                "Введите ID для записи (целое число)",
                "Неверный ввод, попробуйте ещё раз"
                );
            DateTime dt = ParseUntilOk<DateTime>(
                "Введите дату и время (дд.ММ.гггг чч:мм:сс)",
                "Неверный ввод, попробуйте ещё раз"
                );
            double val = ParseUntilOk<double>(
                "Введите значение (дробное число, десятич. разделитель - точка)",
                "Неверный ввод, попробуйте ещё раз"
                );
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
            int id = ParseUntilOk<int>(
                "Введите ID для записи (целое число)",
                "Неверный ввод, попробуйте ещё раз"
                );
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
            Console.WriteLine();
            foreach (var rec in records)
            {
                Console.WriteLine($"{rec.Id}   {rec.Datetime}   {rec.Value}");
            }
            Console.WriteLine();
        }

        static void SolveProblem()
        {
            //throw new NotImplementedException();
            string problemText = @"Дана последовательность показаний некоего датчика во времени. 
Отобразить на экране время начала n-ого вхождения заданной последовательности 
искомая последовательность может быть длиной до 5 символов). 
В измерениях «вхождения» последовательности могут повторяться не один раз;
";
            Console.WriteLine(problemText);    
            int entryNum = ParseUntilOk<int>(
                "Введите номер вхождения (целое число)",
                "Неверный ввод, попробуйте ещё раз"
                );
            string line;
            Console.WriteLine("Введите последовательность значений через пробел(дробное число, десятич. разделитель - точка)");
            double val;
            bool isParseOk = false;
            var nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };
            List<double> valuesSeq = new List<double>();
            while (true)
            {
                line = Console.ReadLine();
                string[] split = line.Split(" ");
                foreach (string s in split)
                {
                    isParseOk = double.TryParse(s, NumberStyles.Number, nfi, out val);
                    if (!isParseOk)
                    {
                        Console.WriteLine("Неверный ввод, попробуйте ещё раз");
                        break;
                    }
                    else
                    {
                        valuesSeq.Add(val);
                    }
                }
                if (isParseOk)
                {
                    break;
                }
            }  
            try
            {
                DateTime dt = Controller.SolveProblem(entryNum, valuesSeq);
                Console.WriteLine();
                Console.WriteLine("OK");
                Console.WriteLine(dt);
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }
    }
}
