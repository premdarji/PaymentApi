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

        public async Task<bool> Delete(int Id)
        {
            var category =await  _context.Catergory.FirstOrDefaultAsync(m => m.CategoryId.Equals(Id));
            var test = category != null ? _context.Catergory.Remove(category) : null;
            //_context.Catergory.Remove(category);
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

        public async Task<Category> GetById(int Id)
        {
            return await _context.Catergory.FirstOrDefaultAsync(m => m.CategoryId.Equals(Id));
        }

        public async Task<bool> Post(Category Model)
        {
            _context.Catergory.Add(Model);
            var status = await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;

        }

        public async Task<bool> Put(Category Model)
        {
            var category = await _context.Catergory.FirstOrDefaultAsync(m => m.CategoryId.Equals(Model.CategoryId));
            category.Name = Model.Name;
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

        Task<bool> Post(Category Model);

        Task<bool> Put(Category Model);

        Task<Category> GetById(int Id);

        Task<bool> Delete(int Id);
    }
}
