using System;

namespace DatabaseManipulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Record rec = new Record(1, DateTime.Parse("01.05.2016 04:15:29"), 0.92d);
            byte[] ser = rec.Serialize();
            //Console.WriteLine(BitConverter.GetBytes(rec.Value)); 
            //long l1 = Convert.ToInt64(0.92d);
            //long l2 = BitConverter.DoubleToInt64Bits(1.234);


        }
    }
}
