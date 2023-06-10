using System.Net;

namespace HealperResponse
{
    public class ResponseEntity
    {
        public static ResponseEntity OK(string msg="")
        {
            return new ResponseEntity(HttpStatusCode.OK, msg);
        }

        public static ResponseEntity ERR(string msg = "")
        {
            return new ResponseEntity(HttpStatusCode.InternalServerError, msg);
        }
        private ResponseEntity(HttpStatusCode statusCode, string msg)
        {
            Code = statusCode;
            Msg = msg;
        }

        public ResponseEntity Body(object data)
        {
            Data = data;
            return this;
        }
        public object? Data { get; set; } = null;

        public HttpStatusCode Code { get; set; }

        public string Msg { get; set; } = "";
    }
}
