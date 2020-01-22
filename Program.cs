using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
namespace DatabaseManipulator
{
    
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DatabaseInteractor.Open("../../../input.csv");
                UserLayer.Interpret();
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Ошибка: {exc.Message}");
            }
            finally
            {
                DatabaseInteractor.Save();
            }
            
        }
    }
}
