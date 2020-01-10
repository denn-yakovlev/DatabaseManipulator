using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace DatabaseManipulator
{
    static class UserLayer
    {
        public static void PrintMenu()
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
            int choice = Console.Read() - '0';
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

        }
        static void ReadRecord()
        {
            throw new NotImplementedException();
        }
        static void UpdateRecord()
        {
            throw new NotImplementedException();
        }
        static void DeleteRecord()
        {
            throw new NotImplementedException();
        }
        static void ReadAll()
        {
            throw new NotImplementedException();
        }
        static void SolveProblem()
        {
            throw new NotImplementedException();
        }
    }
}
