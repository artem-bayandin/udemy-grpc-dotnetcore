using Blog;
using BlogServer.Services;
using Grpc.Core;
using Grpc.Reflection;
using Grpc.Reflection.V1Alpha;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlogServer
{
    class Program
    {
        const string Host = "localhost";
        const int Port = 50088;

        static async Task Main(string[] args)
        {
            var reflectionServiceImpl = new ReflectionServiceImpl(
                    BlogService.Descriptor
                    , ServerReflection.Descriptor
                );

            var server = new Server
            {
                Services =
                {
                    BlogService.BindService(new BlogServiceImpl()),
                    ServerReflection.BindService(reflectionServiceImpl),
                },
                Ports =
                {
                    new ServerPort(Host, Port, ServerCredentials.Insecure)
                }
            };

            try
            {
                server.Start();
                Console.WriteLine("Server is listening.");
                Console.ReadLine();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"IO Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
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
