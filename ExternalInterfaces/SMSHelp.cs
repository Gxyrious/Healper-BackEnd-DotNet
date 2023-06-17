using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using SmsCode;

namespace ExternalInterfaces
{
    public class SMSHelp
    {
        public static IConfiguration Configuration { get; set; }

        public static IMemoryCache MemoryCache { get; set; }

        static SMSHelp()
        {
            Configuration = new ConfigurationBuilder().Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true }).Build();
            MemoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        public static string SendMessage(string userphone)
        {
            var client = CreateClient();
            string code = SmsCode.RandomCodeGenerator.generateCode().ToString();
            AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsRequest request = new AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsRequest
            {
                PhoneNumbers = userphone,
                SignName = HttpUtility.UrlDecode(Configuration["smsSignName"], System.Text.Encoding.UTF8),
                TemplateCode = Configuration["smsTemplateCode"],
                TemplateParam = "{\"code\":" + code + "}"
            };
            var response = client.SendSms(request);
            if (response.Body.Code == "OK")
            {
                return response.Body.Message;
            } else
            {
                throw new Exception(response.Body.Message);
            }
        }

        private static AlibabaCloud.SDK.Dysmsapi20170525.Client CreateClient()
        {
            var conf = new AlibabaCloud.OpenApiClient.Models.Config()
            {
                AccessKeyId = Configuration["smsAccessKeyId"],
                AccessKeySecret = Configuration["smsAccessKeySecret"]
            };
            return new AlibabaCloud.SDK.Dysmsapi20170525.Client(conf);
        }
    }
}
