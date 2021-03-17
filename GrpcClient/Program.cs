using Grpc.Core;
using System;
using System.Threading.Tasks;
using TestDeps;

namespace GrpcClient
{
    class Program
    {
        const string Target = "127.0.0.1:50051";

        static async Task Main(string[] args)
        {
            Channel channel = new Channel(Target, ChannelCredentials.Insecure);

            await channel.ConnectAsync().ContinueWith(task =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine("Client connected successfully");
                }
            });

            var client = new TestService.TestServiceClient(channel);

            await channel.ShutdownAsync();
            Console.ReadLine();
        }
    }
}
