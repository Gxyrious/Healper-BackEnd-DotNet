using Microsoft.AspNetCore.Mvc;
using HealperModels;
using HealperResponse;
using HealperModels.Models;
using HealperDto;
using System.Runtime.InteropServices;
using HealperDto.InDto;
using HealperService;
using HealperDto.OutDto;

namespace Healper_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        const String DLL_LOCATION = "E:/2022.9-2023.9/Healper-BackEnd/x64/Debug/Encryption.dll";
        
        // 调用C++动态链接库，C#端再次封装，防止内存泄漏
        private static string GetEncrytionPassword(string password)
        {
            try
            {
                IntPtr pwPtr = encryption(password);
                string? encrytionPassword = Marshal.PtrToStringUTF8(pwPtr);
                release(pwPtr);
                return encrytionPassword!;
            }
            catch (Exception)
            {
                throw new Exception("Password Encryption Error");
            }
        }

        [DllImport(DLL_LOCATION, CallingConvention = CallingConvention.Cdecl, EntryPoint = "encryption", CharSet = CharSet.Unicode)]
        private extern static IntPtr encryption(string message);

        [DllImport(DLL_LOCATION, CallingConvention = CallingConvention.Cdecl, EntryPoint = "release")]
        private extern static void release(IntPtr ptr);

        private readonly IUserService myUserService;
        public UserController(IUserService userService)
        {
            myUserService = userService;
        }


        [HttpPost(Name = "login")]
        public ResponseEntity Login(LoginInfoInDto inDto)
        {
            IUser user = myUserService.findUserByPhone(inDto.userPhone);
            if (user == null)
            {
                return ResponseEntity.ERR().Body("User Not Exist");
            }
            else
            {
                if (user.GetPassword().Equals(GetEncrytionPassword(inDto.password)))
                {
                    if (user.GetType() == typeof(Client))
                    {
                        return ResponseEntity.OK().Body(new LoginClientInfoOutDto
                        {
                            user = (Client)user,
                            userType = UserType.Client,
                        });
                    } else
                    {
                        return ResponseEntity.OK().Body(new LoginConsultantInfoOutDto
                        {
                            user = (Consultant)user,
                            userType = UserType.Consultant,
                        });
                    }
                }
                else
                {
                    return ResponseEntity.ERR().Body("Password Error");
                }
            }
        }
    }
}
    