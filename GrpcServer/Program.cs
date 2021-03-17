using Grpc.Core;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GrpcServer
{
    class Program
    {
        const int Port = 50051;

        static async Task Main(string[] args)
        {
            Server server = null;

            try
            {
                server = new Server
                {
                    Ports =
                    {
                        new ServerPort("localhost", Port, ServerCredentials.Insecure)
                    }
                };

                server.Start();
                Console.WriteLine($"Server is listening on {Port}");
                Console.ReadLine();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Server failed to start: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server failed: {ex.Message}");
                throw;
            }
            finally
            {
                if (server != null)
                {
                    await server.ShutdownAsync();
                }
            }
        }
    }
}
