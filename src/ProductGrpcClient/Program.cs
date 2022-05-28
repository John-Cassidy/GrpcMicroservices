using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
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
            await GetAllProducts(client);
            await AddProductAsync(client);
            
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

        private static async Task GetAllProducts(ProductProtoServiceClient client) {
            //// GetAllProducts
            //Console.WriteLine("GetAllProducts started...");
            //using (var clientData = client.GetAllProducts(new GetAllProductsRequest()))
            //{
            //    while (await clientData.ResponseStream.MoveNext(new System.Threading.CancellationToken()))
            //    {
            //        var currentProduct = clientData.ResponseStream.Current;
            //        Console.WriteLine(currentProduct);
            //    }
            //}
            //Thread.Sleep(1000);

            // GetAllProducts with C# 8
            Console.WriteLine("GetAllProducts with C#8 started...");
            using var clientData = client.GetAllProducts(new GetAllProductsRequest());
            await foreach (var responseData in clientData.ResponseStream.ReadAllAsync()) {
                Console.WriteLine(responseData);
            }
            Thread.Sleep(1000);
        }

        private static async Task AddProductAsync(ProductProtoServiceClient client) {
            // AddProductAsync
            Console.WriteLine("AddProductAsync started...");
            var addProductResponse = await client.AddProductAsync(
                                new AddProductRequest {
                                    Product = new ProductModel {
                                        Name = "Red",
                                        Description = "New Red Phone Mi10T",
                                        Price = 699,
                                        Status = ProductStatus.Instock,
                                        CreatedTime = Timestamp.FromDateTime(DateTime.UtcNow)
                                    }
                                });

            Console.WriteLine("AddProduct Response: " + addProductResponse.ToString());
            Thread.Sleep(1000);
        }
    }
}
