using Intech.NarimePalletizingSystem.DAL.Entities;
using Intech.NarimePalletizingSystem.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.NarimePalletizingSystem.BLL.Services
{
    public class PalletService
    {
        private PalletRepository _repository = new();

        public List<Pallet> GetAllPallets()
        {
            try
            {
                return _repository.FindAll();
            }
            catch(Exception e)
            {

                throw e;
            }
        }

        public List<Pallet> GetPalletByDate(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _repository.FindByDate(startDate, endDate);
            }
            catch(Exception e)
            {

                throw e;
            }
        }

        public int CountPallet(string start, string end)
        {
            try
            {
                DateTime startDate = DateTime.Parse(start);
                DateTime endDate = DateTime.Parse(end);
                return _repository.CountPalletByDateNow(startDate, endDate);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public void SavePallet(Pallet pallet)
        {
            try
            {
                _repository.Create(pallet);
            }
            catch(Exception e)
            {

                throw e;
            }
        }

        public void DeletePallet(int id)
        {
            try
            {
                _repository.Delete(id);
            }
            catch(Exception e)
            {

                throw e;
            }
        }
    }
}
