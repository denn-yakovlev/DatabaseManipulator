using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseManipulator
{
    static class Controller
    {
        public static ResponseCodes CreateRecord(int id, DateTime dt, double val)
        {
            Request request = new Request(RequestTypes.CREATE, id, dt, val);
            Response response =  DatabaseInteractor.HandleRequest(request);
            return response.ResponseCode;
        }

        public static ResponseCodes ReadRecord(int id, out DateTime dt, out double val)
        {
            Request request = new Request(RequestTypes.READ, id);
            Response response = DatabaseInteractor.HandleRequest(request);
            dt = response.Datetime;
            val = response.Value;
            return response.ResponseCode;
        }

        public static ResponseCodes UpdateRecord(int id, DateTime dt, double val)
        {
            Request request = new Request(RequestTypes.UPDATE, id, dt, val);
            Response response = DatabaseInteractor.HandleRequest(request);
            return response.ResponseCode;
        }

        public static ResponseCodes DeleteRecord(int id)
        {
            Request request = new Request(RequestTypes.DELETE, id);
            Response response = DatabaseInteractor.HandleRequest(request);
            return response.ResponseCode;
        }

        public static List<Record> ReadAll()
        {
            return DatabaseInteractor.Database.Records;
        }


        // wrong

        public static DateTime SolveProblem(int entryNum, double[] valuesSeq)
        {
            /*
                   Дана последовательность показаний некоего датчика во времени. 
                  Отобразить на экране время начала n-ого вхождения заданной последовательности 
                  (искомая последовательность может быть длиной до 5 символов). 
                  В измерениях «вхождения» последовательности могут повторяться не один раз;
            */
            throw new NotImplementedException();
            List<Record> records = ReadAll();
            DateTime dt = DateTime.MinValue;
            int currEntry = 0;
            for (int i = 0; i < records.Count - valuesSeq.Length; i++)
            {
                if (records[i].Value == valuesSeq[0])
                {
                    dt = records[i].Datetime;
                    int j;
                    for (j = 0; j < valuesSeq.Length; i++)
                    {
                        if (records[i + j].Value != valuesSeq[j])
                        {
                            break;
                        }
                    }
                    if (j == valuesSeq.Length)
                    {
                        currEntry++;
                    }
                    if (currEntry == entryNum)
                    {
                        return dt;
                    }
                }
            }
            return dt;
        }
    }
}
