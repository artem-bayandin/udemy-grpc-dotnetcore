using Grpc.Core;
using PrimeNumber;
using System.Threading.Tasks;
using static PrimeNumber.PrimeNumberDecompositionService;

namespace GrpcServer.ServicesImplementations
{
    public class PrimeNumberDecompositionServiceImplementation : PrimeNumberDecompositionServiceBase
    {
        public override async Task Decompose(DecompositionRequest request, IServerStreamWriter<DecompositionResponse> responseStream, ServerCallContext context)
        {
            var k = 2;
            var value = request.Value;

            while (value > 1)
            {
                if (value % k == 0)
                {
                    await responseStream.WriteAsync(new DecompositionResponse { Divider = k });
                    value /= k;
                    await Task.Delay(200);
                }
                else
                {
                    k++;
                }
            }
        }
    }
}
