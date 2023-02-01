using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GrpcService
{
    public class HandlerService : Handler.HandlerBase
    {
        private readonly ILogger<HandlerService> _logger;
        
        private readonly string _basePath = $"C:\\Users\\vladi\\TestTask\\MoveHere\\";
        public HandlerService(ILogger<HandlerService> logger)
        {
            _logger = logger;
        }

        public override Task<Reply> HandleData(Request request, ServerCallContext context)
        {
            try
            {
                foreach (var item in Helper.GroupItems(request.Data.ToList()))
                {
                    CreateDirectoryIfNotExists(item.Key);
                    var filename = DateTime.Now.ToString("yyyy-MM-dd") + ".json";
                    var fullPath = $"{_basePath}{item.Key}\\{filename}";

                    File.AppendAllLines(fullPath, item.Value);
                }

                return Task.FromResult(new Reply
                {
                    Result = Result.Ok
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Task.FromResult(new Reply
                {
                    Result = Result.Error
                });
            }

        }

        private void CreateDirectoryIfNotExists(string directory)
        {
            if (!Directory.Exists($"{_basePath}{directory}"))
                Directory.CreateDirectory($"{_basePath}{directory}");
        }

        
    }
}
