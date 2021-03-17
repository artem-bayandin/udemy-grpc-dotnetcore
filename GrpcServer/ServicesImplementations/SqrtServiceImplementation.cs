using ErrorsSample;
using Grpc.Core;
using System;
using System.Threading.Tasks;
using static ErrorsSample.SqrtService;

namespace GrpcServer.ServicesImplementations
{
    public class SqrtServiceImplementation : SqrtServiceBase
    {
        public override async Task<SqrtResponse> Sqrt(SqrtRequest request, ServerCallContext context)
        {
            var number = request.Number;

            if (number >= 0)
            {
                return new SqrtResponse { SqrtRoot = Math.Sqrt(number) };
            }

            throw new RpcException(new Status(StatusCode.InvalidArgument, "number < 0"));
        }
    }
}
