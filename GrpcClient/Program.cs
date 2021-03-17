﻿using Calculator;
using ErrorsSample;
using Greet;
using Grpc.Core;
using PrimeNumber;
using System;
using System.Collections.Generic;
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
            /*
            var client = new CalculatorService.CalculatorServiceClient(channel);
            var sumRequest = new SumRequest { A = 10, B = 3 };
            var response = await client.SumAsync(sumRequest);
            Console.WriteLine($"Response received: {response.Result}\r\n");
            */

            // 4. Greet server many
            /*
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
            */

            // 5. Prime number decomposition
            /*
            var primeNumDecompositionClient = new PrimeNumberDecompositionService.PrimeNumberDecompositionServiceClient(channel);
            var primeNumDecompositionRequest = new DecompositionRequest { Value = 246 };
            var primeNumDecompoResponse = primeNumDecompositionClient.Decompose(primeNumDecompositionRequest);
            while (await primeNumDecompoResponse.ResponseStream.MoveNext())
            {
                Console.WriteLine(primeNumDecompoResponse.ResponseStream.Current.Divider);
            }
            */

            // 6. Client streaming
            /*
            var clientStreamingClient = new GreetingService.GreetingServiceClient(channel);
            var clientStreamingRequest = new LongGreetRequest { Greeting = "hello" };
            var clientStreamingStream = clientStreamingClient.LongGreet();
            for (var i = 0; i < 10; i++)
            {
                await clientStreamingStream.RequestStream.WriteAsync(clientStreamingRequest);
            }
            await clientStreamingStream.RequestStream.CompleteAsync();
            var clientStreamingResponse = await clientStreamingStream.ResponseAsync;
            Console.WriteLine($"client streaming: {clientStreamingResponse.Result}");
            */

            // 7. calc average
            /*
            var avgClient = new CalculatorService.CalculatorServiceClient(channel);
            var avgStream = avgClient.ComputeAverage();
            var rnd = new Random();
            var avgList = new List<int>();
            for (var i = 0; i < 10; i++)
            {
                var avgRequest = new ComputeAverageRequest { Value = rnd.Next(11, 55) };
                avgList.Add(avgRequest.Value);
                await avgStream.RequestStream.WriteAsync(avgRequest);
            }
            await avgStream.RequestStream.CompleteAsync();
            var avgResponse = await avgStream.ResponseAsync;
            Console.WriteLine($"{avgResponse.Result} is an average for {String.Join(", ", avgList)}");
            */

            // 8. bi-di
            /*
            var bidiClient = new GreetingService.GreetingServiceClient(channel);
            var bidiStream = bidiClient.GreetEveryone();
            var bidiResponseReaderTask = Task.Run(async () =>
            {
                while (await bidiStream.ResponseStream.MoveNext())
                {
                    Console.WriteLine(bidiStream.ResponseStream.Current.Result);
                }
            });
            var list = new List<string> { "John", "Bob", "Max", "Andrew", "Sheldon", "Julia", "Marlen", "Den", "Joseph", "Fisherman", "Dick" };
            foreach (var item in list)
            {
                Console.WriteLine($"{item} has sent a greeting");
                await bidiStream.RequestStream.WriteAsync(new GreetEveryoneRequest { Name = item });
                await Task.Delay(345);
            }

            await bidiStream.RequestStream.CompleteAsync();
            await bidiResponseReaderTask;
            */

            // 9. max number
            /*
            var maxClient = new CalculatorService.CalculatorServiceClient(channel);
            var maxStream = maxClient.FindMax();
            var maxStreamReaderTask = Task.Run(async () =>
            {
                while (await maxStream.ResponseStream.MoveNext())
                {
                    Console.WriteLine($"Next max found: {maxStream.ResponseStream.Current.Max}");
                }
            });
            for (var i = 0; i < 100; i++)
            {
                var testValue = new Random().Next(1, 100);
                Console.WriteLine($"testing {testValue}");
                var request = new FindMaxRequest { Value = testValue };
                await maxStream.RequestStream.WriteAsync(request);
                await Task.Delay(200);
            }
            await maxStream.RequestStream.CompleteAsync();
            await maxStreamReaderTask;
            */

            // 10. Errors
            var errorClient = new SqrtService.SqrtServiceClient(channel);
            for (var i = 10; i > -5; i--)
            {
                try
                {
                    var result = await errorClient.SqrtAsync(new SqrtRequest { Number = i });
                    Console.WriteLine($"Sqrt for {i} is {result}");
                    await Task.Delay(400);
                }
                catch (RpcException ex)
                {
                    Console.WriteLine($"Error message: {ex.Message}, detail: {ex.Status.Detail}, status: {ex.Status}");
                }
            }


            Console.WriteLine("Client is done");
            await channel.ShutdownAsync();
            Console.ReadLine();
        }
    }
}
