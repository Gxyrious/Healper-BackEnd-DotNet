using Aliyun.OSS;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace ExternalInterfaces
{
    public class OssHelp
    {
        private static IConfiguration Configuration { get; set; }

        static OssHelp()
        {
            Configuration = new ConfigurationBuilder().Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true }).Build();
        }

        public static string GetImageTypeFromBase64(string imageBase64)
        {
            return imageBase64.Split('/', 3)[1].Split(';', 2)[0];
        }

        public static MemoryStream Base64ToStream(string imageBase64)
        {
            byte[] imageBytes = Convert.FromBase64String(imageBase64.Split("base64,")[1]);
            MemoryStream stream = new MemoryStream(imageBytes, 0, imageBytes.Length);
            return stream;
        }

        public static string UploadStream(Stream stream, string path)
        {
            OssClient client = new OssClient(Configuration["endPoint"], Configuration["accessKeyId"], Configuration["accessKeySecret"]);
            client.PutObject(Configuration["bucketName"], path, stream);
            return "https://" + Configuration["bucketName"] + '.' + Configuration["endPoint"] + '/' + path;
        }
    }
}