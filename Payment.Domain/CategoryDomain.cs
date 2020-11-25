using Payment.Entity;
using Payment.Entity.DbModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Payment.Domain
{
    public class CategoryDomain : ICategoryDomain
    {
        ApplicationContext _context;
        public CategoryDomain(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(int id)
        {
            var category =await  _context.Catergory.FirstOrDefaultAsync(m => m.CategoryId.Equals(id));
            _context.Catergory.Remove(category);
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<Category>> GetAll()
        {
            return await _context.Catergory.ToListAsync();
        }

        public async Task<Category> GetById(int id)
        {
            return await _context.Catergory.FirstOrDefaultAsync(m => m.CategoryId.Equals(id));
        }

        public async Task<bool> Post(Category model)
        {
            _context.Catergory.Add(model);
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;

        }

        public async Task<bool> Put(int id, Category model)
        {
            var category = await _context.Catergory.FirstOrDefaultAsync(m => m.CategoryId.Equals(id));
            category.Name = model.Name;
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }
    }


    public interface ICategoryDomain
    {
        Task<List<Category>> GetAll();

        Task<bool> Post(Category model);

        Task<bool> Put(int id, Category model);

        Task<Category> GetById(int id);

        Task<bool> Delete(int id);
    }
}
