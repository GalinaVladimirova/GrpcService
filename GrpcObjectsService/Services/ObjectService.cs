using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcObjectsService
{
    public class ObjectService : ObjectsGetter.ObjectsGetterBase
    {
        private readonly ILogger<ObjectService> _logger;
        private readonly string _path = "C:\\Users\\vladi\\TestTask\\MoveHere";
        public ObjectService(ILogger<ObjectService> logger)
        {
            _logger = logger;
        }

        public override Task<GetTypesReply> GetJsonTypes(GetTypesRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Recieved request for getting json types");

            var directories = Directory.GetDirectories(_path);
            var reply = new GetTypesReply();
            reply.Types_.AddRange(directories);

            _logger.LogInformation($"Number of types found in directory {_path}: {reply.Types_.Count()}");
            return Task.FromResult(reply);
        }

        public override async Task GetJsonObjectsByType(GetObjectsRequest request, IServerStreamWriter<GetObjectsReply> responseStream, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.Type))
            {
                var message = "Field 'type' can't be empty";
                _logger.LogError(message);
                var reply = new GetObjectsReply();
                reply.Result.Success = false;
                reply.Result.ErrorMessage = message;
                await responseStream.WriteAsync(reply);
            } 
                
            _logger.LogInformation($"Recieved request for getting json objects of type {request.Type}");
            var path = $"{_path}\\{request.Type}";
            var files = Directory.GetFiles(path);

            foreach (var file in files)
            {
                var data = File.ReadAllText($"{file}").Split('\n');
                var reply = new GetObjectsReply();
                reply.Data.AddRange(data);
                await responseStream.WriteAsync(reply);
            }           
        }
    }
}
