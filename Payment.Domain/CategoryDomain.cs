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
        public CategoryDomain(ApplicationContext Context)
        {
            _context = Context;
        }

        public async Task<bool> delete(int id)
        {
            var category =await  _context.Catergory.FindAsync(id);
            var test = category != null ? _context.Catergory.Remove(category) : null;
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<Category>> getAll()
        {
            return await _context.Catergory.ToListAsync();
        }

        public async Task<Category> getById(int id)
        {
            return await _context.Catergory.FindAsync(id);
        }

        public async Task<bool> post(Category model)
        {
            _context.Catergory.Add(model);
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;

        }

        public async Task<bool> put(Category model)
        {
            var category = await _context.Catergory.FindAsync(model.CategoryId);
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
        Task<List<Category>> getAll();

        Task<bool> post(Category model);

        Task<bool> put(Category model);

        Task<Category> getById(int id);

        Task<bool> delete(int id);
    }
}
