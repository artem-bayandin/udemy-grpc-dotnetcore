using Calculator;
using Greet;
using Grpc.Core;
using PrimeNumber;
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
            Console.WriteLine($"Response received: {response.Result}\r\n");

            // 4. Greet server many
            var greetManyTimesClient = new GreetingService.GreetingServiceClient(channel);
            var greetManyTimesRequest = new GreetManyTimesRequest
            {
                Greeting = new Greeting
                {
                    FirstName = "Bob",
                    LastName = "Marley"
                },
                Times = 7
            };
            var greetManyTimesResponse = greetManyTimesClient.GreetManyTimes(greetManyTimesRequest);
            while (await greetManyTimesResponse.ResponseStream.MoveNext())
            {
                Console.WriteLine(greetManyTimesResponse.ResponseStream.Current.Result);
            }

            // 5. Prime number decomposition
            var primeNumDecompositionClient = new PrimeNumberDecompositionService.PrimeNumberDecompositionServiceClient(channel);
            var primeNumDecompositionRequest = new DecompositionRequest { Value = 246 };
            var primeNumDecompoResponse = primeNumDecompositionClient.Decompose(primeNumDecompositionRequest);
            while (await primeNumDecompoResponse.ResponseStream.MoveNext())
            {
                Console.WriteLine(primeNumDecompoResponse.ResponseStream.Current.Divider);
            }

            Console.WriteLine("Client is done");

            await channel.ShutdownAsync();
            Console.ReadLine();
        }
    }
}
