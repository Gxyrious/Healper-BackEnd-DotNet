using System.Net;

namespace HealperResponse
{
    public class ResponseEntity : HttpResponseMessage
    {
        public static ResponseEntity OK()
        {
            return new ResponseEntity(HttpStatusCode.OK);
        }
        public static ResponseEntity ERR()
        {
            return new ResponseEntity(HttpStatusCode.InternalServerError);
        }
        private ResponseEntity(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public ResponseEntity Body(object data)
        {
            Data = data;
            return this;
        }
        public object? Data { get; set; }
    }
}
