using Greet;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Greet.GreetingService;

namespace GrpcServer.ServicesImplementations
{
    public class GreetingServiceImplementation : GreetingServiceBase
    {
        // Unary
        public override async Task<GreetingResponse> Greet(GreetingRequest request, ServerCallContext context)
        {
            string result = $"Hello, {request.Greeting.FirstName} {request.Greeting.LastName}!";
            return new GreetingResponse { Result = result };
        }

        // Server streaming
        public override async Task GreetManyTimes(GreetManyTimesRequest request, IServerStreamWriter<GreetManyTimesResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine($"Request received: {request}");

            for (var i = 0; i < request.Times; i++)
            {
                var res = $"Attempt #{i + 1} to greet {request.Greeting.FirstName} {request.Greeting.LastName}";
                await responseStream
                    .WriteAsync(new GreetManyTimesResponse
                    {
                        Result = res
                    });
            }
        }

        // Client streaming
        public override async Task<LongGreetResponse> LongGreet(IAsyncStreamReader<LongGreetRequest> requestStream, ServerCallContext context)
        {
            var sb = new StringBuilder();

            while (await requestStream.MoveNext())
            {
                sb.AppendLine($" {requestStream.Current.Greeting}");
            }

            return new LongGreetResponse { Result = sb.ToString() };
        }

        // bi-di
        public override async Task GreetEveryone(IAsyncStreamReader<GreetEveryoneRequest> requestStream, IServerStreamWriter<GreetEveryoneResponse> responseStream, ServerCallContext context)
        {
            var names = new List<string>();
            var sb = new StringBuilder();
            var run = true;

            Task.Run(async () =>
            {
                while (run)
                {
                    var resp = new GreetEveryoneResponse { Result = new Random().Next(100, 999).ToString() };
                    await responseStream.WriteAsync(resp);
                    await Task.Delay(1000);
                }
            });

            while (await requestStream.MoveNext())
            {
                Console.WriteLine(requestStream.Current.Name);
            }

            run = false;
        }
    }
}
