using Calculator;
using Grpc.Core;
using System;
using System.Threading.Tasks;
using static Calculator.CalculatorService;

namespace GrpcServer.ServicesImplementations
{
    public class CalculatorServiceImplementation : CalculatorServiceBase
    {
        public override async Task<SumResponse> Sum(SumRequest request, ServerCallContext context)
        {
            Console.WriteLine($"sum request received: {request.A}:{request.B}");

            var result = request.A + request.B;
            return new SumResponse { Result = result };
        }

        public override async Task<ComputeAverageResponse> ComputeAverage(IAsyncStreamReader<ComputeAverageRequest> requestStream, ServerCallContext context)
        {
            int count = 0;
            double temp = 0;

            while (await requestStream.MoveNext())
            {
                count++;
                temp += requestStream.Current.Value;
            }

            double resut = temp / count;
            return new ComputeAverageResponse { Result = resut };
        }

        public override async Task FindMax(IAsyncStreamReader<FindMaxRequest> requestStream, IServerStreamWriter<FindMaxResponse> responseStream, ServerCallContext context)
        {
            var max = 0;

            while (await requestStream.MoveNext())
            {
                var current = requestStream.Current.Value;
                Console.WriteLine($"Test value received: {current}");

                //if (current % 32 == 0)
                //{
                //    Console.WriteLine("Terminated.");
                //    break;
                //}

                if (current > max)
                {
                    max = current;
                    await responseStream.WriteAsync(new FindMaxResponse { Max = max });
                }
            }
        }
    }
}
