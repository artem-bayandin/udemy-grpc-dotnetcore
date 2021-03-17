using Calculator;
using Deadlines;
using ErrorsSample;
using Greet;
using Grpc.Core;
using PrimeNumber;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TestDeps;

namespace GrpcClient
{
    class Program
    {
        const string Host = "127.0.0.1";
        const int Port = 50051;

        static async Task Main(string[] args)
        {
            var channel = await CreateUnsecureChannel(Host, Port);

            // 1. Test
            await TestServiceClient(channel);

            // 2. Greet unary
            await GreetUnary(channel);

            // 3. Sum
            await Sum(channel);

            // 4. Greet server many
            await GreetServerStreaming(channel);

            // 5. Prime number decomposition
            await PrimeNumberDecomposition(channel);

            // 6. Client streaming
            await ClientStreaming(channel);

            // 7. calc average
            await CalcAverage(channel);

            // 8. bi-di
            await BiDi(channel);

            // 9. max number
            await MaxNumber(channel);

            // 10. Errors
            await ErrorHandling(channel);

            // 11. Deadlines
            await Deadlines(channel);

            // finish
            Console.WriteLine("Client is done");
            await channel.ShutdownAsync();
            Console.ReadLine();
        }

        #region Samples

        static async Task TestServiceClient(Channel channel)
        {
            WriteHeader("TestServiceClient");
            var client = new TestService.TestServiceClient(channel);
        }

        static async Task GreetUnary(Channel channel)
        {
            WriteHeader("GreetUnary");
            var client = new GreetingService.GreetingServiceClient(channel);

            var greeting = new Greeting
            {
                FirstName = "John",
                LastName = "Doe"
            };

            var request = new GreetingRequest() { Greeting = greeting };

            var response = await client.GreetAsync(request);
            Console.WriteLine(response.Result);
        }

        static async Task Sum(Channel channel)
        {
            WriteHeader("Sum");
            var client = new CalculatorService.CalculatorServiceClient(channel);
            var sumRequest = new SumRequest { A = 10, B = 3 };
            var response = await client.SumAsync(sumRequest);
            Console.WriteLine($"Response received: {response.Result}\r\n");
        }

        static async Task GreetServerStreaming(Channel channel)
        {
            WriteHeader("GreetServerStreaming");
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
        }

        static async Task PrimeNumberDecomposition(Channel channel)
        {
            WriteHeader("PrimeNumberDecomposition");
            var primeNumDecompositionClient = new PrimeNumberDecompositionService.PrimeNumberDecompositionServiceClient(channel);
            var primeNumDecompositionRequest = new DecompositionRequest { Value = 246 };
            var primeNumDecompoResponse = primeNumDecompositionClient.Decompose(primeNumDecompositionRequest);
            while (await primeNumDecompoResponse.ResponseStream.MoveNext())
            {
                Console.WriteLine(primeNumDecompoResponse.ResponseStream.Current.Divider);
            }
        }

        static async Task ClientStreaming(Channel channel)
        {
            WriteHeader("ClientStreaming");
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
        }

        static async Task CalcAverage(Channel channel)
        {
            WriteHeader("CalcAverage");
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
        }

        static async Task BiDi(Channel channel)
        {
            WriteHeader("BiDi");
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
        }

        static async Task MaxNumber(Channel channel)
        {
            WriteHeader("MaxNumber");
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
        }

        static async Task ErrorHandling(Channel channel)
        {
            WriteHeader("ErrorHandling");
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
        }

        static async Task Deadlines(Channel channel)
        {
            WriteHeader("Deadlines");
            var client = new DeadlineService.DeadlineServiceClient(channel);
            try
            {
                var response = await client
                    .DieOrNotAsync(
                        new DeadlineRequest { Name = "John Doe " },
                        deadline: DateTime.UtcNow.AddMilliseconds(3000)
                    );
                Console.WriteLine($"Resp: {response.Result}");
            }
            catch (RpcException e) when (e.StatusCode == StatusCode.DeadlineExceeded)
            {
                Console.WriteLine($"Err: {e.Status.Detail}");
            }
        }

        #endregion

        #region Channel creation

        static async Task<Channel> CreateSecureChannel(string host, int port)
        {
            // ssl
            var clientCert = await File.ReadAllTextAsync("ssl/client.crt");
            var clientKey = await File.ReadAllTextAsync("ssl/client.key");
            var caCrt = await File.ReadAllTextAsync("ssl/ca.crt");
            var channelCredentials = new SslCredentials(caCrt, new KeyCertificatePair(clientCert, clientKey));
            return await CreateChannel(host, port, channelCredentials);
        }
        static async Task<Channel> CreateUnsecureChannel(string host, int port) => await CreateChannel(host, port, ChannelCredentials.Insecure);
        static async Task<Channel> CreateChannel(string host, int port, ChannelCredentials credentials)
        {
            var channel = new Channel(host, port, credentials);

            await channel.ConnectAsync().ContinueWith(task =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine("Client connected successfully");
                }
            });

            return channel;
        }

        #endregion

        #region Helpers

        static int HeaderCounter = 1;
        static void WriteHeader(string header)
        {
            Console.WriteLine($"{Environment.NewLine}{HeaderCounter++} : {header}");
        }

        #endregion
    }
}
