using System;
using System.IO;
using System.Collections.Generic;

namespace DatabaseManipulator
{

    class Record
    {
        /// <summary>
        /// Класс, представляющий одну запись из БД
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
        /// Cериализует запись из БД
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

        public override bool Equals(object obj)
        {
            if (obj is Record)
            {
                Record record = obj as Record;
                return Id == record.Id &&
                       Datetime == record.Datetime &&
                       Value == record.Value;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Datetime, Value);
        }

        public static bool operator == (Record r1, Record r2)
        {
            return r1.Equals(r2);
        }
        public static bool operator !=(Record r1, Record r2)
        {
            return !(r1 == r2);
        }


    }

    class Database
    {
        public List<Record> Records { get; private set; }
        public Database(List<Record> records)
        {
            Records = records;
        }
        public Record this[int id]
        {
            get
            { 
                foreach (Record rec in Records)
                {
                    if (rec.Id == id)
                    {
                        return rec;
                    }

                }
                throw new ArgumentOutOfRangeException($"No such Id in Database: {id}");
            }
            set
            {
                foreach (Record rec in Records)
                {
                    if (rec.Id == id)
                    {
                        // Shit code
                        Records.Remove(rec);
                        Records.Add(value);
                    }
                    
                }
                throw new ArgumentOutOfRangeException($"No such Id in Database: {id}");
            }
        }
    }


    static class DbInteractor
    {
        public static Database Database { get; private set; }

        public static void Open(string path)
        {
            //FileStream db =  File.Open(path, FileMode.Open, FileAccess.ReadWrite);
            //byte[] buf = new byte[db.Length];
            //db.Read(buf);
            string[] lines = File.ReadAllLines(path);
            Database = ParseCsv(lines);

        }
        static Database ParseCsv(string[] lines)
        {

            string[] splittedLine = new string[3];
            List<Record> records = new List<Record>();
            int id;
            DateTime dt;
            double val;

            for (int i = 1; i < lines.Length; i++)
            {
                splittedLine = lines[i].Split(',');
                id = int.Parse(splittedLine[0]);
                dt = DateTime.Parse(splittedLine[1]);
                val = double.Parse(splittedLine[2]);
                records.Add( new Record(id, dt, val) );
            }
            return new Database(records);
        }
        public static Response Create(Request request)
        {
            ResponseCodes respCode;
            Record recToCreate = request.ToRecord();
            if (Database.Records.Contains(recToCreate))
            {
                respCode = ResponseCodes.EXISTS_ID;
            }
            else
            {
                respCode = ResponseCodes.OK;
            }
            return new Response(respCode);
        }
        public static Response Read(Request request)
        {
            ResponseCodes respCode;
            byte[] respData = null;
            int id = int.Parse(request.ToString());
            try
            {
                Record rec = Database[id];
                respCode = ResponseCodes.OK;
                respData = rec.Serialize();
            }
            catch (ArgumentOutOfRangeException)
            {
                respCode = ResponseCodes.NO_ID;
                //string message = $"В базе данных нет записи с id == {id}";
                //respData = message.Serialize();
            }
            return new Response(respCode, respData);
        }
        public static Response Update(Request request)
        {
            ResponseCodes respCode;
            Record recToUpdate = request.ToRecord();
            int id = recToUpdate.Id;
            try
            {
                Database[id] = recToUpdate;
                respCode = ResponseCodes.OK;
            }
            catch (ArgumentOutOfRangeException)
            {
                respCode = ResponseCodes.NO_ID;
            }
            return new Response(respCode);
        }
        public static Response Delete(Request request)
        {
            ResponseCodes respCode;
            int id = int.Parse(request.ToString());
            try
            {
                Database.Records.RemoveAt(id);
                respCode = ResponseCodes.OK;
            }
            catch(ArgumentOutOfRangeException)
            {
                respCode = ResponseCodes.NO_ID;
            }
            return new Response(respCode);
        }
    }

    public static class StringSerializer
    {
        /// <summary>
        /// Метод-расширитель для сериализации строки
        /// </summary>
        
        public static byte[] Serialize(this string s)
        {
            char[] charsSplit = s.ToCharArray();
            byte[] bytes = new byte[2 * charsSplit.Length];
            for (int i = 0; i < charsSplit.Length; i++)
            {
                BitConverter.GetBytes(charsSplit[i]).CopyTo(bytes, 2 * i);
            }
            return bytes;
        }
    }

    
}