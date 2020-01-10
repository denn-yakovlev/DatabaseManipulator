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

        public static ResponseCodes SolveProblem(int entryNum, double[] valuesSeq, out DateTime result)
        {
            /*
             Дана последовательность показаний некоего датчика во времени. 
            Отобразить на экране время начала n-ого вхождения заданной последовательности 
            (искомая последовательность может быть длиной до 5 символов). 
            В измерениях «вхождения» последовательности могут повторяться не один раз;
            */
            throw new NotImplementedException();
        }
    }
}
