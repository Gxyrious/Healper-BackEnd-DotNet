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
                return password;
            }
        }

        [DllImport(DLL_LOCATION, CallingConvention = CallingConvention.Cdecl, EntryPoint = "encryption", CharSet = CharSet.Unicode)]
        private extern static IntPtr encryption(string message);

        [DllImport(DLL_LOCATION, CallingConvention = CallingConvention.Cdecl, EntryPoint = "release")]
        private extern static void release(IntPtr ptr);

        private readonly IUserService myUserService;

        private readonly IScaleService myScaleService;
        public UserController(IUserService userService, IScaleService scaleService)
        {
            myUserService = userService;
            myScaleService = scaleService;
        }


        [HttpPost("login")]
        public ResponseEntity Login(LoginInfoInDto inDto)
        {
            IUser? user = myUserService.FindUserByPhone(inDto.userPhone);
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

        [HttpPost("logout")]
        public ResponseEntity Logout()
        {
            return ResponseEntity.OK();
        }

        [HttpGet("info")]
        public ResponseEntity GetInfoByUserId(int id, string userType)
        {
            if (userType.Equals("client"))
            {
                ClientInfo? clientInfo = myUserService.FindClientInfoById(id);
                if (clientInfo != null)
                {
                    return ResponseEntity.OK().Body(clientInfo);
                }
            } else if (userType.Equals("consultant"))
            {
                ConsultantInfo? consultantInfo = myUserService.FindConsultantInfoById(id);
                if (consultantInfo != null)
                {
                    return ResponseEntity.OK().Body(consultantInfo);
                } 
            }
            return ResponseEntity.ERR("Person Does Not Exist");
        }

        [HttpPost("register")]
        public ResponseEntity Register(RegisterInfoInDto inDto)
        {
            try
            {
                string userphone = inDto.userPhone;
                IUser? user = myUserService.FindUserByPhone(userphone);
                if (user != null)
                {
                    return ResponseEntity.ERR("Phone Already Exists");
                } else
                {
                    if (SMSHelp.JudgeSmsCode(userphone, inDto.code)) // 检测验证码是否正确
                    {
                        Client newClient = myUserService.AddClientInfo(inDto.nickname, GetEncrytionPassword(inDto.password), inDto.userPhone, inDto.sex, inDto.age);
                        return ResponseEntity.OK().Body(newClient.Id);
                    } else
                    {
                        return ResponseEntity.ERR("Captcha Incorrect");
                    }
                }
            } catch (Exception err)
            { 
                return ResponseEntity.ERR().Body(err); 
            }
        }

        [HttpPut("info")]
        public ResponseEntity UpdateClientBasicInfo(ClientInfo client)
        {
            try
            {
                myUserService.UpdateClientInfo(client);
                return ResponseEntity.OK();
            } catch (Exception err) 
            { 
                return ResponseEntity.ERR().Body(err);
            }
        }

        [HttpPut("passwd")]
        public ResponseEntity UpdateClientPasswd(UpdatePasswdInDto inDto)
        {
            try
            {
                if (myUserService.CheckPasswdWithId(inDto.id, inDto.userType, GetEncrytionPassword(inDto.oldPasswd)))
                {
                    myUserService.UpdateUserPasswd(inDto.id, inDto.userType, GetEncrytionPassword(inDto.newPasswd));
                    return ResponseEntity.OK("Password Updated");
                } else
                {
                    return ResponseEntity.ERR("Old Password Wrong");
                }
            } catch(Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }

        [HttpPost("sendMsg")]
        public ResponseEntity SendMsg(LoginInfoInDto inDto)
        {
            try
            {
                string userphone = inDto.userPhone;
                if (myUserService.FindUserByPhone(userphone) != null)
                {
                    throw new Exception("Phone Already Exists");
                } else
                {
                    return ResponseEntity.OK(SMSHelp.SendMessage(userphone));
                }
            } catch(Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }

        [HttpPost("uploadProfile")]
        public ResponseEntity UploadImage(UploadImageInDto inDto)
        {
            try
            {
                string imageBase64 = inDto.base64;

                Stream inputStream = OssHelp.Base64ToStream(imageBase64);

                string imagePath = inDto.userType.ToString()
                    + '-' + inDto.id + '.' + OssHelp.GetImageTypeFromBase64(imageBase64);

                string url = OssHelp.UploadStream(inputStream, imagePath);

                return ResponseEntity.OK().Body(url);
            } catch (Exception err)
            {
                return ResponseEntity.ERR("Upload Failed").Body(err);
            }
        }

        [HttpPost("uploadQrCode")]
        public ResponseEntity UploadConsultantQrCode(UploadImageInDto inDto)
        {
            try
            {
                string imageBase64 = inDto.base64;

                Stream inputStream = OssHelp.Base64ToStream(imageBase64);

                string imagePath = "QrCode-" + inDto.id + '.' + OssHelp.GetImageTypeFromBase64(imageBase64);

                string url = OssHelp.UploadStream(inputStream, imagePath);

                return ResponseEntity.OK().Body(url);
            } catch (Exception err)
            {
                return ResponseEntity.ERR("Upload Failed").Body(err);
            }
        }

        [HttpGet("consultants/label")]
        public ResponseEntity FindConsultantsWithLabel(int page, int size, string label)
        {
            try
            {
                List<ConsultantInfo> consultants = myUserService.FindConsultantsByLabel(label, page, size);
                return ResponseEntity.OK().Body(consultants);
            } catch (Exception err)
            {
                return ResponseEntity.ERR($"No {label} Consultant Found").Body(err);
            }
        }

        [HttpGet("consultants/client")]
        public ResponseEntity FindConsultantsWithClient(int clientId, int page, int size)
        {
            try
            {
                Dictionary<string, int> labelValues = myScaleService.FindLabelsWithClient(clientId);
                List<KeyValuePair<string, int>> labels = new(labelValues);
                labels.Sort((o1, o2) => o2.Value - o1.Value);
                string label = labelValues.Count == 0 ? "" : labels[0].Key;
                List<ConsultantStatus> consultants = myUserService.FindConsultantsWithClient(clientId, label, page, size);
                return ResponseEntity.OK().Body(consultants);
            } catch (Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }

        [HttpGet("client/info")]
        public ResponseEntity FindClientInfo(int clientId)
        {
            try
            {
                ClientInfo? clientInfo = myUserService.FindClientInfoById(clientId);
                Dictionary<string, string> map = new Dictionary<string, string>();
                map.Add("nickname", clientInfo!.Value.nickname!);
                map.Add("sex", clientInfo!.Value.sex!);
                map.Add("age", clientInfo!.Value.age.ToString()!);
                List<ScaleRecordInfo> scaleRecords = myScaleService.FindScaleRecordInfoByClientId(clientId, 1, 2);
                if (scaleRecords.Count == 0)
                {
                    map.Add("scale", "");
                } else
                {
                    map.Add("scale", scaleRecords.First().record);
                }
                return ResponseEntity.OK().Body(map);
            } catch (Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }

        [HttpPut("consultant/info")]
        public ResponseEntity UpdateConsultantLabel(ConsultantInfo consultant)
        {
            try
            {
                myUserService.UpdateConsultantInfo(consultant);
                return ResponseEntity.OK("OK");
            } catch (Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }
    }
}
    