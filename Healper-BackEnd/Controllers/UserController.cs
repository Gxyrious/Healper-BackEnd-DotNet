using Microsoft.AspNetCore.Mvc;
using System.Net;
using Healper_BackEnd.Models;
using Healper_BackEnd.Utils;

namespace Healper_BackEnd.Controllers
{
    [ApiController]
    [Route("api/user/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ModelContext myContext;
        public UserController(ModelContext modelContext)
        {
            myContext = modelContext;
        }
        [HttpGet(Name = "login")]
        public HttpResponseMessage Login(string userphone, string userPassword)
        {
            var User = myContext.Clients.Where(e => e.Userphone == userphone && e.Password == userPassword);
            return ResponseEntity.OK().Body(User);
        }
    }

    
}
