using Calculator;
using Deadlines;
using ErrorsSample;
using Greet;
using Grpc.Core;
using Grpc.Reflection;
using Grpc.Reflection.V1Alpha;
using GrpcServer.ServicesImplementations;
using PrimeNumber;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GrpcServer
{
    // Tool to view a list of grpc resources available (aka Swagger) :
    // https://github.com/ktr0731/evans/releases

    class Program
    {
        const string Host = "127.0.0.1";
        const int Port = 50051;

        static async Task Main(string[] args)
        {
            Server server = null;

            try
            {
                // reflection-1                
                var reflectionServiceImpl = new ReflectionServiceImpl(
                    GreetingService.Descriptor
                    , CalculatorService.Descriptor
                    , PrimeNumberDecompositionService.Descriptor
                    , SqrtService.Descriptor
                    , DeadlineService.Descriptor
                    , ServerReflection.Descriptor
                );

                server = new Server
                {
                    Services =
                    {
                        GreetingService.BindService(new GreetingServiceImplementation()),
                        CalculatorService.BindService(new CalculatorServiceImplementation()),
                        PrimeNumberDecompositionService.BindService(new PrimeNumberDecompositionServiceImplementation()),

                        // errors
                        SqrtService.BindService(new SqrtServiceImplementation()),
                        // deadlines
                        DeadlineService.BindService(new DeadlineServiceImplementation()),

                        // reflection-2
                        ServerReflection.BindService(reflectionServiceImpl)
                    },
                    Ports =
                    {
                        await CreateUnsecureServerPort(Host, Port)
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

        #region ServerPort creation

        static async Task<ServerPort> CreateSecureServerPort(string host, int port)
        {
            var serverCert = await File.ReadAllTextAsync("ssl/server.crt");
            var serverKey = await File.ReadAllTextAsync("ssl/server.key");
            var caCrt = await File.ReadAllTextAsync("ssl/ca.crt");
            var credentials = new SslServerCredentials(new List<KeyCertificatePair> { new KeyCertificatePair(serverCert, serverKey) }, caCrt, true);
            return await CreateServerPort(host, port, credentials);
        }
        static async Task<ServerPort> CreateUnsecureServerPort(string host, int port) => await CreateServerPort(host, port, ServerCredentials.Insecure);
        static async Task<ServerPort> CreateServerPort(string host, int port, ServerCredentials credentials) => await Task.FromResult(new ServerPort(host, port, credentials));

        #endregion
    }
}
