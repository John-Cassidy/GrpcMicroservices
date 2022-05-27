using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductGrpc.Data;
using ProductGrpc.Models;
using ProductGrpc.Protos;
using System;
using System.Threading.Tasks;

namespace ProductGrpc.Services {
    public class ProductService : ProductProtoService.ProductProtoServiceBase {
        private readonly ProductsContext _productDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ProductsContext productDbContext, IMapper mapper, ILogger<ProductService> logger) {
            _productDbContext = productDbContext ?? throw new ArgumentNullException(nameof(productDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override Task<Empty> Test(Empty request, ServerCallContext context) {
            return base.Test(request, context);
        }

        public override async Task<ProductModel> GetProduct(GetProductRequest request,
                                                                ServerCallContext context) {
            var product = await _productDbContext.Product.FindAsync(request.ProductId);
            if (product == null) {
                throw new RpcException(new Status(StatusCode.NotFound, $"Product with ID={request.ProductId} is not found."));
            }

            //var productModel = new ProductModel
            //{
            //    ProductId = product.Id,
            //    Name = product.Name,
            //    Description = product.Description,
            //    Price = product.Price,
            //    Status = ProductStatus.Instock,
            //    CreatedTime = Timestamp.FromDateTime(product.CreateTime)
            //};

            var productModel = _mapper.Map<ProductModel>(product);

            return productModel;
        }       

    }
}
