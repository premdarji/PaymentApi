using Payment.Entity;
using Payment.Entity.DbModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Payment.Entity.ViewModels;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Payment.Domain
{
    public class CityDomain:ICityDomain
    {
        ApplicationContext _context;

        private readonly IMapper _mapper;
        public CityDomain(ApplicationContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Delete(int id)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.CityId.Equals(id));
            
             _context.Cities.Remove(city);
            var status =await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<City>> GetAll()
        {
            return await _context.Cities.ToListAsync();
          
            //return cities;
           
        }

        public async Task<bool> Post(City model)
        {
            await _context.Cities.AddAsync(model);
            var status =await  _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;

        }

        public async Task<bool> Put(int id, City model)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.CityId.Equals(id));
            city.CityName = model.CityName;
            var status =await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;

        }
    }

    public interface ICityDomain
    {
        Task<bool> Post(City model);
        Task<bool> Put(int id, City model);
        Task<bool> Delete(int id);

        Task<List<City>> GetAll();
    }
}
