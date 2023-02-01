using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace GrpcClient
{
    class Program
    {
        private static readonly string _path = @"C:\Users\vladi\TestTask\ReadAndDeleteFromHere";

        static async Task Main(string[] args)
        {
            try
            {
                using var watcher = new FileSystemWatcher(_path);

                watcher.Created += OnCreated;
                watcher.Filter = "*.json";
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;

                await SystemObjectHandler.HandleDirectory(_path);

            }
            catch (Exception)
            {
                throw;
            }
            
            
            Console.ReadKey();
        }
        

        private static async void OnCreated(object sender, FileSystemEventArgs e)
        {
            await SystemObjectHandler.HandleFile(e.FullPath);
        }

        
    }
}
