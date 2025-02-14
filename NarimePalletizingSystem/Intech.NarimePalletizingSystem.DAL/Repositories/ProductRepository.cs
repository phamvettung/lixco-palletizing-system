using Intech.NarimePalletizingSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.NarimePalletizingSystem.DAL.Repositories
{
    public class ProductRepository
    {
        private Lixco2024Context _context;

        public List<Product> FindAll()
        {
            try
            {
                _context = new();
                return _context.Products.ToList();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public Product FindByProductCode(string productCode)
        {
            try
            {
                _context = new();
                List<Product> products = _context.Products.Where(o => o.ProductCode == productCode).ToList();
                if (products.Count > 0)
                    return products[0];
                else
                    return null;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public Product FindByModel(int model)
        {
            try
            {
                _context = new();
                List<Product> products = _context.Products.Where(o => o.Model == model).ToList();
                if (products.Count > 0)
                    return products[0];
                else
                    return null;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public void Create(Product product)
        {
            try
            {
                _context = new();
                _context.Products.Add(product);
                _context.SaveChanges();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public void Update(Product product)
        {
            try
            {
                _context = new();
                var itemToUpdate = _context.Products.SingleOrDefault(o => o.ProductCode == product.ProductCode);
                if (itemToUpdate != null)
                {
                    itemToUpdate.ProductCode = product.ProductCode;
                    itemToUpdate.ProductName = product.ProductName;
                    itemToUpdate.NetWeight = product.NetWeight;
                    itemToUpdate.NumBinsOnPallet = product.NumBinsOnPallet;
                    itemToUpdate.Model = product.Model;
                    itemToUpdate.PacketWeight = product.PacketWeight;
                    itemToUpdate.NumPacketsOnBin = product.NumPacketsOnBin;
                    itemToUpdate.ProductImages = product.ProductImages;
                    _context.Update(itemToUpdate);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public void Delete(string productCode)
        {
            try
            {
                _context = new();
                var itemToDelete = _context.Products.SingleOrDefault(o => o.ProductCode == productCode);
                if (itemToDelete != null)
                {
                    _context.Products.Remove(itemToDelete);
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
