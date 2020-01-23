using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Globalization;

namespace DatabaseManipulator
{

    class Record
    {

        public int Id { get; private set; }
        public DateTime Datetime { get; private set; }
        public double Value { get; private set; }
   
        public Record(int id, DateTime dt, double val)
        {
            Id = id;
            Datetime = dt;
            Value = val;
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

        public static bool operator ==(Record r1, Record r2)
        {
            return r1.Equals(r2);
        }
        public static bool operator !=(Record r1, Record r2)
        {
            return !(r1 == r2);
        }

        public enum ComparisonBy
        {
            ID = 1,
            DATETIME,
            VALUE
        }

        public class Comparer : IComparer<Record>
        {
            public ComparisonBy By { get; private set; }

            public Comparer(ComparisonBy by)
            {
                By = by;
            }

            public int Compare(Record x, Record y)
            {
                switch (By)
                {
                    case ComparisonBy.ID: return x.Id.CompareTo(y.Id);
                    case ComparisonBy.DATETIME: return x.Datetime.CompareTo(y.Datetime);
                    case ComparisonBy.VALUE: return x.Value.CompareTo(y.Value);
                    default: throw new Exception();
                }
            }
        }

    }

    class DataBase: IEnumerable
    {
        public List<Record> Records { get; private set; }
        public DataBase()
        {
            Records = new List<Record>();
        }
        public DataBase(List<Record> records)
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
                for (int i = 0; i < Records.Count; i++)
                {
                    if (Records[i].Id == id)
                    {
                        Records[i] = value;
                        return;
                    }
                }
                throw new ArgumentOutOfRangeException($"No such Id in Database: {id}");
            }
        }
        
        public IEnumerator GetEnumerator()
        {
            return Records.GetEnumerator();
        }

        public void Sort(Record.ComparisonBy by)
        {
            Records.Sort(new Record.Comparer(by));
        }
    }

    static class CsvSerializer
    {
        const string sep = ",";
        static NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };
 
        public static byte[] Serialize(Record rec)
        {
            string ser = string.Format(nfi, "{0}{1}{2}{3}{4}", rec.Id, sep, rec.Datetime, sep, rec.Value);
            return new UTF8Encoding().GetBytes(ser);
        }
        public static Record Deserialize(byte[] bytes)
        {

            string[] subs = new UTF8Encoding().GetString(bytes).Split(sep);
            int id = int.Parse(subs[0]);
            DateTime dt = DateTime.Parse(subs[1]);
            double val = double.Parse(subs[2], nfi);
            return new Record(id, dt, val);
        }
    }

    static class DatabaseInteractor
    {
        
        public static DataBase Database { get; private set; }
        static FileStream DbFile { get; set; }
        /// <summary>
        /// Считывает базу данных из CSV-файла и сохраняет её как объект DataBase
        /// </summary>
        /// <param name="path">Путь до СSV-файла</param>
        public static void Open(string path)
        {
            Database = new DataBase();
            byte[] content;
            UTF8Encoding ue = new UTF8Encoding();
            foreach (string line in File.ReadLines(path))
            {
                content = ue.GetBytes(line);
                Record rec = CsvSerializer.Deserialize(content);
                Database.Records.Add(rec);
            }
            DbFile = File.Open(path, FileMode.Open, FileAccess.ReadWrite); 
        }
        
        public static void Save()
        {
            byte[] content;
            UTF8Encoding ue = new UTF8Encoding();
            
            //очищает содежимое файла
            DbFile.SetLength(0);
            DbFile.Flush();

            foreach (Record rec in Database)
            {
                content = CsvSerializer.Serialize(rec);
                DbFile.Write(content);
                DbFile.Write(ue.GetBytes(Environment.NewLine));
            }  
            DbFile.Close();   
        }

        static bool ExistsId(int id)
        {
            foreach (Record rec in Database)
            {
                if (rec.Id == id)
                {
                    return true;
                }
            }
            return false;
        }

        public static Response HandleRequest(Request request)
        {
            RequestTypes reqType = request.RequestType;
            int id = request.Id;
            DateTime dt = request.Datetime;
            double val = request.Value;
            switch(reqType)
            {
                case RequestTypes.CREATE:
                    {
                        return Create(id, dt, val);
                    }
                case RequestTypes.READ:
                    {
                        return Read(id);
                    }
                case RequestTypes.UPDATE:
                    {
                        return Update(id, dt, val);
                    }
                case RequestTypes.DELETE:
                    {
                        return Delete(id);
                    }
                default:
                    throw new Exception();
            }
        }

        static Response Create(int id, DateTime dt, double val)
        {
            ResponseCodes respCode;
            if (ExistsId(id))
            {
                respCode = ResponseCodes.EXISTS_ID;
            }
            else
            {
                Database.Records.Add(new Record(id, dt, val));
                Database.Sort(Record.ComparisonBy.DATETIME);
                respCode = ResponseCodes.OK;
            }
            return new Response(respCode);
        }
        static Response Read(int id)
        {
            ResponseCodes respCode;
            DateTime dt = DateTime.MinValue;
            double val = 0;
            try
            {
                Record rec = Database[id];
                respCode = ResponseCodes.OK;
                dt = rec.Datetime;
                val = rec.Value;
            }
            catch (ArgumentOutOfRangeException)
            {
                respCode = ResponseCodes.NO_ID;
            }
            return new Response(respCode, id, dt, val);
        }
        static Response Update(int id, DateTime newDt, double newVal)
        {
            ResponseCodes respCode;
            try
            {
                Database[id] = new Record(id, newDt, newVal);
                Database.Sort(Record.ComparisonBy.DATETIME);
                respCode = ResponseCodes.OK;
            }
            catch (ArgumentOutOfRangeException)
            {
                respCode = ResponseCodes.NO_ID;
            }
            return new Response(respCode);
        }
        static Response Delete(int id)
        {
            ResponseCodes respCode;
            try
            {
                Record rec = Database[id];
                Database.Records.Remove(rec);
                Database.Sort(Record.ComparisonBy.DATETIME);
                respCode = ResponseCodes.OK;
            }
            catch(ArgumentOutOfRangeException)
            {
                respCode = ResponseCodes.NO_ID;
            }
            return new Response(respCode);
        }
    }


}