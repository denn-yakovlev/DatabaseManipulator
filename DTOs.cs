using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseManipulator
{
    abstract class Dto
    {
        public byte[] SerializedData { get; private set; }
        public Dto()
        {
            SerializedData = null;
        }
        public Dto(byte[] data)
        {
            SerializedData = data;
        }

        public new string ToString()
        {
            char[] chArray = new char[SerializedData.Length / 2];
            for (int i = 0; i < chArray.Length; i++)
            {
                chArray[i] = BitConverter.ToChar(SerializedData, 2 * i);
            }
            return new string(chArray);
        }
        public Record ToRecord()
        {
            int id;
            DateTime dt;
            double val;

            id = BitConverter.ToInt32(SerializedData, 0);
            dt = DateTime.FromBinary(BitConverter.ToInt64(SerializedData, 4));
            val = BitConverter.ToDouble(SerializedData, 12);

            return new Record(id, dt, val);
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
        public Response(ResponseCodes code) : this(code, null)
        {

        }
        public Response(ResponseCodes code, byte[] data) : base(data)
        {
            ResponseCode = code;
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
        public Request(RequestTypes reqType, byte[] data): base(data)
        {
            RequestType = reqType;
        }
    }
}
