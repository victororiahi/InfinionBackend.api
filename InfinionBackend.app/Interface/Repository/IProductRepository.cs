using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinionBackend.Data.Entities;
using InfinionBackend.Infrastructure.DTOs;
using InfinionBackend.Infrastructure.Utitlities;
using static InfinionBackend.Infrastructure.Utitlities.Enum;

namespace InfinionBackend.Infrastructure.Interface.Repository
{
    public interface IProductRepository
    {
        Task<Page<Product>> GetAll(Filter filter, string query, int pageNum = 1, int pageSize = 20);

        Task<Product> AddProduct(ProductDTO product);
        Task<Product> UpdateProduct(int id,ProductDTO product);
        Task<bool> DeleteProduct(int id);
        Task<Product> GetProduct(int id);
        

    }
}
