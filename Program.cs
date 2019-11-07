using System;
using System.Collections.Generic;
using System.Globalization;
namespace DatabaseManipulator
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            Record rec = new Record(1, DateTime.Parse("01.05.2016 04:15:29"), 0.92d);
            
            DbInteractor.Open("input.csv");
      
            byte[] bs = "aёc".Serialize();
            Response resp = new Response(ResponseCodes.OK, bs);
            Console.WriteLine(resp.ToString() == "aёc");

            byte[] ser = rec.Serialize();
            resp = new Response(ResponseCodes.NO_ID, ser);
            Console.WriteLine(resp.ToRecord() == rec);

            Request req = new Request(RequestTypes.READ, "100".Serialize());
            resp = DbInteractor.Read(req);
            if (resp.ResponseCode == ResponseCodes.OK)
            {
                resp.ToRecord();
            }
        }
    }
}
