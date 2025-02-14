using Intech.NarimePalletizingSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.NarimePalletizingSystem.DAL.Repositories
{
    public class PalletRepository
    {
        private Lixco2024Context _context;

        public List<Pallet> FindAll()
        {
            try
            {
                _context = new();
                return _context.Pallets.Include("ProductCodeNavigation").ToList();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public List<Pallet> FindByDate(DateTime startDate, DateTime endDate)
        {
            try
            {
                _context = new();
                return _context.Pallets.Include("ProductCodeNavigation").Where(o => o.DateTime >= startDate && o.DateTime <= endDate).ToList();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public int CountPalletByDateNow(DateTime startDate, DateTime endDate)
        {
            try
            {
                _context = new();
                return _context.Pallets.Count(o => o.DateTime >= startDate && o.DateTime <= endDate);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public void Create(Pallet pallet)
        {
            try
            {
                _context = new();
                _context.Add(pallet);
                _context.SaveChanges();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public void Delete(int id)
        {
            try
            {
                _context = new();
                var itemToDelete = _context.Pallets.SingleOrDefault(o => o.PalletId == id);
                if (itemToDelete != null)
                {
                    _context.Pallets.Remove(itemToDelete);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

    }
}
