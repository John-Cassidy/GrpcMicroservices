using Grpc.Net.Client;
using ProductGrpc.Protos;
using System;
using System.Threading;
using System.Threading.Tasks;
using static ProductGrpc.Protos.ProductProtoService;

namespace ProductGrpcClient {
    class Program {
        static async Task Main(string[] args) {
            // wait for grpc server is running
            Console.WriteLine("Waiting for server is running");
            Thread.Sleep(2000);

            using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new ProductProtoService.ProductProtoServiceClient(channel);

            await GetProductAsync(client);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static async Task GetProductAsync(ProductProtoServiceClient client) {
            // GetProductAsync
            Console.WriteLine("GetProductAsync started...");
            var response = await client.GetProductAsync(
                                new GetProductRequest {
                                    ProductId = 1
                                });

            Console.WriteLine("GetProductAsync Response: " + response.ToString());
            Thread.Sleep(1000);
        }
    }
}
