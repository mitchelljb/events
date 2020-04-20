﻿using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DistractedDriverDetectionConsoleTest02
{
    public static class Program
    {
        const string Url = "http://127.0.0.1:8080/image";

        public static void Main()
        {
            var imageFilePath = "c2-01.jpg";

            MakePredictionRequest(imageFilePath).Wait();

            Console.WriteLine("\n\nHit ENTER to exit...");
            Console.ReadLine();
        }

        public static async Task MakePredictionRequest(string imageFilePath)
        {
            var client = new HttpClient();
       
            var byteData = GetImageAsByteArray(imageFilePath);

            using var content = new ByteArrayContent(byteData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            var response = await client.PostAsync(Url, content);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var prettyJson = JToken.Parse(jsonResponse).ToString(Formatting.Indented);
            Console.WriteLine(prettyJson);
        }

        private static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
    }
}