using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseManipulator
{
    class Controller
    {
        public ResponseCodes CreateRecord(int id, DateTime dt, double val)
        {
            Request req = new Request(RequestTypes.CREATE, id, dt, val);
            Response resp = DatabaseInteractor.Create(req);
            return resp.ResponseCode;
        }
        public ResponseCodes ReadRecord(int id, out Record result)
        { 
            Request req = new Request(RequestTypes.CREATE, id);
            Response resp = DatabaseInteractor.Read(req);
            result = resp.ToRecord();
            return resp.ResponseCode;
        }
    }
}
