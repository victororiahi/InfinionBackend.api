using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinionBackend.Data;
using InfinionBackend.Data.Entities;
using InfinionBackend.Infrastructure.DTOs;
using InfinionBackend.Infrastructure.Interface.Repository;
using InfinionBackend.Infrastructure.Utitlities;
using Microsoft.EntityFrameworkCore;
using static InfinionBackend.Infrastructure.Utitlities.Enum;

namespace InfinionBackend.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;
        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product> AddProduct(ProductDTO product)
        {
            var entity = await _dbContext.Set<Product>().FirstOrDefaultAsync(x => x.Name == product.Name);
            if (entity != null) 
                throw new Exception($"Product: {product.Name} already exists!");
            var ent = new Product
            {
                DateAdded = DateTime.Now,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };

            await _dbContext.AddAsync(ent);
            await _dbContext.SaveChangesAsync();
            return ent;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var entity = await _dbContext.Set<Product>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                throw new Exception($"Product with Id: {id} not found!");

            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Page<Product>> GetAll(Filter filter, string query, int pageNum, int pageSize)
        {
            var records = _dbContext.Set<Product>().Where(x => x.Id > 0);
            switch (filter)
            {
                case Filter.Name:
                    records = records.Where(x => EF.Functions.Like(x.Name, $"%{query}%"));
                    break;
                
                case Filter.Description:
                    records = records.Where(x => EF.Functions.Like(x.Description, $"%{query}%"));

                    break;
                case Filter.Price:
                    decimal.TryParse(query, out decimal price);
                    records = records.Where(x => x.Price == price);
                    break;
                default:
                    break;
            }

            var results = await records.OrderByDescending(x => x.DateAdded).ToPageListAsync(pageNum,pageSize);
            return results;
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await _dbContext.Set<Product>().FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) throw new Exception("Product not found");
            return product;
        }

        public async Task<Product> UpdateProduct(int id,ProductDTO product)
        {
            var entity = await _dbContext.Set<Product>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) throw new Exception("Product not found");

            entity.Name = product.Name ?? entity.Name;
            entity.Description = product.Description ?? entity.Description;
            entity.Price = product.Price > 0 ? product.Price : entity.Price;

            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
