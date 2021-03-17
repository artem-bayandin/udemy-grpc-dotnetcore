using Greet;
using Grpc.Core;
using System.Threading.Tasks;
using static Greet.GreetingService;

namespace GrpcServer.ServicesImplementations
{
    public class GreetingServiceImplementation : GreetingServiceBase
    {
        public override async Task<GreetingResponse> Greet(GreetingRequest request, ServerCallContext context)
        {
            string result = $"Hello, {request.Greeting.FirstName} {request.Greeting.LastName}!";
            return new GreetingResponse { Result = result };
        }
    }
}
