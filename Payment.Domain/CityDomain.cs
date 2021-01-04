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

        public async Task<bool> Delete(int Id)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.CityId.Equals(Id));
            var test = city != null ? _context.Cities.Remove(city) : null;
             //_context.Cities.Remove(city);
            var status =await _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<City>> GetAll()
        {
            //var cities = (from city in _context.Cities
            //              orderby city.CityName descending
            //             select city).ToList();
            //return cities;
            return await _context.Cities.ToListAsync();

         

        }

        public async Task<bool> Post(City Model)
        {
            await _context.Cities.AddAsync(Model);
            var status =await  _context.SaveChangesAsync();
            if (status > 0)
            {
                return true;
            }
            return false;

        }

        public async Task<bool> Put(City Model)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.CityId.Equals(Model.CityId));
            city.CityName = Model.CityName;
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
        Task<bool> Post(City Model);
        Task<bool> Put(City Model);
        Task<bool> Delete(int Id);

        Task<List<City>> GetAll();
    }
}
