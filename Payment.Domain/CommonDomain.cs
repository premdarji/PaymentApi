using Payment.Entity;
using Payment.Entity.DbModels;
using Payment.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Payment.Domain
{
    public class CommonDomain : ICommonDomain
    {
        ApplicationContext _context;
        public CommonDomain(ApplicationContext context)
        {
            _context = context;
        }

        public Dictionary<string,string> get(int id)
        {
            Dictionary<string, string> commonFields = new Dictionary<string, string>();

            var list = _context.CommonFields.Where(x => x.LanguageId == id).ToList();

            foreach (var iten in list)
            {
                commonFields.Add(iten.Parameter, iten.Value);
            }
            return commonFields;
        }

        public void RegisterError(ErrorDetails err)
        {
            _context.Errordetails.Add(err);
            _context.SaveChanges();
        }
    }

    public interface ICommonDomain
    {
        Dictionary<string,string> get(int id);

         void RegisterError(ErrorDetails err);
    }

   
}
