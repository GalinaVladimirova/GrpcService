using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;
using GrpcGreeterClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcClient
{
    public static class SystemObjectHandler
    {
        private const int NumberOfElementsInABatch = 5;
        public static async Task HandleDirectory(string path)
        {
            var files = Directory.GetFiles(path, "*.json");
            foreach (var file in files)
                await HandleFile(file);
            
        }

        public static async Task HandleFile(string path)
        {
            var jsons = File.ReadAllText($"{path}").Trim().Split('\n');
            var jsonBatches = jsons.Split(NumberOfElementsInABatch);
            foreach (var batch in jsonBatches)
            {
                var reply = await SendRequest(batch.ToArray());
                if (reply.Result == Result.Ok)
                    File.Delete(path);
            }            
        }

        private static IEnumerable<IEnumerable<T>> Split<T>(this T[] arr, int size)
        {
            return arr.Select((s, i) => arr.Skip(i * size).Take(size)).Where(a => a.Any());
        }

        private static async Task<Reply> SendRequest(string[] json)
        {
            using var channel = GrpcChannelSettings.Configure();
            var client = new Handler.HandlerClient(channel);
            var data = new Request();
            data.Data.AddRange(json);
            return await client.HandleDataAsync(data);

        }

        
    }
}
