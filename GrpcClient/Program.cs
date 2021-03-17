using Calculator;
using Grpc.Core;
using System;
using System.Threading.Tasks;

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

            // 1.
            // var client = new TestService.TestServiceClient(channel);

            // 2. Greet unary
            /*
            var client = new GreetingService.GreetingServiceClient(channel);

            var greeting = new Greeting
            {
                FirstName = "John",
                LastName = "Doe"
            };

            var request = new GreetingRequest() { Greeting = greeting };

            var response = await client.GreetAsync(request);
            */

            // 3. Sum
            var client = new CalculatorService.CalculatorServiceClient(channel);
            var sumRequest = new SumRequest { A = 10, B = 3 };
            var response = await client.SumAsync(sumRequest);

            Console.Write($"Response received: {response.Result}");

            await channel.ShutdownAsync();
            Console.ReadLine();
        }
    }
}
