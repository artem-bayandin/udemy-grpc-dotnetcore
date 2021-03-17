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
    }
}
