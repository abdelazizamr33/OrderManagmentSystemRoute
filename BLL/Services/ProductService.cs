using AutoMapper;
using BLL.Interfaces;
using BLL.Interfaces.Services;
using BLL.Repositories;
using DAL.Data.Contexts;
using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ProductService( IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<ProductResponseDto?> CreateProductAsync(CreateProductDto createProductDto)
        {
            var product=mapper.Map<Product>(createProductDto);
            await unitOfWork.GetRepository<Product>().AddAsync(product);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<ProductResponseDto>(product);
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await unitOfWork.GetRepository<Product>().GetByIdAsync(productId);
            if (product == null) return ;

            // Check if product has any order items
            var orderItems = await unitOfWork.GetRepository<OrderItem>().GetAllAsync(false);
            var productOrderItems = orderItems.OfType<OrderItem>().Where(oi => oi.ProductID == productId);

            if (productOrderItems.Any())
            {
                throw new InvalidOperationException($"Cannot delete product with ID {productId} because it has existing order items");
            }

            unitOfWork.GetRepository<Product>().Delete(product);
            await unitOfWork.SaveChangesAsync();
            
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            var products =await  unitOfWork.GetRepository<Product>().GetAllAsync(false);
            return mapper.Map<IEnumerable<ProductResponseDto>>(products);
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(int productId)
        {
            var product = await unitOfWork.GetRepository<Product>().GetByIdAsync(productId);
            if(product == null) return null;
            return mapper.Map<ProductResponseDto?>(product);
        }

        public async Task UpdateProductAsync(int productId, UpdateProductDto updateProductDto)
        {
            var product = await unitOfWork.GetRepository<Product>().GetByIdAsync(productId);
            if (product == null) return;

            if (!string.IsNullOrEmpty(updateProductDto.Name))
            {
                product.Name = updateProductDto.Name;
            }

            if (updateProductDto.Price.HasValue)
            {
                product.Price = updateProductDto.Price.Value;
            }

            if (updateProductDto.Stock.HasValue)
            {
                product.Stock = updateProductDto.Stock.Value;
            }

            unitOfWork.GetRepository<Product>().Update(product);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateStockAsync(int productId, int quantity)
        {
            var product = await unitOfWork.GetRepository<Product>().GetByIdAsync(productId);
            if (product == null) return ;

            product.Stock += quantity;

            if (product.Stock < 0)
            {
                throw new InvalidOperationException($"Cannot reduce stock below 0 for product {product.Name}");
            }

            unitOfWork.GetRepository<Product>().Update(product);
            await unitOfWork.SaveChangesAsync();
        }
    }
    
}
