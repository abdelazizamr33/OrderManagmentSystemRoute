using DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Services
{
    public interface IProductService
    {
        Task<ProductResponseDto?> CreateProductAsync(CreateProductDto createProductDto);
        Task<ProductResponseDto?> GetProductByIdAsync(int productId);
        Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
        Task UpdateProductAsync(int productId, UpdateProductDto updateProductDto);
        Task DeleteProductAsync(int productId);
        Task UpdateStockAsync(int productId, int quantity);
        //Task<IEnumerable<ProductResponseDto>> GetProductsByStockLevelAsync(int minStock);
    }
}
