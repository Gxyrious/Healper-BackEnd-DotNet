using Microsoft.AspNetCore.Mvc;
using HealperModels;
using Healper_BackEnd.Utils;
using System.Runtime.InteropServices;


namespace Healper_BackEnd.Controllers
{
    [ApiController]
    [Route("api/user/[controller]")]
    public class UserController : ControllerBase
    {
        const String DLL_LOCATION = "E:\\2022.9-2023.9\\Healper-BackEnd\\x64\\Debug\\Encryption.dll";
        [DllImport(DLL_LOCATION, CallingConvention = CallingConvention.Cdecl, EntryPoint = "encryption", CharSet = CharSet.Unicode)]
        private extern static IntPtr encryption(string message);

        [DllImport(DLL_LOCATION, CallingConvention = CallingConvention.Cdecl, EntryPoint = "release")]
        private extern static void release(IntPtr ptr);

        private readonly ModelContext myContext;
        public UserController(ModelContext modelContext)
        {
            myContext = modelContext;
        }
        [HttpGet(Name = "login")]
        public HttpResponseMessage Login(string userphone, string userPassword)
        {
            IntPtr pw = encryption(userPassword);
            string? str5 = Marshal.PtrToStringUTF8(pw);
            release(pw);
            return ResponseEntity.OK().Body(str5!);
        }
    }
}
    