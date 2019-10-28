using System;
using System.IO;

namespace DatabaseManipulator
{

    class Record
    {
        /// <summary>
        /// Класс, представляющий одну запись в БД
        /// </summary>

        public int Id { get; private set; }
        public DateTime Datetime { get; private set; }
        public double Value { get; private set; }

        public Record(int id, DateTime dt, double val)
        {
            Id = id;
            Datetime = dt;
            Value = val;
        }

        /// <summary>
        /// Cериализует объект
        /// </summary>
        /// <returns></returns>
        public byte[] Serialize()
        {
            byte[] bytes = new byte[20];
            BitConverter.GetBytes(Id).CopyTo(bytes, 0);
            BitConverter.GetBytes(Datetime.ToBinary()).CopyTo(bytes, 4);
            BitConverter.GetBytes(Value).CopyTo(bytes, 12);
            return bytes;
        }
    }

    interface IDto
    {

    }

    static class DbInteractor
    {
        public static Database Database { get; private set; }
        public static void Open(string path)
        {
            //
        }
        //public static bool Create(Record r)
        //{
        //    // 
        //}
        //public static bool Read(int id, out IDto responseDto)
        //{
        //    //
        //}
        //public static bool Update(int id, Record r)
        //{
        //    //
        //}
        //public static bool Delete(int id)
        //{
        //    //
        //}
    }

    class Database
    {
        public Record[] Records { get; private set; }
        public Database(Record[] records)
        {
            Records = records;
        }
        public Record this[int i]
        {
            get => Records[i];
            set { Records[i] = value; }
        }
    }
}