using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseManipulator
{
    abstract class Dto
    {
        public int Id { get; private set; }
        public DateTime Datetime { get; private set; }
        public double Value { get; private set; }
        public Dto() { }
        public Dto(int id)
        {
            Id = id;
        }
        public Dto(int id, DateTime dt, double val): this(id)
        {
            Datetime = dt;
            Value = val;
        }
        public static explicit operator Record(Dto dto)
        {
            return new Record(dto.Id, dto.Datetime, dto.Value);
        }
    }

    enum ResponseCodes
    {
        OK,
        NO_ID,
        EXISTS_ID
    }

    class Response : Dto
    {
        public ResponseCodes ResponseCode { get; private set; }
        public Response(ResponseCodes respCode)
        {
            ResponseCode = respCode;
        }
        //shit code
        public Response(ResponseCodes respCode, int id, DateTime dt, double val): base(id, dt, val)
        {
            ResponseCode = respCode;
        }
    }

    enum RequestTypes
    {
        CREATE,
        READ,
        UPDATE,
        DELETE
    }
    class Request: Dto
    {
        public RequestTypes RequestType { get; private set; }
        public Request(RequestTypes reqType, int id): base(id)
        {
            RequestType = reqType;
        }
        // also shit code
        public Request(RequestTypes reqType, int id, DateTime dt, double val): base(id, dt, val)
        {
            RequestType = reqType;
        }
        
    }
}
