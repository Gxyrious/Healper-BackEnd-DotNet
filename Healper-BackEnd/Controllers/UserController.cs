using Microsoft.AspNetCore.Mvc;
using HealperModels;
using HealperResponse;
using HealperModels.Models;
using HealperDto;
using System.Runtime.InteropServices;
using HealperDto.InDto;
using HealperService;
using HealperDto.OutDto;
using ExternalInterfaces;

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


        [HttpPost("login")]
        public ResponseEntity Login(LoginInfoInDto inDto)
        {
            IUser user = myUserService.findUserByPhone(inDto.userPhone);
            if (user == null)
            {
                HttpContext.Response.StatusCode = 500;
                return ResponseEntity.ERR("User Not Exist");
            }
            else
            {
                if (user.GetPassword().Equals(GetEncrytionPassword(inDto.userPassword)))
                {
                    if (user.GetType() == typeof(Client))
                    {
                        return ResponseEntity.OK("Client Login Success").Body(new LoginClientInfoOutDto
                        {
                            user = (Client)user,
                            userType = "client",
                        });
                    } else
                    {
                        return ResponseEntity.OK("Consultant Login Success").Body(new LoginConsultantInfoOutDto
                        {
                            user = (Consultant)user,
                            userType = "consultant",
                        });
                    }
                }
                else
                {
                    return ResponseEntity.ERR().Body("Password Error");
                }
            }
        }

        [HttpPost("uploadProfile")]
        public ResponseEntity UploadImage(UploadImageInDto inDto)
        {
            string imageBase64 = inDto.base64;
            
            Stream inputStream = OssHelp.Base64ToStream(imageBase64);

            string imagePath = inDto.userType.ToString() 
                + '-' + inDto.id + '.' + OssHelp.GetImageTypeFromBase64(imageBase64);

            string url = OssHelp.UploadStream(inputStream, imagePath);

            return ResponseEntity.OK().Body(url);
            
        }
    }
}
    