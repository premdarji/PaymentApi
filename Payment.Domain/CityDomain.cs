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
        public CityDomain(ApplicationContext Context,IMapper mapper)
        {
            _context = Context;
            _mapper = mapper;
        }

        public async Task<bool> delete(int id)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.CityId.Equals(id));
            var test = city != null ? _context.Cities.Remove(city) : null;
             //_context.Cities.Remove(city);
            var status =await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<City>> getAll()
        {
            return await _context.Cities.ToListAsync();
        }

        public async Task<bool> post(City model)
        {
            await _context.Cities.AddAsync(model);
            var status =await  _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> put(City model)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.CityId.Equals(model.CityId));
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
        Task<bool> post(City model);
        Task<bool> put(City model);
        Task<bool> delete(int id);

        Task<List<City>> getAll();
    }
}
