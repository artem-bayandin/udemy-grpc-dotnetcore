using Deadlines;
using Grpc.Core;
using System.Threading.Tasks;
using static Deadlines.DeadlineService;

namespace GrpcServer.ServicesImplementations
{
    public class DeadlineServiceImplementation : DeadlineServiceBase
    {
        public override async Task<DeadlineResponse> DieOrNot(DeadlineRequest request, ServerCallContext context)
        {
            await Task.Delay(5000);
            return new DeadlineResponse { Result = $"Hello, {request.Name}" };
        }
    }
}
